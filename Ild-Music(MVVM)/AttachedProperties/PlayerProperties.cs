using ShareInstances;
using System.Windows;

namespace Ild_Music_MVVM_.AttachedProperties
{
    internal class PlayerProperties
    {
        public static readonly DependencyProperty PlayerState = DependencyProperty
            .Register("PlayerState", typeof(PlayerState), typeof(IPlayer), new PropertyMetadata(ShareInstances.PlayerState.PAUSED));

        public static void SetPlayerState(DependencyObject element, PlayerState playerState) =>
            element.SetValue(PlayerState, playerState);

        public static PlayerState GetPlayerState(DependencyObject element) =>
            (ShareInstances.PlayerState)element.GetValue(PlayerState);
    }
}
