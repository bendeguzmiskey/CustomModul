using DotNetNuke.Web.Api;

namespace CustomModul.CustomModul
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute(
                moduleFolderName: "CustomModul", // Must match the folder name in your .dnn manifest
                routeName: "default", // A unique name for this route
                url: "{controller}/{action}", // URL pattern: [ControllerName]/[ActionName]
                namespaces: new[] { "CustomModul.CustomModul.Controllers" } // Namespace of your controllers
            );
        }
    }
}