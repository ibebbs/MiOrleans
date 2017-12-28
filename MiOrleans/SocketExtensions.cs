using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MiOrleans
{
    public static class SocketExtensions
    {
        public static string GetIpAddress(this EndPoint endpoint)
        {
            switch (endpoint)
            {
                case IPEndPoint ipEndpoint: return ipEndpoint.Address.ToString();
                default: throw new ArgumentException("Endpoint must be an IPEndpoint");
            }
        }
        public static IPEndPoint AsIpEndPoint(this EndPoint endpoint)
        {
            return (IPEndPoint)endpoint;
        }

        public static Common.Datagram ReceiveFrom(this Socket socket)
        {
            var data = new byte[1024];

            EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            var len = socket.ReceiveFrom(data, ref endpoint);

            string packet = Encoding.ASCII.GetString(data, 0, len);
            string from = endpoint.GetIpAddress();

            return new Common.Datagram(packet, from);
        }
    }
}
