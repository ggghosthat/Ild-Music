using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class MainStageControl : System.Windows.Controls.UserControl
    {
        private static OpenFileDialog dialog = new();

        public MainStageControl()
        {
            InitializeComponent();
            StagePathPrecolor();
            DataContext = new StageViewModel();
        }

        private void StagePathPrecolor()
        {
            txtPlayerPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtPlayerPath.Text = "Click twice to select player dll.";

            txtSynchPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtSynchPath.Text = "Click twice to select synch dll.";
        }

        private void Path2Player(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPlayerPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtPlayerPath.Text = dialog.FileName;
            }
        }

        private void Path2Synch(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSynchPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtSynchPath.Text = dialog.FileName;
            }
        }

    }
}
