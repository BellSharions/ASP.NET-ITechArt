using Business.Interfaces;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private ApplicationDbContext db;

        public ProductRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async void Create(Product item)
        {
            await db.Products.AddAsync(item);
        }

        public async void Delete(int id)
        {
            db.Products.Remove(await db.Products.FindAsync(id));
        }

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public async Task<Product> Get(int id)
        {
            return await db.Products.FindAsync(id);
        }

        public IEnumerable<Product> GetList()
        {
            return db.Products;
        }

        public async Task<int> Save()
        {
            return await db.SaveChangesAsync();
        }

        public void Update(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        Product IRepository<Product>.Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
