using Microsoft.Owin;
using Owin;
using Umbraco.Core;
using Umbraco.Core.Security;
using Umbraco.Web.Security.Identity;
using Umbraco.IdentityExtensions;
using BackofficeADFSAuthenticationDemo;

//To use this startup class, change the appSetting value in the web.config called 
// "owin:appStartup" to be "UmbracoCustomOwinStartup"

[assembly: OwinStartup("ConfigureOwinStartup", typeof(ConfigureOwinStartup))]
namespace BackofficeADFSAuthenticationDemo
{
    /// <summary>
    /// A custom way to configure OWIN for Umbraco
    /// </summary>
    /// <remarks>
    /// The startup type is specified in appSettings under owin:appStartup - change it to "ConfigureOwinStartup" to use this class
    /// This startup class would allow you to customize the Identity IUserStore and/or IUserManager for the Umbraco Backoffice
    /// </remarks>
    public class ConfigureOwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure back office users membership provider
            app.ConfigureUserManagerForUmbracoBackOffice(
                ApplicationContext.Current,
                MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider());

            // Ensure OWIN is configured for Umbraco back office authentication
            app.UseUmbracoBackOfficeCookieAuthentication(ApplicationContext.Current)
               .UseUmbracoBackOfficeExternalCookieAuthentication(ApplicationContext.Current);

            // Configure additional back office authentication options            
            app.ConfigureBackOfficeAdfsAuthentication();
        }
    }
}
