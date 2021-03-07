using LoongEgg.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoongEgg.Chart
{
    /// <summary>
    /// SignalList.xaml 的交互逻辑
    /// </summary>
    public partial class SignalList : UserControl
    {
        public SignalList()
        {
            Focusable = true;
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
