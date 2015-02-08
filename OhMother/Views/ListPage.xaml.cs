using OhMother.Common;
using OhMother.Models;
using OhMother.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OhMother.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ListPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.DataContext = App.listVM.ItemList;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            var x = listview.SelectedItems;
            await App.listVM.deleteMultipleItems(x);
            Debug.WriteLine("Selected Count = {0}", x.Count);
            listview.SelectionMode = ListViewSelectionMode.Single;
            listview.IsItemClickEnabled = true;

            DeleteItems.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AddItem.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Select.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Debug.WriteLine("Back button pressed");
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }
            listview.SelectionMode = ListViewSelectionMode.Single;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            listview.IsItemClickEnabled = true;
            DeleteItems.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AddItem.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Select.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Frame.Navigate(typeof(MainPage));
            e.Handled = true;
            
        }


        private void Select_Click(object sender, RoutedEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            DeleteItems.Visibility = Windows.UI.Xaml.Visibility.Visible;
            AddItem.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Select.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            Debug.WriteLine("Select Mode ");
            listview.SelectionMode = ListViewSelectionMode.Multiple;
            listview.IsItemClickEnabled = false;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Adding a new row for itemsaddition");
            TextBox itemNameEntry = new TextBox();
            itemNameEntry.Name = "ItemName";
            itemNameEntry.PlaceholderText = "Enter Item Name here";
            TextBox itemTagEntry = new TextBox();
            itemTagEntry.Name = "ItemTag";
            itemTagEntry.PlaceholderText = "Enter the Tag/Category associated";
            itemNameEntry.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            itemTagEntry.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;

            spanel.Children.Add(itemNameEntry);
            spanel.Children.Add(itemTagEntry);
            AcceptItems.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private async void AcceptItems_Click(object sender, RoutedEventArgs e)
        {
            string itemNamed = null ;
            foreach(object obj in (this.spanel as Panel).Children)
            {
                string boxname = ((TextBox)obj).Name.ToString();
                Debug.WriteLine("The val was {0}",((TextBox)obj).Text);
                if(boxname.Equals("ItemName"))
                {
                    itemNamed = ((TextBox)obj).Text.ToString();
                }
                else
                {
                    Item newItem = new Item();
                    newItem.ItemName = itemNamed;
                    //newItem.Tag = "asdasd";
                    newItem.ItemId = DateTime.Now.ToString() + newItem.ItemName;
                    newItem.Tag = ((TextBox)obj).Text.ToString();
                    try
                    {
                        
                        App.listVM.addItemsToList(newItem);
                        Debug.WriteLine("The val was {0} inserted", ((TextBox)obj).Text);
                    }
                    catch
                    {
                        Debug.WriteLine("Failed to insert into the list");
                    }
                }
                
            }
            await App.listVM.saveListToFile();
            spanel.Children.Clear();
            AcceptItems.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
