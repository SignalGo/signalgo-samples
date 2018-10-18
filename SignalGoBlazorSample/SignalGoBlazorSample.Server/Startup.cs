using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using SignalGo.Server.ServiceManager;
using System.Linq;
using System.Net.Mime;

namespace SignalGoBlazorSample.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });
        }

        private ServerProvider serverProvider = new ServerProvider();
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            serverProvider.RegisterServerService<SignalGoServices.HelloWorldService>();
            //to handle cross origin errors
            serverProvider.ProviderSetting.HttpSetting.HandleCrossOriginAccess = true;

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });
            app.UseMiddleware<SignalGo.Server.Owin.SignalGoHttpMiddleware>(serverProvider);
            app.UseBlazor<Client.Startup>();
        }
    }
}
