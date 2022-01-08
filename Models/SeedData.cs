
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AppDelBagno.Data;
using System;
using System.Linq;

namespace AppDelBagno.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDelBagnoContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDelBagnoContext>>()))
            {
                // Look for any movies.
                if (context.Bagno.Any())
                {
                    return;   // DB has been seeded
                }

                context.Bagno.AddRange(
                    new Bagno
                    {
                        Utente = "Mirko",
                        Entrata = DateTime.Today
                    }

                    //new Bagno
                    //{
                    //    Title = "Ghostbusters ",
                    //    ReleaseDate = DateTime.Parse("1984-3-13"),
                    //    Genre = "Comedy",
                    //    Price = 8.99M
                    //},

                    //new Bagno
                    //{
                    //    Title = "Ghostbusters 2",
                    //    ReleaseDate = DateTime.Parse("1986-2-23"),
                    //    Genre = "Comedy",
                    //    Price = 9.99M
                    //},

                    //new Bagno
                    //{
                    //    Title = "Rio Bravo",
                    //    ReleaseDate = DateTime.Parse("1959-4-15"),
                    //    Genre = "Western",
                    //    Price = 3.99M
                    //}
                    );
                context.SaveChanges();
            }
        }
    }
}