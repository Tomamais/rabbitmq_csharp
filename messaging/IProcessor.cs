using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace messaging
{
    public interface IProcessor<T>
    {
        Task Process(T message);

        Task OnError(T message, Exception ex);
    }
}
