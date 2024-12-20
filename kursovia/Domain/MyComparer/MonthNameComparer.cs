using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovia.Domain.MyComparer
{
    public class MonthNameComparer : IComparer<string>
    {
        private readonly string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        public int Compare(string x, string y)
        {
            if (x == null || y == null)
                return 0;

            int indexX = Array.IndexOf(months, x);
            int indexY = Array.IndexOf(months, y);

            if (indexX == -1 || indexY == -1)
                return 0;

            return indexX.CompareTo(indexY);
        }
    }
}
