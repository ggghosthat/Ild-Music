using System.Windows;
using System.Linq;
using System;
using System.Windows.Controls;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using ShareInstances.PlayerResources;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl, IFactorySubControl
    {
        #region Fields
        private SupporterService supporter;
        #endregion

        #region Properties
        public string Header { get; init; } = "Playlist";
        #endregion

        #region Events
        private event Action OnCheckInstance;
        #endregion

        #region Const
        public FacPlaylistSubControl()
        {
            InitializeComponent();
            CheckInstance();
        }
        #endregion

        #region Private Methods
        private void CheckInstance()
        {
            var subControlVM = (SubControlViewModel)DataContext;
            supporter = subControlVM.Supporter;

            if (subControlVM.Instance is Playlist playlistInstance)
            {
                txtName.Text = playlistInstance.Name;
                txtDescription.Text = playlistInstance.Description;

                foreach (var artist in supporter.ArtistSup)
                    artist.Playlists.ToList().ForEach(p => 
                    {
                        if (p.Id.Equals(playlistInstance.Id))
                            lvArtistsRoot.Items.Add(artist.Name);
                    });                
            }
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            subControlVM.CreatePlaylistInstance(values);
        }
        #endregion

        #region Public Methods
        public void InvokeCheckInstance() =>
            OnCheckInstance?.Invoke();
        #endregion
    }
}
