using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiOrleans.Common.Message.Inbound
{
    public class IdList : IInbound
    {
        public string Command { get; set; }

        public string Sid { get; set; }

        public string IpAddress { get; set; }

        public string Token { get; set; }

        public IEnumerable<string> SubDevices { get; set; }
    }
}
