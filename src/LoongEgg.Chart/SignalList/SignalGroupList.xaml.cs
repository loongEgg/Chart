﻿using LoongEgg.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    /// <summary>
    /// SignalGroupList.xaml 的交互逻辑
    /// </summary>
    public partial class SignalGroupList : UserControl
    {
        public SignalGroupList()
        {
            InitializeComponent();
        }
         
        [Description("")]
        public ObservableCollection<SignalGroup> SignalGroups
        {
            get { return (ObservableCollection<SignalGroup>)GetValue(SignalGroupsProperty); }
            set { SetValue(SignalGroupsProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="SignalGroups"/>
        /// </summary>
        public static readonly DependencyProperty SignalGroupsProperty = DependencyProperty.Register
            (
                nameof(SignalGroups),
                typeof(ObservableCollection<SignalGroup>),
                typeof(SignalGroupList),
                new PropertyMetadata(default(ObservableCollection<SignalGroup>))
            );

    }
}
