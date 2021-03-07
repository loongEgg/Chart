using LoongEgg.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LoongEgg.Chart
{
    /// <summary>
    /// TimerChartLegend.xaml 的交互逻辑
    /// </summary>
    public partial class TimerChartLegend : UserControl
    {
        public TimerChartLegend()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public SignalGroup SignalGroup
        {
            get { return (SignalGroup)GetValue(SignalGroupProperty); }
            set { SetValue(SignalGroupProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="SignalGroup"/>
        /// </summary>
        public static readonly DependencyProperty SignalGroupProperty =
            DependencyProperty.Register(
                nameof(SignalGroup),
                typeof(SignalGroup),
                typeof(TimerChartLegend),
                new PropertyMetadata(default(SignalGroup)));
 
    }
}
