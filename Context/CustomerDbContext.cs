using ChurchAdmin.Models;
using Customer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchAdmin.Context
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {

        }
        public DbSet<User> Customers { get; set; }
        public DbSet<State> State { get; set; }

        public DbSet<Lga> Lga { get; set; }



    }

}
