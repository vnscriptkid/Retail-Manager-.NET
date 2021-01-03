using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;╕
        private string _itemQuantity;

        public BindingList<string> Products
        {
            get { return _products; }
            set { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }


        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value; }
        }

        public bool CanAddToCart
        {
            get
            {
                // TODO: add logic
                return false;
            }
        }

        public void AddToCart() { }

        public bool CanRemoveFromCart
        {
            get
            {
                // TODO: add logic
                return false;
            }
        }

        public void RemoveFromCart() { }

        public string SubTotal
        {
            get
            {
                // TODO - Replace with calculation
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO - Replace with calculation
                return "$0.00";
            }
        }

        public bool CanCheckOut
        {
            get
            {
                // TODO: add logic
                return false;
            }
        }

        public void CheckOut() { }
    }
}
