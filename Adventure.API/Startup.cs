using System;
using System.IO;
using Adventure.API.DataAccess.Repositories;
using Adventure.API.Provider;
using Adventure.API.Provider.Contracts;
using Adventure.API.System;
using Adventure.DataAccessLayer.DBContexts;
using Adventure.DataAccessLayer.Repositories;
using Adventure.Provider;
using Adventure.Provider.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace Adventure.API
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
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            });

            // Providers
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IQuestionRouteProvider, QuestionRouteProvider>();
            services.AddScoped<IAdventureProvider, AdventureProvider>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQuestionRouteRepository, QuestionRouteRepository>();
            services.AddScoped<IAdventureRepository, AdventureRepository>();
            services.AddScoped<IUserQuestionRoutesRepository, UserQuestionRoutesRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1", Title = "AssessmentAPI", Description = "Assesment Api" });

            });
            services.AddDbContext<AdventureContext>(options =>
            {
                var server = Configuration["ServerName"];
                var port = "1433";
                var database = Configuration["Database"];
                var user = Configuration["UserName"];
                var password = Configuration["Password"];
                Console.WriteLine(server);
                Console.WriteLine(database);
                Console.WriteLine(user);
                user = "sa";
                Console.WriteLine(password);

                options.UseSqlServer(
                    $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}",
                    sqlServer => sqlServer.MigrationsAssembly("Adventure.API"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web experience api");
                c.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
