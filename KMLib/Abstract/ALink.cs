namespace KMLib.Abstract
{
    using System.Xml.Serialization;

    // --note: this abstract class is not part of the official KML class diagram, but it makes sense given that "<Icon> has the same child elements as <Link>"
    public class ALink
    {
        private float m_refreshInterval = 4;
        private RefreshMode m_refreshMode = RefreshMode.onChange;
        private float m_viewBoundScale = 1;
        private ViewRefreshMode m_viewRefreshMode = ViewRefreshMode.never;
        private float m_viewRefreshTime = 4;
        [XmlIgnore] public bool refreshIntervalSpecified;

        [XmlIgnore] public bool refreshModeSpecified;
        [XmlIgnore] public bool viewBoundScaleSpecified;
        [XmlIgnore] public bool viewRefreshModeSpecified;
        [XmlIgnore] public bool viewRefreshTimeSpecified;
        public string href { get; set; }

        public RefreshMode refreshMode
        {
            get { return m_refreshMode; }
            set
            {
                m_refreshMode = value;
                refreshModeSpecified = true;
            }
        }

        public float refreshInterval
        {
            get { return m_refreshInterval; }
            set
            {
                m_refreshInterval = value;
                refreshIntervalSpecified = true;
            }
        }

        public ViewRefreshMode viewRefreshMode
        {
            get { return m_viewRefreshMode; }
            set
            {
                m_viewRefreshMode = value;
                viewRefreshModeSpecified = true;
            }
        }

        public float viewRefreshTime
        {
            get { return m_viewRefreshTime; }
            set
            {
                m_viewRefreshTime = value;
                viewRefreshTimeSpecified = true;
            }
        }

        public float viewBoundScale
        {
            get { return m_viewBoundScale; }
            set
            {
                m_viewBoundScale = value;
                viewBoundScaleSpecified = true;
            }
        }

        public string viewFormat { get; set; }

        public string httpQuery { get; set; }
    }
}