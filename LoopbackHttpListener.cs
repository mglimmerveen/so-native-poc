using System;
using System.Net;
using System.Threading.Tasks;

public class LoopbackHttpListener : IDisposable
{
    private readonly string _url;
    private readonly HttpListener _listener;

    public LoopbackHttpListener(string url)
    {
        _url = url ?? throw new ArgumentNullException(nameof(url));
        _listener = new HttpListener();
        _listener.Prefixes.Add(url);
        _listener.Start();
    }

    public void Start()
    {
        _listener.Start();
    }

    public async Task<string> WaitForCallbackAsync()
    {
        var context = await _listener.GetContextAsync();
        var request = context.Request;
        var response = context.Response;

        var responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://sod.superoffice.com/'></head><body>Please return to the app.</body></html>";
        var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        var responseOutput = response.OutputStream;
        await responseOutput.WriteAsync(buffer, 0, buffer.Length);
        responseOutput.Close();

        return request.Url.ToString();
    }

    public void Dispose()
    {
        _listener?.Stop();
        _listener?.Close();
    }
}
