using System;

namespace NovAtelLogReader.LogRecordFormats
{
    internal enum ParserFromat
    {
        Ascii,
        Binary
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class ParserAttribute : Attribute
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ParserFromat Fromat { get; set; }
    }
}
