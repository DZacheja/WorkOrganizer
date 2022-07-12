using DatabaseConnection.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private async void AddNewWork_Click(object sender, System.Windows.RoutedEventArgs e) {
            try {
                await nwpg.InsertNewWorkType();
                await showLblInfo("Pomyślnie dodano element do listy", "#00cc00");
            } catch (Exception ex) {
                await showLblInfo(ex.Message, "#DC143C");
            }
        }

        private async Task showLblInfo(string txt, string color) {
            _ = Task.Run(() => {
                this.lblInfo.Dispatcher.Invoke(new Action(async () => {
                    this.lblInfo.Text = txt;
                    var bc = new BrushConverter();
                    this.lblInfo.Foreground = (Brush)bc.ConvertFrom(color);
                    this.lblInfo.Visibility = System.Windows.Visibility.Visible;
                    await Task.Delay(3000);
                    this.lblInfo.Visibility = System.Windows.Visibility.Collapsed;
                }));
            });

        }

        private async void InsertNewWork_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (this.dateWorkEndDate.SelectedDate != null) {
                if (this.lstWorktypes.SelectedItems.Count > 0) {
                    var it = this.lstWorktypes.SelectedItems;
                    foreach (var es in it) {
                        nwpg.SelectedWorkTypes.Add((WorkType)es);
                    }
                    try {
                        await nwpg.InsertNewWork((DateTime)this.dateWorkEndDate.SelectedDate);
                        await showLblInfo("Pomyślnie dodano element do listy", "#00cc00");
                        MainModel mw = (MainModel)Application.Current.MainWindow.DataContext;
                        mw.RefreshPrincipals();
                    } catch (Exception ex) {
                        await showLblInfo(ex.Message, "#DC143C");
                    }
                }
            }
        }
    }
}
