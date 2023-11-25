using Avalonia.Platform.Storage;
using Avalonia.Interactivity;
using Avalonia.Controls;
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

        private async void BrowseAvatarFile(object sender, RoutedEventArgs args)
        {
            var topLevel = TopLevel.GetTopLevel(this);
        
            var files = await topLevel?.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Your music file",
                AllowMultiple = true,
                FileTypeFilter = new[] { ImageAll }
            });
            if(files.Count >= 1)
            {}
        }

        public static FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp" },
            MimeTypes = new[] { "image/*" }
        };
    }
}
