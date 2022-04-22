using Ild_Music_MVVM_.ViewModel.Base;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{ 
    public partial class FacArtistSubControl : UserControl
    {

        public FacArtistSubControl()
        {            
            InitializeComponent();
            DataContext = factoryVM.CurrentFactoryEntity
        }
    }
}
