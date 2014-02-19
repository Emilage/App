using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDarkTheme.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TravelDarkTheme.VariableTemplate
{
    public class VariableTiles:DataTemplateSelector
    {
        public DataTemplate BigTemplate { get; set; }
        public DataTemplate SmallTemplate { get; set; }
        public DataTemplate MediumTemplate { get; set; }
        public DataTemplate WideTemplate { get; set; }
        public DataTemplate NormalTemplate { get; set; }
        public DataTemplate BigOneTemplate { get; set; }

        public DataTemplate DisplayImageTemplate { get; set; }
        public DataTemplate ThumbImageTemplate { get; set; }
        public DataTemplate DescriptionTemplate { get; set; }
        public DataTemplate DetailTemplate { get; set; }
        public DataTemplate MapTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item.GetType() == typeof(HubPageDataItem))
                {
                    if ((item as HubPageDataItem).UniqueId.StartsWith("Big"))
                        return BigTemplate;
                    if ((item as HubPageDataItem).UniqueId.StartsWith("Small"))
                        return SmallTemplate;
                    if ((item as HubPageDataItem).UniqueId.StartsWith("Medium"))
                        return MediumTemplate;
                    if ((item as HubPageDataItem).UniqueId.StartsWith("Wide"))
                        return WideTemplate;
                }

                else if (item.GetType() == typeof(SpokeDataItem))
                {
                    if ((item as SpokeDataItem).UniqueId.StartsWith("Big"))
                        return BigTemplate;
                    if ((item as SpokeDataItem).UniqueId.StartsWith("Small"))
                        return SmallTemplate;
                    if ((item as SpokeDataItem).UniqueId.StartsWith("Medium"))
                        return MediumTemplate;
                    if ((item as SpokeDataItem).UniqueId.StartsWith("Wide"))
                        return WideTemplate;
                    if ((item as SpokeDataItem).UniqueId.StartsWith("Normal"))
                        return NormalTemplate;
                    if ((item as SpokeDataItem).UniqueId.StartsWith("BigOne"))
                        return BigOneTemplate;
                }

                else if (item.GetType() == typeof(DetailDataItem))
                {
                    if ((item as DetailDataItem).UniqueId.StartsWith("Big"))
                        return BigTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Small"))
                        return SmallTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Medium"))
                        return MediumTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Wide"))
                        return WideTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Normal"))
                        return NormalTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("BigOne"))
                        return BigOneTemplate;

                    if ((item as DetailDataItem).UniqueId.StartsWith("Display"))
                        return DisplayImageTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Thumb"))
                        return ThumbImageTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Description"))
                        return DescriptionTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Detail"))
                        return DetailTemplate;
                    if ((item as DetailDataItem).UniqueId.StartsWith("Map"))
                        return MapTemplate;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
