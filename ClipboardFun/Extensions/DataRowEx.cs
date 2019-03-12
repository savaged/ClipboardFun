using Savaged.ClipboardFun.Models;
using System;
using System.Data;

namespace Savaged.ClipboardFun.Extensions
{
    public static class DataRowEx
    {
        public static T ToModel<T>(this DataRow dr)
            where T : Model, new()
        {
            var value = new T();
            foreach (DataColumn column in dr.Table.Columns)
            {
                var property = value.GetType().GetProperty(column.ColumnName);

                if (property != null && dr[column] != DBNull.Value)
                {
                    var result = Convert.ChangeType(dr[column], property.PropertyType);
                    property.SetValue(value, result, null);
                }
            }
            return value;
        }
    }
}
