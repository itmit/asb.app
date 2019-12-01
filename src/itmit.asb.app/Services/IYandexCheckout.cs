using System;
using System.Threading.Tasks;
using itmit.asb.app.Models;

namespace itmit.asb.app.Services
{
	public interface IYandexCheckout
	{
		void Buy();

		Task<string> GetPaymentStatus(string paymentToken, UserToken userToken);

		Task<bool> Activate(string paymentToken, UserToken userToken);

		Task<Uri> CreatePayment(string paymentToken, UserToken userToken);

		Task<Payment> CapturePayment(string paymentToken, UserToken userToken);
	}
}
