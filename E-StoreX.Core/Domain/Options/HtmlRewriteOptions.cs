using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Core.Domain.Options
{
    public class HtmlRewriteOptions
    {
        public Dictionary<string, string> Mappings { get; set; } = new();
    }
}
