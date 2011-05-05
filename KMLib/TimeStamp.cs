namespace KMLib
{
    using System.Xml.Serialization;
    using Abstract;

    public class TimeStamp : ATimePrimitive
    {
        public TimeStamp()
        {
        }

        public TimeStamp(string time)
        {
            Time = time;
        }

        [XmlElement("when")]
        public string Time { get; set; }
    }
}