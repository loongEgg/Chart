using LoongEgg.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public SignalGroup TargetGroup
        {
            get { return (SignalGroup)GetValue(TargetGroupProperty); }
            set { SetValue(TargetGroupProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="TargetGroup"/>
        /// </summary>
        public static readonly DependencyProperty TargetGroupProperty =
            DependencyProperty.Register(
                nameof(TargetGroup),
                typeof(SignalGroup),
                typeof(SignalGroupList),
                new PropertyMetadata(default(SignalGroup)));


        private void SignalList_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TargetGroup != null)
            {
                var list = sender as SignalList;

                if (list != null)
                {
                    var selectedSignal = list.SelectedSignal;
                    if (selectedSignal != null && TargetGroup.Signals.Contains(selectedSignal) == false)
                    {
                        TargetGroup.Signals.Add(selectedSignal);
                    }
                }

            }
        }
    }
}
