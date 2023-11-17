using FreeCourse.Web.Models.Payments;
using FreeCourse.Web.Services.Interface;

namespace FreeCourse.Web.Services
{
    public class PaymentService : IPaymentService
    {
        HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("payments", paymentInfoInput);
            return response.IsSuccessStatusCode;
        }
    }
}
