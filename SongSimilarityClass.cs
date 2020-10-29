using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jagger
{
    public class SongSimilarityClass
    {
        public Song baseSong;
        public Song similarSong;
        public string Name { get { return similarSong.Name; } set { } }
        public int BPM { get { return similarSong.BPM; } set { } }
        public string Key { get { return similarSong.Key; } set { } }
        public string Artists { get { return similarSong.Artists; } set { } }
        public int BPMDifference { get { return Math.Abs(similarSong.BPM - baseSong.BPM); } set { } }
        public int keyDifference { get { return Helper.getKeyDiff(baseSong.Key, similarSong.Key); } set { } }
        public int similarityPercent
        {
            get
            {
                int percent = 100;
                percent -= 3 * BPMDifference;
                percent -= 5 * keyDifference;
                if (baseSong.Key.Contains("A") && similarSong.Key.Contains("B"))
                    percent -= 5;
                if (baseSong.Key.Contains("B") && similarSong.Key.Contains("A"))
                    percent -= 5;
                return percent;
            }
            set { }
        }
    }
}
