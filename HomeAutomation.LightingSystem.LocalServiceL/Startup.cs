using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.Extensions;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HomeAutomation.LightingSystem.LocalServiceL
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
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.Configure<Configuration>(Configuration.GetSection("Configuration"));
            services.AddSingleton<ISignalRClient, SignalRClient>();
            services.AddSingleton<IHomeAutomationMqttServer, Mqtt.MqttServer>();
            services.AddScoped<IRestClient, RestClient>();
            //services.AddScoped<IMqttServerClientDisconnectedHandler, ClientDisconnectedHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ISignalRClient signalRClient,
            IRestClient restClient,
            IHomeAutomationMqttServer homeAutomationMqttServer,
            IMediator mediator,
            IOptions<Configuration> config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await app.RunSignalRClientAsync(config, signalRClient, restClient);

            await app.RunMqttServerAsync(
                homeAutomationMqttServer,
                restClient,
                mediator);
        }
    }
}
