using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace HolbertonCRM.Middleware
{
    public class GlobalRoutePrefixConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var prefix = "api/v1";
                if (controller.Selectors.Count > 0)
                {
                    foreach (var selector in controller.Selectors)
                    {
                        selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{prefix}/{controller.ControllerName}"));
                    }
                }
            }
        }
    }
}
