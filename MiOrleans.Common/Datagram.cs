using System;

namespace MiOrleans.Common
{
    [Serializable]
    public class Datagram
    {
        public Datagram(string data, string ipAddress)
        {
            Data = data;
            IpAddress = ipAddress;
        }

        public string Data { get; }
        public string IpAddress { get; }
    }
}
