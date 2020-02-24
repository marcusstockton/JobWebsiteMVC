using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace JobWebsiteMVC.Services
{
    public class UserTypesService : IUserTypesService
    {
        private ApplicationDbContext _context;
        public UserTypesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserType>> GetUserTypes()
        {
            return await _context.UserTypes.ToListAsync();
        }

    }
}
