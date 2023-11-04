using Clash.CoreNet.Models;
using Clash.CoreNet.Models.Enums;
using System.ComponentModel;

namespace Clash.CoreNet.Helper;

public static class RuleHelper
{
    public async static Task<List<string>> MakeRule(List<string> keyvalues, string groupname)
    {
        List<string> rules = new List<string>();
        foreach (string key in keyvalues)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edg/112.0.1722.58");
            var result = await (await httpClient.GetAsync(key)).Content.ReadAsStreamAsync();
            var str = result.StreamToString();
            var strs = str.Split("\n");
            foreach (string s in strs)
            {
                if (s.StartsWith("#")) continue;
                if (string.IsNullOrWhiteSpace(s)) continue;
                if (s.StartsWith("IP-CIDR"))
                {
                    var linelist = s.Split(",");
                    rules.Add($"{linelist[0]},{linelist[1]},{groupname},no-resolve");
                }
                else
                {
                    rules.Add(s + $",{groupname}");
                }
            }
        }
        rules.Add($"MATCH,{groupname}");
        return rules;
    }
}
