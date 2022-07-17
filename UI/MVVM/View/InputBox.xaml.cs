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

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window {
        public string? Results;
        public InputBox(string info, string title = null) {
            InitializeComponent();
            if (title != null) {
                this.txtTitle.Text = title;
            }
            this.txtInfo.Text = info;
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            Results = this.txtResults.Text;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            Results = null;
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }
    }
}
