using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Ild_Music.Controllers;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System.Linq;
using Ild_Music.Controllers.ControllerServices;

namespace Ild_Music.UI
{
    /// <summary>
    /// Логика взаимодействия для AddTrack.xaml
    /// </summary>
    public partial class AddTrack : Window
    {
        private ModelFactoryController factory;
        private SynchSupporter synchSupporter;
        internal delegate void RefreshListContent();
        internal event RefreshListContent ListRefresh;

        private IList<Artist> _artists = null;
        

        public AddTrack(ModelFactoryController factory, ListWindow listWindow)
        {
            this.factory = factory;
            synchSupporter = listWindow.SynchSupporter;
            ListRefresh += new RefreshListContent(listWindow.SetList);
            _artists = listWindow.SynchSupporter.ExistedArtists; InitializeComponent();
            InitialImage();
            lvListProvider.ItemsSource = _artists.Select(a => a.Name);
            
           
        }


        private void InitialImage()
        {
            var bitImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/open_file.png"));

            var image = new System.Windows.Controls.Image();

            image.Stretch = Stretch.Fill;

            image.Source = bitImage;

            btnPath.Background = new ImageBrush(bitImage);

            var bitImage1 = new BitmapImage(new Uri(Environment.CurrentDirectory + "/plate1.png"));

            var image1 = new System.Windows.Controls.Image();

            image1.Stretch = Stretch.Fill;

            image1.Source = bitImage1;

            img.Background = new ImageBrush(bitImage1);
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



        private void btn_Click(object sender, RoutedEventArgs e)
        {
            factory.CreateTrack(pathway:txtPath.Text, name:string.Empty, description: txtDescription.Text);
            ListRefresh.Invoke();
            InitRelations();
            this.Close();
        }

        private void btnPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            txtPath.Text = dialog.FileName;
        }
               

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var index = lvListProvider.SelectedIndex;
            if (index < 0)
                index = index * -1;

            lvListRoot.Items.Add(_artists[index].Name);

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
                var artistsSelected = _artists.Where(x => x.Name.Equals(item)).ToList();
                
                foreach (var artist in artistsSelected)
                {
                    artist.AddTrack(factory.Track);
                    synchSupporter.EditInstanceObject(artist, EntityState.Artist);
                }
            }
        }

    }
}
