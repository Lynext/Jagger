using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Jagger
{
    /// <summary>
    /// SongInfoWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class SongInfoWindow : Window
    {
        public Song loadedSong = null;
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isPlaying = false;
        private DispatcherTimer timer;
        public SongInfoWindow()
        {
            InitializeComponent();
            Vars.songInfoWindow = this;
            mediaPlayer.MediaOpened += PreviewMedia_MediaOpened;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void updateList()
        {
            CollectionViewSource.GetDefaultView(SongSimilarityList.ItemsSource).Refresh();
        }

        private void lvUsersColumnHeader_Click(object sender, object e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
            }
            SongSimilarityList.Items.SortDescriptions.Clear();
            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            SongSimilarityList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        public void loadSong(Song song)
        {
            loadedSong = song;
            List<SongSimilarityClass> songSimilarityList = new List<SongSimilarityClass>();
            foreach (Song i in Vars.songList)
            {
                if (i == loadedSong)
                    continue;
                SongSimilarityClass ssc = new SongSimilarityClass();
                ssc.baseSong = loadedSong;
                ssc.similarSong = i;
                songSimilarityList.Add(ssc);
            }
            SongSimilarityList.ItemsSource = songSimilarityList;
            Title = song.FullName;
            SongNameLabel.Content = song.FullName;
            BPMLabel.Content = song.BPM.ToString() + " BPM";
            KeyLabel.Content = song.Key;
            AlbumImage.Source = song.Image;
            if (isPlaying)
            {
                mediaPlayer.Pause();
                PlayButton.Content = "Play";
                isPlaying = false;
            }
            SongPosition.Value = 0;
            mediaPlayer.Open(new Uri(song.path));

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SongSimilarityList.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("similarityPercent", ListSortDirection.Descending));
            updateList();
        }

        void PreviewMedia_MediaOpened(object sender, object e)
        {
            SongPosition.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            //TimeLabel.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Vars.songInfoWindow = null;
            timer.Stop();
            mediaPlayer.Close();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            SongPosition.Value = mediaPlayer.Position.TotalSeconds;
            //TimeLabel.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying == false)
            {
                isPlaying = true;
                (sender as Button).Content = "Pause";
                mediaPlayer.Play();
            }
            else
            {
                (sender as Button).Content = "Play";
                mediaPlayer.Pause();
                isPlaying = false;
            }
        }

        private void SongPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue - e.OldValue != 1)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
                TimeLabel.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
        }

        private void VolumePosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = VolumePosition.Value;
        }

        private void SongSimilarityList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SongSimilarityClass item = (SongSimilarityClass)(((ListView)sender).SelectedItem);
            if (item != null)
            {
                loadSong(item.similarSong);
            }
        }
    }
}
