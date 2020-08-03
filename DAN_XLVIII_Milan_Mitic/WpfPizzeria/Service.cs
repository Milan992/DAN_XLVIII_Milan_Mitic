using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WpfPizzeria.Model;

namespace WpfPizzeria
{
    class Service
    {
        /// <summary>
        /// Checks if string is in JMBG format.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsJmbg(string userName)
        {
            bool jmbg = false;
            if (userName.Length == 13)
            {
                try
                {
                    long i = Convert.ToInt64(userName);
                    string date = "1" + userName.Substring(4, 3) + "-" + userName.Substring(2, 2) + "-" + userName.Substring(0, 2);
                    DateTime dateOfBirth = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    jmbg = true;
                }
                catch
                {
                    jmbg = false;
                }
            }
            else
            {
                jmbg = false;
            }
            return jmbg;
        }

        /// <summary>
        /// Gets all records with the ID of the selected order, which is kept in the txt file.
        /// Adds them to the list.
        /// </summary>
        /// <returns></returns>
        public List<vwOrder> GetRecords()
        {
            string id = "";
            using (StreamReader sr = new StreamReader("../../Order.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    id = line;
                }
            }
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    int i = Convert.ToInt32(id);
                    List<vwOrder> list = new List<vwOrder>();
                    list = (from x in context.vwOrders where x.OrderID == i select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Checks if user has any waiting orders.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsWaiting(string userName)
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    tblOrder order =  new tblOrder();
                    order = (from x in context.tblOrders where x.JMBG == userName && x.StatusID == 1 select x).First();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all orders from a DataBase and adds the to the list.
        /// </summary>
        /// <returns></returns>
        public List<tblOrder> GetAllOrders()
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    List<tblOrder> list = new List<tblOrder>();
                    list = (from x in context.tblOrders select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// changes order status
        /// </summary>
        /// <param name="order"></param>
        public void ApproveOrder(tblOrder order)
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    tblOrder orderToEdit = (from p in context.tblOrders where p.OrderID == order.OrderID select p).First();
                    if (order.StatusID == 2)
                    {
                        orderToEdit.StatusID = 3;
                        MessageBox.Show("Order declined.");
                    }
                    else
                    {
                        orderToEdit.StatusID = 2;
                        MessageBox.Show("Order approved.");
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Deletes an order from database.
        /// </summary>
        /// <param name="order"></param>
        internal void DeleteOrder(tblOrder order)
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    if (order.StatusID == 1)
                    {
                        MessageBox.Show("Orders with waiting status can not be deleted.");
                    }
                    else
                    {
                        tblOrder orderToDelete = (from o in context.tblOrders where o.OrderID == order.OrderID select o).First();
                        context.tblOrders.Remove(orderToDelete);

                        List<tblRecord> record = (from r in context.tblRecords where r.OrderID == order.OrderID select r).ToList();
                        for (int i = 0; i < record.Count; i++)
                        {
                            context.tblRecords.Remove(record[i]);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Order deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Adds all the meals from the database to a list.
        /// </summary>
        /// <returns></returns>
        public List<tblMenu> GetAllMeals()
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    List<tblMenu> list = new List<tblMenu>();
                    list = (from x in context.tblMenus select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Adds new order to the DataBase.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public tblOrder AddOrder(int price)
        {
            tblOrder order = new tblOrder();
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    order.JMBG = GetJmbg();
                    order.DateAndTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd-hh-mm"), "yyyy-MM-dd-hh-mm", System.Globalization.CultureInfo.InvariantCulture);
                    order.StatusID = 1;
                    order.Price = price;
                    context.tblOrders.Add(order);
                    context.SaveChanges();
                    int id = order.OrderID;
                    order.OrderID = id;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
                return null;
            }
            return order;
        }

        /// <summary>
        /// Reads a line from a txt file that keeps JMBG of the logged in user.
        /// </summary>
        /// <returns></returns>
        private string GetJmbg()
        {
            string jmbg = "";
            using (StreamReader sr = new StreamReader("../../Username.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    jmbg = line;
                }
            }
            return jmbg;
        }

        /// <summary>
        /// Adds new record to the DataBase.
        /// </summary>
        /// <param name="recordList"></param>
        /// <param name="orderID"></param>
        public void AddRecord(List<tblRecord> recordList, int orderID)
        {
            try
            {
                using (PizzeriaEntities context = new PizzeriaEntities())
                {
                    foreach (var item in recordList)
                    {
                        item.OrderID = orderID;
                        context.tblRecords.Add(item);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Exception" + ex.Message.ToString());
            }
        }
    }
}
