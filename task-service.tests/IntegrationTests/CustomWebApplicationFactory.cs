using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using task_service.DBContexts;
using Microsoft.EntityFrameworkCore.InMemory;
using task_service.Models;

namespace task_service.tests.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<TaskContext>));

            services.Remove(descriptor);

            services.AddDbContext<TaskContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TaskContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureCreated();

                try
                {
                    db.Tasks.Add(new TaskModel { Id = 1, ListId = 1, Name = "Task 1", Description = "Description 1" });
                    db.Tasks.Add(new TaskModel { Id = 2, ListId = 1, Name = "Task 2", Description = "Description 2" });
                    db.Tasks.Add(new TaskModel { Id = 3, ListId = 2, Name = "Task 3", Description = "Description 3" });
                    db.Tasks.Add(new TaskModel { Id = 4, ListId = 2, Name = "Task 4", Description = "Description 4" });
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }
}