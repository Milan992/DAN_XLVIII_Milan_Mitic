using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WpfPizzeria.Model;
using WpfPizzeria.Views;

namespace WpfPizzeria.ViewModels
{
    class EmployeeViewModel : ViewModelBase
    {
        Employee employee;
        Service service = new Service();

        #region Constructors

        public EmployeeViewModel(Employee employeeOpen)
        {
            employee = employeeOpen;

            OrderList = service.GetAllOrders();
        }

        #endregion

        #region Properties 

        private tblOrder order;

        public tblOrder Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }


        private List<tblOrder> orderList;

        public List<tblOrder> OrderList
        {
            get
            {
                return orderList;
            }
            set
            {
                orderList = value;
                OnPropertyChanged("OrderList");

            }
        }

        #endregion

        #region Commands

        private ICommand viewOrder;

        public ICommand ViewOrder
        {
            get
            {
                if (viewOrder == null)
                {
                    viewOrder = new RelayCommand(param => ViewOrderExecute(), param => CanViewOrderExecute());
                }

                return viewOrder;
            }
        }

        private void ViewOrderExecute()
        {
            try
            {
                File.WriteAllText(@"..\..\Order.txt", Convert.ToString(Order.OrderID));
                Order order = new Order();
                order.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanViewOrderExecute()
        {
            return true;
        }

        #endregion
    }
}
