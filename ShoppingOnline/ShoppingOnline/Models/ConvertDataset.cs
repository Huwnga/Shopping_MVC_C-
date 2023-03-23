using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ShoppingOnline.Models
{
    public class ConvertDataset
    {
        public static List<T> ToList<T>(DataSet ds)
        where T : new()
        {
            List<T> list = new List<T>();
            DataRowCollection rows = ds.Tables[0].Rows;

            foreach (DataRow row in rows)
            {
                T s = AddByDataRow<T>(row);

                list.Add(s);
            }

            return list;
        }

        public static T AddByDataRow<T>(DataRow row)
        where T : new ()
        {
            T item = new T();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && row[column] != DBNull.Value && row[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(row[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}