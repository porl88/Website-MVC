namespace MVC.WebUI.Helpers
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using PA.Text.String;

    public static class BreadcrumbHelper
    {
        public static MvcHtmlString Breadcrumb(this HtmlHelper html, string controllerName = null, string actionName = null)
        {
            var controller = html.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            var action = html.ViewContext.RouteData.Values["action"].ToString().ToLower();
            controllerName = controllerName ?? controller;
            actionName = actionName ?? action;

            if (controller == "home" && action == "index")
            {
                return MvcHtmlString.Empty;
            }

            var orderedList = new TagBuilder("ol");
            orderedList.AddCssClass("breadcrumb");
            orderedList.InnerHtml = AddFirstLink(html);
            orderedList.InnerHtml += AddSecondLink(html, controllerName, controller, action);
            orderedList.InnerHtml += AddThirdLink(html, actionName, action);

            return MvcHtmlString.Create(orderedList.ToString());
        }

        private static string AddFirstLink(HtmlHelper html)
        {
            var firstItem = new TagBuilder("li");
            firstItem.InnerHtml += html.ActionLink("Home", "Index", "Home");
            return firstItem.ToString();
        }

        private static string AddSecondLink(HtmlHelper html, string controllerName, string controller, string action)
        {
            var secondItem = new TagBuilder("li");
            if (action == "index")
            {
                secondItem.SetInnerText(controllerName);
            }
            else
            {
                secondItem.InnerHtml += html.ActionLink(controllerName, "Index", controller);
            }

            return secondItem.ToString();
        }

        private static string AddThirdLink(HtmlHelper html, string actionName, string action)
        {
            if (action != "index")
            {
                var thirdItem = new TagBuilder("li");
                thirdItem.SetInnerText(actionName.DeSlugify());
                return thirdItem.ToString();
            }

            return string.Empty;
        }
    }
}