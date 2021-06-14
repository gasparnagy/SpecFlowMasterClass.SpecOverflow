using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SpecFlowMasterClass.SpecOverflow.Web.Utils
{
    public static class ViewHelperExtensions
    {
        public static IHtmlContent PageLink(this IHtmlHelper urlHelper, string label, string pageName, string cssClass = null)
        {
            var htmlContent = new TagBuilder("a");
            if (cssClass != null)
                htmlContent.AddCssClass(cssClass);
            htmlContent.InnerHtml.Append(label);
            htmlContent.MergeAttribute("href", $"/{pageName}");
            return htmlContent;
        }

        public static string HomeUrl(this IHtmlHelper htmlHelper)
        {
            return "/";
        }

        public static string Api(this IUrlHelper urlHelper, string path)
        {
            return path;
        }
    }
}