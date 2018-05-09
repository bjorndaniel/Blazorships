using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazorships.Client
{
    public class JsInterop
    {
        public static string InitChat()
        {
            return RegisteredFunction.Invoke<string>(
                "Blazorships.Client.JsInterop.InitChat");

        }

        public static string Alert(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "Blazorships.Client.JsInterop.Alert", message);

        }
    }
}
