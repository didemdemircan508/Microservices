using FreeCourse.Web.Models.Payments;

namespace FreeCourse.Web.Services.Interface
{
    public interface IPaymentService
    {

        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);

    }
}
