using Clash.CoreNet.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashNet.Models
{
    /// <summary>
    /// Clash 全局配置
    /// </summary>
    public static class ClashC
    {
        public static RuleMode Rulemode { get; set; } = RuleMode.Rule;
    }
}
