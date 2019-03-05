using FireworksFramework.Interfaces;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using static FireworksFramework.Types.Enums;

namespace TestAppDesigner.Views
{
    /// <summary>
    /// Interaction logic for TestAppDesignerView.xaml
    /// </summary>
    public partial class TestAppDesignerView : UserControl, IFireworksDesigner
    {
        public TestAppDesignerView()
        {
            InitializeComponent();
        }
        public System.Drawing.Image PluginImage
        {
            get
            {
                return System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("TestAppDesigner.TestApp.ico"));
            }
        }

        public PluginType PluginType { get { return PluginType.Designer; } }

        public string PluginInformation
        {
            get
            {
                return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("TestAppDesigner.License.txt")).ReadToEnd();
            }
        }

        public string PluginName
        {
            get { return "Test Application"; }
        }

        public string PluginOrder
        {
            get { return "testapp_group1"; }
        }
        public bool IsValidContext()
        {
            return true;
        }

        public void LoadData()
        {
            viewModel.LoadData();
        }
    }
}
