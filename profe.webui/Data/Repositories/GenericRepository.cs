using System;
using Microsoft.EntityFrameworkCore;
using profe.webui.Data.Context;
using profe.webui.Entities.Common;
using SendGrid.Helpers.Mail;

namespace profe.webui.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
            where T : class
    {
        public readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private DbSet<T> Table { get => _context.Set<T>(); }


        public List<T> GetAll()
        {
            return Table.ToList();
        }

        public async Task<T> GetById(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            await Table.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

        }

        public async Task  Delete(T entity)
        {
            Table.Remove(entity);
            _context.SaveChanges();

        }


        public async Task HardDeleteRangeAsync(IList<T> entity)
        {
            Table.RemoveRange(entity);
            _context.SaveChanges();

        }

        public async Task<T> UpdateAsync(T entity)
        {
            //Update işlemi senkron olduğu için, kendi asenkron metodumuzu oluşturuyoruz
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}

