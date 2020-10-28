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
    /// EditorWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class EditorWindow : Window
    {
        int index = 0;
        public EditorWindow()
        {
            InitializeComponent();
            Vars.editorWindow = this;
        }

        public void updateList()
        {
            SongList.Items.Clear();
            List<Song> songListSorted = Vars.songList.ToList();
            songListSorted.Sort((a, b) => a.checkFormat().CompareTo(b.checkFormat()));
            int correctCount = 0;
            for (int j = 0; j < songListSorted.Count; j++)
            {
                Song i = songListSorted[j];
                if (i.checkFormat())
                {
                    correctCount++;
                    if (OnlyShowWrong.IsChecked == true)
                        continue;
                }
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = i.FullName;
                if (i.checkFormat())
                    lbi.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF29805C"));
                else
                    lbi.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF802929"));
                lbi.PreviewMouseLeftButtonDown += clickedListBoxItem;
                lbi.Tag = new int[2] { i.index, SongList.Items.Count};
                SongList.Items.Add(lbi);
            }
            int percent = (int)(((float)correctCount / (float)Vars.songList.Count) * 100.0f);
            IndexLabel.Content = (index + 1).ToString() + " / " + SongList.Items.Count.ToString();
            CompletedLabel.Content = "%" + percent.ToString() + " Completed";
            CompletedBar.Value = percent;
        }

        public void clickedListBoxItem(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            saveCurrent();
            loadIndex(((int[])item.Tag)[1]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateList();
            loadIndex(index);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            saveCurrent();
            Vars.main.updateList();
            Vars.editorWindow = null;
        }

        public void saveCurrent()
        {
            if (index >= SongList.Items.Count)
            {
                Console.WriteLine("ERROR CANT SAVE, INDEX WAS BIGGER THAN SONGLIST COUNT");
                return;
            }
            int realSongIndex = ((int[])((ListBoxItem)SongList.Items[index]).Tag)[0];
            if (Vars.songList[realSongIndex].Name.ToString() != NameBox.Text.ToString() || Vars.songList[realSongIndex].Artists.ToString() != ArtistsBox.Text.ToString())
            {
                Console.WriteLine(Vars.songList[realSongIndex].Name + " | " + NameBox.Text);
                Console.WriteLine(Vars.songList[realSongIndex].Artists + " | " + ArtistsBox.Text);
                Vars.unsavedContent = true;
            }
            Vars.songList[realSongIndex].Name = NameBox.Text;
            Vars.songList[realSongIndex].Artists = ArtistsBox.Text;
            Vars.main.updateList();
            updateList();
            Helper.calculateAllArtists();
        }

        public void loadIndex(int ind)
        {
            index = ind;
            if (ind >= SongList.Items.Count)
            {
                Console.WriteLine("ERROR CANT LOAD, INDEX WAS BIGGER THAN SONGLIST COUNT");
                return;
            }
            int realSongIndex = ((int[])((ListBoxItem)SongList.Items[index]).Tag)[0];
            NameBox.Text = Vars.songList[realSongIndex].Name;
            ArtistsBox.Text = Vars.songList[realSongIndex].Artists;
            BPMLabel.Content = Vars.songList[realSongIndex].BPM.ToString() + " BPM";
            KeyLabel.Content = Vars.songList[realSongIndex].Key;
            IndexLabel.Content = (index + 1).ToString() + " / " + SongList.Items.Count.ToString();
            if (Vars.songList[realSongIndex].checkFormat())
            {
                OKImage.Source = new BitmapImage(new Uri(@"/Jagger;component/icons8-checkmark-48.png", UriKind.Relative));
            }
            else
            {
                OKImage.Source = new BitmapImage(new Uri(@"/Jagger;component/icons8-delete-64.png", UriKind.Relative));
            }
            Vars.main.updateList();
            updateList();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            saveCurrent();

            index++;
            if (index == SongList.Items.Count)
            {
                index = 0;
            }
            loadIndex(index);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            saveCurrent();

            index--;
            if (index == -1)
            {
                index = SongList.Items.Count - 1;
            }
            loadIndex(index);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            index = 0;
            loadIndex(index);
            updateList();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            index = 0;
            loadIndex(index);
            updateList();
        }
    }
}
