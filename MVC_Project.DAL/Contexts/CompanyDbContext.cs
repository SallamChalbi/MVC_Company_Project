using Microsoft.EntityFrameworkCore;
using MVC_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.DAL.Contexts
{
    public class CompanyDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server = .; Database = CompanyMVCProject; Trusted_Connection = true");

        public DbSet<Department> Departments { get; set; }
    }
}
