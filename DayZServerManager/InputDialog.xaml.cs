using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DayZServerManager
{
    /// <summary>
    /// Interaktionslogik für ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow
    {
        public ParameterWindow(string title, string caption, string defaultInput)
        {
            InitializeComponent();
            this.Title = title;
            this.parameterLabel.Content = caption;
            this.textbox.Text = defaultInput;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
