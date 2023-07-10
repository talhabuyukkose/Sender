using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Sender.Core.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sender.Infrastructure.Services.ActionFilter
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger<CustomActionFilter> logger;

        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("siteUser", out var action))
            {
                if (action is SiteUser)
                {
                    var siteUser = (SiteUser)action;

                    string pattern = @"^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b(?:[-a-zA-Z0-9()@:%_\+.~#?&\/=]*)$";

                    var match = Regex.IsMatch(siteUser.BaseUrl, pattern);

                    if(match is false)
                    {
                        throw new ArgumentException($"{siteUser.BaseUrl} has fault");
                    }

                }
            }

            //if (context.RouteData.Values["action"].ToString() == "GetTsoftProducts" && context.RouteData.Values["controller"].ToString() == "Product")
            //{
            //    context.ActionArguments.TryGetValue("siteUser", out var action);

            //    (action as SiteUser).BaseUrl = (action as SiteUser).BaseUrl == "a" ? "b" : (action as SiteUser).BaseUrl;
            //}
        }
    }
}
