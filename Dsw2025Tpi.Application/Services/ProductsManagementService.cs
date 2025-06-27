using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsw2025Tpi.Application.Dtos.ProductModel;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService : IProductsManagementService
    {
        private readonly IRepository _repository;
        private readonly Dsw2025TpiContext _context;

        public ProductsManagementService(Dsw2025TpiContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }


        //Add Product with validations
        public async Task<ProductModel.ProductResponse> AddProduct(ProductModel.ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku) ||
                string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                string.IsNullOrWhiteSpace(request.Name) ||
                request.CurrentUnitPrice < 0 ||
                request.StockQuantity < 0
                )
            {
                throw new ArgumentException("Valores para el producto no válidos");
            }

            var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

            var product = new Product(request.Sku,
    request.InternalCode,
    request.Name,
    request.Description,
    request.CurrentUnitPrice,
    request.StockQuantity);
            await _repository.Add(product);
            return new ProductModel.ProductResponse(product.Id, product.Sku, product.InternalCode, product.Name, product.Description, product.CurrentUnitPrice, product.StockQuantity);
        }

        public async Task<bool> DisableProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null || !product.IsActive)
                return false;

            product.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductModel.ProductResponseUpdate>? GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null) return null;

            return new ProductModel.ProductResponseUpdate(
                product.Id,
                product.Sku,
                product.InternalCode,
                product.Name,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive
            );
        }

        public async Task<List<ProductModel.ProductResponseUpdate>?> GetProducts()
        {
            var products = await _repository.GetAll<Product>();
            return products.Where(p => p.IsActive == true).Select(p => new ProductModel.ProductResponseUpdate(
                p.Id,
                p.Sku,
                p.InternalCode,
                p.Name,
                p.Description,
                p.CurrentUnitPrice,
                p.StockQuantity,
                p.IsActive
            )).ToList();
        }

        public async Task<ProductModel.ProductResponseUpdate> UpdateAsync(ProductModel.ProductRequest request, Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null || !product.IsActive)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado o está inhabilitado.");

            if (string.IsNullOrWhiteSpace(request.Sku) ||
                string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                string.IsNullOrWhiteSpace(request.Name) ||
                request.CurrentUnitPrice <= 0 ||
                request.StockQuantity < 0)
            {
                throw new ArgumentException("Valores para el producto no válidos.");
            }

            // Verifica la unicidad de Sku (si se modificó)
            var existing = await _repository.First<Product>(p => p.Sku == request.Sku && p.Id != id);
            if (existing != null)
                throw new DuplicatedEntityException($"Ya existe otro producto con el SKU {request.Sku}");

            // Actualiza campos
            product.Sku = request.Sku;
            product.InternalCode = request.InternalCode;
            product.Name = request.Name;
            product.Description = request.Description;
            product.CurrentUnitPrice = request.CurrentUnitPrice;
            product.StockQuantity = request.StockQuantity;

            await _repository.Update(product);

            return new ProductModel.ProductResponseUpdate(
                product.Id,
                product.Sku,
                product.Name,
                product.InternalCode,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive
            );
        }
    }
}
