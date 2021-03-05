using System.Windows;

namespace LoongEgg.Chart
{
    public static class Helper
    {
        public static class TemplateHelper
        {
            public static T GetParent<T>(FrameworkElement element) where T : class
            {
                if (element == null || element.TemplatedParent == null) return null;

                T parent = element.TemplatedParent as T;
                FrameworkElement container = parent as FrameworkElement;
                while (parent == null && container!=null)
                {
                    parent = container as T;
                    container = container.TemplatedParent as FrameworkElement;
                }
                return parent;
            }
        }
    }
}
