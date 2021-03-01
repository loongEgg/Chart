using System.Windows.Controls;

namespace LoongEgg.Chart
{

    interface IChartElement
    {
        /// <summary>
        /// 根容器
        /// </summary>
        /// <remarks>
        ///  on set, set: <code>This.Content = Root;</code>
        /// </remarks>
        Panel Root { get; }
        /// <summary>
        /// 容纳这个元素的图表
        /// </summary>
        IChart Container { get; }
        /// <summary>
        /// 内部改动标志, 抑制重复刷新
        /// </summary>
        bool InternalChange { get; set; }


        /// <summary>
        /// 更新, 更新前会清空<see cref="Root"/>的<see cref="Panel.Children"/>
        /// </summary>
        void Update();

        /// <summary>
        /// 当实例初始化时需要触发的binding
        /// </summary>
        void OnInitializing();

        /// <summary>
        /// 重置控件的位置
        /// </summary>
        /// <remarks>
        ///     基类重置前调用<code>(Parent as Panel)?Children.Remove(this)</code> 这样override时只需要设置自己的位置
        /// </remarks>
        void ResetPlacement();

        /// <summary>
        /// 当父级容器被设置时
        /// </summary> 
        void OnContainerSet();
    }
}
