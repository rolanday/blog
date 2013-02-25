using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using ClientTest.Serializers;

namespace ClientTest.Controls
{
    /// <summary>
    /// Interaction logic for SerializedDataViewControl.xaml
    /// </summary>
    public partial class SerializedDataViewControl
    {
        private XmlDocument _xmldocument;
        public SerializedDataViewControl()
        {
            InitializeComponent();
        }

        public void BindData(byte[] data, SerializerType serializerType)
        {
            string viewType;
            var encoding = new UTF8Encoding(false);
            switch (serializerType)
            {
                case SerializerType.Xml:
                case SerializerType.XmlDataContract:
                    viewType = "xml";
                    _xmldocument = new XmlDocument();
                    _xmldocument.LoadXml(encoding.GetString(data, 0, data.Length));
                    var provider = new XmlDataProvider
                                       {
                                           Document = _xmldocument
                                       };
                    var binding = new Binding
                                      {
                                          Source = provider, XPath = "child::node()"
                                      };
                    XmlView.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                    break;
                case SerializerType.JsonDataContract:
                case SerializerType.JsonNewtonsoft:
                    viewType = "json";
                    XmlView.SetBinding(ItemsControl.ItemsSourceProperty, new Binding());
                    JsonView.Text = encoding.GetString(data, 0, data.Length);
                    break;
                default:
                    viewType = "binary";
                    BinaryView.BindData(data);
                    break;
            }

            foreach (TabItem item in ViewTabControl.Items)
            {
                var tag = item.Tag as string;
                Debug.Assert(!String.IsNullOrWhiteSpace(tag));

                if (string.Compare(tag, viewType, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ViewTabControl.SelectedItem = item;
                }
            }
        }
    }
}
