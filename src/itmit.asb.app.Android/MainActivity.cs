using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Webkit;
using Android.Widget;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.ViewModels;
using ImageCircle.Forms.Plugin.Droid;
using Java.Lang;
using Java.Math;
using Java.Util;
using Matcha.BackgroundService.Droid;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Realms;
using RU.Yandex.Money.Android.Sdk;
using Xamarin;
using Xamarin.Android.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using IOException = Java.IO.IOException;
using Platform = Xamarin.Essentials.Platform;

[assembly: Dependency(typeof(AuthService))]

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
		private const int PermissionsRequestAccessFineLocation = 50;
		private const int RequestCheckSettings = 1;
		private const int RequestCode3Ds = 30;
		private const int RequestCodeTokenize = 33;
		#endregion

		#region Static
		private static readonly Currency Rub = Currency.GetInstance("RUB");
		private string _token;
		private string _oldActivityTo;
		#endregion
		#endregion

		#region Properties
		public static MainActivity Instance
		{
			get;
			private set;
		}

		public IYandexCheckout CheckoutService
		{
			get;
			set;
		}
		#endregion

		#region Public
		public void InitUi()
		{
			var settings = new Settings(this);

			var paymentMethodTypes = GetPaymentMethodTypes(settings);

			var amount = new BigDecimal(BigInteger.One);
			var paymentParameters = new PaymentParameters(new Amount(amount, Rub),
														  "АСБ Подписка",
														  "Для получения возможности отправки тревоги, необходимо оплатить подписку.",
														  "live_NjQwOTI43n8qs0roPyYJGGSb5248EIztMBFQaTyfJZ4",
														  "640928",
														  paymentMethodTypes);

			var uiParameters = new UiParameters(settings.ShowYandexCheckoutLogo, new ColorScheme(settings.GetPrimaryColor));

			MockConfiguration mockConfiguration;
			if (settings.IsTestModeEnabled)
			{
				mockConfiguration = new MockConfiguration(settings.ShouldCompletePaymentWithError,
														  settings.IsPaymentAuthPassed,
														  settings.GetLinkedCardsCount,
														  new Amount(new BigDecimal(settings.GetServiceFee), Rub));
			}
			else
			{
				mockConfiguration = null;
			}

			var testParameters = new TestParameters(true, false, mockConfiguration);
			var intent = Checkout.CreateTokenizeIntent(this, paymentParameters, testParameters, uiParameters);

			StartActivityForResult(intent, RequestCodeTokenize);
		}

		/* Checks if external storage is available to at least read */
		public bool IsExternalStorageReadable(File file)
		{
			var state = Environment.GetExternalStorageState(file);
			if (Environment.MediaMounted.Equals(state) || Environment.MediaMountedReadOnly.Equals(state))
			{
				return true;
			}

			return false;
		}

		/* Checks if external storage is available for read and write */
		public bool IsExternalStorageWritable(File file)
		{
			var state = Environment.GetExternalStorageState(file);
			if (Environment.MediaMounted.Equals(state))
			{
				return true;
			}

			return false;
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
		#endregion

		#region Overrided
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted ||
				ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Granted)
			{
				InitLog();
			}
		}

		/// <param name="requestCode">
		/// The integer request code originally supplied to
		/// startActivityForResult(), allowing you to identify who this
		/// result came from.
		/// </param>
		/// <param name="resultCode">
		/// The integer result code returned by the child activity
		/// through its setResult().
		/// </param>
		/// <param name="data">
		/// An Intent, which can return result data to the caller
		/// (various data can be attached to Intent "extras").
		/// </param>
		/// <summary>
		/// Called when an activity you launched exits, giving you the requestCode
		/// you started it with, the resultCode it returned, and any additional
		/// data from it.
		/// </summary>
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">
		///     Called when an activity you launched exits, giving you the requestCode
		///     you started it with, the resultCode it returned, and any additional
		///     data from it.  The
		///     <format type="text/html">
		///         <var>resultCode</var>
		///     </format>
		///     will be
		///     <c>
		///         <see cref="F:Android.App.Result.Canceled" tool="ReplaceLinkValue" />
		///     </c>
		///     if the activity explicitly returned that,
		///     didn't return any result, or crashed during its operation.
		///     </para>
		///     <para tool="javadoc-to-mdoc">
		///     You will receive this call immediately before onResume() when your
		///     activity is re-starting.
		///     </para>
		///     <para tool="javadoc-to-mdoc">
		///     This method is never invoked if your activity sets
		///     <c>
		///         <see
		///             cref="!:NoType:android/R$styleable;Href=../../../reference/android/R.styleable.html#AndroidManifestActivity_noHistory" />
		///     </c>
		///     to
		///     <c>true</c>.
		///     </para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a
		///                 href="http://developer.android.com/reference/android/app/Activity.html#onActivityResult(int, int, android.content.Intent)"
		///                 target="_blank">
		///             [Android Documentation]
		///             </a>
		///         </format>
		///     </para>
		/// </remarks>
		/// <since version="Added in API level 1" />
		/// <altmember cref="M:Android.App.Activity.StartActivityForResult(Android.Content.Intent, System.Int32)" />
		/// <altmember
		///     cref="M:Android.App.Activity.CreatePendingResult(System.Int32, Android.Content.Intent, Android.Content.Intent)" />
		/// <altmember cref="M:Android.App.Activity.SetResult(Android.App.Result)" />
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			switch (requestCode)
			{
				case RequestCodeTokenize:
					if (CheckoutService == null)
					{
						break;
					}
					var result = Checkout.CreateTokenizationResult(data);
					switch (resultCode)
					{
						case Result.Ok:

							// successful tokenization
							_token = result.PaymentToken;
							CreatePayment(_token, new UserToken
							{
								Token = (string) App.User.UserToken.Token.Clone()
							});
							break;
						case Result.Canceled:

							// user canceled tokenization
							Toast.MakeText(this, "Оплата отменена.", ToastLength.Short)
								 .Show();
							break;
					}

					break;
				case RequestCode3Ds:
					if (string.IsNullOrEmpty(_token))
					{
						break;
					}

					CapturePayment(_token,
								   new UserToken
								   {
									   Token = (string) App.User.UserToken.Token.Clone()
								   });
					break;
			}
		}

		private async void CapturePayment(string token, UserToken userToken)
		{
			_oldActivityTo = AboutViewModel.Instance.ActiveTo;
			AboutViewModel.Instance.IsShowedIndicator = true;
			AboutViewModel.Instance.IsShowedActivityTitle = false;
			AboutViewModel.Instance.ActiveTo = "";
			var payment = await CheckoutService.CapturePayment(token, userToken);
			if (payment != null)
			{
				if (payment.IsActive)
				{
					new AlertDialog.Builder(this).SetMessage("Подписка оплачена.")
												 .SetNegativeButton("Ок",
																	(dialog, which) =>
																	{
																	})
												 .Show();
				}

				UpdateActiveUser(payment.ActiveFrom);

				AboutViewModel.Instance.IsShowedActivityTitle = true;
				AboutViewModel.Instance.ActiveTo = payment.ActiveFrom.Add(new TimeSpan(30, 3, 0, 0)) 
				                                          .ToString("dd.MM.yyyy hh:mm");
			}
			else
			{

				AboutViewModel.Instance.IsShowedActivityTitle = false;
				AboutViewModel.Instance.ActiveTo = _oldActivityTo;
				new AlertDialog.Builder(this).SetMessage("Возникла ошибка при оплате.")
											 .SetNegativeButton("Ок",
																(dialog, which) =>
																{
																})
											 .Show();
			}
			AboutViewModel.Instance.IsShowedIndicator = false;
		}

		private void UpdateActiveUser(DateTimeOffset activeForm)
		{
			var con = RealmConfiguration.DefaultConfiguration;
			con.SchemaVersion = 7;
			var realm = Realm.GetInstance(con);
			using (realm)
			{
				var user = App.User;
				using (var transaction = realm.BeginWrite())
				{
					user.IsActive = true;
					user.ActiveFrom = activeForm;
					transaction.Commit();
				}
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			ImageCircleRenderer.Init();
			BackgroundAggregator.Init(this);
			Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			FormsMaps.Init(this, savedInstanceState);

			Instance = this;

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			CheckPermissions();

			CrossCurrentActivity.Current.Init(this, savedInstanceState);

			DisplayLocationSettingsRequest();

			LoadApplication(new App());

			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Granted ||
				ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Granted)
			{
				InitLog();
			}
		}
		#endregion

		#region Private
		private void CheckPermission(string[] permissions, int permissionsRequestCode)
		{
			ActivityCompat.RequestPermissions(this, permissions, permissionsRequestCode);
		}

		private void CheckPermissions()
		{
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted ||
				ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
			{
				CheckPermission(new[]
								{
									Manifest.Permission.AccessFineLocation,
									Manifest.Permission.AccessCoarseLocation,
									Manifest.Permission.ReadExternalStorage,
									Manifest.Permission.WriteExternalStorage
								},
								PermissionsRequestAccessFineLocation);
			}
		}

		private async void CreatePayment(string token, UserToken userToken)
		{
			var securityUri = await CheckoutService.CreatePayment(token, userToken);
			if (securityUri == null)
			{
				return;
			}
			if (URLUtil.IsHttpsUrl(securityUri.AbsoluteUri) || URLUtil.IsAssetUrl(securityUri.AbsoluteUri))
			{
				StartActivityForResult(Checkout.Create3dsIntent(this, securityUri.AbsoluteUri), RequestCode3Ds);
			}
		}

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
							Console.WriteLine(e);
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

		private static HashSet<PaymentMethodType> GetPaymentMethodTypes(Settings settings)
		{
			var paymentMethodTypes = new HashSet<PaymentMethodType>();

			if (settings.IsYandexMoneyAllowed)
			{
				paymentMethodTypes.Add(PaymentMethodType.YandexMoney);
			}

			if (settings.IsNewCardAllowed)
			{
				paymentMethodTypes.Add(PaymentMethodType.BankCard);
			}

			if (settings.IsSberbankOnlineAllowed)
			{
				paymentMethodTypes.Add(PaymentMethodType.Sberbank);
			}

			if (settings.IsGooglePayAllowed)
			{
				paymentMethodTypes.Add(PaymentMethodType.GooglePay);
			}

			return paymentMethodTypes;
		}

		private void InitLog()
		{
			var appDirectory = new File($"{Environment.ExternalStorageDirectory}/Asb");
			var logDirectory = new File(appDirectory + "/log");
			var time = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
							   .TotalSeconds;
			var logFile = new File(logDirectory, $"logcat-{time}.txt");

			if (IsExternalStorageWritable(logFile))
			{
				// create app folder
				if (!appDirectory.Exists())
				{
					Directory.CreateDirectory(appDirectory.AbsolutePath);
				}

				// create log folder
				if (!logDirectory.Exists())
				{
					Directory.CreateDirectory(logDirectory.AbsolutePath);
				}

				// clear the previous logcat and then write the new one to the file
				try
				{
					Runtime.GetRuntime()
						   .Exec("logcat -c");
					Runtime.GetRuntime()
						   .Exec($"logcat -f {logFile.AbsolutePath} *:E");
				}
				catch (IOException e)
				{
					e.PrintStackTrace();
				}
			}
		}
		#endregion
	}
}
