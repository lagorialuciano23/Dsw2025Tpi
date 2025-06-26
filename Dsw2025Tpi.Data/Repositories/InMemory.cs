using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Repositories
{
    public class InMemory : IRepository
    {
        private List<Product> _products;
        public InMemory() { LoadProducts(); }

        private void LoadProducts()
        {
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\products.json"));
            _products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        private List<T>? GetSet<T>() where T : EntityBase
        {
            if(typeof(T) == typeof(Product))
            {
                return _products as List<T>;
            }
            throw new NotSupportedException();
        }

        public Task<T> Add<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public async Task<T?> First<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            var product = GetSet<T>()?.FirstOrDefault(predicate.Compile());
            return await Task.FromResult(product);
        }

        public Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<T> Update<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            throw new NotImplementedException();
        }
    }
}