using System.Linq;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Windows.Forms;
using System.Windows.Media;
using System;
using ShareInstances.PlayerResources;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : System.Windows.Controls.UserControl, IFactorySubControl
    {
        #region Fields
        private SupporterService supporter;
        #endregion


        #region Properties
        public string Header { get; init; } = "Track";
        #endregion

        #region Events
        private event Action OnCheckInstance;
        #endregion

        public FacTrackSubControl()
        {
            InitializeComponent();
            OnCheckInstance += CheckInstance;
            PathFieldDecorate();
        }


        private void CheckInstance()
        {
            var subControlVM = (SubControlViewModel)DataContext;
            supporter = subControlVM.Supporter;

            if (subControlVM.Instance is Track trackInstance)
            {
                txtPath.Text = trackInstance.Pathway;
                txtName.Text = trackInstance.Name;
                txtDescription.Text = trackInstance.Description;

                foreach (var artist in supporter.ArtistSup)
                    artist.Tracks.ToList().ForEach(t =>
                    {
                        if (t.Id.Equals(trackInstance.Id))
                            lvArtistsRoot.Items.Add(artist.Name);
                    });                

                foreach (var playlist in supporter.PlaylistSup)
                    playlist.Tracks.ToList().ForEach(t =>
                    {
                        if (t.Id.Equals(trackInstance.Id))
                            lvPlaylistsRoot.Items.Add(playlist.Name);
                    });
                
            }
        }

        private void PathFieldDecorate()
        {
            txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtPath.Text = "Click twice to select your track.";
        }

        private void Path2Track(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtPath.Text = dialog.FileName;
            }
        }




        private void TrackFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;

            object[] values = { txtPath.Text, txtName.Text, txtDescription.Text, lvArtistsRoot.SelectedIndex, lvPlaylistsRoot.SelectedIndex};
            
            subControlVM.CreateTrackInstance(values);
        }





        public void InvokeCheckInstance() =>
            OnCheckInstance?.Invoke();
    }
}
