using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Consumer
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly RedisService _redisService;

        public CourseNameChangeEventConsumer(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async  Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var cursor = 0;
            var pattern = "*";
            const int pageSize = 10;
            var redisKeys = _redisService.GetDb().Execute(pattern, context);


            var test = context.Message.CourseId;
            var basketItems =await _redisService.GetDb().StringGetAsync(context.Message.CourseId);
           




            //orderItems.ForEach(x =>
            //{
            //    x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
            //});

            //await _orderDbContext.SaveChangesAsync();
        }
    }
}
