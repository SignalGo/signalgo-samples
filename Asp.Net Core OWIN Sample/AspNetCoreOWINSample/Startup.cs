using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SignalGo.Server.Owin;
using SignalGo.Server.ServiceManager;
using System;

namespace AspNetCoreOWINSample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //create your server provider
            ServerProvider serverProvider = new ServerProvider();
            //register your server service
            serverProvider.RegisterServerService<HelloWorldService>();
            serverProvider.RegisterServerService<TestStreamService>();
            serverProvider.RegisterClientService<IHelloCallbackClientService>();
            //handle cross origin
            serverProvider.ProviderSetting.HttpSetting.HandleCrossOriginAccess = true;
            //serverProvider.Start("http://localhost:6235/any");
            //websocket for duplex clients
            WebSocketOptions webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            //add signalgo middlleware
            app.UseMiddleware<SignalGoNetCoreMiddleware>(serverProvider);
        }
    }
}
