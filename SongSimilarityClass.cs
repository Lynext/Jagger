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

        public string BPMString { get {
                int mn = Math.Min(similarSong.BPM, baseSong.BPM);
                int mx = Math.Max(similarSong.BPM, baseSong.BPM);
                int firstDiff = Math.Abs(mx - mn);
                int secondDiff = Math.Abs(mx / 2 - mn);
                // FOR DEBUG
                int thirdDiff = Math.Abs(mn * 2 - mx);
                if (firstDiff <= secondDiff)
                {
                    return BPM.ToString();
                }
                if (secondDiff <= thirdDiff)
                {
                    if (mn == similarSong.BPM)
                    {
                        return BPM.ToString() + " (B " + (mx / 2).ToString() + ")";
                    }
                    return BPM.ToString() + " (" + (mx / 2).ToString() + ")";
                }
                return "!!!!!";
            } set { } }
        public string Key { get { return similarSong.Key; } set { } }
        public string Artists { get { return similarSong.Artists; } set { } }
        public int BPMDifference { get {
                int mn = Math.Min(similarSong.BPM, baseSong.BPM);
                int mx = Math.Max(similarSong.BPM, baseSong.BPM);
                return Math.Min(Math.Abs(mx - mn), Math.Abs(mx / 2 - mn));
            } set { } }
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
