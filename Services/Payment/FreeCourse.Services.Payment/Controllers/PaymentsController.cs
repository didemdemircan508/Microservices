using FreeCourse.Services.Payment.Model;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {

        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]

        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {

            //ödeme başarılıysa
            var sendEndpoint =await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));
          //  var sendMailEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:mail-order-service"));
            var createOrderMessageCommand = new CreateOrderMessageCommand();

            createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
            createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
            createOrderMessageCommand.District = paymentDto.Order.Address.District;
            createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
            createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
            createOrderMessageCommand.ZipCode=paymentDto.Order.Address.ZipCode;
            paymentDto.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = x.PictureUrl,
                    Price= x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName


                });


            });

           // var sendMailMessageCommand=new SendMailMessageCommand();


            await   sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
            //await sendMailEndpoint.Send<SendMailMessageCommand>(sendMailMessageCommand);


            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
        
        
        }
    }
}
