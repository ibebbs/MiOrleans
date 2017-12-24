namespace MiOrleans.Common.Message.Inbound.Gateway
{
    public class Reading : IInbound
    {
        public string Sid { get; set; }

        public string Rgb { get; set; }

        public int Illumination { get; set; }

        public string Version { get; set; }
    }
}
