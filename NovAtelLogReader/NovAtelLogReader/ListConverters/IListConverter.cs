using NovAtelLogReader.LogData;
using System.Collections.Generic;

namespace NovAtelLogReader.ListConverters
{
    interface IListConverter
    {
        IEnumerable<object> ToList(LogRecord record);
    }
}
