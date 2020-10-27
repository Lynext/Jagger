using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jagger
{
    public class Song
    {

        public string Name { get; set; }

        public int BPM { get; set; }

        public string Key { get; set; }

        public string Artists { get; set; }
        public List<string> ArtistsList { get; set; }
    }

    public sealed class SongMap : ClassMap<Song>
    {
        public SongMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ArtistsList).Ignore();
        }
    }
}
