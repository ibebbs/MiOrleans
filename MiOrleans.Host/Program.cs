using System;
using System.Reactive;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Orleans.Streams;

namespace MiOrleans.Host
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            siloConfig.AddSimpleMessageStreamProvider(Common.Constants.InboundTransmissionStreamProvider);
            siloConfig.AddSimpleMessageStreamProvider(Common.Constants.OutboundTransmissionStreamProvider);
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.Config.AddMemoryStorageProvider("PubSubStore");
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            clientConfig.AddSimpleMessageStreamProvider(Common.Constants.InboundTransmissionStreamProvider);
            clientConfig.AddSimpleMessageStreamProvider(Common.Constants.OutboundTransmissionStreamProvider);
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //
            // This is the place for your test code.
            //
            var transport = new Transport();

            var inboundSubscription = transport.Received
                .Subscribe(transmission => client.InboundTransmissionStream(transmission.IpAddress).OnNextAsync(transmission));

            var outboundSubscription = client.OutboundTransmissionStream()
                .SubscribeAsync((transmission, token) => transport.Send(transmission))
                .Result;

            var connection = transport.Connect();

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();
            Console.WriteLine("\nTerminating...");

            inboundSubscription.Dispose();
            outboundSubscription.UnsubscribeAsync().Wait();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }

    internal static class Extensions
    {
        public static IAsyncStream<Common.Transmission> OutboundTransmissionStream(this IClusterClient client)
        {
            return client
                .GetStreamProvider(Common.Constants.OutboundTransmissionStreamProvider)
                .GetStream<Common.Transmission>(Guid.Empty, Common.Constants.OutboundTransmissionStream);
        }

        public static IAsyncStream<Common.Transmission> InboundTransmissionStream(this IClusterClient client, string ipAddress)
        {
            return client
                .GetStreamProvider(Common.Constants.InboundTransmissionStreamProvider)
                .GetStream<Common.Transmission>(Common.StringToGuidConverter.Default.Convert(ipAddress), Common.Constants.InboundTransmissionStream);
        }
    }
}
