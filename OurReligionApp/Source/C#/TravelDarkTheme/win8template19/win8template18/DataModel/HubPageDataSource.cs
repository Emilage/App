using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace TravelDarkTheme.Data
{
    /// <summary>
    /// Base class for <see cref="HubPageDataItem"/> and <see cref="HubPageDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class HubPageDataCommon : TravelDarkTheme.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public HubPageDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(HubPageDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class HubPageDataItem : HubPageDataCommon
    {
        public HubPageDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, int ColSpan, int RowSpan, HubPageDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._ColSpan = ColSpan;
            this._RowSpan = RowSpan;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private int _ColSpan = 1;
        public int ColSpan
        {
            get { return this._ColSpan; }
            set { this.SetProperty(ref this._ColSpan, value); }
        }

        private int _RowSpan = 1;
        public int RowSpan
        {
            get { return this._RowSpan; }
            set { this.SetProperty(ref this._RowSpan, value); }
        }

        private HubPageDataGroup _group;
        public HubPageDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class HubPageDataGroup : HubPageDataCommon
    {
        public HubPageDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<HubPageDataItem> _items = new ObservableCollection<HubPageDataItem>();
        public ObservableCollection<HubPageDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<HubPageDataItem> _topItem = new ObservableCollection<HubPageDataItem>();
        public ObservableCollection<HubPageDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// HubPageDataSource initializes with placeholder data rather than live production
    /// data so that HubPage data is provided at both design-time and run-time.
    /// </summary>
    public sealed class HubPageDataSource
    {
        private static HubPageDataSource _HubPageDataSource = new HubPageDataSource();

        private ObservableCollection<HubPageDataGroup> _allGroups = new ObservableCollection<HubPageDataGroup>();
        public ObservableCollection<HubPageDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<HubPageDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _HubPageDataSource.AllGroups;
        }

        public static HubPageDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _HubPageDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static HubPageDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _HubPageDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() >= 1) return matches.First();
            return null;
        }

        public HubPageDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new HubPageDataGroup("Group-1",
                    "top destinations",
                    "Group Subtitle: 1",
                    "Assets/Images/HubPage/Destinations1.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group1.Items.Add(new HubPageDataItem("Big-Group-1-Item-1",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations1.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,70,54,
                    group1));
            group1.Items.Add(new HubPageDataItem("Small-Group-1-Item-2",
                     "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18,18,
                    group1));
            group1.Items.Add(new HubPageDataItem("Small-Group-1-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group1));
            group1.Items.Add(new HubPageDataItem("Small-Group-1-Item-4",
                     "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group1));           
            this.AllGroups.Add(group1);

            var group2 = new HubPageDataGroup("Group-2",
                    "places near you",
                    "Group Subtitle: 2",
                    "Assets/Images/HubPage/PlacesNear.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new HubPageDataItem("Medium-Group-2-Item-1",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 36, 36,
                    group2));
            group2.Items.Add(new HubPageDataItem("Small-Group-2-Item-2",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group2));
            group2.Items.Add(new HubPageDataItem("Small-Group-2-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group2));
            group2.Items.Add(new HubPageDataItem("Small-Group-2-Item-2",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear4.png",
                   "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                   ITEM_CONTENT, 18, 18,
                   group2));
            group2.Items.Add(new HubPageDataItem("Small-Group-2-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear5.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group2));
            group2.Items.Add(new HubPageDataItem("Small-Group-2-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear6.png",
                   "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                   ITEM_CONTENT, 18, 18,
                   group2));
            this.AllGroups.Add(group2);

            var group3 = new HubPageDataGroup("Group-3",
                    "featured locations",
                    "Group Subtitle: 3",
                    "Assets/Images/HubPage/FeaturedLoc1.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-1",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/FeaturedLoc1.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-2",
                  "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/FeaturedLoc2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-4",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-5",
                      "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));
            group3.Items.Add(new HubPageDataItem("Small-Group-3-Item-6",
                     "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear4.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group3));           
            this.AllGroups.Add(group3);

            var group4 = new HubPageDataGroup("Group-4",
                    "international locations",
                    "Group Subtitle: 4",
                    "Assets/Images/HubPage/International.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-1",
                  "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/International.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group4));
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-2",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/International2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group4));
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/FeaturedLoc2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group4));
            group4.Items.Add(new HubPageDataItem("Wide-Group-4-Item-4",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/International3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 54, 36,
                    group4));
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-5",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group4));
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-6",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 18, 18,
                    group4));
            group4.Items.Add(new HubPageDataItem("Small-Group-4-Item-6",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/PlacesNear4.png",
                   "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                   ITEM_CONTENT, 18, 18,
                   group4));
            this.AllGroups.Add(group4);

        }
    }
}
