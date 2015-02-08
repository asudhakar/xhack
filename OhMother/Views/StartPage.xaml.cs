using OhMother.Common;
using OhMother.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    public sealed partial class StartPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public DispatcherTimer fetchLocation;
        public StartPage()
        {
            Init_Geofence();
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private async void fetch_Location(object sender, object e)
        {
            try
            {
                Debug.WriteLine("Fetching loc");
                //Geolocator geolocator = new Geolocator();
                //geolocator.DesiredAccuracy = PositionAccuracy.High;
                //geolocator.MovementThreshold = 100; // The units are meters.
                //Geoposition geoposition = await geolocator.GetGeopositionAsync();


                var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
                Geoposition location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));

                Debug.WriteLine("The location is {0}  and  {1}", location.Coordinate.Latitude, location.Coordinate.Longitude);
            }
            catch
            {
                Debug.WriteLine("Failed to fetch loc");
            }
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

        private void MyList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListPage));
        }

        private void Stores_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StoresPage));
        }

        public object _timer_Tick { get; set; }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Creating the dispatcher here");
            fetchLocation = new DispatcherTimer();
            fetchLocation.Interval = new TimeSpan(0, 0, 5);
            fetchLocation.Tick += fetch_Location;
            // fetchLocation.Start();
        }

        private async Task Init_Geofence()
        {
            var geofenceMonitor = GeofenceMonitor.Current;
            var GeoId = "jaywayPoint";
            var loc = await new Geolocator().GetGeopositionAsync(
                //TimeSpan.FromMinutes(2),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(10));   //needed to trig user acceptance

            geofenceMonitor.GeofenceStateChanged += (sender, args) =>
            {
                var geoReports = geofenceMonitor.ReadReports();
                foreach (var geofenceStateChangeReport in geoReports)
                {
                    var id = geofenceStateChangeReport.Geofence.Id;
                    var newState = geofenceStateChangeReport.NewState;
                    Debug.WriteLine("The geolocation was {0}",GeoId);
                    if (id == GeoId && newState == GeofenceState.Entered)
                    {
                        Store str = App.storeVM.getStoreById(GeoId);
                        foreach(Item item in App.storeVM.StoreItems)
                        {
                            foreach(Item itemLocal in App.listVM.ItemList)
                            {
                                if(item.ItemName.Equals(itemLocal.ItemName) || item.Tag.Equals(itemLocal.Tag))
                                {
                                    App.storeVM.StoreInArea.Add(str);
                                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                        new MessageDialog("You are near {0} and one or more of products in your list is available here!!!!", str.StoreName)
                                        .ShowAsync());
                                }
                            }
                        }
                       
                    }
                }
            };

            foreach (Store str in App.storeVM.StoreList)
            {
                var jayway = new BasicGeoposition()
                {
                    Latitude = Double.Parse(str.Latitude),
                    Longitude = Double.Parse(str.Longitude)
                };
                GeoId = str.StoreId;
                var geofence = new Geofence(GeoId, new Geocircle(jayway, 2000),
                    MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited,
                    true, TimeSpan.FromSeconds(1));
                geofenceMonitor.Geofences.Add(geofence);
            }
        }
        private void CreateStores_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StoreInsert));
        }
    }
}
