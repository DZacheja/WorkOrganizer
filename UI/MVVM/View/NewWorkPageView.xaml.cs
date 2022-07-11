using System.Windows.Controls;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for NewWorkPageView.xaml
    /// </summary>
    public partial class NewWorkPageView : UserControl {
        private NewWorkPageModel nwpg;
        public NewWorkPageView() {
            nwpg = NewWorkPageModel.Instance();
            this.DataContext = nwpg;
            InitializeComponent();
        }
    }
}
