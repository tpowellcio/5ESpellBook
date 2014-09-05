using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_5ESpellbook.Startup))]
namespace _5ESpellbook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
