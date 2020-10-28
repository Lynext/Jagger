using System;
using System.Collections.Generic;
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
            for (int j = 0; j < Vars.songList.Count; j++)
            {
                Song i = Vars.songList[j];
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = i.FullName;
                if (i.checkFormat())
                    lbi.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF29805C"));
                else
                    lbi.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF802929"));
                lbi.PreviewMouseLeftButtonDown += clickedListBoxItem;
                lbi.Tag = j;
                SongList.Items.Add(lbi);
            }
        }

        public void clickedListBoxItem(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            saveCurrent();
            loadIndex((int)item.Tag);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadIndex(index);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Vars.songList[index].Name = NameBox.Text;
            Vars.songList[index].Artists = ArtistsBox.Text;
            Vars.main.updateList();
            Vars.editorWindow = null;
        }

        public void saveCurrent()
        {
            Vars.songList[index].Name = NameBox.Text;
            Vars.songList[index].Artists = ArtistsBox.Text;
            Vars.main.updateList();
            updateList();
        }

        public void loadIndex(int ind)
        {
            index = ind;
            NameBox.Text = Vars.songList[index].Name;
            ArtistsBox.Text = Vars.songList[index].Artists;
            BPMLabel.Content = Vars.songList[index].BPM.ToString() + " BPM";
            KeyLabel.Content = Vars.songList[index].Key;
            IndexLabel.Content = (index + 1).ToString() + " / " + Vars.songList.Count.ToString();
            if (Vars.songList[index].checkFormat())
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
            if (index == Vars.songList.Count)
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
                index = Vars.songList.Count - 1;
            }
            loadIndex(index);
        }
    }
}
