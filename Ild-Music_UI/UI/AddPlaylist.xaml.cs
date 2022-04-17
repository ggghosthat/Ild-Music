using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ild_Music.Controllers;
using Ild_Music.Controllers.ControllerServices;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Microsoft.Win32;

namespace Ild_Music.UI
{
    /// <summary>
    /// Логика взаимодействия для AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        private ModelFactoryController factory;
        private SynchSupporter synchSupporter;
        internal delegate void RefreshListContent();
        internal event RefreshListContent ListRefresh;


        private IList<Artist> lsArtists;


        public AddPlaylist(ModelFactoryController factory, ListWindow listWindow)
        {
            this.factory = factory;
            ListRefresh += new RefreshListContent(listWindow.SetList);
            synchSupporter = listWindow.SynchSupporter;
            lsArtists = listWindow.SynchSupporter.ExistedArtists;
            InitializeComponent();
            lvListProvider.ItemsSource = lsArtists.Select(a => a.Name);
            InitialImage();
        }


        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            txSelectImage.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void txSelectImage_MouseLeave(object sender, MouseEventArgs e)
        {
            txSelectImage.Foreground = new SolidColorBrush(Colors.White);
        }

        private void txSelectImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                var bitImage = new BitmapImage(new Uri(dialog.FileName));

                var image = new System.Windows.Controls.Image();

                image.Stretch = Stretch.Fill;

                image.Source = bitImage;

                img.Background = new ImageBrush(bitImage);
            }
        }

        private void InitialImage()
        {
            var bitImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/mus.png")); 

            var image = new System.Windows.Controls.Image();

            image.Stretch = Stretch.Fill;

            image.Source = bitImage;

            img.Background = new ImageBrush(bitImage);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            factory.CreatePlaylist(name:txtName.Text, description:txtDescription.Text);
            ListRefresh.Invoke();
            InitRelations();
            this.Close();
        }

        

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var index = lvListProvider.SelectedIndex;
            if (index < 0) 
                index = index * -1;

            lvListRoot.Items.Add(lsArtists[index].Name);
                    
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = lvListRoot.SelectedItem;
            lvListRoot.Items.Remove(item);            
        }

        private void InitRelations()
        {
            foreach (var item in lvListRoot.Items) 
            {
                var artistsSelected = lsArtists.Where(x => x.Name.Equals(item)).ToList();

                foreach (var artist in artistsSelected) 
                {
                    artist.AddTracklist(factory.Playlist);
                    synchSupporter.EditInstanceObject(artist, EntityState.Artist);
                }
            }
        }
    }
}
