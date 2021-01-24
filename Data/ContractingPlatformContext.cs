using Microsoft.EntityFrameworkCore;
using ContractingPlatform.Models;

namespace ContractingPlatform.Data
{
    public class ContractingPlatformContext : DbContext
    {
        public ContractingPlatformContext (DbContextOptions<ContractingPlatformContext> options)
            : base(options)
        {
        }

        public DbSet<Carrier> Carrier { get; set; }
        public DbSet<MGA> MGA { get; set; }
        public DbSet<Advisor> Advisor { get; set; }
        public DbSet<Contract> Contract { get; set; }
    }
}