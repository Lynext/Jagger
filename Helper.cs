using System;
using System.Collections.Generic;
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

        public static void SetID3(Song song, string mp3FilePath)
        {
            
        }
    }
}
