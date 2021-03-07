using LoongEgg.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoongEgg.Chart
{
    /// <summary>
    /// TimerChartList.xaml 的交互逻辑
    /// </summary>
    public partial class TimerChartList : UserControl
    {
        public TimerChartList()
        {
            InitializeComponent();
            Focusable = true;

            KeyDown += TimerChartList_KeyDown;
        }

        private void TimerChartList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                if (SelectedGroup != null && SignalGroups != null)
                    SignalGroups.Remove(SelectedGroup);

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
                typeof(TimerChartList),
                new PropertyMetadata(default(ObservableCollection<SignalGroup>))
            );

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public SignalGroup SelectedGroup
        {
            get { return (SignalGroup)GetValue(SelectedGroupProperty); }
            set { SetValue(SelectedGroupProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="SelectedGroup"/>
        /// </summary>
        public static readonly DependencyProperty SelectedGroupProperty =
            DependencyProperty.Register(
                nameof(SelectedGroup),
                typeof(SignalGroup),
                typeof(TimerChartList),
                new PropertyMetadata(default(SignalGroup)));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (button == null) return;

            if (button.Content.ToString() == "+")
            {
                if (SignalGroups != null)
                    SignalGroups.Add(new SignalGroup());
            }
            else if (button.Content.ToString() == "-")
            {
                if (SignalGroups != null && SignalGroups.Count > 1)
                    SignalGroups.RemoveAt(SignalGroups.Count - 1);
            }
        }
    }
}
