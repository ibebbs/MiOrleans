namespace MiOrleans.Common.Message.Inbound.DoorSensor
{
    public class Report : IInbound
    {
        public string Sid { get; set; }

        public Status Status { get; set; }
    }
}
