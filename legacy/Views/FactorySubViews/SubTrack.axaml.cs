using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;

namespace Ild_Music.Views.FactorySubViews
{
    [DoNotNotifyAttribute]
    public partial class SubTrack : UserControl
    {
        public SubTrack()
        {
            InitializeComponent();
        }
    }
}