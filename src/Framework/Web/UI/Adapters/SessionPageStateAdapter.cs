using System.Web.UI;
using System.Web.UI.Adapters;

namespace InfoControl.Web.UI.Adapters
{
    public class SessionPageStateAdapter : PageAdapter
    {
        public override PageStatePersister GetStatePersister()
        {
            return new SessionPageStatePersister(Page);
        }
    }
}