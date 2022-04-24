using Ild_Music_MVVM_.ViewModel.VM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{    
    public partial class FacPlaylistSubControl : UserControl
    {
        //TODO: reset DataContext connection
        private FactoryViewModel factoryViewModel;
        public FacPlaylistSubControl()
        {
            InitializeComponent();
            DataContext = factoryViewModel;
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            factoryViewModel.CreatePlaylistInstance(values);
        }
    }
}
