using System;
using Android.Content;
using Android.Runtime;
using Java.Interop;
using Object = Java.Lang.Object;

namespace RU.Yandex.Money.Android.Sdk
{
	public sealed partial class Checkout : global::Java.Lang.Object
	{
		public static unsafe TokenizationResult CreateTokenizationResult(global::Android.Content.Intent p0)
		{
			const string id = "createTokenizationResult.(Landroid/content/Intent;)Lru/yandex/money/android/sdk/TokenizationResult;";
			JniArgumentValue* args = stackalloc JniArgumentValue[1];
			args[0] = new JniArgumentValue(p0?.Handle ?? IntPtr.Zero);
			JniObjectReference rm = default;
			try
			{
				
				rm = _members.StaticMethods.InvokeObjectMethod(id, args);
			}
			catch (Java.Lang.IllegalArgumentException e)
			{
				Console.WriteLine(e);
				//throw;
			}
			return GetObject<TokenizationResult>(rm.Handle, JniHandleOwnership.TransferLocalRef);
		}
	}
}
