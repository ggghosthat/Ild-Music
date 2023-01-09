using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace Ild_Music.Views
{
    [DoNotNotifyAttribute]
    public partial class StartView : UserControl
    {
        public StartView()
        {
            InitializeComponent();
        }
    }
}