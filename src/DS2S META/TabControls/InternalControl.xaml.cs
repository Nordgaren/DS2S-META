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
    /// Interaction logic for InternalControl.xaml
    /// </summary>
    public partial class InternalControl : METAControl
    {
        public InternalControl()
        {
            InitializeComponent();
        }

        internal override void EnableCtrls(bool enable)
        {
            cbxSpeeds.IsEnabled = enable;
        }
    }
}
