using System;
using ObjCRuntime;

[assembly: LinkWith ("libYandexCheckout.a", LinkTarget.Simulator, ForceLoad = true)]
