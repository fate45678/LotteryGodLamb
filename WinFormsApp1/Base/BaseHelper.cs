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
    }
}
