using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiOrleans.Grain.Implementations
{
    public class Gateway : Orleans.Grain, Interfaces.IGateway
    {
        private string _rgb;
        private int _illumination;
        private string _version;

        public Task<IEnumerable<Common.Message.IOutbound>> ProcessReading(Common.Message.Inbound.Gateway.Reading reading)
        {
            _rgb = reading.Rgb;
            _illumination = reading.Illumination;
            _version = reading.Version;

            return Task.FromResult(Enumerable.Empty<Common.Message.IOutbound>());
        }
    }
}
