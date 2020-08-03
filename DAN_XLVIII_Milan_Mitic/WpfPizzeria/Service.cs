using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPizzeria
{
    class Service
    { /// <summary>
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
    }
}
