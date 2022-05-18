using ChurchAdmin.Models;
using Microsoft.EntityFrameworkCore;
using Schedular.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchAdmin.Context
{
    public class SchedularDbContext : DbContext
    {
        public SchedularDbContext(DbContextOptions<SchedularDbContext> options) : base(options)
        {

        }
        public DbSet<Users> User { get; set; }
        public DbSet<Virtual> Virtual { get; set; }


    }

}
