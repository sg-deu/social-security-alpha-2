using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FormUI.Controllers.Harness
{
    public class HarnessModel
    {
        public string       Text1       { get; set; }
        public string       Text2       { get; set; }
        public string       Text3       { get; set; }
        public string       Password    { get; set; }
        public DateTime     DateTime1   { get; set; }
        public DateTime     DateTime2   { get; set; }
        public int          Int1        { get; set; }
        public RValues1?    Radio1      { get; set; }
    }

    public enum RValues1
    {
        [Description("Value 1")]
        Value1,

        [Description("Value 2")]
        Value2,
    }

    public static class RValues1Util
    {
        public static IDictionary<RValues1, string> Descriptions = new Dictionary<RValues1, string>()
        {
            { RValues1.Value1, "Custom Value 1" },
            { RValues1.Value2, "Custom Value 2" },
        };
    }
}