﻿using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync() => await _context.Authors.Include(m=>m.Blogs). 
                                                                                       ToListAsync();
    }
}
