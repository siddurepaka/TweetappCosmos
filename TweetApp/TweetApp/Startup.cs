using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TweetApp.Service;
using TweetApp.Service.TweetAppEntity;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace TweetApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITweetCosmosService>(InitializeCosmosClientInstanceAsyncforTweet(Configuration.GetSection("CosmosDb1")).GetAwaiter().GetResult());
            services.AddSingleton<ITweetCosmoDBService>(InitializeCosmosClientInstanceAsyncforUser(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());         
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Tweet",
new OpenApiInfo { Title = "TweetApp", Version = "Tweet" });
            });
            services.AddControllers().AddNewtonsoftJson();

            services.AddCors(options => {
                options.AddPolicy("MODPolicy", builder => {
                    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/Tweet/swagger.json", "TweetApp"); });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("MODPolicy");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               
            });
        }

        private static async Task<TweetCosmoDbService> InitializeCosmosClientInstanceAsyncforUser(IConfigurationSection configurationSection)
       {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName1"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new TweetCosmoDbService(client, databaseName, containerName);
            return cosmosDbService;
        }

        private static async Task<TweetCosmosService> InitializeCosmosClientInstanceAsyncforTweet(IConfigurationSection configurationSection)
        {
            
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName2"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new TweetCosmosService(client, databaseName, containerName);
            return cosmosDbService;
        }
    }
}
