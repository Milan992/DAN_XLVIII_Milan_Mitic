using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfPizzeria.Model;
using WpfPizzeria.Views;

namespace WpfPizzeria.ViewModels
{
    class GuestViewModel : ViewModelBase
    {
        Guest guest;
        Service service = new Service();

        #region Constructors

        public GuestViewModel(Guest guestOpen)
        {
            meal = new tblMenu();
            guest = guestOpen;
            recordList = new List<tblRecord>();

            MealList = service.GetAllMeals();
        }

        #endregion

        #region Properties

        private tblMenu meal;

        public tblMenu Meal
        {
            get
            {
                return meal;
            }
            set
            {
                meal = value;
                OnPropertyChanged("Meal");
            }
        }

        private List<tblMenu> mealList;

        public List<tblMenu> MealList
        {
            get
            {
                return mealList;
            }
            set
            {
                mealList = value;
                OnPropertyChanged("MealList");
            }
        }

        private List<tblRecord> recordList;

        public List<tblRecord> RecordList
        {
            get
            {
                return recordList;
            }
            set
            {
                recordList = value;
            }
        }

        private string amount;

        public string Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public string Price
        {
            get
            {
                return $"{AmountInt}";
            }
            set
            {
                amount = value;
            }
        }

        private int amountInt;

        public int AmountInt
        {
            get
            {
                return amountInt;
            }
            set
            {
                amountInt = value;
                OnPropertyChanged("Price");
            }
        }

        #endregion

        #region Commands

        private ICommand addToCart;

        public ICommand AddToCart
        {
            get
            {
                if (addToCart == null)
                {
                    addToCart = new RelayCommand(param => AddToCartExecute(), param => CanAddToCartExecute());
                }

                return addToCart;
            }
        }

        private void AddToCartExecute()
        {
            try
            {
                AmountInt = Meal.Price * Convert.ToInt32(Amount) + AmountInt;
                tblRecord record = new tblRecord();
                record.MealID = Meal.MealID;
                record.Amount = Convert.ToInt32(Amount);
                recordList.Add(record);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddToCartExecute()
        {
            return true;
        }

        private ICommand order;

        public ICommand Order
        {
            get
            {
                if (order == null)
                {
                    order = new RelayCommand(param => OrderExecute(), param => CanOrderExecute());
                }

                return order;
            }
        }

        private void OrderExecute()
        {
            try
            {
                tblOrder order = service.AddOrder(AmountInt);
                service.AddRecord(recordList, order.OrderID);
                MessageBox.Show("Order placed and waiting to be aprroved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanOrderExecute()
        {
            return true;
        }
        #endregion
    }
}
