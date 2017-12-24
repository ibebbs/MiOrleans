using Newtonsoft.Json;

namespace MiOrleans.Common.Message
{
    public class Deserializer
    {
        public static readonly Deserializer Default = new Deserializer();

        private class Received
        {
            [JsonProperty("cmd")]
            public string Command { get; set; }

            [JsonProperty("model")]
            public string Model { get; set; }

            [JsonProperty("sid")]
            public string Sid { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("data")]
            public string Data { get; set; }
        }

        private class HeartbeatData
        {
            [JsonProperty("ip")]
            public string IpAddress { get; set; }
        }

        private class GatewaySensorData
        {
            [JsonProperty("rgb")]
            public string Rgb { get; set; }

            [JsonProperty("illumination")]
            public int Illumination { get; set; }

            [JsonProperty("proto_version")]
            public string Version { get; set; }
        }

        private static IInbound ParseReadAck(Received received, string from)
        {
            switch (received.Model)
            {
                case "gateway": return ParseGatewaySensorRead(received, from);
                default: return new Inbound.Unknown { Sid = received.Sid };
            }
        }

        private static IInbound ParseGatewaySensorRead(Received received, string from)
        {
            GatewaySensorData data = JsonConvert.DeserializeObject<GatewaySensorData>(received.Data);

            return new Inbound.Gateway.Reading
            {
                Sid = received.Sid,
                Rgb = data.Rgb,
                Illumination = data.Illumination,
                Version = data.Version
            };
        }

        private static IInbound ParseGetIdList(Received received, string from)
        {
            string[] subDevices = JsonConvert.DeserializeObject<string[]>(received.Data);

            return new Inbound.IdList
            {
                Sid = received.Sid,
                Token = received.Token,
                SubDevices = subDevices,
                IpAddress = from
            };
        }

        private static IInbound ParseHeartbeat(Received received, string from)
        {
            HeartbeatData data = JsonConvert.DeserializeObject<HeartbeatData>(received.Data);

            return new Inbound.Heartbeat
            {
                Sid = received.Sid,
                Model = received.Model,
                Token = received.Token,
                IpAddress = from
            };
        }

        public IInbound Deserialize(Transmission transmission)
        {
            Received received = JsonConvert.DeserializeObject<Received>(transmission.Data);
            string from = transmission.IpAddress;

            switch (received.Command)
            {
                case "heartbeat": return ParseHeartbeat(received, from);
                case "get_id_list_ack": return ParseGetIdList(received, from);
                case "read_ack": return ParseReadAck(received, from);
                default: return new Inbound.Unknown { Sid = received.Sid };
            }
        }
    }
}
