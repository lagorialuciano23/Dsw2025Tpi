using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public interface IProductsManagementService
    {
        Task<ProductModel.ProductResponse> AddProduct(ProductModel.ProductRequest request);
        Task<ProductModel.ProductResponseUpdate>? GetProductById(Guid id);
        Task<List<ProductModel.ProductResponseUpdate>?> GetProducts();
        Task<bool> DisableProductAsync(Guid id);
        Task<ProductModel.ProductResponseUpdate> UpdateAsync(ProductModel.ProductRequest request, Guid id);
    }
}
