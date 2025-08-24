using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsw2025Tpi.Application.Dtos.OrderModel;

namespace Dsw2025Tpi.Application.Services
{
    public interface IOrdersManagementService
    {
        Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request);
        Task<List<OrderResponse>> GetAllAsync(OrderFilterRequest filter);
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<OrderResponse> UpdateStatusAsync(Guid id, string newStatus);

    }
}
