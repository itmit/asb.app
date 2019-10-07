using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Android.Util;
using Android.Widget;
using AndroidX.Work;
using Com.Xamarin.Formsviewgroup;
using itmit.asb.app.Droid;
using itmit.asb.app.Droid.Services;
using itmit.asb.app.Models;
using ImageCircle.Forms.Plugin.Droid;
using itmit.asb.app.Services;
using itmit.asb.app.ViewModels;
using Java.Lang;
using Java.Math;
using Java.Util;
using Matcha.BackgroundService.Droid;
using Newtonsoft.Json;
using Plugin.Permissions;
using Realms;
using RU.Yandex.Money.Android.Sdk;
using Xamarin;
using Xamarin.Android.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Console = Java.IO.Console;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using IOException = Java.IO.IOException;
using Process = Android.OS.Process;
using String = System.String;

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

			InitLog();
		}

		private void InitLog()
		{
			File appDirectory = new File($"{Environment.ExternalStorageDirectory}/Asb");
			File logDirectory = new File(appDirectory + "/log");
			var time = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
							   .TotalSeconds;
			File logFile = new File(logDirectory,
									$"logcat-{time}.txt");

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
					Runtime.GetRuntime().Exec("logcat -c");
					Runtime.GetRuntime().Exec($"logcat -f {logFile.AbsolutePath} *:E");
				}
				catch (IOException e)
				{
					e.PrintStackTrace();
				}
			}
		}

		/* Checks if external storage is available for read and write */
		public bool IsExternalStorageWritable(File file)
		{
			string state = Environment.GetExternalStorageState(file);
			if (Environment.MediaMounted.Equals(state))
			{
				return true;
			}

			return false;
		}

		/* Checks if external storage is available to at least read */
		public bool IsExternalStorageReadable(File file)
		{
			string state = Environment.GetExternalStorageState(file);
			if (Environment.MediaMounted.Equals(state) ||
				Environment.MediaMountedReadOnly.Equals(state))
			{
				return true;
			}

			return false;
		}
		#endregion

		public static MainActivity Instance
		{
			get;
			private set;
		}

		private const int RequestCodeTokenize = 33;

		private static readonly Currency Rub = Currency.GetInstance("RUB");

		public void InitUi()
		{
			Settings settings = new Settings(this);

			HashSet<PaymentMethodType> paymentMethodTypes = GetPaymentMethodTypes(settings);

			BigDecimal amount = new BigDecimal(new BigInteger("11"));
			PaymentParameters paymentParameters = new PaymentParameters(
				new Amount(amount, Rub),
				"prod_name",
				"prod_desc",
				"test_NjM5MDYw4_MI8X9BbkIMK20BqJ84Iw4gLyeWnXJrqrk",
				"639060", 
				paymentMethodTypes
				);

			UiParameters uiParameters = new UiParameters(
				settings.ShowYandexCheckoutLogo, new ColorScheme(settings.GetPrimaryColor));

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

			TestParameters testParameters = new TestParameters(true, false, mockConfiguration);
			var intent = Checkout.CreateTokenizeIntent(this,
														  paymentParameters,
														  testParameters,
														  uiParameters
			);

			StartActivityForResult(intent, RequestCodeTokenize);
		}

		private static HashSet<PaymentMethodType> GetPaymentMethodTypes(Settings settings)
		{
			HashSet<PaymentMethodType> paymentMethodTypes = new HashSet<PaymentMethodType>();

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

		/// <param name="requestCode">The integer request code originally supplied to
		/// startActivityForResult(), allowing you to identify who this
		/// result came from.</param>
		/// <param name="resultCode">The integer result code returned by the child activity
		/// through its setResult().</param>
		/// <param name="data">An Intent, which can return result data to the caller
		/// (various data can be attached to Intent "extras").</param>
		/// <summary>Called when an activity you launched exits, giving you the requestCode
		/// you started it with, the resultCode it returned, and any additional
		/// data from it.</summary>
		/// <remarks>
		///                <para tool="javadoc-to-mdoc">Called when an activity you launched exits, giving you the requestCode
		/// you started it with, the resultCode it returned, and any additional
		/// data from it.  The <format type="text/html"><var>resultCode</var></format> will be
		/// <c><see cref="F:Android.App.Result.Canceled" tool="ReplaceLinkValue" /></c> if the activity explicitly returned that,
		/// didn't return any result, or crashed during its operation.
		/// </para>
		///                <para tool="javadoc-to-mdoc">You will receive this call immediately before onResume() when your
		/// activity is re-starting.
		/// </para>
		///                <para tool="javadoc-to-mdoc">This method is never invoked if your activity sets
		/// <c><see cref="!:NoType:android/R$styleable;Href=../../../reference/android/R.styleable.html#AndroidManifestActivity_noHistory" /></c> to
		/// <c>true</c>.</para>
		///                <para tool="javadoc-to-mdoc">
		///                    <format type="text/html">
		///                        <a href="http://developer.android.com/reference/android/app/Activity.html#onActivityResult(int, int, android.content.Intent)" target="_blank">[Android Documentation]</a>
		///                    </format>
		///                </para>
		///            </remarks>
		/// <since version="Added in API level 1" />
		/// <altmember cref="M:Android.App.Activity.StartActivityForResult(Android.Content.Intent, System.Int32)" />
		/// <altmember cref="M:Android.App.Activity.CreatePendingResult(System.Int32, Android.Content.Intent, Android.Content.Intent)" />
		/// <altmember cref="M:Android.App.Activity.SetResult(Android.App.Result)" />
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == RequestCodeTokenize)
			{
				TokenizationResult result = Checkout.CreateTokenizationResult(data);
				switch (resultCode)
				{
					case Result.Ok:

						// successful tokenization
						var token = result.PaymentToken;
						var type = result.PaymentMethodType;

						SendToken(token, new UserToken
						{
							Token = (string)App.User.UserToken.Token.Clone()
						});

						break;
					case Result.Canceled:

						// user canceled tokenization
						Toast.MakeText(this, "Tokenization canceled", ToastLength.Short).Show();
						break;
				}
			}
		}

		private const string SendTokenUri = "http://lk.asb-security.ru/api/client/setActivityFrom";

		private async void SendToken(string token, UserToken userToken)
		{
			using (var client = new HttpClient(new AndroidClientHandler()))
			{
				var oldActivityTo = AboutViewModel.Instance.ActiveTo;
				AboutViewModel.Instance.IsShowedIndicator = true;
				AboutViewModel.Instance.IsShowedActivityTitle = false;
				AboutViewModel.Instance.ActiveTo = "";

				client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"{userToken.TokenType} {userToken.Token}");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var data = new Dictionary<string, string>
				{
					{
						"payment_token", token
					}
				};

				var response = await client.PostAsync(SendTokenUri, new FormUrlEncodedContent(data));
				var jsonString = await response.Content.ReadAsStringAsync();
				System.Diagnostics.Debug.WriteLine(jsonString);
				if (response.IsSuccessStatusCode)
				{
					if (jsonString != null)
					{
						var jsonData = JsonConvert.DeserializeObject<JsonDataResponse<Payment>>(jsonString);
						var con = RealmConfiguration.DefaultConfiguration;
						con.SchemaVersion = 7;
						var realm = Realm.GetInstance(con);

						if (jsonData.Data.IsActive)
						{
							new AlertDialog.Builder(this)
								.SetMessage("Подписка оплачена.")
								.SetNegativeButton("Отмена", (dialog, which) => { }).Show();
						}

						using (realm)
						{
							var user = App.User;
							using (var transaction = realm.BeginWrite())
							{
								user.IsActive = jsonData.Data.IsActive;
								user.ActiveFrom = jsonData.Data.ActiveFrom;
								transaction.Commit();
							}
						}
						AboutViewModel.Instance.ActiveTo = jsonData.Data.ActiveFrom.Add(new TimeSpan(30, 3, 0, 0)).ToString("dd.MM.yyyy hh:mm");
					}
				}
				else
				{
					AboutViewModel.Instance.ActiveTo = oldActivityTo;
					new AlertDialog.Builder(this)
						.SetMessage("Возникла ошибка при оплате.")
						.SetNegativeButton("Отмена", (dialog, which) => { }).Show();
				}

				AboutViewModel.Instance.IsShowedActivityTitle = true;
				AboutViewModel.Instance.IsShowedIndicator = false;
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
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted
			    || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
			{
				CheckPermission(new[]
				{
					Manifest.Permission.AccessFineLocation,
					Manifest.Permission.AccessCoarseLocation,
					Manifest.Permission.ReadExternalStorage,
					Manifest.Permission.WriteExternalStorage

				}, PermissionsRequestAccessFineLocation);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private void CheckPermission(string[] permissions, int permissionsRequestCode)
		{
			ActivityCompat.RequestPermissions(this,
											  permissions,
											  permissionsRequestCode);
		}
		#endregion
	}
}
