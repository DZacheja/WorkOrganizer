using System.Windows.Controls;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        private SettingsModel settingsVM;
        public SettingsView() {
            settingsVM = SettingsModel.Instance;
            this.DataContext = settingsVM;
            InitializeComponent();
        }

        

    }
}
