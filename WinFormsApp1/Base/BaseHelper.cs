using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Collections;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Threading;

namespace WpfAppTest.Base
{
    public static class BaseHelper
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();
            //Create the columns in the DataTable
            foreach (PropertyInfo pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }
            //Populate the table
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 是否Runtime
        /// </summary>
        public static bool RunTime { get { return !DesignerProperties.GetIsInDesignMode(new DependencyObject()); } }

        /// <summary>
        /// 處理所有事件的委派
        /// </summary>
        public static void DoEvent()
        {
            //Application.DoEvents()
            var tmp = new Application();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        public static void DataFieldBinding(FrameworkElement container)
        {
            Hashtable ht = new Hashtable();
            BaseHelper.GetChildren(container, ht);

            foreach (FrameworkElement tmp in ht.Values)
            {
                Binding bi = null;
                if (tmp is TextBlock)
                {
                    if (tmp.Tag == null)
                        tmp.Tag = ((TextBlock)tmp).Text;

                    string fieldname = tmp.Tag.ToString();
                    //df = Fields[fieldname];
                    //if (df != null)
                    //{
                    //    df.CaptionControl = tmp;
                    //    Help.InitHelpEvents(df.CaptionControl);
                    TextBlock tb = (TextBlock)tmp;
                    tb.TextAlignment = TextAlignment.Right;
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 100;
                    //    tb.Text = df.Caption;
                    //    if (df.Remark != "")
                    //        tb.ToolTip = df.Remark;
                    //}
                }
                else if (tmp is TextBox)
                {
                    bi = BindingOperations.GetBinding(tmp, TextBox.TextProperty);
                    if (bi != null)
                    {
                        string fieldname = bi.Path.Path;
                        //df = Fields[fieldname];
                        TextBox tb = (TextBox)tmp;
                        //if (df != null)
                        //{
                        //    df.DataControl = tmp;
                        //    Help.InitHelpEvents(df.DataControl);
                        //    tb.MaxLength = df.TextLength;
                        //    if (df.Remark != "")
                        //        tb.ToolTip = df.Remark;
                        //}
                        //tb.IsReadOnly = true;
                    }
                }
                else if (tmp is ComboBox)
                {
                    bi = BindingOperations.GetBinding(tmp, ComboBox.SelectedValueProperty);
                    if (bi != null)
                    {
                        string fieldname = bi.Path.Path;
                        //df = Fields[fieldname];
                        ComboBox cbb = (ComboBox)tmp;
                        //if (df != null)
                        //{
                        //    df.DataControl = tmp;
                        //    Help.InitHelpEvents(df.DataControl);
                        //    if (df.OptionNo > 0)
                        //    {
                        //        if (string.IsNullOrEmpty(cbb.DisplayMemberPath))
                        //            cbb.DisplayMemberPath = "ID_Name";
                        //        cbb.SelectedValuePath = "ID";
                        //        cbb.ItemsSource = df.OptionItems;
                        //        SetContextMenu(cbb, fieldname);
                        //        Form.AddDataCacheControl(cbb, df.OptionNo.ToString());
                        //    }
                        //    if (df.Remark != "")
                        //        cbb.ToolTip = df.Remark;
                        //}
                        //cbb.IsHitTestVisible = false;
                    }
                }
                else if (tmp is CheckBox)
                {
                    bi = BindingOperations.GetBinding(tmp, CheckBox.IsCheckedProperty);
                    if (bi != null)
                    {
                        string fieldname = bi.Path.Path;
                        //df = Fields[fieldname];
                        CheckBox cb = (CheckBox)tmp;
                        //if (df != null)
                        //{
                        //    df.DataControl = tmp;
                        //    Help.InitHelpEvents(df.DataControl);
                        //    cb.Content = df.Caption;
                        //    if (df.Remark != "")
                        //        cb.ToolTip = df.Remark;
                        //}
                        //cb.IsEnabled = false;
                    }
                }

                //if (bi != null && bi.NotifyOnSourceUpdated)
                //    Binding.AddSourceUpdatedHandler(tmp, Control_SourceUpdated);
            }
        }

        public static void GetChildren(UIElement objContainer, Hashtable result)
        {
            if (objContainer is Panel)
            {
                foreach (UIElement tmp in ((Panel)objContainer).Children)
                    GetChildren(tmp, result);
            }
            else if (objContainer is Border)
            {
                GetChildren(((Border)objContainer).Child, result);
            }
            else if (objContainer is TabControl)
            {
                foreach (TabItem tmp in ((TabControl)objContainer).Items)
                    GetChildren(tmp.Content as UIElement, result);
            }
            else if (objContainer is GroupBox)
            {
                GetChildren(((GroupBox)objContainer).Content as UIElement, result);
            }
            else if (objContainer is ScrollViewer)
            {
                GetChildren(((ScrollViewer)objContainer).Content as UIElement, result);
            }
            else if (objContainer is Expander)
            {
                GetChildren(((Expander)objContainer).Content as UIElement, result);
            }
            else
                result.Add(result.Count, objContainer);
        }

        static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
        {
            int childrenCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];

            for (int i = 0; i < childrenCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                    return (T)child;
            }

            for (int i = 0; i < childrenCount; i++)
                if (children[i] != null)
                {
                    var subChild = FindFirstChild<T>(children[i]);
                    if (subChild != null)
                        return subChild;
                }

            return null;
        }

        /// <summary>
        /// 動態繫結公用Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicPublicMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, Type[] MethodTypes = null)
        {
            BindingFlags MethodFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
            return DynamicExecuteMethod(MethodNoumenon, MethodName, MethodVariable, MethodFlag, MethodTypes);
        }

        /// <summary>
        /// 動態繫結非公用Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicNonPublicMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, Type[] MethodTypes = null)
        {
            BindingFlags MethodFlag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;
            return DynamicExecuteMethod(MethodNoumenon, MethodName, MethodVariable, MethodFlag, MethodTypes);
        }
        /// <summary>
        /// 動態繫結Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodFlag">位元遮罩，由一個或多個 BindingFlags 組成，而這些旗標會指定執行搜尋的方式</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicExecuteMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, BindingFlags MethodFlag, Type[] MethodTypes = null)
        {
            //Type[] MethodTypes = null;
            if (MethodTypes == null && MethodVariable != null)
            {
                int n = MethodVariable.Length;
                MethodTypes = new Type[n];
                for (int i = 0; i < n; i++)
                    MethodTypes[i] = MethodVariable[i].GetType();
            }
            Type type = MethodNoumenon.GetType();
            MethodInfo method;
            if (MethodTypes == null)
                method = type.GetMethod(MethodName, MethodFlag);
            else
                method = type.GetMethod(MethodName, MethodFlag, null, MethodTypes, null);
            if (method != null)
                return method.Invoke(MethodNoumenon, MethodFlag, Type.DefaultBinder, MethodVariable, null);
            else
                return null;
        }
    }
}
