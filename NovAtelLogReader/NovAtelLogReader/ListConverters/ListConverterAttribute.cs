using NovAtelLogReader.ListConverters;
using System;

namespace NovAtelLogReader.ListConverters
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ListConverterAttribute : Attribute
    {
        public string Name { get; set; }
    }
}