using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ClientTest.Model;
using ClientTest.Serializers;


namespace ClientTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const long Iterations = 1000;
        private readonly PropertyGrid _propertyGrid = new PropertyGrid {HelpVisible = false};
        private readonly Collection<SerializerTest<Employee>> _testCollection = new Collection<SerializerTest<Employee>>
        {
            new BinaryFormatterSerializerTest<Employee>(Employee.JamesTKirk, Iterations),
            new JsonDataContractSerializerTest<Employee>(Employee.JamesTKirk, Iterations),
            new JsonNewtonsoftSerializerTest<Employee>(Employee.JamesTKirk, Iterations),
            new ProtobufSerializerTest<Employee>(Employee.JamesTKirk, Iterations),
            new XmlDataContractSerializerTest<Employee>(Employee.JamesTKirk, Iterations),
            new XmlSerializerTest<Employee>(Employee.JamesTKirk, Iterations)
        };
  
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            var formsHost = new System.Windows.Forms.Integration.WindowsFormsHost
                                {
                                    Child = _propertyGrid
                                };
            FormsHostContainer.Child = formsHost;
            DataGrid.ItemsSource = _testCollection;
            DataGrid.Columns[1].Visibility = Visibility.Hidden;
            DataGrid.Columns[2].Visibility = Visibility.Hidden;
            DataGrid.Columns[3].Visibility = Visibility.Hidden;
            DataGrid.Columns[6].Visibility = Visibility.Hidden;
            DataGrid.Columns[7].Visibility = Visibility.Hidden;

            StatusText.Text = string.Format(CultureInfo.InvariantCulture, "Executing serialization performance test...");
            foreach (var item in _testCollection)
            {
                item.Execute();
            }
            StatusText.Text = string.Format(CultureInfo.InvariantCulture, "{0} iterations executed.", Iterations);
            DataGrid.SelectedIndex = 3;
        }

        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DataGrid.SelectedItem as SerializerTest<Employee>;
            if (item != null && !string.IsNullOrWhiteSpace(item.SerializedDataStringView))
            {
                // Perf is not a consideration for this app an all data view operations are
                // performed on UI thread so show wait cursor in case of large object
                // graphs w/ lots of data to display.
                Cursor = System.Windows.Input.Cursors.Wait;
                ViewControl.BindData(item.SerializedInstance, item.SerializerType);
                Cursor = System.Windows.Input.Cursors.Arrow;
            }
            _propertyGrid.SelectedObject = item == null ? null : item.DeserializedInstance;
        }
    }
}
