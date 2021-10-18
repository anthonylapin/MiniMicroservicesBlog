using System;
using HttpClients;
using HttpClients.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using QueryService.Data;
using QueryService.Util;

namespace QueryService
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
            var frontendBaseUrl = Configuration.GetSection("FrontendOptions").GetValue<string>("BaseUrl");

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(p => p
                    .WithOrigins(frontendBaseUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddEventBusClient(Configuration.GetSection("EventBusClientOptions").Bind);

            services.AddSingleton<IDataContext, DataContext>();

            services.AddSingleton<IEventHandler, Util.EventHandler>();

            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "QueryService", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QueryService v1"));
            }

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            GetMissedEvents(app);
        }

        private void GetMissedEvents(IApplicationBuilder app)
        {
            var eventBusClient = app.ApplicationServices.GetService<IEventBusClient>();

            if (eventBusClient == null)
            {
                throw new InvalidProgramException("Event bus client is not registered as a service");
            }

            Console.WriteLine("--> Attempting to get missing events from event bus");

            var events = eventBusClient.GetEvents().GetAwaiter().GetResult();

            Console.WriteLine($"--> {events.Count} events were received from event bus");
            
            var eventHandler = app.ApplicationServices.GetService<IEventHandler>();

            if (eventHandler == null)
            {
                throw new InvalidProgramException("Event handler is not registered as a service");
            }

            foreach (var eventModel in events)
            {
                eventHandler.HandleEvent(eventModel);   
            }

            Console.WriteLine("--> Missed events were processed");
        }
    }
}