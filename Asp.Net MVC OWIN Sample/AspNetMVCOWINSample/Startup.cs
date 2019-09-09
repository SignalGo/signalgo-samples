using Owin;
using SignalGo.Server.ServiceManager;
using System.Web.Http;

namespace AspNetMVCOWINSample
{
    public class Startup
    {
        private readonly ServerProvider serverProvider = new ServerProvider();

        public void Configuration(IAppBuilder app)
        {

            //create your server provider
            ServerProvider serverProvider = new ServerProvider();
            //register your server service
            serverProvider.RegisterServerService<HelloWorldService>();
            //register client service
            serverProvider.RegisterClientService<IHelloCallbackClientService>();
            //handle cross origin
            serverProvider.ProviderSetting.HttpSetting.HandleCrossOriginAccess = true;
            //add signalgo middlleware
            app.Use<SignalGo.Server.Owin.SignalGoOwinMiddleware>(serverProvider);

            HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
           "DefaultApi", // Route name
           "", "http://localhost:10012/HellowWorld/Hello");
            app.UseWebApi(config);
        }
    }
}