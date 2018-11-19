using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevolutionGolf.EventRequestBroker.Configuration;
using RevolutionGolf.EventRequestBroker.EventServices;
using RevolutionGolf.EventRequestBroker.Extensions;
using RevolutionGolf.EventRequestBroker.Middleware;
using RevolutionGolf.EventRequestBroker.Options;
using RevolutionGolf.EventServices.Billing;
using RevolutionGolf.EventServices.Payments;

namespace RevolutionGolf.PayPalListener
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
            services
                .AddAuthentication(EventRequestAuthenticationDefaults.AuthenticationScheme)
                .AddPayPalWebHook<EventRequestAuthenticationService>(options =>
                {
                    options.WebHookId = "1";
                });

            services
                .Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Add(IPAddress.Any);
                })
                .AddLogging()
                .AddOptions()
                .Configure<EventRequestBrokerConfiguration>(Configuration.GetSection("EventBroker"))
                .AddSingleton<IEventService, BillingSubscriptionService>()
                .AddSingleton<IEventService, AuthorizationService>()
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug();
            }

            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"));

            app
                .UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                })
                //.UseAuthentication()
                .UseEventRequestDeserialization()
                .UseEventRequestBroker<IEventService, BillingSubscriptionService>()
                .UseEventRequestBroker<IEventService, AuthorizationService>()
                .UseMvc();
        }
    }
}
