using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiOrleans.Common.Message.Inbound
{
    public class Unknown : IInbound
    {
        public string Sid { get; set; }
    }
}
