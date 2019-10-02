
using Android.Content;
using Android.Graphics;
using Android.Preferences;

namespace itmit.asb.app.Droid
{
	/// <summary>
	/// Представляет настройки для Яндекс кассы.
	/// </summary>
	public class Settings
	{

		private const string KeyLinkedCardsCount = "linked_cards_count";
		private const string KeyPrimaryColorRedValue = "primary_color_red_value";
		private const string KeyPrimaryColorGreenValue = "primary_color_green_value";
	    private const string KeyPrimaryColorBlueValue = "primary_color_blue_value";
	    private const string KeyYandexMoneyAllowed = "yandex_money_allowed";
	    private const string KeySberbankOnlineAllowed = "sberbank_online_allowed";
	    private const string KeyGooglePayAllowed = "google_pay_allowed";
	    private const string KeyNewCardAllowed = "new_card_allowed";
	    private const string KeyShowYandexCheckoutLogo = "show_yandex_checkout_logo";
	    private const string KeyAutoFillUserPhoneNumber = "autofill_user_phone_number";
	    private const string KeyTestModeEnabled = "test_mode_enabled";
	    private const string KeyPaymentAuthPassed = "payment_auth_passed";
	    private const string KeyServiceFee = "fee";
	    private const string KeyShouldCompletePaymentWithError = "should_complete_with_error";

		private readonly ISharedPreferences _sp;

		public Settings(Context context)
		{
			_sp = PreferenceManager.GetDefaultSharedPreferences(context);
		}

		public bool IsYandexMoneyAllowed => _sp.GetBoolean(KeyYandexMoneyAllowed, false);

		public bool IsSberbankOnlineAllowed => _sp.GetBoolean(KeySberbankOnlineAllowed, true);

		public bool IsGooglePayAllowed => _sp.GetBoolean(KeyGooglePayAllowed, true);

		public bool IsNewCardAllowed => _sp.GetBoolean(KeyNewCardAllowed, true);

		public bool ShowYandexCheckoutLogo => _sp.GetBoolean(KeyShowYandexCheckoutLogo, true);

		public bool AutoFillUserPhoneNumber => _sp.GetBoolean(KeyAutoFillUserPhoneNumber, false);

		public bool IsTestModeEnabled => _sp.GetBoolean(KeyTestModeEnabled, false);

		public bool IsPaymentAuthPassed => _sp.GetBoolean(KeyPaymentAuthPassed, false);

		public float GetServiceFee => _sp.GetFloat(KeyServiceFee, 0f);

		public int GetLinkedCardsCount => _sp.GetInt(KeyLinkedCardsCount, 1);

		public int GetPrimaryColor =>
			Color.Rgb(
				_sp.GetInt(KeyPrimaryColorRedValue, 0),
				_sp.GetInt(KeyPrimaryColorGreenValue, 114),
				_sp.GetInt(KeyPrimaryColorBlueValue, 245)
			);

		public bool ShouldCompletePaymentWithError => _sp.GetBoolean(KeyShouldCompletePaymentWithError, false);
	}
}
