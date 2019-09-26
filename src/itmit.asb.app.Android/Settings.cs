
using Android.Content;
using Android.Graphics;
using Android.Preferences;

namespace itmit.asb.app.Droid
{
	public class Settings
	{

		static string KEY_LINKED_CARDS_COUNT = "linked_cards_count";
	    static string KEY_PRIMARY_COLOR_RED_VALUE = "primary_color_red_value";
	    static string KEY_PRIMARY_COLOR_GREEN_VALUE = "primary_color_green_value";
	    static string KEY_PRIMARY_COLOR_BLUE_VALUE = "primary_color_blue_value";
	    static string KEY_YANDEX_MONEY_ALLOWED = "yandex_money_allowed";
	    static string KEY_SBERBANK_ONLINE_ALLOWED = "sberbank_online_allowed";
	    static string KEY_GOOGLE_PAY_ALLOWED = "google_pay_allowed";
	    static string KEY_NEW_CARD_ALLOWED = "new_card_allowed";
	    static string KEY_SHOW_YANDEX_CHECKOUT_LOGO = "show_yandex_checkout_logo";
	    static string KEY_AUTOFILL_USER_PHONE_NUMBER = "autofill_user_phone_number";
	    static string KEY_TEST_MODE_ENABLED = "test_mode_enabled";
	    static string KEY_PAYMENT_AUTH_PASSED = "payment_auth_passed";
	    static string KEY_SERVICE_FEE = "fee";
	    static string KEY_SHOULD_COMPLETE_PAYMENT_WITH_ERROR = "should_complete_with_error";

		private ISharedPreferences sp;

		public Settings(Context context)
		{
			sp = PreferenceManager.GetDefaultSharedPreferences(context);
		}

		public bool IsYandexMoneyAllowed()
		{
			return sp.GetBoolean(KEY_YANDEX_MONEY_ALLOWED, false);
		}

		public bool IsSberbankOnlineAllowed()
		{
			return sp.GetBoolean(KEY_SBERBANK_ONLINE_ALLOWED, true);
		}

		public bool IsGooglePayAllowed()
		{
			return sp.GetBoolean(KEY_GOOGLE_PAY_ALLOWED, true);
		}

		public bool IsNewCardAllowed()
		{
			return sp.GetBoolean(KEY_NEW_CARD_ALLOWED, true);
		}

		public bool ShowYandexCheckoutLogo()
		{
			return sp.GetBoolean(KEY_SHOW_YANDEX_CHECKOUT_LOGO, true);
		}

		public bool AutofillUserPhoneNumber()
		{
			return sp.GetBoolean(KEY_AUTOFILL_USER_PHONE_NUMBER, false);
		}

		public bool IsTestModeEnabled()
		{
			return sp.GetBoolean(KEY_TEST_MODE_ENABLED, true);
		}

		public bool IsPaymentAuthPassed()
		{
			return sp.GetBoolean(KEY_PAYMENT_AUTH_PASSED, false);
		}

		public float GetServiceFee()
		{
			return sp.GetFloat(KEY_SERVICE_FEE, 0f);
		}

		public int GetLinkedCardsCount()
		{
			return sp.GetInt(KEY_LINKED_CARDS_COUNT, 1);
		}

		public int GetPrimaryColor()
		{
			return Color.Rgb(
					sp.GetInt(KEY_PRIMARY_COLOR_RED_VALUE, 0),
					sp.GetInt(KEY_PRIMARY_COLOR_GREEN_VALUE, 114),
					sp.GetInt(KEY_PRIMARY_COLOR_BLUE_VALUE, 245)
			);
		}

		public bool ShouldCompletePaymentWithError()
		{
			return sp.GetBoolean(KEY_SHOULD_COMPLETE_PAYMENT_WITH_ERROR, false);
		}
	}
}
