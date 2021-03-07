using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using LoongEgg.Data;

namespace LoongEgg.Chart
{
    /// <summary>
    /// SignalList.xaml 的交互逻辑
    /// </summary>
    public partial class SignalList : UserControl
    {
        public SignalList()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (list.Visibility == Visibility.Collapsed)
            {
                list.Visibility = Visibility.Visible;
            }
            else
            {
                list.Visibility = Visibility.Collapsed;
            }
        }

        [Description("")]
        public SignalGroup SignalGroup
        {
            get { return (SignalGroup)GetValue(SignalGroupProperty); }
            set { SetValue(SignalGroupProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="SignalGroup"/>
        /// </summary>
        public static readonly DependencyProperty SignalGroupProperty = DependencyProperty.Register
            (
                nameof(SignalGroup),
                typeof(SignalGroup),
                typeof(SignalList),
                new PropertyMetadata(default(SignalGroup))
            );


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Signal SelectedSignal
        {
            get { return (Signal)GetValue(SelectedSignalProperty); }
            set { SetValue(SelectedSignalProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="SelectedSignal"/>
        /// </summary>
        public static readonly DependencyProperty SelectedSignalProperty =
            DependencyProperty.Register(
                nameof(SelectedSignal),
                typeof(Signal),
                typeof(SignalList),
                new PropertyMetadata(default(Signal)));

    }
}
