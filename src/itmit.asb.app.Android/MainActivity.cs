using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using AndroidX.Work;
using Com.Xamarin.Formsviewgroup;
using itmit.asb.app.Droid;
using itmit.asb.app.Droid.Services;
using ImageCircle.Forms.Plugin.Droid;
using itmit.asb.app.Services;
using Java.Math;
using Java.Util;
using Matcha.BackgroundService.Droid;
using Plugin.Permissions;
using RU.Yandex.Money.Android.Sdk;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: Dependency(typeof(AuthService))]
[assembly: Dependency(typeof(BidsService))]
namespace itmit.asb.app.Droid
{
	[Activity(Label = "itmit.asb.app",
		Icon = "@mipmap/icon2",
		Theme = "@style/MainTheme",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : FormsAppCompatActivity
	{
		#region Data
		#region Consts
		private const int PermissionsRequestAccessCoarseLocation = 100;

		private const int PermissionsRequestAccessFineLocation = 50;

		private const int PermissionsRequestAccessReadStorage = 10;

		private const int PermissionsRequestAccessWriteStorage = 20;

		private const int RequestCheckSettings = 1;
		#endregion
		#endregion

		#region Overrided
		protected override void OnCreate(Bundle savedInstanceState)
		{
            ImageCircleRenderer.Init();
			BackgroundAggregator.Init(this);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			FormsMaps.Init(this, savedInstanceState);

			Instance = this;

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			CheckPermissions();

			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

			DisplayLocationSettingsRequest();


			LoadApplication(new App());
		}
		#endregion

		public static MainActivity Instance
		{
			get;
			private set;
		}

		private static int REQUEST_CODE_TOKENIZE = 33;
		private static Currency RUB = Currency.GetInstance("RUB");
		public void InitUi()
		{
			Settings settings = new Settings(this);

			HashSet<PaymentMethodType> paymentMethodTypes = GetPaymentMethodTypes(settings);

			BigDecimal amount = new BigDecimal(new BigInteger("11"));
			PaymentParameters paymentParameters = new PaymentParameters(
				new Amount(amount, RUB),
				"prod_name",
				"prod_desc",
				"test_NjM5MDYw4_MI8X9BbkIMK20BqJ84Iw4gLyeWnXJrqrk",
				"639060", 
				paymentMethodTypes);

			UiParameters uiParameters = new UiParameters(
				settings.ShowYandexCheckoutLogo(), new ColorScheme(settings.GetPrimaryColor()));

			MockConfiguration mockConfiguration;
			if (settings.IsTestModeEnabled())
			{
				mockConfiguration = new MockConfiguration(settings.ShouldCompletePaymentWithError(),
														  settings.IsPaymentAuthPassed(),
														  settings.GetLinkedCardsCount(),
														  new Amount(new BigDecimal(settings.GetServiceFee()), RUB));
			}
			else
			{

				mockConfiguration = null;
			}
			TestParameters testParameters = new TestParameters(true, false, mockConfiguration);
			Intent intent = Checkout.CreateTokenizeIntent(this,
														  paymentParameters,
														  testParameters,
														  uiParameters
			);

			StartActivityForResult(intent, REQUEST_CODE_TOKENIZE);
		}

		private static HashSet<PaymentMethodType> GetPaymentMethodTypes(Settings settings)
		{
			HashSet<PaymentMethodType> paymentMethodTypes = new HashSet<PaymentMethodType>();

			if (settings.IsYandexMoneyAllowed())
			{
				paymentMethodTypes.Add(PaymentMethodType.YandexMoney);
			}

			if (settings.IsNewCardAllowed())
			{
				paymentMethodTypes.Add(PaymentMethodType.BankCard);
			}

			if (settings.IsSberbankOnlineAllowed())
			{
				paymentMethodTypes.Add(PaymentMethodType.Sberbank);
			}

			if (settings.IsGooglePayAllowed())
			{
				paymentMethodTypes.Add(PaymentMethodType.GooglePay);
			}

			return paymentMethodTypes;
		}

		public static void StartForegroundServiceCompat<T>(Context context, Bundle args = null) where T : Service
		{
			var intent = new Intent(context, typeof(T));
			if (args != null)
			{
				intent.PutExtras(args);
			}
			intent.SetAction(Constants.ActionStartService);
			context.StopService(intent);
			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				context.StartForegroundService(intent);
			}
			else
			{
				context.StartService(intent);
			}
		}

		#region Private
		private void DisplayLocationSettingsRequest()
		{
			var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API)
																   .Build();
			googleApiClient.Connect();

			var locationRequest = LocationRequest.Create();
			locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
			locationRequest.SetInterval(10000);
			locationRequest.SetFastestInterval(10000 / 2);

			var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
			builder.SetAlwaysShow(true);

			var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
			result.SetResultCallback((LocationSettingsResult callback) =>
			{
				switch (callback.Status.StatusCode)
				{
					case CommonStatusCodes.Success:
					{
						//DoStuffWithLocation();
						break;
					}
					case CommonStatusCodes.ResolutionRequired:
					{
						try
						{
							// Show the dialog by calling startResolutionForResult(), and check the result
							// in onActivityResult().
							callback.Status.StartResolutionForResult(this, RequestCheckSettings);
						}
						catch (IntentSender.SendIntentException e)
						{
						}

						break;
					}
					default:
					{
						// If all else fails, take the user to the android location settings
						StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
						break;
					}
				}
			});
		}

		private void CheckPermissions()
		{
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
			{
				CheckPermission(Manifest.Permission.AccessFineLocation, PermissionsRequestAccessFineLocation);
			}

			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
			{
				CheckPermission(Manifest.Permission.AccessCoarseLocation, PermissionsRequestAccessCoarseLocation);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private void CheckPermission(string permission, int permissionsRequestCode)
		{
			ActivityCompat.ShouldShowRequestPermissionRationale(this, permission);
			ActivityCompat.RequestPermissions(this,
											  new[]
											  {
												  permission
											  },
											  permissionsRequestCode);
		}
		#endregion
	}
}
