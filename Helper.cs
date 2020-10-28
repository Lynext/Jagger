using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
