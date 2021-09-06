using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Signalr.Server
{
    public interface ISender
    {
        Task SendMessageAsync(object data);
    }
}
