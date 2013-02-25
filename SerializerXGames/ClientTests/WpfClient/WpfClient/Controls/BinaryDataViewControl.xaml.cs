using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ClientTest.Controls
{
    /// <summary>
    /// Interaction logic for BinaryDataViewControl.xaml
    /// </summary>
    public partial class BinaryDataViewControl
    {
        private readonly ObservableCollection<byte> _data = new ObservableCollection<byte>(); 

        public BinaryDataViewControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            HexView.ItemsSource = _data;
        }

        public void BindData(byte[] data)
        {

            _data.Clear();
            if (data != null && data.Length > 0)
            {
                var encoding = new UTF8Encoding(false);
                TextView.Text = encoding.GetString(
                    data,
                    0,
                    data.Length
                    );
                foreach (var b in data)
                {
                    _data.Add(b);
                }
            }
            else
            {
                TextView.Text = string.Empty;
            }
        }

    }

    public class ByteToHexViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (byte)value;

            return string.Format("{0:x2}", b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }


}
