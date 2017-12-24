using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiOrleans.Common.Message.Inbound
{
    public class Heartbeat : IInbound
    {
        public string Sid { get; set; }
        public string Model { get; set; }
        public string Token { get; set; }
    }
}
