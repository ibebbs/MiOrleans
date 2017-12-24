using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiOrleans.Grain.Interfaces
{
    public interface IGateway : IGrainWithStringKey
    {
        Task<IEnumerable<Common.Message.IOutbound>> ProcessReading(Common.Message.Inbound.Gateway.Reading reading);
    }
}
