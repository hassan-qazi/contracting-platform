using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContractingPlatform.Data;
using System;
using System.Linq;

namespace ContractingPlatform.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ContractingPlatformContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ContractingPlatformContext>>()))
            {
                // Look for any movies.
                if (context.Carrier.Any())
                {
                    return;   // DB has been seeded
                }

                context.Carrier.AddRange(
                    new Carrier
                    {
                        Name = "Carrier A",
                        Address = "Address A",
                        Phone = "5195731987"
                    },

                    new Carrier
                    {
                        Name = "Carrier B",
                        Address = "Address B",
                        Phone = "5195731987"
                    },

                    new Carrier
                    {
                        Name = "Carrier C",
                        Address = "Address C",
                        Phone = "5195731987"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}