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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DS2S_META
{
    /// <summary>
    /// Interaction logic for metaHotkey.xaml
    /// </summary>
    public partial class HotkeyControl : UserControl
    {
        public string HotkeyName
        {
            get { return (string)GetValue(HotkeyNameProperty); }
            set { SetValue(HotkeyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HotkeyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HotkeyNameProperty =
            DependencyProperty.Register("HotkeyName", typeof(string), typeof(HotkeyControl), new PropertyMetadata(default));

        public HotkeyControl()
        {
            InitializeComponent();
        }
    }
}
