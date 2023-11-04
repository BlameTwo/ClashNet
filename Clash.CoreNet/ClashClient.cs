using Clash.CoreNet.Helper;
using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Clash;
using Clash.CoreNet.Models.Enums;
using Clash.CoreNet.Models.Interfaces;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Clash.CoreNet;

/// <summary>
/// Clash 的核心控制，Client还未定型
/// </summary>
public class ClashClient : IClashClient
{
    int _pid = -1;


    /// <summary>
    /// 配置文件地址
    /// </summary>
    string? configpath { get; set; } = AppProperty.ClashConfigFolder + "\\config.yaml";

    /// <summary>
    /// 获得当前Clash客户端的监听端口
    /// </summary>
    /// <returns></returns>
    public int GetClashPort() => selectcore == null ? -1 : selectcore.Port;

    public Config GetClashCoreConfig() => selectcore == null ? null : selectcore;

    /// <summary>
    /// 获得当前配置文件中的所有代理
    /// </summary>
    /// <returns></returns>
    public List<IProxyItem> GetFileProxyLists()
    {
        List<IProxyItem> proxies = new();
        selectcore.proxies.ForEach((val) =>
        {
            proxies.Add((IProxyItem)val.Clone());
        });
        return proxies;
    }

    /// <summary>
    /// 获得当前配置文件中的代理分组
    /// </summary>
    /// <returns></returns>
    public List<ProxyGroup> GetFileProxyGroups() => selectcore.proxy_groups;
    public int GetPid() => _pid;

    /// <summary>
    /// 启动Clash
    /// </summary>
    /// <returns></returns>
    public async Task StartCore()
    {
        await Task.Run(async () =>
        {
            Process process = null;
            var builder = ClashYamlHelper.GetDefaultYamlDeserializerBuilder().Build();
            if (File.Exists(AppProperty.ClashConfigJsonFile))
            {
                var jsonstr = await File.ReadAllTextAsync(AppProperty.ClashConfigJsonFile);
                //读取Clash保存的版本信息
                ClashFileConfig clashFileConfig =
                    JsonSerializer.Deserialize<ClashFileConfig>(jsonstr)!;
                process = new Process()
                {
                    StartInfo = new()
                    {
                        FileName = AppProperty.ClashConfigFolder + "\\" + clashFileConfig.FileName,
                        WorkingDirectory = AppProperty.ClashConfigFolder,
                        Arguments = "-f config.yaml",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    } ,
                    EnableRaisingEvents = true
                };
                process.Start();
            }
            _pid = process!.Id;
        }).ConfigureAwait(false);
    }

    public Config selectcore;

    public Config selectproxies;



    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="clashname">Clash进程名称</param>
    /// <returns></returns>
    public bool StopCore(string clashname)
    {
        var result = Process.GetProcessesByName("clash-windows-amd64");
        if (result.Length == 0)
        {
            return false;
        }
        foreach (var process in result)
        {
            process.Kill();
        }
        return true;
    }

    public bool StopCore(int id)
    {
        Process result = null;
        try
        {
            result = Process.GetProcessById(id);
        }
        catch (Exception)
        {
            return true;
        }
        if (result != null)
        {
            result.Kill();
            return true;
        }
        else return false;
    }



    private static object logaction()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 获得已经保存的配置文件信息
    /// </summary>
    /// <returns></returns>
    public async Task<List<Tuple<Config, string>>> GetAppclashYaml()
    {
        var builder = ClashYamlHelper.GetDefaultYamlDeserializerBuilder().Build();
        List<Tuple<Config, string>> value = new();
        DirectoryInfo info = new DirectoryInfo(AppProperty.AppClashCacheFolder);
        foreach (var item in info.GetFiles())
        {
            if (item.FullName.EndsWith(".yaml"))
            {
                var configstr = await File.ReadAllTextAsync(item.FullName);
                var config = builder.Deserialize<Config>(configstr);
                if (config == null) continue;
                value.Add(new Tuple<Config, string>(config, item.FullName));
            }
        }
        return value;
    }


