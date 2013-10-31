using System.Web;
using System.Web.Optimization;

namespace RemotePluginsTestService
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery")
				.Include(
					"~/Scripts/jquery-{version}.js",
					"~/Scripts/bootstrap.js"
				));

			bundles.Add(new StyleBundle("~/Content/css")
				.Include(
					"~/Content/site.css",
					"~/Content/bootstrap/bootstrap.css",
					"~/Content/bootstrap/bootstrap-theme.css"
				));
		}
	}
}