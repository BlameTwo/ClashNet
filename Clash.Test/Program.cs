using Clash.CoreNet;
using Clash.CoreNet.Helper;

//定义一个Client
ClashClient client = new();

#region 更新或生成
#region 更新一个订阅，未完整，没有写对比guid，还有更新订阅链接也未增加到里面
//Console.WriteLine("开始从订阅中获得代理IP");
//https://vnsubc.xyz/api/v1/client/subscribe?token=aab550068497aff538a86d1d7abb8136
//http://hneko.site/api/v1/client/subscribe?token=42972de711a10122347bc7d566a13ae8
//var result = await client.UpdateFormUrl("http://hneko.site/api/v1/client/subscribe?token=42972de711a10122347bc7d566a13ae8");
#endregion

#region 根据更新一个订阅随机生成一个缓存配置文件
//Console.WriteLine("生成配置文件");
//var config = await ClashYamlHelper.MakeConfig(new(result,"Proxy",null));
#endregion
#endregion

#region 进行代理具体操作

#region 获得已经配置好的文件

//var yamllist = await client.GetAppclashYaml();
//foreach (var yaml in yamllist)
//{
//    Console.WriteLine(yaml.Item2);
//}
//Console.WriteLine("请输入序号选择配置文件");
//int index = -1;
//index = int.Parse(Console.ReadLine()!);
#endregion

#region 进行选择启动
//Console.WriteLine("开始启动Clash");
//var process = await client.StartCore(yamllist[index], new(Clash.CoreNet.Models.Enums.RuleMode.Rule, null));
//Console.WriteLine($"Clash已经启动成功,PID:{process.Item1}");
//Console.WriteLine($"Clash本体监听端口:{client.GetClashPort()}");
//Console.WriteLine("输入N退出软件");
//client.CoreLogMessageEvent += Client_CoreLogMessageEvent1;
//client.SpendMessageEvent += Client_SpendMessageEvent;

#endregion

#endregion


#region Clash Core核心操作
#region 更新Clash
//var result = await client.CheckUpdateCore();
#endregion
#endregion

//while (true)
//{
//    var key = Console.ReadKey();
//    if (key.Key == ConsoleKey.N)
//    {
//        client.StopCore("clash-windows-amd64");
//        break;
//    }
//}
// Console.ReadKey();
await client.StartCore();
Console.ReadLine();