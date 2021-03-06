﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Jagger
{
    public class Song
    {
        public bool checkFormat()
        {
            if (string.IsNullOrWhiteSpace(Artists))
                return false;
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            string[] str = FullName.Split(new string[] { " - " }, StringSplitOptions.None);
            string artistName = str[0];
            string songName = str[1];
            if (str.Length > 2)
            {
                for (int i = 2; i < str.Length; i++)
                {
                    songName += " - " + str[i];
                }
            }
            if (artistName != Artists || songName != Name)
                return false;
            return true;
        }

        public string FullName {
            get { return Artists + " - " + Name; }
        }
        public string Name { get; set; } = "";

        public int BPM { get; set; } = 0;

        public string Key { get; set; } = "";
        public string path { get; set; } = "";
        public string Artists { get; set; } = "";

        public BitmapImage Image { get; set; } = null;

        public int index = 0;
        public List<string> ArtistsList {
            get {
                List<string> rtn = new List<string>();
                if (Artists.Contains(", "))
                {
                    string[] str = Artists.Split(new string[] { ", " }, StringSplitOptions.None);
                    rtn = str.ToList<string>();
                }
                else
                {
                    rtn.Add(Artists);
                }
                return rtn;
                } 
        }
    }
}
