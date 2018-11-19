using messaging;
using System;

namespace domain
{
    [Serializable]
    public class GenericMessage : IGenericMessage
    {
        public Guid Identifier { get; set; }
    }
}
