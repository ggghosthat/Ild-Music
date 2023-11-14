using Ild_Music.ViewModels.Base;
using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Ild_Music.Views.FactorySubViews
{
    [DoNotNotifyAttribute]
    public partial class SubPlaylist : UserControl
    {
        public SubPlaylist()
        {
            InitializeComponent();
        }
    }
}