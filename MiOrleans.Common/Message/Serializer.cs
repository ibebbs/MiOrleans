namespace MiOrleans.Common.Message
{
    public class Serializer
    {
        public static readonly Serializer Default = new Serializer();

        private static string Serialize(Outbound.GetIdList message)
        {
            return "{\"cmd\":\"get_id_list\"}";
        }

        private static string Serialize(Outbound.Read read)
        {
            return string.Format(@"{{""cmd"":""read"",""sid"":""{0}""}}", read.Sid);
        }

        private static string Serialize(IOutbound message)
        {
            switch (message)
            {
                case Outbound.GetIdList getIdList:
                    return Serialize(getIdList);
                case Outbound.Read read:
                    return Serialize(read);
                default:
                    return string.Empty;
            }
        }

        public Datagram Serialize(IOutbound message, string ipAddress)
        {
            string data = Serialize(message);

            return new Datagram(data, ipAddress);
        }
    }
}
