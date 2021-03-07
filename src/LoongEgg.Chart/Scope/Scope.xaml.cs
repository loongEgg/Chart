using LoongEgg.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Scope.xaml 的交互逻辑
    /// </summary>
    public partial class Scope : UserControl
    {
        public Scope()
        {
            InitializeComponent();

            MonitoredGroups = new ObservableCollection<SignalGroup>
            {
                new SignalGroup(),
                new SignalGroup(),
                new SignalGroup()
            };
        }


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public ObservableCollection<SignalGroup> SignalGroups
        {
            get { return (ObservableCollection<SignalGroup>)GetValue(SignalGroupsProperty); }
            set { SetValue(SignalGroupsProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="SignalGroups"/>
        /// </summary>
        public static readonly DependencyProperty SignalGroupsProperty =
            DependencyProperty.Register(
                nameof(SignalGroups),
                typeof(ObservableCollection<SignalGroup>),
                typeof(Scope),
                new PropertyMetadata(default(ObservableCollection<SignalGroup>)));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public ObservableCollection<SignalGroup> MonitoredGroups
        {
            get { return (ObservableCollection<SignalGroup>)GetValue(MonitoredGroupsProperty); }
            protected set { SetValue(MonitoredGroupsProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="MonitoredGroups"/>
        /// </summary>
        public static readonly DependencyProperty MonitoredGroupsProperty =
            DependencyProperty.Register(
                nameof(MonitoredGroups),
                typeof(ObservableCollection<SignalGroup>),
                typeof(Scope),
                new PropertyMetadata(default(ObservableCollection<SignalGroup>), OnMonitoredGroupsChanged));

        private static void OnMonitoredGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                var collection = e.OldValue as ObservableCollection<SignalGroup>;
                collection.CollectionChanged -= MonitoredGroups_CollectionChanged;
            }
            if (e.NewValue != null)
            {
                var collection = e.NewValue as ObservableCollection<SignalGroup>;
                for (int i = 0; i < collection.Count; i++)
                {
                    collection[i].Label = $"monitor {i}";
                }
                collection.CollectionChanged += MonitoredGroups_CollectionChanged; 
            }
        }

        private static void MonitoredGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var groups = sender as ObservableCollection<SignalGroup>;
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].Label = $"monitor {i}";
                }
            }
        }
    }
}
