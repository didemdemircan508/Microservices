using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using MassTransit;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService   _redisService;
       


        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
           

        }

        public async Task<Shared.Dtos.Response<bool>> Delete(string userId)
        {
           
            var status=await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? Shared.Dtos.Response<bool>.Success(204) : Shared.Dtos.Response<bool>.Fail("Basket not found ", 404);

        }

       

        public async Task<Shared.Dtos.Response<BasketDto>> GetBasket(string userId)
        {
          
            var existBasket =await _redisService.GetDb().StringGetAsync(userId);
            if (string.IsNullOrEmpty(existBasket))
            {
                return Shared.Dtos.Response<BasketDto>.Fail("Basket Not Found", 404);
            
            }

            return Shared.Dtos.Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
        }

        public async Task<Shared.Dtos.Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
          
            var status= await _redisService.GetDb().StringSetAsync(basketDto.UserId,JsonSerializer.Serialize(basketDto));

            return status ? Shared.Dtos.Response<bool>.Success(204) : Shared.Dtos.Response<bool>.Fail("Basket could not update or save ", 500);
        }
    }
}
