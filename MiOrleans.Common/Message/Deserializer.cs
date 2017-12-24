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

        private class DoorSensorData
        {
            [JsonProperty("voltage")]
            public int Voltage { get; set; }

            [JsonProperty("status")]
            public Inbound.DoorSensor.Status Status { get; set; }
        }

        private static IInbound DeserializeGatewaySensorRead(Received received)
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

        private static IInbound DeserializeDoorSensorRead(Received received)
        {
            DoorSensorData data = JsonConvert.DeserializeObject<DoorSensorData>(received.Data);

            return new Inbound.DoorSensor.Reading
            {
                Sid = received.Sid,
                Voltage = data.Voltage,
                Status = data.Status
            };
        }

        private static IInbound DeserializeDoorSensorReport(Received received)
        {
            DoorSensorData data = JsonConvert.DeserializeObject<DoorSensorData>(received.Data);

            return new Inbound.DoorSensor.Report
            {
                Sid = received.Sid,
                Status = data.Status
            };
        }

        private static IInbound DeserializeReadAck(Received received)
        {
            switch (received.Model)
            {
                case "gateway": return DeserializeGatewaySensorRead(received);
                case "magnet": return DeserializeDoorSensorRead(received);
                default: return new Inbound.Unknown { Sid = received.Sid };
            }
        }

        private static IInbound DeserializeReport(Received received)
        {
            switch (received.Model)
            {
                case "magnet": return DeserializeDoorSensorReport(received);
                default: return new Inbound.Unknown { Sid = received.Sid };
            }
        }

        private static IInbound DeserializeGetIdList(Received received)
        {
            string[] subDevices = JsonConvert.DeserializeObject<string[]>(received.Data);

            return new Inbound.IdList
            {
                Sid = received.Sid,
                Token = received.Token,
                SubDevices = subDevices
            };
        }

        private static IInbound DeserializeHeartbeat(Received received)
        {
            HeartbeatData data = JsonConvert.DeserializeObject<HeartbeatData>(received.Data);

            return new Inbound.Heartbeat
            {
                Sid = received.Sid,
                Model = received.Model,
                Token = received.Token
            };
        }

        public IInbound Deserialize(Transmission transmission)
        {
            Received received = JsonConvert.DeserializeObject<Received>(transmission.Data);

            switch (received.Command)
            {
                case "heartbeat": return DeserializeHeartbeat(received);
                case "get_id_list_ack": return DeserializeGetIdList(received);
                case "read_ack": return DeserializeReadAck(received);
                case "report": return DeserializeReport(received);
                default: return new Inbound.Unknown { Sid = received.Sid };
            }
        }
    }
}
