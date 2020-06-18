using System;

namespace NovAtelLogReader.DataPoints
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class DataPointAttribute : Attribute
    {
        public string Name { get; set; }
        public string Queue { get; set; }
    }
}