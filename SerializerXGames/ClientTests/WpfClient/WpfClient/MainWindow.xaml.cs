using System.Collections.ObjectModel;
using System.ComponentModel;
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
            StatusText.Text = string.Format(CultureInfo.InvariantCulture, "Executing serialization performance test...");

            // A different data type can be swapped for Employee to measure
            // performance on it. Templated because some of the serializers
            // have only templated deserializer methods.

            
            var i = Employee.JamesTKirk;
            var c = new Collection<SerializerTest<Employee>>
            {
                new BinaryFormatterSerializerTest<Employee>(i, Iterations),
                new JsonDataContractSerializerTest<Employee>(i, Iterations),
                new JsonNewtonsoftSerializerTest<Employee>(i, Iterations),
                new ProtobufSerializerTest<Employee>(i, Iterations),
                new XmlDataContractSerializerTest<Employee>(i, Iterations),
                new XmlSerializerTest<Employee>(i, Iterations)
            };
            
            /*
            var i = TelemetryLog.SampleData;
            var c = new Collection<SerializerTest<TelemetryLog>>
            {
                new ProtobufSerializerTest<TelemetryLog>(i, Iterations),
                new BinaryFormatterSerializerTest<TelemetryLog>(i, Iterations),
                new JsonDataContractSerializerTest<TelemetryLog>(i, Iterations),
                new JsonNewtonsoftSerializerTest<TelemetryLog>(i, Iterations),
                new XmlDataContractSerializerTest<TelemetryLog>(i, Iterations),
                new XmlSerializerTest<TelemetryLog>(i, Iterations)
            };
            */
            foreach (var test in c)
            {
                test.Execute();
            }

            DataGrid.ItemsSource = c;
            DataGrid.SelectedIndex = 0;
            StatusText.Text = string.Format(CultureInfo.InvariantCulture, "{0} iterations of type {1} executed.", Iterations, c[0].ClassName);
        }

        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DataGrid.SelectedItem as IDataView;
            if (item != null && !string.IsNullOrWhiteSpace(item.SerializedDataStringView))
            {
                // Perf is not a consideration for this app an all data view operations are
                // performed on UI thread so show wait cursor in case of large object
                // graphs w/ lots of data to display.
                Cursor = System.Windows.Input.Cursors.Wait;
                ViewControl.BindData(item.SerializedInstance, item.SerializerType);
                Cursor = System.Windows.Input.Cursors.Arrow;
            }
            _propertyGrid.SelectedObject = item == null ? null : item.DeserializedObject;
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
                e.Cancel = true;
        }
    }
}
