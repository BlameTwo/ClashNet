namespace Clash.CoreNet.Helper;

/// <summary>
/// Http 下载
/// </summary>
public static class HttpClientHelper
{
    static HttpClient  _httpclient;
    static HttpClientHelper()
    {
        _httpclient = new();
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="httpmessage"></param>
    /// <returns></returns>
    public static async Task<Stream> SendAsync(HttpRequestMessage httpmessage)
    {
        return await (await _httpclient.SendAsync(httpmessage)).Content.ReadAsStreamAsync();
    }


    public static Task<HttpClient> GetMsgHttpClient()
    {
        return Task.Run(() =>
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(handler);
            return client;
        });
    }
}
