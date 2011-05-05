namespace KmlTestbed
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Core.FileHandling;
    using KMLib;
    using KMLib.Feature;
    using KMLib.Geometry;

    public partial class TestFrm : Form
    {
        public TestFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var kml = CreateKmlDoc();
            var ls = new LoadSave("kml");
            var fpath = ls.GetSavePath();
            if (fpath != null)
            {
                kml.Save(fpath);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var kml = CreateKmlFeat();
            var ls = new LoadSave("kml");
            var fpath = ls.GetSavePath();
            if (fpath != null)
            {
                kml.Save(fpath);
            }
        }

        private KMLRoot CreateKmlDoc()
        {
            var kml = new KMLRoot();
            var pm = new Placemark();
            pm.name = "foo";
            pm.Point = new KmlPoint(120, 45, 50);
            pm.Snippet = "foo is cool";
            pm.Snippet.maxLines = 1;

            var fldr = new Folder("Test Folder");

            kml.Document.Add(pm);
            kml.Document.Add(new Placemark());
            kml.Document.Add(fldr);

            return kml;
        }

        private KMLRoot CreateKmlFeat()
        {
            var kml = new KMLRoot();
            var fldr = new Folder("Test Folder");
            fldr.Add(new Folder("Sub Folder"));

            var g = new GroundOverlay();
            g.altitude = 100;
            g.altitudeMode = AltitudeMode.relativeToGround;
            g.color = Color.Blue;
            g.description = "Cool overlay";

            fldr.Add(g);
            kml.Feature = fldr;

            return kml;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ls = new LoadSave("kml");
            var fpath = ls.GetLoadPath();
            if (fpath != null)
            {
                var kml = KMLRoot.Load(fpath);
                if (kml.UsesDocument)
                {
                    MessageBox.Show("Loaded kml (doc): " + kml.Document.List.Count);
                }
                else
                {
                    MessageBox.Show("Loaded kml (feature): " + kml.Feature.name);
                }
            }
        }
    }
}