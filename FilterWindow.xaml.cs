﻿using System;
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
    /// Window1.xaml etkileşim mantığı
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();
            Vars.filterWindow = this;
            CheckboxBPM.IsChecked = Vars.BPMFilterEnabled;
            BPMStart.Text = Vars.BPMFilterStart.ToString();
            BPMEnd.Text = Vars.BPMFilterEnd.ToString();

            Left = Vars.main.Left + Vars.main.Width - 12;
            Top = Vars.main.Top;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Vars.BPMFilterEnabled == false)
            {
                BPMEnd.Visibility = Visibility.Hidden;
                BPMStart.Visibility = Visibility.Hidden;
                BPMLine.Visibility = Visibility.Hidden;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Vars.BPMFilterEnabled = true;
            BPMEnd.Visibility = Visibility.Visible;
            BPMStart.Visibility = Visibility.Visible;
            BPMLine.Visibility = Visibility.Visible;
            Vars.main.updateList();
        }
        private void CheckboxBPM_Unchecked(object sender, RoutedEventArgs e)
        {
            Vars.BPMFilterEnabled = false;
            BPMEnd.Visibility = Visibility.Hidden;
            BPMStart.Visibility = Visibility.Hidden;
            BPMLine.Visibility = Visibility.Hidden;
            Vars.main.updateList();
        }

        private void BPMEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckboxBPM.IsChecked == false)
                return;
            Vars.BPMFilterEnd = Helper.FailParse(BPMEnd.Text, 0);
            Vars.main.updateList();
        }

        private void BPMStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckboxBPM.IsChecked == false)
                return;
            Vars.BPMFilterStart = Helper.FailParse(BPMStart.Text, 0);
            Vars.main.updateList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Vars.filterWindow = null;
        }
    }
}
