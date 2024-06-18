using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

public class SystemBrowser : IBrowser
{
    private readonly string _redirectUri;

    public SystemBrowser(string redirectUri)
    {
        _redirectUri = redirectUri;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = options.StartUrl,
            UseShellExecute = true
        };
        Process.Start(processStartInfo);

        if (_redirectUri.StartsWith("http://"))
        {
            using var listener = new LoopbackHttpListener(_redirectUri);
            var result = await listener.WaitForCallbackAsync();
            return new BrowserResult
            {
                Response = result,
                ResultType = BrowserResultType.Success
            };
        }

        // Handle custom schemes (e.g., yourapp://callback)
        return new BrowserResult
        {
            Response = string.Empty,
            ResultType = BrowserResultType.UnknownError,
            Error = "Custom scheme redirects are not supported by this example."
        };
    }
}
