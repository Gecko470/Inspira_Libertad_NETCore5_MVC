using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Inspira_Libertad.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public float ImporteTotal { get; set; } = 0;
        public int Status { get; set; } = 0;
        public List<OrderItem> OrderItems { get; set; }
    }
}
