namespace MiOrleans.Common.Message.Inbound.DoorSensor
{
    public class Reading : IInbound
    {
        public string Sid { get; set; }

        public int Voltage { get; set; }

        public Status Status { get; set; }
    }
}
