using BusinessLogic.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public IGenericRepository<Category> Categories { get; }
        public IGenericRepository<Product> Products { get; } // ← Add this

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
            Categories = new GenericRepository<Category>(_context);
            Products = new GenericRepository<Product>(_context); 
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
