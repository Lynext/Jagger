using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jagger
{
    public static class Vars
    {
        public static string folderPath = "";
        public static MainWindow main = null;
        public static FilterWindow filterWindow = null;
        public static EditorWindow editorWindow = null;
        public static List<Song> songList = new List<Song>();
        public static bool unsavedContent = false;
        public static List<string> allArtists = new List<string>();

        // Filters
        public static bool BPMFilterEnabled = false;
        public static int BPMFilterStart = 0;
        public static int BPMFilterEnd = 200;
        public static List<string> artistsFilter = new List<string>();
    }
}
