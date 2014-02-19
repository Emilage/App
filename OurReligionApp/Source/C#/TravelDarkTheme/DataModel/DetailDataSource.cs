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
    /// Base class for <see cref="DetailDataItem"/> and <see cref="DetailDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class DetailDataCommon : TravelDarkTheme.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public DetailDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._Para1 = Para1;
            this._Para2 = Para2;
            this._Para3 = Para3;
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

        private string _Para1 = string.Empty;
        public string Para1
        {
            get { return this._Para1; }
            set { this.SetProperty(ref this._Para1, value); }
        }

        private string _Para2 = string.Empty;
        public string Para2
        {
            get { return this._Para2; }
            set { this.SetProperty(ref this._Para2, value); }
        }

        private string _Para3 = string.Empty;
        public string Para3
        {
            get { return this._Para3; }
            set { this.SetProperty(ref this._Para3, value); }
        }


        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(DetailDataCommon._baseUri, this._imagePath));
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
    public class DetailDataItem : DetailDataCommon
    {
        public DetailDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, int ColSpan, int RowSpan, DetailDataGroup group)
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

        private DetailDataGroup _group;
        public DetailDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class DetailDataGroup : DetailDataCommon
    {
        public DetailDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
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

        private ObservableCollection<DetailDataItem> _items = new ObservableCollection<DetailDataItem>();
        public ObservableCollection<DetailDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<DetailDataItem> _topItem = new ObservableCollection<DetailDataItem>();
        public ObservableCollection<DetailDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// DetailDataSource initializes with placeholder data rather than live production
    /// data so that Detail data is provided at both design-time and run-time.
    /// </summary>
    public sealed class DetailDataSource
    {
        private static DetailDataSource _DetailDataSource = new DetailDataSource();

        private ObservableCollection<DetailDataGroup> _allGroups = new ObservableCollection<DetailDataGroup>();
        public ObservableCollection<DetailDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<DetailDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _DetailDataSource.AllGroups;
        }

        public static DetailDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _DetailDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() >= 1) return matches.First();
            return null;
        }

        public static DetailDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _DetailDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public DetailDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new DetailDataGroup("Group-1",
                    "snapshots",
                    "Group Subtitle: 1",
                    "Assets/Images/Spoke/DetailImg1.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group1.Items.Add(new DetailDataItem("Display-Group-1-Item-1",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/Spoke/DetailImg1.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,57,38,
                    group1));
            group1.Items.Add(new DetailDataItem("Thumb-Group-1-Item-2",
                     "Lorem ipsum dolor",
                    "Destination name",
                   "Assets/Images/Spoke/recommended2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 19,12,
                    group1));
            group1.Items.Add(new DetailDataItem("Thumb-Group-1-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/Spoke/recommended1.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 19, 12,
                    group1));
            group1.Items.Add(new DetailDataItem("Thumb-Group-1-Item-4",
                     "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/Spoke/recommended3.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 19,12,
                    group1));           
            this.AllGroups.Add(group1);

            var group2 = new DetailDataGroup("Group-2",
                    "about the place",
                    "Group Subtitle: 2",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new DetailDataItem("Description-Group-2-Item-1",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque viverra, risus non ultrices eleifend, ante elit scelerisque felis, a egestas enim lacus in ante. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque viverra, risus non ultrices eleifend, ante elit scelerisque felis, a egestas enim lacus in ante. Sed egestas mollis magna, in feugiat felis dapibus non. ",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 43, 51,
                    group2) { Para1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque viverra, risus non ultrices eleifend, ante elit scelerisque felis, a egestas enim lacus in ante. Sed egestas mollis magna, in feugiat felis dapibus non. ", Para2 = "Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.", Para3 = "Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper." });                  
            this.AllGroups.Add(group2);

            var group3 = new DetailDataGroup("Group-3",
                    "places to visit here",
                    "Group Subtitle: 3",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group3.Items.Add(new DetailDataItem("Detail-Group-3-Item-1",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 51,10,
                    group3));
            group3.Items.Add(new DetailDataItem("Detail-Group-3-Item-2",
                  "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/FeaturedLoc2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 51, 10,
                    group3));
            group3.Items.Add(new DetailDataItem("Detail-Group-3-Item-3",
                    "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/FeaturedLoc2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 51, 10,
                    group3));
            group3.Items.Add(new DetailDataItem("Detail-Group-3-Item-4",
                   "Lorem ipsum dolor",
                    "Destination name",
                    "Assets/Images/HubPage/Destinations4.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 51, 10,
                    group3));           
            this.AllGroups.Add(group3);

            var group4 = new DetailDataGroup("Group-4",
                    "how to reach here",
                    "Group Subtitle: 4",
                    "Assets/Images/Spoke/DetailImg2.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group4.Items.Add(new DetailDataItem("Map-Group-4-Item-1",
                  "travel by rail",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque viverra, risus non ultrices eleifend, ante elit scelerisque felis, a egestas enim lacus in ante. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.",
                    "Assets/Images/Spoke/DetailImg2.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT, 109, 50,
                    group4) { Para1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque viverra, risus non ultrices eleifend, ante elit scelerisque felis, a egestas enim lacus in ante. Sed egestas mollis magna, in feugiat felis dapibus non. ", Para2 = "Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper.", Para3 = "Quisque ornare risus at ligula tincidunt ullamcorper.. Sed egestas mollis magna, in feugiat felis dapibus non. Quisque ornare risus at ligula tincidunt ullamcorper." });          
            this.AllGroups.Add(group4);

        }
    }
}
