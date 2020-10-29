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
        public SongInfoWindow()
        {
            InitializeComponent();
            Vars.songInfoWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SongSimilarityList.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("similarityPercent", ListSortDirection.Descending));
            updateList();
        }

        public void updateList()
        {
            CollectionViewSource.GetDefaultView(SongSimilarityList.ItemsSource).Refresh();
        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
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
            Title = song.FullName;
            SongNameLabel.Content = song.FullName;
            BPMLabel.Content = song.BPM.ToString() + " BPM";
            KeyLabel.Content = song.Key;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Vars.songInfoWindow = null;
        }
    }
}
