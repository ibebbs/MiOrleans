using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MiOrleans
{
    public class Transport
    {
        private const string MulticastAddress = "224.0.0.50";
        private const int ServerPort = 9898;

        private readonly IConnectableObservable<Common.Datagram> _received;
        private readonly Socket _socket;

        public Transport()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(MulticastAddress)));
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            _received = Observable
                .Defer(() => Observable.Return(_socket.ReceiveFrom()))
                .Repeat()
                .SubscribeOn(NewThreadScheduler.Default)
                .Publish();
        }

        public IDisposable Connect()
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, ServerPort));

            return _received.Connect();
        }

        public Task Send(Common.Datagram datagram)
        {
            _socket.SendTo(Encoding.ASCII.GetBytes(datagram.Data), new IPEndPoint(IPAddress.Parse(datagram.IpAddress), ServerPort));

            return Task.CompletedTask;
        }

        public IObservable<Common.Datagram> Received
        {
            get { return _received; }
        }
    }
}
