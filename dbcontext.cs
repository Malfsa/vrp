﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }
   
        public DbSet<InputTable> InputTables { get; set; }
        public DbSet<ResultTable> ResultTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResultTable>()
                .HasOne(r => r.InputTable)
                .WithMany()
                .HasForeignKey(r => r.InputTableId);
        }
    }
}
