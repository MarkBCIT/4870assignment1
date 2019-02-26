using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using assignment.Data;
using assignment.ModelViews;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 6;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireLowercase = false;
                }
            ).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    });









            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Site"],
                    ValidIssuer = Configuration["Jwt:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            DummyData.Initialize(app);
            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var u = new ApplicationUser
            {
                UserName = "a",
                Email = "a@a.a",
                FirstName = "afirst",
                LastName = "alast",
                Country = "CA",
                MobileNumber = "1-234-567-8901"

            };
            var u2 = new ApplicationUser
            {
                UserName = "m",
                Email = "m@m.m",
                FirstName = "mfirst",
                LastName = "mlast",
                Country = "CA",
                MobileNumber = "0-987-654-3210"
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = "P@$$w0rd";

            await UserManager.CreateAsync(u, userPWD);
            await UserManager.CreateAsync(u2, userPWD);

            await UserManager.AddToRoleAsync(u, "Admin");
            await UserManager.AddToRoleAsync(u2, "Member");



            //var hasher = new PasswordHasher<IdentityUser>();
            //var u = new IdentityUser()
            //{
            //    UserName = "a",
            //    NormalizedUserName = "A",
            //    Email = "a@a.a",
            //    NormalizedEmail = "A@A.A",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //};
            //var u2 = new IdentityUser()
            //{
            //    UserName = "m",
            //    NormalizedUserName = "M",
            //    Email = "m@m.m",
            //    NormalizedEmail = "M@M.M",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //};

            //var pw = new PasswordHasher<IdentityUser>();
            //var hashed = pw.HashPassword(u, "P@$$w0rd");
            //u.PasswordHash = hashed;
            //var hashed2 = pw.HashPassword(u2, "P@$$w0rd");
            //u2.PasswordHash = hashed2;



        }
    }

}
