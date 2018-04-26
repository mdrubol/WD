using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace WD.WebApi.Extensions
{
    public static class HtmlBundleExtensions
    {

        /// <summary>
        /// Returns the content of a style bundle in one style block
        /// </summary>
        /// <param name="htmlHelper">Extension for HtmlHelper</param>
        /// <param name="paths">String array of bundle paths</param>
        /// <returns>HtmlString with all css bundle contents in one style block</returns>
        public static IHtmlString InlineStyles(this HtmlHelper htmlHelper, params string[] bundlePaths)
        {
            StringBuilder bundleContent = new StringBuilder();
            foreach (string path in bundlePaths)
            {
                bundleContent.Append(GetBundleContent(htmlHelper.ViewContext.HttpContext, path));
            }
            return new HtmlString(string.Format("<style>{0}</style>", bundleContent.ToString()));
        }

        /// <summary>
        /// Returns the contents of script bundles as individual script blocks
        /// </summary>
        /// <param name="htmlHelper">Extension for HtmlHelper</param>
        /// <param name="paths">String array of bundle paths</param>
        /// <returns>HtmlString with a script block for each script bundle path passed in.</returns>
        public static IHtmlString InlineScripts(this HtmlHelper htmlHelper, params string[] bundlePaths)
        {
            StringBuilder bundleContent = new StringBuilder();
            foreach (string path in bundlePaths)
            {
                bundleContent.Append(string.Format("<script>{0}</script>", GetBundleContent(htmlHelper.ViewContext.HttpContext, path)));
            }
            return new HtmlString(bundleContent.ToString());
        }

        /// <summary>
        /// Gets the content of a bundle
        /// </summary>
        /// <param name="httpContext">The HttpContext</param>
        /// <param name="bundleVirtualPath">Path of the bundle</param>
        /// <returns>String containing the contents of the bundle</returns>
        private static string GetBundleContent(HttpContextBase httpContext, string bundleVirtualPath)
        {
            return BundleTable.Bundles
                .Single(b => b.Path == bundleVirtualPath)
                .GenerateBundleResponse(new BundleContext(httpContext, BundleTable.Bundles, bundleVirtualPath))
                .Content;
        }
    }

}