using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Filter
    {
        public string Title { get; private set; } = "";
        public string Progress { get; private set; } = "";
        public string Implementer { get; private set; } = "";
        public string Description { get; private set; } = "";
        public int DaysLeftFrom { get; private set; } = int.MinValue;
        public int DaysLeftTo { get; private set; } = int.MaxValue;

        int StringToInt(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (name == "DaysLeftFrom") return int.MinValue;
                else return int.MaxValue;
            }
            try
            {
                Debug.WriteLine(value);
                return Convert.ToInt32(value);
            }
            catch 
            {
                throw;
            }
        }

        public void UpdateFilter(string name, string value)
        {
            PropertyInfo filterToSet = GetType().GetProperty(name);
            Debug.WriteLine(filterToSet);
            Type filterType = filterToSet.PropertyType;
            if (filterType == typeof(string))
            {
                filterToSet.SetValue(this, value.Trim().ToLower());
            }
            else
            {
                try
                {
                    int converted = StringToInt(name, value);
                    filterToSet.SetValue(this, converted);
                }
                catch 
                {
                    throw;
                }
            }
        }

        public bool CheckTodo(Todo candidate)
        {
            var match = (candidate.Title.Trim().ToLower().Contains(Title.ToLower().Trim())
                && candidate.Progress.Trim().ToLower().Contains(Progress.ToLower().Trim())
                && candidate.Implementer.Trim().ToLower().Contains(Implementer.ToLower().Trim())
                && candidate.Description.Trim().ToLower().Contains(Description.ToLower().Trim())
                && (Convert.ToInt32(candidate.DaysLeft) >= DaysLeftFrom)
                && (Convert.ToInt32(candidate.DaysLeft) <= DaysLeftTo));
            return match;
        }
    }
}
