#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace CourseWork
{
    public partial class ProcessWindow : Window
    {
        private readonly OS _os;


        public ProcessWindow(OS os)
        {
            InitializeComponent();
            _os = os;
            Priorities.SelectedItem = Default;
            PerformanceBox.SelectedItem = First;
        }

        private void Size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!char.IsDigit(Size.Text, 0) || ProcessName.Text == string.Empty)
            {
                MessageBox.Show("Invalid data");
                return;
            }
            var priority = byte.Parse((Priorities.SelectedItem as TextBlock).Text);
            var performance = Performance.Medium.ReverseToString(((TextBlock)PerformanceBox.SelectedItem).Text);
            var size = uint.Parse(Size.Text);
            Process process = new(0, ProcessName.Text, priority, performance, size);
            _os.RaiseNewProcess(process);
            Close();
        }
    }
}
