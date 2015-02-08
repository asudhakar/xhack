using OhMother.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace OhMother.ViewModels
{
    public class ItemVM
    {
        public ObservableCollection<Item> ItemList { get { return _ItemList; } }
        private ObservableCollection<Item> _ItemList = new ObservableCollection<Item>();
        const string listfilename = "lists.json";


        #region JSONOpertations
        public async Task getListFromFile()
        {
            if (_ItemList.Count != 0)
                return;

            var JsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Item>));
            try
            {

                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(listfilename))
                {
                    _ItemList= (ObservableCollection<Item>)JsonSerializer.ReadObject(stream);
                }
                Debug.WriteLine("Tried : {0}", _ItemList.Count);
            }
            catch
            {
                Debug.WriteLine("Caught");
                _ItemList = new ObservableCollection<Item>();
            }
        }


        public async Task saveListToFile()
        {
            var JsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<Item>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(listfilename, CreationCollisionOption.ReplaceExisting))
            {
                JsonSerializer.WriteObject(stream, _ItemList);
            }
        }
        #endregion

        public ItemVM()
        {
        //    _ItemList.Add(new Item{
        //        ItemId = "12",
        //        ItemName = "Sabir",
        //        Tag = "Something"
        //});

        //    _ItemList.Add(new Item
        //    {
        //        ItemId = "13",
        //        ItemName = "asdSabir",
        //        Tag = "Somethinasdg"
        //    });

        //    _ItemList.Add(new Item
        //    {
        //        ItemId = "1223",
        //        ItemName = "Sasdfbir",
        //        Tag = "Somethingsdfads"
        //    });
        //    _ItemList.Add(new Item
        //    {
        //        ItemId = "1223423",
        //        ItemName = "Sabir23423",
        //        Tag = "Somethinwewdwedg"
        //    });
             getListFromFile();
        }

        public  async Task deleteMultipleItems(IList<object> xyz)
        {
            int con = xyz.Count;
            for (int i = 0; i < con; i++)
            {
                Item x = (Item)xyz.ElementAt(0);
                Debug.WriteLine("Journal Name : {0}, {1}", x.ItemId, xyz.Count);
                _ItemList.Remove(x);

            }
            await saveListToFile();
        }

        public async void addItemsToList(Item item)
        {
            _ItemList.Add(item);
        }
    }

}
