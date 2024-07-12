using Ild_Music.ViewModels;

using System;
using Avalonia.Input;
using Avalonia.Controls;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class ListView : UserControl
{
    private bool isListenScrollEvent = true;
    public ListView()
    {
        InitializeComponent();
    }

    private void OnScrollChanged(object sender, PointerWheelEventArgs e)
    {
        if (Math.Abs(e.Delta.Y) == e.Delta.Length && e.Delta.Y < 0 && isListenScrollEvent)
        {
            isListenScrollEvent = false;
            ((ListViewModel)DataContext).ExtendCurrentList().Wait();
            isListenScrollEvent = true;
        }
    }
}
