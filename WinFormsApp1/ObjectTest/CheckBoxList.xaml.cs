using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System;

namespace WpfAppTest
{
    /// <summary>
    /// CheckBoxList.xaml 的互動邏輯
    /// </summary>
    public partial class CheckBoxList : ItemsControl
    {
        /// <summary>
        /// 顯示欄位DependencyProperty
        /// </summary>
        new public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(CheckBoxList));
        /// <summary>
        /// 值欄位DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(CheckBoxList));
        /// <summary>
        /// 選取值DependencyProperty
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(string), typeof(CheckBoxList), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(SelectedValuePropertyChanged)));

        private static void SelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CheckBoxList cbl = (CheckBoxList)d;
            if ((e.NewValue == null) != (e.OldValue == null) || e.NewValue.ToString() != e.OldValue.ToString())
                cbl.SetChecked(e.NewValue == null ? "" : e.NewValue.ToString());
        }
        /// <summary>
        /// 建構子
        /// </summary>
        public CheckBoxList()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)this.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _ValueLength = 0;
            CheckBoxs = null;
            //InitCheckBoxs();
        }

        int _MaxLength;
        /// <summary>
        /// 資料目的最大值長度
        /// </summary>
        public int MaxLength
        {
            get { return _MaxLength; }
            set
            {
                if (_MaxLength != value)
                {
                    _MaxLength = value;
                    ValueLength = this.ItemsSource.Cast<BaseOptions>().Where(x => x.ID <= _MaxLength).Max(x => x.ID);
                }
            }
        }

        int _ValueLength;
        /// <summary>
        /// 值長度
        /// </summary>
        public int ValueLength
        {
            get
            {
                if (_ValueLength == 0 && this.ItemsSource != null)
                    _ValueLength = this.ItemsSource.Cast<BaseOptions>().Max(x => x.ID);
                return _ValueLength;
            }
            set
            {
                if (_ValueLength != value)
                {
                    _ValueLength = value;
                    SetCheckBoxVisible();
                    SelectedValue = SelectedValue.PadRight(_ValueLength, '0');
                }
            }
        }

        string _DisabledItems;
        /// <summary>
        /// 不可使用項目
        /// </summary>
        public string DisabledItems
        {
            get { return _DisabledItems; }
            set
            {
                if (_DisabledItems != value)
                {
                    _DisabledItems = value;
                    SetCheckBoxVisible();
                }
            }
        }

        IEnumerable<CheckBox> CheckBoxs;

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            InitCheckBoxs();
        }

        private void InitCheckBoxs()
        {
            if (CheckBoxs == null)
            {
                if (this.Items.Count > 0)
                    this.UpdateLayout();        //強制更新,立即執行Binding,避免物件在TabControl的第二頁時,出現未正常Binding狀況

                //CheckBoxs = GetEnumerable<CheckBox>(this);
                CheckBoxs = null;
                //if (CheckBoxs.Count<CheckBox>() == 0)
                //    CheckBoxs = null;
                //else
                //{
                //    SetCheckBoxVisible();
                //    SetChecked(SelectedValue);
                //}
            }
        }

        void SetCheckBoxVisible()
        {
            if (CheckBoxs == null) return;
            foreach (var cb in CheckBoxs)
            {
                int i = int.Parse(cb.Tag.ToString());
                if (i <= 0 || i > ValueLength)
                    cb.Visibility = Visibility.Collapsed;

                if (!string.IsNullOrEmpty(DisabledItems) && DisabledItems.Length >= i)
                    cb.IsEnabled = (DisabledItems[i - 1] == '0');
                else
                    cb.IsEnabled = true;
            }
        }

        /// <summary>
        /// 選取值
        /// </summary>
        [Browsable(true), Category("附加属性"), Description("選取值"), DefaultValue(null)]
        public string SelectedValue
        {
            get
            {
                if (GetValue(SelectedValueProperty) == null)
                    return "";
                else
                    return GetValue(SelectedValueProperty).ToString().PadRight(_ValueLength, '0');
            }
            set
            {
                SetValue(SelectedValueProperty, value);
                OnPropertyChanged("SelectedValue");
            }
        }
        /// <summary>
        /// 選取值變更事件
        /// </summary>
        public event PropertyChangedEventHandler SelectedValueChanged;

        /// <summary>
        /// 屬性值變更引發的動作
        /// </summary>
        /// <param name="PropertyName">屬性名稱</param>
        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = SelectedValueChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        bool isBinding = false;
        /// <summary>
        /// 指定選取
        /// </summary>
        /// <param name="value">選取值</param>
        public void SetChecked(string value)
        {
            if (CheckBoxs != null)
            {
                isBinding = true;
                if (value == null || value == string.Empty)
                {
                    foreach (var rb in CheckBoxs)
                    {
                        if ((bool)rb.IsChecked)
                            rb.IsChecked = false;
                    }
                }
                else
                {
                    value = value.PadRight(_ValueLength, '0');
                    foreach (var cb in CheckBoxs)
                    {
                        if (cb.Tag != null && cb.Visibility == Visibility.Visible)
                            cb.IsChecked = (value[int.Parse(cb.Tag.ToString()) - 1] == '1');
                    }
                }
                isBinding = false;
            }
        }
        /// <summary>
        /// 顯示欄位
        /// </summary>
        new public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }
        /// <summary>
        /// 值指定欄位
        /// </summary>
        public string ValueMemberPath
        {
            get
            {
                return (string)GetValue(ValueMemberPathProperty);
            }
            set
            {
                SetValue(ValueMemberPathProperty, value);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!isBinding)
                ChangeValue((CheckBox)sender, true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isBinding)
                ChangeValue((CheckBox)sender, false);
        }

        void ChangeValue(CheckBox cb, bool isChecked)
        {
            int i = int.Parse(cb.Tag.ToString()) - 1;
            string tmp = SelectedValue;
            tmp = tmp.Remove(i, 1);
            tmp = tmp.Insert(i, (isChecked ? "1" : "0"));
            SelectedValue = tmp;
        }

        bool _IsReadOnly = false;
        /// <summary>
        /// 唯讀
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                _IsReadOnly = value;
                this.IsHitTestVisible = !_IsReadOnly;
                this.Focusable = !_IsReadOnly;
            }
        }

        /// <summary>
        /// 清除勾選項目
        /// </summary>
        public void Clear()
        {
            this.SelectedValue = "";
        }

        /// <summary>
        /// 選取全部項目
        /// </summary>
        public void SelectedAll()
        {
            this.SelectedValue = "".PadRight(ValueLength, '1');
        }

        //public IEnumerable<CheckBox> GetEnumerable<CheckBox>(DependencyObject objContainer)
        //{
        //    //var e = new VisualTreeEnumerator(objContainer);
        //    //while (e.MoveNext())
        //    //{
        //    //    if (e.Current is CheckBox)
        //    //        yield return (CheckBox)Convert.ChangeType(e.Current, typeof(CheckBox));
        //    //}
        //}
    }
}
