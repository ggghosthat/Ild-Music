using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class MainStageControl : System.Windows.Controls.UserControl
    {
        private string playerPath;
        private string synchPath;

        private static FolderBrowserDialog dialog = new();
       
        public MainStageControl()
        {
            InitializeComponent();
            StagePathPrecolor();
            DataContext = new StageViewModel();
        }

        private void StagePathPrecolor()
        {
            txtPlayerPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtPlayerPath.Text = "Click twice to select player.";

            txtSynchPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtSynchPath.Text = "Click twice to select synch.";
        }

        private void Path2Player(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPlayerPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtPlayerPath.Text = dialog.SelectedPath;
            }
        }

        private void Path2Synch(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSynchPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtSynchPath.Text = dialog.SelectedPath;
            }
        }

        private void OnPlayerSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var playerIndex = lsPlayers.SelectedIndex;
            var stageVM = (StageViewModel)DataContext;

            App.mainPlatform.PlayerInstance = stageVM.PlayerList[playerIndex];
        }

        private void OnSynchAreaSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var synchIndex = lsSynch.SelectedIndex;
            var stageVM = (StageViewModel)DataContext;

            App.mainPlatform.SynchAreaInstance = stageVM.SynchList[synchIndex];
        }

        private void OnPlayerAdd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtPlayerPath.Text))
            {
                playerPath = txtPlayerPath.Text;
                var stageVM = (StageViewModel)DataContext;
                stageVM.SetPlayerPath(playerPath);
            }
        }


    }
}
