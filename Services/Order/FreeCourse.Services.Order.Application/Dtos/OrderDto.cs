using FreeCourse.Services.Order.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Dtos
{
    public class OrderDto
    {

        public int Id { get; set; }
        public DateTime CreatedDate { get; private set; }


        //veri tabınındaorder tablosunda adres biilgilerini kolon olarak eklenmesi istemiyorsak owned dememiz gerek 
        public Address Address { get; private set; }

        public string BuyerId { get; private set; }
        public List<OrderItemDto> OrderItems { get; set; }



    }
}
