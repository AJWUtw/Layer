using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Layer.Startup))]
namespace Layer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
