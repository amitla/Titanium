using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Web.Proxy.Test.BO
{
    public class BlackListRecord
    {
        public int Id { get; set; }

        public string Regex { get; set; }

        public string ReplacementHTML { get; set; }
    }
}
