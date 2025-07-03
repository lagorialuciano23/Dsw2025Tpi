using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dsw2025Tpi.Application.Dtos.OrderModel;

namespace Dsw2025Tpi.Application.Services
{
    public class OrdersManagment : IOrdersManagementService
    {
        private readonly IRepository _repository;
        public OrdersManagment(IRepository repository)
        {
            _repository = repository;
        }
        //ADD ORDER
        public async Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request)
        {
            //Ver si el cliente existe
            var customer = await _repository.GetById<Customer>(request.CustomerId);
            if (customer == null)
                throw new EntityNotFoundException($"Cliente no encontrado.");

            //Validar que no este vacio
            if (request.CustomerId == Guid.Empty ||
                string.IsNullOrWhiteSpace(request.ShippingAddress) ||
                string.IsNullOrWhiteSpace(request.BillingAddress))

            {
                throw new ArgumentException("Ingrese dirección de envio y/o facturación");
            }

            // Validar que OrdersItems lista no este vacio
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                throw new ArgumentException("La orden debe tener al menos un producto.");

            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);

                if (product == null)
                    throw new EntityNotFoundException($"Producto con ID {item.ProductId} no encontrado.");

                if (!product.IsActive)
                    throw new ArgumentException("Producto no disponible, campo IsActive = false.");

                if (item.UnitPrice != product.CurrentUnitPrice)
                    throw new ArgumentException("Precio de producto no coincidente.");

                if (item.Description != product.Description || item.Name != product.Name)
                    throw new ArgumentException("Datos de descripción o nombre no coincidentes.");

                if (product.StockQuantity < item.Quantity)
                    throw new ArgumentException($"No hay suficiente stock para el producto {product.Name}.");

                if (item.Quantity <= 0)
                    throw new ArgumentException($"La cantidad del producto {product.Name} debe ser mayor a 0.");

                if (item.UnitPrice <= 0)
                    throw new ArgumentException($"El precio del producto {product.Name} debe ser mayor a 0.");

                //SI está bien, restamos stock y actualizamos
                product.SubtractStock(item.Quantity);
                await _repository.Update(product);
            }

            //Iniciar una orden de items

            var orderItems = new List<OrderItem>();

            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                var orderItem = new OrderItem(
                                product.Id,
                                product,
                                item.Quantity,
                                product.CurrentUnitPrice,
                                item.Name,
                                item.Description
                                 );

                orderItems.Add(orderItem);
            }
            // Creamos la orden
            var order = new Order(customer.Id, request.ShippingAddress, request.BillingAddress,
                orderItems, DateTime.UtcNow, OrderStatus.Pending);

            var added = await _repository.Add(order);

            return new OrderModel.OrderResponse(
                added.CustomerId,
                added.ShippingAddress,
                added.BillingAddress,
                added.CreatedAt,
                added.TotalAmount,
                added.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                    oi.ProductId,
                    oi.Quantity,
                    oi.Name,
                    oi.Description,
                    oi.UnitPrice,
                    oi.Subtotal)).ToList(),
                added.Status.ToString());

        }

        //GET ORDERS
        public async Task<List<OrderResponse>> GetAllAsync(OrderFilterRequest filter)
        {
            var query = _repository.Query<Order>()
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<OrderStatus>(filter.Status, true, out var parsedStatus))
            {
                query = query.Where(o => o.Status == parsedStatus);
            }

            if (filter.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filter.CustomerId.Value);

            var orders = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return orders.Select(o => new OrderResponse(
    
    o.CustomerId,
    o.ShippingAddress,
    o.BillingAddress,
    o.CreatedAt,
    o.TotalAmount,
    o.OrderItems.Select(oi => new OrderItemResponse(
        oi.ProductId,
        oi.Quantity,
        oi.Name,
        oi.Description,
        oi.UnitPrice,
        oi.Subtotal)).ToList(),
    o.Status.ToString()
)).ToList();
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _repository.Query<Order>()
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new EntityNotFoundException($"Orden con ID {id} no encontrada.");

            return new OrderResponse(
                order.Id,
                order.ShippingAddress,
                order.BillingAddress,
                order.CreatedAt,
                order.TotalAmount,
                order.OrderItems.Select(oi => new OrderItemResponse(
        oi.ProductId,
        oi.Quantity,
        oi.Name,
        oi.Description,
        oi.UnitPrice,
        oi.Subtotal)).ToList(),
             order.Status.ToString()
            );
        }

    }
}
