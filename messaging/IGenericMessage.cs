using System;
using System.Collections.Generic;
using System.Text;

namespace messaging
{
    public interface IGenericMessage
    {
        Guid Identifier { get; set; }
    }
}
