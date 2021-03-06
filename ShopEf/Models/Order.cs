﻿using System;

namespace ShopEf.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }

        public Order()
        {
        }

        public Order(int customerId, int productId, int count)
        {
            CustomerId = customerId;
            ProductId = productId;
            Count = count;
            OrderDate = DateTime.UtcNow;
        }
    }
}