    /// <summary>
    /// 检查CheckUpdate更新
    /// </summary>
    /// <returns>返回一个元组，item1是bool检查是否更新完整，item2是更新完毕后信息</returns>
    public async Task<Tuple<bool, string>> CheckUpdateCore(Action<string> c,bool uicheck)
    {
        IProgress<string> progress2 = new Progress<string>();
        void ProgressChanged(object _,string e) => c(e);
        ((Progress<string>)progress2).ProgressChanged += ProgressChanged;
        bool configflage = false;
        try
        {
            progress2.Report("获取版本信息……");
            HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("sb", "1.0"));
            var result = await client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new(AppProperty.ClashCoreUpdateUrl)
            });
            StreamReader reader = new StreamReader(await result.Content.ReadAsStreamAsync());
            var json = await reader.ReadToEndAsync();
            reader.Close();
            ClashCoreUpdateData data = JsonSerializer.Deserialize<ClashCoreUpdateData>(json)!;
            progress2.Report("获取版本信息完毕……");
            ClashFileConfig oldconfig = null;
            if (File.Exists(AppProperty.ClashConfigJsonFile))
            {
                string txt = await File.ReadAllTextAsync(AppProperty.ClashConfigJsonFile);
                if (string.IsNullOrWhiteSpace(txt))
                    File.Delete(AppProperty.ClashConfigJsonFile);
                progress2.Report("对比旧版信息……");
                oldconfig = JsonSerializer.Deserialize<ClashFileConfig>(txt)!;
            }
            if (data.TagName == (oldconfig == null?"f":oldconfig.TagName))
            {
                progress2.Report("不需要更新，已经是最新");
                return new Tuple<bool, string>(true, "不需要更新，本地已经是最新");
            }
            progress2.Report("开始下载新文件");
            //https://ghproxy.com/
            var downloadurl = $"https://ghproxy.com/https://github.com/Dreamacro/clash/releases/download/{data.TagName}/clash-windows-amd64-{data.TagName}.zip";
            WebClient webclient = new();
            webclient.Headers.Add("User-Agent", "BlameTwo");
            var zip_byte = await webclient.DownloadDataTaskAsync(downloadurl);
            MemoryStream stream = new MemoryStream(zip_byte);
            string cacheguid = GuidHelper.GetNewGuid().ToString();
            string cachefilename = "";
            string cachefilepath = "";
            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var item in zip.Entries)
                {
                    if (item.FullName.StartsWith("clash-windows"))
                    {
                        cachefilename = item.FullName;
                        progress2.Report("释放新版核心……");
                        cachefilepath = AppProperty.CacheFolder + $"\\{cacheguid}";
                        item.ExtractToFile(cachefilepath);
                    }
                }
            }
            ClashFileConfig newconfig = new()
            {
                TagName = data.TagName,
                Version = data.TagName,
                FileName = cachefilename,
                CreateTime = data.PublishTime
            };
            progress2.Report("下载完毕，正在替换文件");
            this.StopCore(_pid);
            var newconfigjson = JsonSerializer.Serialize(newconfig);
            File.Delete(AppProperty.ClashConfigJsonFile);
            if(oldconfig != null)
                File.Delete($"{AppProperty.ClashConfigFolder}//{oldconfig.FileName}");
            File.Move(cachefilepath, $"{AppProperty.ClashConfigFolder}//{cachefilename}");
            await File.WriteAllTextAsync(AppProperty.ClashConfigJsonFile, newconfigjson);
            progress2.Report("更新完毕，正在重启核心");
            if(!uicheck)
                await this.StartCore().ConfigureAwait(false);
            return new Tuple<bool, string>(true, "更新完毕");
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }
        
    }

    /// <summary>
    /// 初始化函数，不使用构造函数
    /// <paramref name="isconfig">是否重新生成配置文件</paramref>
    /// </summary>
    public async Task<bool> Init(string configpath=null)
    {
        var builder = ClashYamlHelper.GetDefaultYamlDeserializerBuilder().Build();
        if (string.IsNullOrWhiteSpace(configpath) || !File.Exists(configpath))
        {//如果说配置文件为空或者说未找到配置文件,重建main文件
            CreateDefaultConfig();
            return true;
        }
        try
        {
            var yamlstr = await File.ReadAllTextAsync(configpath);
            Config config = builder.Deserialize<Config>(yamlstr);
            if (config == null)
            {
                CreateDefaultConfig();
                return true;
            }
            var makeflage = await ClashYamlHelper.ToClashFile(config,new ToClashFileData(RuleMode.Direct,null));
            if (!makeflage)CreateDefaultConfig();
            this.selectcore = config;
            await StartCore().ConfigureAwait(false);
            return false;
        }
        catch (Exception ex) 
        {
            CreateDefaultConfig();
            return true;
        }
    }

    async void CreateDefaultConfig()
    {
        var result = await ClashYamlHelper.MakeConfig("main");
        var fileoutput = await ClashYamlHelper.ToClashFile(result.Item1, new(RuleMode.Direct, null));
        this.selectcore = result.Item1;
        await StartCore().ConfigureAwait(false);
    }

    /// <summary>
    /// 选择配置文件
    /// </summary>
    /// <param name="path">地址</param>
    public async void SelectConfig(string path, RuleMode rulemode = RuleMode.Rule)
    {
        await Task.Run(async () =>
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine("SelectConfig:未找到配置文件地址");
                return;
            }
            else
            {
                this.configpath = path;
                if (_pid != -1)
                    StopCore(_pid);
                var builder = ClashYamlHelper.GetDefaultYamlDeserializerBuilder().Build();
                var yamlstr = await File.ReadAllTextAsync(path);
                var config = builder.Deserialize<Config>(yamlstr);
                var result = await ClashYamlHelper.ToClashFile(config, new(rulemode, null));
                if (result == false)
                {
                    Debug.WriteLine("输出配置文件失败！");
                    return;
                }
                selectcore = config;
                await StartCore().ConfigureAwait(false);
            }
        });

    }


    /// <summary>
    /// 全局模式下进行选择指定代理
    /// </summary>
    public async void SelectProxies()
    {

    }
    public static async Task<string> GetCoreString(string url)
    {
        try
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(handler);
            Stream stream = await client.GetStreamAsync(url);
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadLineAsync()!;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<ClashFileConfig> GetClashVersion()
    {
        if (File.Exists(AppProperty.ClashConfigJsonFile))
        {
            using FileStream filestr = new(AppProperty.ClashConfigJsonFile, FileMode.Open) ;
           if (filestr.Length == 0)
            {
                File.Delete(AppProperty.ClashConfigJsonFile);
                return null;
            }
            try
            {
                var jsonconfig = await JsonSerializer.DeserializeAsync<ClashFileConfig>(filestr)!;
                return jsonconfig!;
            }
            catch (Exception)
            {
                File.Delete(AppProperty.ClashConfigJsonFile);
                return null;
            }
        }
        else
        {
            return null;
        }
    }


    public async Task<bool> ChangedConfig(ChangedClashConfig config)
    {
        string url = $"{Api.CoreRun}/configs";
        var configjson = JsonSerializer.Serialize(config, JsonHelper.GetJsonSerializerOptions());
        using var httpClient = new HttpClient();
        using var httpContent = new StringContent(configjson);
        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpResponseMessage = await httpClient.PatchAsync(url, httpContent);

        var strin = await httpResponseMessage.Content.ReadAsStringAsync();

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine("Config updated successfully.");
        }
        else
        {
            Console.WriteLine($"Error updating config. Status code: {httpResponseMessage.StatusCode}");
        }
        return true;
    }
}
