using Clash.CoreNet.Helper;
using Clash.CoreNet.Models.EventHandler;
using Clash.CoreNet.Models.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Clash.CoreNet
{
    public class ClashWebHttp : IClashWebHttp
    {
        public string BaseAddress { get; set; } = Api.CoreRun;

        HttpClient _httpclient = null;

        System.Threading.Timer time = null;

        public ClashWebHttp()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpclient = new HttpClient(handler);
            _httpclient.BaseAddress = new(BaseAddress);
        }

        public async Task<CoreLogData> GetCoreLog()
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Get,"logs");
            var resultquest = await _httpclient.SendAsync(requestmsg);
            return JsonSerializer.Deserialize<CoreLogData>(await resultquest.Content.ReadAsStringAsync())!;
        }

        public async Task<IsRunCore>? GetCoreRun()
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Get, "");
            var resultquest = await _httpclient.SendAsync(requestmsg);
            return JsonSerializer.Deserialize<IsRunCore>( await resultquest.Content.ReadAsStringAsync())!;
        }


       
        public async Task InitMsg()
        {
            time = new Timer(Changetime,null,0,1000);
            return;
        }

        private void Changetime(object? state)
        {

        }

        public async Task<CoreProxies> GetProxies()
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Get, "proxies");
            var resultquest = await _httpclient.SendAsync(requestmsg);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new CoreProxiesConverter());
            return JsonSerializer.Deserialize<CoreProxies>(await resultquest.Content.ReadAsStringAsync(),options)!;
        }

        public async Task<string> ChangSelectProxy(string groupname,string newproxyname)
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Put, $"proxies/{groupname}");
            requestmsg.Content = JsonContent.Create(new { name = newproxyname });
            var resultquest = await _httpclient.SendAsync(requestmsg);
            resultquest.EnsureSuccessStatusCode();
            return await resultquest.Content.ReadAsStringAsync();
        }

        public async Task<ProxyConnections> GetConnections()
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Get, $"connections");
            var resultquest = await _httpclient.SendAsync(requestmsg);
            resultquest.EnsureSuccessStatusCode();
            string json = await resultquest.Content.ReadAsStringAsync();
            if (json != null)
                return JsonSerializer.Deserialize<ProxyConnections>(json)!;
            return null;
        }

        public async Task<string> DelectConnection(string id)
        {
            HttpRequestMessage requestmsg = new(HttpMethod.Delete, $"connections/{id}");
            var resultquest = await _httpclient.SendAsync(requestmsg);
            resultquest.EnsureSuccessStatusCode();
            if (resultquest.IsSuccessStatusCode)
            {
                return "删除成功";
            }
            return "失败！";
        }
    }
}
