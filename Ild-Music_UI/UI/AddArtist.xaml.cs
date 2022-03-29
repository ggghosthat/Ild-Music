using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ild_Music.Controllers;

namespace Ild_Music.UI
{
    /// <summary>
    /// Логика взаимодействия для AddArtist.xaml
    /// </summary>
    public partial class AddArtist : Window
    {
        private ModelFactoryController factory;
        internal delegate void RefreshListContent();
        internal event RefreshListContent ListRefresh;


        public AddArtist(ModelFactoryController factory, ListWindow listWindow)
        {
            this.factory = factory;
            ListRefresh += new RefreshListContent(listWindow.SetList);
            InitializeComponent();
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
            var bitImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "/open_file.png"));

            var image = new System.Windows.Controls.Image();

            image.Stretch = Stretch.Fill;

            image.Source = bitImage;

            img.Background = new ImageBrush(bitImage);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            factory.CreateArtist(name: txtName.Text, description: txtDescription.Text);
            ListRefresh.Invoke();
            this.Close();
        }
    }
}
