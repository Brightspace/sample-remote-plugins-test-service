using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Framework.Signing;

using Microsoft.Ajax.Utilities;

using RemotePluginsTestService.Models;

namespace RemotePluginsTestService.Controllers {

	public class HomeController : Controller {

		private readonly Dog[] m_dogs;

		public HomeController() {

			const string BASE_IMG_URL = "~/Content/img/thumb";

			m_dogs = new Dog[3];

			for (int i = 0; i < 3; ++i ) {
				m_dogs[i] = new Dog { ImageUrl = "{0}{1}.jpg".FormatInvariant(BASE_IMG_URL, i) };
			}
		}

		private bool IsOAuthValid( string secret ) {

			try {

				var context = new OAuthContextBuilder().FromHttpRequest( Request );

				IOAuthContextSigner signer = new OAuthContextSigner();

				SigningContext signingContext = new SigningContext {ConsumerSecret = secret};

				return signer.ValidateSignature( context, signingContext );

			} catch( OAuthException ) {

				return false;
			}
		}

		private string CreateReturnUrl( FormCollection collection ) {

			Uri requestUrl = new Uri(collection["launch_presentation_return_url"]);
			string returnUrl = requestUrl.Scheme + "://" + requestUrl.Authority + requestUrl.AbsolutePath;

			return returnUrl;
		}

		private Dictionary<string, string> ExtractLaunchParameters( FormCollection collection ) {

			var parameters = new Dictionary<string, string>();

			foreach( var key in collection.AllKeys ) {

				string value = collection[key] ?? "NULL";

				parameters.Add( key, value );
			}

			return parameters;
		}

		private ViewResult CreateView( FormCollection collection ) {

			bool oauthEnabled = ConfigurationManager.AppSettings["oauth_enabled"].ToLowerInvariant() == "true";
			string oauthSecret = ConfigurationManager.AppSettings["oauth_secret"];

			ViewBag.OAuthValid = true;

			if( oauthEnabled ) {
				ViewBag.OAuthValid = IsOAuthValid( oauthSecret );
			}

			ViewBag.LaunchParameters = ExtractLaunchParameters( collection );

			string hostname = ConfigurationManager.AppSettings["hostname"];

			ViewBag.HostName = hostname;

			try {

				ViewBag.ReturnUrl = CreateReturnUrl( collection );
				return View( m_dogs );

			} catch( ArgumentNullException ) {

				ViewBag.ErrorMessage = "Invalid request URL.";
				return View( "Error" );
			}
		}

		[HttpGet]
		public ActionResult Index() {

			return View( "Splash" );
		}

		[HttpPost]
		public ActionResult Index( FormCollection collection ) {

			return CreateView( collection );
		}

		[HttpPost]
		public ActionResult IsfSelector( FormCollection collection ) {

			return CreateView( collection );
		}

		[HttpPost]
		public ActionResult QuickLinksSelector( FormCollection collection ) {

			return CreateView( collection );
		}

		[HttpPost]
		public ActionResult Nav( FormCollection collection ) {

			return CreateView( collection );
		}

		[HttpPost]
		public ActionResult Widget( FormCollection collection ) {

			return CreateView( collection );
		}
	}
}
