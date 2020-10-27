using CsvHelper;
using Microsoft.Win32;
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
            Vars.songList.Add(new Song() { Name = "Umrumda Değil", BPM = 140, Key = "2A" });
            Vars.songList.Add(new Song() { Name = "Paranoya", BPM = 140, Key = "1A" });
            Vars.songList.Add(new Song() { Name = "Neyim Var Ki", BPM = 90, Key = "11A" });
            SongList.ItemsSource = Vars.songList;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Vars.csvPath = openFileDialog.FileName;
                Vars.csvFolderPath = System.IO.Path.GetDirectoryName(Vars.csvPath);
                Vars.songList.Clear();
                using (var reader = new StreamReader(openFileDialog.FileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new Song
                        {
                            Name = csv.GetField("Name"),
                            BPM = csv.GetField<int>("BPM"),
                            Key = csv.GetField("Key"),
                            Artists = csv.GetField("Artists")
                        };
                        Vars.songList.Add(record);
                    }
                }
                
                updateList();
            }

            
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
            Application.Current.Shutdown();
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
            using (var writer = new StreamWriter(Vars.csvPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(Vars.songList);
            }
            bool successful = true;
            List<string> files = Directory.GetFiles(Vars.csvFolderPath).ToList<string>();
            foreach (Song i in Vars.songList)
            {
                int exists = 0;
                foreach (string x in files)
                {
                    if (x.ToLower().Contains(i.Name.ToLower()))
                    {
                        exists++;
                    }
                }
                if (exists == 0)
                {
                    MessageBox.Show("Couldn't find file containing name " + i.Name);
                    successful = false;
                    continue;
                }
                if (exists > 1)
                {
                    MessageBox.Show("Multiple files containing name " + i.Name);
                    successful = false;
                    continue;
                }

            }
            if (successful)
            {
                MessageBox.Show("Save successful.");
            }
            else
            {
                MessageBox.Show("Save unsuccessful.");
            }
        }
    }
}
