using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OhMother.Models
{
    public class Item : INotifyPropertyChanged
    {
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                if (this.id != value)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        

        private string itemId;

        public string ItemId
        {
            get { return itemId; }
            set
            {
                if (this.itemId != value)
                {
                    itemId = value;
                    NotifyPropertyChanged("ItemId");
                }
            }
        }

        private string storeId;

        public string StoreId
        {
            get { return storeId; }
            set
            {
                if (this.storeId != value)
                {
                    storeId = value;
                    NotifyPropertyChanged("StoreId");
                }
            }
        }


        private string price;

        public string Price
        {
            get { return price; }
            set
            {
                if (this.price != value)
                {
                    price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        private string itemName;

        public string ItemName
        {
            get { return itemName; }
            set
            {
                if (this.itemName != value)
                {
                    itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }


        private string tag;

        public string Tag
        {
            get { return tag; }
            set
            {
                if (this.tag != value)
                {
                    tag = value;
                    NotifyPropertyChanged("Tag");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
