using assignment.ModelViews;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace assignment.Data
{
    public class DummyData
    {

        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                //context.Database.Migrate();

                // Look for any teams.
                if (context.Boats.Any())
                {
                    return;   // DB has already been seeded
                }

                var boats = DummyData.GetBoats().ToArray();
                context.Boats.AddRange(boats);
                context.SaveChanges();


            }
        }



        public static List<Boat> GetBoats()
        {
            List<Boat> boats = new List<Boat>() {
            new Boat {
            BoatName = "boat1",
            Picture = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
            LengthInFeet = 100,
            Make = "Factory1",
            Description = "This is first boat."
            },
            new Boat {
            BoatName = "boat2",
            Picture = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
            LengthInFeet = 200,
            Make = "Factory2",
            Description = "This is second boat."
            },
            new Boat {
            BoatName = "boat3",
            Picture = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
            LengthInFeet = 300,
            Make = "Factory3",
            Description = "This is third boat."
            },
            new Boat {
            BoatName = "boat4",
            Picture = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
            LengthInFeet = 400,
            Make = "Factory4",
            Description = "This is fourth boat."
            },
            new Boat {
            BoatName = "boat5",
            Picture = "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png",
            LengthInFeet = 500,
            Make = "Factory5",
            Description = "This is fifth boat."
            },
        };

            return boats;
        }
    }
}
