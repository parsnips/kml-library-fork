namespace Core.Xml
{
    using System.Xml;

    public abstract class AXmlHandler
    {
        protected XmlDocument doc;
        protected string m_FilePath;

        public AXmlHandler()
        {
        }

        public AXmlHandler(string filepath)
        {
            LoadFile(filepath);
        }

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

        protected void SetupXmlDeclaration()
        {
            doc = new XmlDocument();
            var xmldecl = doc.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty);
            doc.AppendChild(xmldecl);
        }

        protected void LoadFile(string filepath)
        {
            var reader = new XmlTextReader(filepath);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            doc = new XmlDocument();
            doc.Load(reader);
            m_FilePath = filepath;
            reader.Close();
        }

        public virtual void SaveAs(string path)
        {
            doc.Save(path);
        }

        public void Save()
        {
            doc.Save(m_FilePath);
        }
    }
}