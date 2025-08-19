using CastAmNow.UI.Services;

namespace CastAmNow.Web.Client.Services
{
    public class FormFactor : IFormFactor
    {
        public string GetFormFactor()
        {
            return "Web";
        }

        public string GetPlatform()
        {
            return "Web - " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        }
    }
}
