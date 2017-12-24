using System;

namespace MiOrleans.Common
{
    [Serializable]
    public class Transmission
    {
        public Transmission(string data, string ipAddress)
        {
            Data = data;
            IpAddress = ipAddress;
        }

        public string Data { get; }
        public string IpAddress { get; }
    }
}
