using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OhMother.Models
{
    public class Store: INotifyPropertyChanged
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

        private string phone;

        public string Phone
        {
            get { return phone; }
            set
            {
                if (this.phone != value)
                {
                    phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }
        

        private string storeName;

        public string StoreName
        {
            get { return storeName; }
            set
            {
                if (this.storeName != value)
                {
                    storeName = value;
                    NotifyPropertyChanged("StoreName");
                }
            }
        }

        private string latitude;

        public string Latitude
        {
            get { return latitude; }
            set
            {
                if (this.latitude != value)
                {
                    latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }


        private string longitude;

        public string Longitude
        {
            get { return longitude; }
            set
            {
                if (this.longitude != value)
                {
                    longitude = value;
                    NotifyPropertyChanged("Longitude");
                }
            }
        }

        private bool marked;

        public bool Marked
        {
            get { return marked; }
            set
            {
                if (this.marked != value)
                {
                    marked = value;
                    NotifyPropertyChanged("Marked");
                }
            }
        }
        

        private string rating;

        public string Rating
        {
            get { return rating; }
            set
            {
                if (this.rating != value)
                {
                    rating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }

        private string distance;

        public string Distance
        {
            get { return distance; }
            set
            {
                if (this.distance != value)
                {
                    distance = value;
                    NotifyPropertyChanged("Distance");
                }
            }
        }

        private string matches;

        public string Matches
        {
            get { return matches; }
            set
            {
                if (this.matches != value)
                {
                    matches = value;
                    NotifyPropertyChanged("Matches");
                }
            }
        }


        private string timing;

        public string Timing
        {
            get { return timing; }
            set
            {
                if (this.timing != value)
                {
                    timing = value;
                    NotifyPropertyChanged("Timing");
                }
            }
        }
        

        private string address;

        public string Address
        {
            get { return address; }
            set
            {
                if (this.address != value)
                {
                    address = value;
                    NotifyPropertyChanged("Address");
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
