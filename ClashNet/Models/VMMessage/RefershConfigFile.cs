using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashNet.Models.VMMessage
{
    /// <summary>
    /// 刷新配置文件
    /// </summary>
    /// <param name="isrefersh"></param>
    /// <param name="message"></param>
    /// <param name="log"></param>
    public record RefershConfigFile(bool isrefersh,string message,string log);
}
