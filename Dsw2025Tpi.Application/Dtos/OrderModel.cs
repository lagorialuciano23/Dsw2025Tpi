using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        //REQUESTS
        public record OrderRequest(string ShippingAddress, string BillingAddress, Guid CustomerId, List<OrderItemModel> OrderItems);
        public record OrderItemModel(Guid ProductId, int Quantity, string Name, string Description, decimal UnitPrice);

        //RESPONSES
        public record OrderResponse(Guid Id,
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            DateTime Date,
            decimal TotalAmount,
            List<OrderItemResponse> OrderItems,
            string Status);
        public record OrderItemResponse(Guid ProductId, string Name, string Description, decimal UnitPriceint, int Quantity, decimal Subtotal);
    }
}
