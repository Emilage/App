using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDarkTheme.Data;
using Windows.UI.Xaml.Controls;

namespace TravelDarkTheme.VariableTemplate
{
    public class VariableTileControl:GridView
    {
        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            if (item.GetType() == typeof(HubPageDataItem))
            {
                var viewModel = item as HubPageDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }

            else if (item.GetType() == typeof(SpokeDataItem))
            {
                var viewModel = item as SpokeDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }

            else if (item.GetType() == typeof(DetailDataItem))
            {
                var viewModel = item as DetailDataItem;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, viewModel.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, viewModel.RowSpan);
                base.PrepareContainerForItemOverride(element, item);
            }

        }
    }
}
