using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Jagger
{
    public static class Helper
    {
        public static int FailParse(string s, int fail)
        {
            int rtn = fail;
            int.TryParse(s, out rtn);
            return rtn;
        }

        public static int getKeyInt(string key)
        {
            return Int32.Parse(key.Substring(0, key.Length - 1));
        }

        public static int getKeyDiff(string key1, string key2)
        {
            int k1 = getKeyInt(key1);
            int k2 = getKeyInt(key2);
            int diff1 = Math.Abs(k1 - k2);
            int diff2 = Math.Abs(12 - diff1);
            return Math.Min(diff1, diff2);
        }


        public static void SetID3(Song song, string filePath)
        {
            string newPath = Vars.folderPath + "/" + song.FullName + Path.GetExtension(filePath);
            File.Move(filePath, newPath);

            filePath = newPath;
            song.path = newPath;
            var tfile = TagLib.File.Create(filePath);
            tfile.Tag.Title = song.Name;
            tfile.Tag.Performers = new string[1] { song.Artists };
            tfile.Save();
        }

        public static Song getFromFile(string filePath)
        {
            Song song = new Song();
            var tfile = TagLib.File.Create(filePath);
            song.path = filePath;
            song.BPM = (int)tfile.Tag.BeatsPerMinute;
            song.Name = tfile.Tag.Title;
            song.Key = tfile.Tag.InitialKey;

            TagLib.IPicture pic = tfile.Tag.Pictures[0];
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            song.Image = bitmap;

            if (tfile.Tag.Performers.Length > 0)
                song.Artists = tfile.Tag.Performers[0];
            return song;
        }

        public static void calculateAllArtists()
        {
            Vars.allArtists.Clear();
            foreach (Song i in Vars.songList)
            {
                foreach (string x in i.ArtistsList)
                {
                    if (!string.IsNullOrWhiteSpace(x) && !Vars.allArtists.Contains(x))
                        Vars.allArtists.Add(x);
                }
            }
        }
    }
}
