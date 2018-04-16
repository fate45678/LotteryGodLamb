using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace WpfAppTest
{
    /// <summary>
    /// Path轉換器(MultiBinding)
    /// </summary>
    public class PathConverter : IMultiValueConverter
    {
        /// <summary>
        /// 執行轉換
        /// </summary>
        /// <param name="values">0: dataType, 1: dataPath</param>
        /// <param name="targetType">目標型別</param>
        /// <param name="parameter">(未使用)</param>
        /// <param name="culture">語系文化</param>
        /// <returns>dataType.InvokingMember dataPath</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Type dataType = values[0].GetType();
            string dataPath = values[1].ToString();
            return dataType.InvokeMember(dataPath, BindingFlags.GetProperty, null, values[0], null);
        }
        /// <summary>
        /// 執行反向轉換(目前無作用)
        /// </summary>
        /// <param name="value">ataType.InvokingMember dataPath</param>
        /// <param name="targetTypes">目標型別</param>
        /// <param name="parameter">(未使用)</param>
        /// <param name="culture">語系文化</param>
        /// <returns>0: dataType, 1: dataPath</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
