using Business.Interfaces;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ApplicationDbContext db;

        public UserRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async void Create(User item)
        {
            await db.Users.AddAsync(item);
        }

        public async void Delete(int id)
        {
            db.Users.Remove(await db.Users.FindAsync(id));
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

        public async Task<User> Get(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public IEnumerable<User> GetList()
        {
            return db.Users;
        }

        public async Task<int> Save()
        {
            return await db.SaveChangesAsync());
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        User IRepository<User>.Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
