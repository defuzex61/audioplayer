using System;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace audioplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        DispatcherTimer timer = new DispatcherTimer();


        bool isPlaying;
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(musicSlider.Value));
        }

      
        public MainWindow()
        {
            InitializeComponent();
            BitmapImage bitmapImage = new BitmapImage(new Uri("play.png", UriKind.Relative));
            playButtonBrush.ImageSource = bitmapImage;
            media.Volume = 0.5;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            Thread thread = new Thread(_ =>
            {
                while (true)
                {

                    Dispatcher.Invoke(() => { 
                        musicSlider.Value = media.Position.Ticks;
                    });
                    Thread.Sleep(1000);

                }
            });
            thread.Start();

        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            pauseMedia();
            Files_list.SelectedIndex += 1;
            string path = dialog.SelectedPath.ToString() + "\\" + Files_list.SelectedValue.ToString();
            media.Source = new Uri(path);
            if (isPlaying)
            {
                startMedia();
            }

        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            pauseMedia();
            Files_list.SelectedIndex -= 1;
            string path = dialog.SelectedPath.ToString() + "\\" + Files_list.SelectedValue.ToString();
            media.Source = new Uri(path);
            if (isPlaying)
            {
                startMedia();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            dialog.ShowDialog();
            choose_path.Content = dialog.SelectedPath;
            Files_list.Items.Clear();
            foreach (var file in Directory.GetFiles(dialog.SelectedPath))
            {
                Files_list.Items.Add(System.IO.Path.GetFileName(file));
            }
        }
        private void startMedia()
        {
            media.Play();
            isPlaying = true;
            
        }

        private void pauseMedia()
        {
            media.Pause();
            isPlaying = false;
            

        }
        private void Files_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = dialog.SelectedPath.ToString() + "\\" + Files_list.SelectedValue.ToString();
            media.Source = new Uri(path);
            media.Volume = 0.5;
            startMedia();
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            musicSlider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                pauseMedia();

                        BitmapImage bitmapImage = new BitmapImage(new Uri("pause1.png", UriKind.Relative));
                        Dispatcher.BeginInvoke(new Action(() => {
                            playButtonBrush.ImageSource = bitmapImage;
                        }));

            }
            else
            {
                startMedia();

                        BitmapImage bitmapImage = new BitmapImage(new Uri("play.png", UriKind.Relative));
                        Dispatcher.BeginInvoke(new Action(() => {
                            playButtonBrush.ImageSource = bitmapImage;
                        }));

               
            }
        }

       

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = volumeSlider.Value / 100.0;
        }



    }
}
