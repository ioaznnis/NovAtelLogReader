using System;
using System.Collections.Generic;

namespace NovAtelLogReader.Publishers
{
    interface IPublisher
    {
        void Open();
        void Close();
        void Publish(Type type, IEnumerable<object> value);
        void Publish<T>(List<T> value);
    }
}
