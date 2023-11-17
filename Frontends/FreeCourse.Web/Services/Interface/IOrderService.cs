using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interface
{
    public interface IOrderService
    {
        //senkron--direk olarak istek yapılacak
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput input);
        //asekron-siparş bilgileri kuyruğa gönderilecek
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput input);

        Task<List<OrderViewModel>> GetOrder();
    }
}
