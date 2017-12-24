using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace MiOrleans.Grain.Implementations
{
    [ImplicitStreamSubscription(Common.Constants.InboundTransmissionStream)]
    public class TransmissionProcessor : Orleans.Grain, Interfaces.ITransmissionProcessor
    {
        private Task<IEnumerable<Common.Message.IOutbound>> Process(Common.Message.Inbound.Gateway.Reading reading)
        {
            return GrainFactory.GetGrain<Interfaces.IGateway>(reading.Sid).ProcessReading(reading);
        }

        private Task<IEnumerable<Common.Message.IOutbound>> Process(Common.Message.Inbound.IdList idList)
        {
            return Task.FromResult<IEnumerable<Common.Message.IOutbound>>(Enumerable
                .Concat(new[] { idList.Sid }, idList.SubDevices)
                .Select(sid => new Common.Message.Outbound.Read { Sid = sid })
            );
        }

        private Task<IEnumerable<Common.Message.IOutbound>> Process(Common.Message.Inbound.Heartbeat heartbeat)
        {
            return Task.FromResult<IEnumerable<Common.Message.IOutbound>>(new[] { new Common.Message.Outbound.GetIdList() });
        }

        private Task<IEnumerable<Common.Message.IOutbound>> Process(Common.Message.IInbound inbound)
        {
            switch (inbound)
            {
                case Common.Message.Inbound.Heartbeat heartbeat:
                    return Process(heartbeat);
                case Common.Message.Inbound.IdList idList:
                    return Process(idList);
                case Common.Message.Inbound.Gateway.Reading reading:
                    return Process(reading);
                default:
                    return Task.FromResult(Enumerable.Empty<Common.Message.IOutbound>());
            }
        }

        private async Task Process(Common.Transmission inboundTransmission)
        {
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(inboundTransmission);

            IEnumerable<Common.Message.IOutbound> outbound = await Process(inbound);

            if (outbound.Any())
            {
                var streamProvider = GetStreamProvider(Common.Constants.OutboundTransmissionStreamProvider);
                var stream = streamProvider.GetStream<Common.Transmission>(Guid.Empty, Common.Constants.OutboundTransmissionStream);

                foreach(var outboundTransmission in outbound.Select(message => Common.Message.Serializer.Default.Serialize(message, inboundTransmission.IpAddress)))
                {
                    await stream.OnNextAsync(outboundTransmission);
                }
            }
        }

        public async override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(Common.Constants.InboundTransmissionStreamProvider);
            var stream = streamProvider.GetStream<Common.Transmission>(this.GetPrimaryKey(), Common.Constants.InboundTransmissionStream);
            await stream.SubscribeAsync((transmission, token) => Process(transmission));
        }
    }
}
