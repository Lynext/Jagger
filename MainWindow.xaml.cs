using CsvHelper;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jagger
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public MainWindow()
        {
            InitializeComponent();
            Vars.main = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.FolderLocation == "None")
            {
                if (!selectMainFolder())
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            else
            {
                Vars.folderPath = (string)Properties.Settings.Default.FolderLocation;
                loadFromFolder();
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SongList.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            view.Filter = UserFilter;
            updateList();
        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                SongList.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            SongList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            
        }

        public void updateList()
        {
            CollectionViewSource.GetDefaultView(SongList.ItemsSource).Refresh();
            int percent = (int)(((float)SongList.Items.Count / (float)Vars.songList.Count) * 100.0f);
            resultNumberLabel.Content = "Results : " + SongList.Items.Count + " / " + Vars.songList.Count.ToString() + " (%" + percent.ToString() + ")";
        }

        private bool UserFilter(object item)
        {
            // Text Filter
            if (!string.IsNullOrEmpty(SearchBox.Text) && (item as Song).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) < 0)
            {
                return false;
            }
            if (Vars.BPMFilterEnabled == true && ((item as Song).BPM < Vars.BPMFilterStart || (item as Song).BPM > Vars.BPMFilterEnd))
            {
                return false;
            }
            if (Vars.artistsFilter.Count > 0)
            {
                bool containsAny = false;
                foreach (string i in Vars.artistsFilter)
                {
                    if ((item as Song).ArtistsList.Contains(i))
                    {
                        containsAny = true;
                        break;
                    }
                }
                if (containsAny == false)
                    return false;
            }
            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
            {
                SearchPlaceholder.Visibility = Visibility.Visible;
            }
            else 
            {
                SearchPlaceholder.Visibility = Visibility.Hidden;
            }
            updateList();
        }

        public void loadFromFolder()
        {
            Vars.songList.Clear();
            List<string> files = Directory.GetFiles(Vars.folderPath).ToList();
            foreach (string i in files)
            {
                if (!(i.EndsWith(".mp3") || i.EndsWith(".m4a")))
                    continue;
                Song newSong = Helper.getFromFile(i);
                newSong.index = Vars.songList.Count();
                Vars.songList.Add(newSong);
            }
            Helper.calculateAllArtists();
            SongList.ItemsSource = Vars.songList;
            updateList();
        }

        public bool selectMainFolder()
        {
            var ookiiDialog = new VistaFolderBrowserDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                Vars.folderPath = ookiiDialog.SelectedPath;
                Properties.Settings.Default.FolderLocation = Vars.folderPath;
                Properties.Settings.Default.Save();
                loadFromFolder();
                return true;
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Vars.editorWindow != null || Vars.filterWindow != null)
            {
                MessageBox.Show("Close other windows before loading.", "Jagger", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectMainFolder();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Vars.filterWindow == null)
            {
                FilterWindow filterWindow = new FilterWindow();
                filterWindow.Show();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Vars.unsavedContent == false)
            {
                Application.Current.Shutdown();
                return;
            }
            MessageBoxResult dialogResult = MessageBox.Show("UNSAVED CONTENT. EXIT ?", "Jagger", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Vars.editorWindow == null)
            {
                EditorWindow editorWindow = new EditorWindow();
                editorWindow.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            foreach (Song i in Vars.songList)
            {
                Helper.SetID3(i, i.path);
            }
            MessageBox.Show("Save successful", "Jagger", MessageBoxButton.OK, MessageBoxImage.Information);
            Vars.unsavedContent = false;
        }
    }
}
