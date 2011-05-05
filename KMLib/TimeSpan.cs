namespace KMLib
{
    using System.Xml.Serialization;
    using Abstract;

    public class TimeSpan : ATimePrimitive
    {
        public TimeSpan()
        {
        }

        public TimeSpan(string beginTime, string endTime)
        {
            Begin = beginTime;
            End = endTime;
        }

        [XmlElement("begin")]
        public string Begin { get; set; }

        [XmlElement("end")]
        public string End { get; set; }
    }
}