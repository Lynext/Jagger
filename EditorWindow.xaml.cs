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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameBox.Text = Vars.songList[index].Name;
            ArtistsBox.Text = Vars.songList[index].Artists;
            BPMLabel.Content = Vars.songList[index].BPM.ToString() + " BPM";
            KeyLabel.Content = Vars.songList[index].Key;
            IndexLabel.Content = (index + 1).ToString() + " / " + Vars.songList.Count.ToString();
            Vars.main.updateList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Vars.songList[index].Name = NameBox.Text;
            Vars.songList[index].Artists = ArtistsBox.Text;
            Vars.main.updateList();
            Vars.editorWindow = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Vars.songList[index].Name = NameBox.Text;
            Vars.songList[index].Artists = ArtistsBox.Text;

            index++;
            if (index == Vars.songList.Count)
            {
                index = 0;
            }
            NameBox.Text = Vars.songList[index].Name;
            ArtistsBox.Text = Vars.songList[index].Artists;
            BPMLabel.Content = Vars.songList[index].BPM.ToString() + " BPM";
            KeyLabel.Content = Vars.songList[index].Key;
            IndexLabel.Content = (index + 1).ToString() + " / " + Vars.songList.Count.ToString();
            Vars.main.updateList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Vars.songList[index].Name = NameBox.Text;
            Vars.songList[index].Artists = ArtistsBox.Text;

            index--;
            if (index == -1)
            {
                index = Vars.songList.Count - 1;
            }
            NameBox.Text = Vars.songList[index].Name;
            ArtistsBox.Text = Vars.songList[index].Artists;
            BPMLabel.Content = Vars.songList[index].BPM.ToString() + " BPM";
            KeyLabel.Content = Vars.songList[index].Key;
            IndexLabel.Content = (index + 1).ToString() + " / " + Vars.songList.Count.ToString();
            Vars.main.updateList();
        }
    }
}
