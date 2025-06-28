using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem() { }
        public OrderItem(Guid productId, Product product, int quantity, decimal currentUnitPrice)
        {
            ProductId = productId;
            Product = product;
            Quantity = quantity;
            UnitPrice = currentUnitPrice;
        }

        public Guid ProductId { get; set; }   //Foreign Key Order
        public Product? Product { get; set; }

        public Guid OrderId { get; set; }   //Foreign Key Order
        public Order? Order { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal Subtotal => UnitPrice * Quantity;
    }
}
