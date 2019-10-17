using System.Configuration;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using Umbraco.Web.Security.Identity;
using Constants = Umbraco.Core.Constants;

namespace SKEACMS_ADFS
{
    public static class AdfsAuthenticationExtensions
    {
        public static void ConfigureBackOfficeAdfsAuthentication(
            this IAppBuilder app,
            string caption = "AD FS",
            string style = "btn-microsoft",
            string icon = "fa-windows")
        {
            var adfsMetadataEndpoint = ConfigurationManager.AppSettings["AdfsMetadataEndpoint"];
            var adfsRelyingParty = ConfigurationManager.AppSettings["AdfsRelyingParty"];
            var adfsFederationServerIdentifier = ConfigurationManager.AppSettings["AdfsFederationServerIdentifier"];

            app.SetDefaultSignInAsAuthenticationType(Constants.Security.BackOfficeExternalAuthenticationType);

            var wsFedOptions = new WsFederationAuthenticationOptions
            {
                Wtrealm = adfsRelyingParty,
                MetadataAddress = adfsMetadataEndpoint,
                SignInAsAuthenticationType = Constants.Security.BackOfficeExternalAuthenticationType,
                Caption = caption, 
                Wreply = $"{adfsRelyingParty}umbraco" // Redirect to the Umbraco back office after succesful authentication
            };

            wsFedOptions.ForUmbracoBackOffice(style, icon);

            wsFedOptions.AuthenticationType = adfsFederationServerIdentifier;
            
            // https://our.umbraco.com/apidocs/csharp/api/Umbraco.Web.Security.Identity.ExternalSignInAutoLinkOptions.html
            wsFedOptions.SetExternalSignInAutoLinkOptions(new ExternalSignInAutoLinkOptions(true, "editor", new string[] { "en-US" }));

            app.UseWsFederationAuthentication(wsFedOptions);
        }
    }
}
