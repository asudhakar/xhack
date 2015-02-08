using OhMother.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace OhMother.ViewModels
{
    public class StoreVM
    {
        public List<Store> StoreList { get { return _StoreList; } }
        private List<Store> _StoreList = new List<Store>();

        public List<Item> StoreItems { get { return _StoreItems; } }
        private List<Item> _StoreItems = new List<Item>();
        
        public List<Store> StoreInArea { get { return _StoreInArea; } }
        private List<Store> _StoreInArea = new List<Store>();

        const string storefilename = "stores.json";


        #region JSONOpertations
        public async Task getListFromFile()
        {
            if (_StoreList.Count != 0)
                return;

            var JsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Store>));
            try
            {

                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(storefilename))
                {
                    _StoreList = (List<Store>)JsonSerializer.ReadObject(stream);
                }
                Debug.WriteLine("Tried : {0}", _StoreList.Count);

            }
            catch
            {
                Debug.WriteLine("Caught");
                _StoreList = new List<Store>();
            }
        }


        public async Task saveListToFile()
        {
            var JsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Store>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(storefilename, CreationCollisionOption.ReplaceExisting))
            {
                JsonSerializer.WriteObject(stream, _StoreList);
            }
        }
        #endregion

        public StoreVM()
        {
        //    _StoreList.Add(new Store
        //    {
        //        StoreId = "1",
        //        StoreName = "Sabir ahmed",
        //        Distance = "02.312",
        //        Matches = "23",
        //        Address = "No 12/ kodihalli old airport road bangalore",
        //        Phone = "8105846408"
        //    });

        //    _StoreList.Add(new Store
        //    {
        //        StoreName = "Sabir ahmed",
        //        Distance = "324.234",
        //        Matches = "231",
        //        Address = "No 12/ kodihalli old airport road bangalore",
        //        Phone = "8105846408"
        //    });
        //    _StoreList.Add(new Store
        //    {
        //        StoreName = "Sabir Ahefmnksfnd",
        //        Distance = "4.452",
        //        Matches = "2",
        //        Address = "No 12/ kodihalli old airport road bangalore",
        //        Phone = "8105846408"
        //    });
        //    _StoreList.Add(new Store
        //    {
        //        StoreName = "Sabir ewrl,snfk",
        //        Distance = "9.324",
        //        Address = "No 12/ kodihalli old airport road bangalore",
        //        Phone = "8105846408",
        //        Matches = "231"
        //    });

            //_StoreItems.Add(new Item
            //{
            //    ItemId = "13",
            //    ItemName = "asdSabir",
            //    Tag = "Somethinasdg"
            //});


            //_StoreItems.Add(new Item
            //{
            //    StoreId = "1",
            //    ItemId = "1223",
            //    ItemName = "Sasdfbir",
            //    Tag = "Somethingsdfads"
            //});
            //_StoreItems.Add(new Item
            //{
            //    StoreId = "1",
            //    ItemId = "1223423",
            //    ItemName = "Sabir23423",
            //    Tag = "Somethinwewdwedg"
            //});

           // insertIntoTheDatabase();
           getStoreTableFromDatabase();
           getStoreItemsFromDatabase();
        }

        private async void getStoreTableFromDatabase()
        {
            _StoreItems = await App.MobileService.GetTable<Item>().ToListAsync();
            Debug.WriteLine("The items that were in the storeitem were {0}", _StoreItems.Count);
            Debug.WriteLine("The first item was {0}", _StoreItems.ElementAt(0).ItemName);
        }

        private async void getStoreItemsFromDatabase()
        {
            _StoreList = await App.MobileService.GetTable<Store>().ToListAsync();
            Debug.WriteLine("Got the stores from the db");
        }

        private async void insertIntoTheDatabase()
        {
            try
            {
                Debug.WriteLine("Inserting into the db");
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Foodparadise08/02/2015 11:12:18", ItemName = "Momos", Tag = "chinese" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Foodparadise08/02/2015 11:12:18", ItemName = "Pizza", Tag = "italian" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Foodparadise08/02/2015 11:12:18", ItemName = "Pasta", Tag = "italian" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Foodparadise08/02/2015 11:12:18", ItemName = "Bread", Tag = "bread" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Stop and shop08/02/2015 11:13:25", ItemName = "Curd", Tag = "curd" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Stop and shop08/02/2015 11:13:25", ItemName = "Nandini Milk", Tag = "milk" });
                await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "Stop and shop08/02/2015 11:13:25", ItemName = "Nilgiris Milk", Tag = "milk" });
            }
            catch
            {
                Debug.WriteLine("Couldnt insert into db");
            }
        }

        public async Task insertStoreIntoList(Store str)
        {
            _StoreList.Add(str);
            try
            {
                Debug.WriteLine("Adding to database");
               // await App.MobileService.GetTable<Item>().InsertAsync(new Item { StoreId = "1", ItemName = "sabir ahmed", ItemId = "34" });
               // await App.MobileService.GetTable<Store>().InsertAsync(str);
               // await App.MobileService.GetTable<Store>().InsertAsync(new Store { StoreId = "123", StoreName = "yeah sure" });
                Store newstore = new Store();
                newstore.StoreName = str.StoreName;
                newstore.StoreId = str.StoreId;
                newstore.Address = str.Address;
                newstore.Latitude = str.Latitude;
                newstore.Longitude = str.Longitude;
                //newstore.Phone = str.Phone;
                newstore.Timing = str.Timing;
                await App.MobileService.GetTable<Store>().InsertAsync(newstore);
            }
            catch
            {
                Debug.WriteLine("Failed to insert to db");
            }
        }

        public ObservableCollection<Item> getListByStore(Store currStore)
        {
            ObservableCollection<Item> tempList = new ObservableCollection<Item>();
            foreach(Item itm in _StoreItems)
            {
                if( currStore.StoreId!= null && itm.StoreId!= null && itm.StoreId.Equals(currStore.StoreId))
                foreach(Item itmLocal in App.listVM.ItemList)
                {
                    if(itm.ItemName.Equals(itmLocal.ItemName) || itm.Tag.Equals(itmLocal.Tag))
                    {
                        tempList.Add(itm);
                    }

                }
            }
            return tempList;
        }

        public  Store getStoreById(string GeoId)
        {
            foreach(Store str in StoreList)
            {
                if(str.StoreId.Equals(GeoId))
                {
                    return str;
                }
            }
            return null;
        }
    }
}
