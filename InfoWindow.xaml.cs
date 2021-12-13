using System.Windows;
using System.Collections.ObjectModel;

namespace CourseWork
{
    public partial class InfoWindow : Window
    {
        public InfoWindow(ObservableCollection<ProcessStatistic> source)
        {
            InitializeComponent();
            ProcessesList.ItemsSource = source;
        }
    }
}
