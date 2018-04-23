using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfAppTest.Base;

namespace WpfAppTest
{
    /// <summary>
    /// 多RadioButton控制項
    /// </summary>
    public partial class RadioButtonList : ItemsControl
    {
        /// <summary>
        /// 顯示欄位DependencyProperty
        /// </summary>
        new public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(RadioButtonList));
        /// <summary>
        /// 值欄位DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(RadioButtonList));
        /// <summary>
        /// 選取值DependencyProperty
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(Object), typeof(RadioButtonList), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SelectedValuePropertyChanged)));
        static void SelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)d;
            if ((e.NewValue == null) != (e.OldValue == null) || e.NewValue.ToString() != e.OldValue.ToString())
                rbl.SetChecked(e.NewValue);
        }
        /// <summary>
        /// 建構子
        /// </summary>
        public RadioButtonList()
        {
            InitializeComponent();
            if (GroupName == null)
                GroupName = Guid.NewGuid().ToString();
            ((INotifyCollectionChanged)this.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            radioButtons = null;
            if (BaseHelper.RunTime)
                RefreshDataSource();
        }

        List<RadioButton> radioButtons;
        /// <summary>
        /// 群組名稱
        /// </summary>
        public string GroupName { get; private set; }
        void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (BaseHelper.RunTime)
                RefreshDataSource();
        }

        void RefreshDataSource()
        {
            if (radioButtons == null)
            {
                if (this.Items.Count > 0)
                    this.UpdateLayout();  //強制更新,立即執行Binding,避免物件在TabControl的第二頁時,出現未正常Binding狀況

                radioButtons = new List<RadioButton>();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ContentPresenter c = (ContentPresenter)this.ItemContainerGenerator.ContainerFromItem(this.Items[i]);
                    if (c != null)
                    {
                        int childrenCount = VisualTreeHelper.GetChildrenCount(c);
                        var children = new FrameworkElement[childrenCount];

                        for (int j = 0; j < childrenCount; j++)
                        {
                            var child = VisualTreeHelper.GetChild(c, j);
                            if (child is RadioButton)
                                radioButtons.Add((RadioButton)child);
                        }
                    }
                }

                //radioButtons = DXHelper.GetEnumerable<RadioButton>(this);
                if (radioButtons.Count<RadioButton>() == 0)
                    radioButtons = null;
                else
                    SetChecked(SelectedValue);
            }
        }

        /// <summary>
        /// 選取值
        /// </summary>
        [Browsable(true), Category("附加属性"), Description("選取值"), DefaultValue(null)]
        public Object SelectedValue
        {
            get
            {
                return GetValue(SelectedValueProperty);
            }
            set
            {
                SetValue(SelectedValueProperty, value);
                OnPropertyChanged("SelectedValue");
            }
        }
        /// <summary>
        /// 選取值事件
        /// </summary>
        public event PropertyChangedEventHandler SelectedValueChanged;

        /// <summary>
        /// 屬性值變更時所引發的動作
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

        /// <summary>
        /// 選取項目文字
        /// </summary>
        public string SelectedText
        {
            get
            {
                if (radioButtons != null)
                {
                    var tmp = radioButtons.Where(x => x.IsChecked == true).FirstOrDefault();
                    if (tmp != null)
                        return tmp.Content.ToString();
                }
                return "";
            }
        }

        /// <summary>
        /// 指定選取
        /// </summary>
        /// <param name="value">選取值</param>
        public void SetChecked(Object value)
        {
            if (radioButtons != null)
            {
                if (value == null)
                {
                    foreach (var rb in radioButtons)
                    {
                        if ((bool)rb.IsChecked)
                            rb.IsChecked = false;
                    }
                }
                else
                {
                    bool isFound = false;
                    foreach (var rb in radioButtons)
                    {
                        if (rb.Tag != null && rb.Tag.ToString() == value.ToString())
                        {
                            if (!(bool)rb.IsChecked)
                                rb.IsChecked = true;
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        var tmp = radioButtons.Where(x => x.IsChecked == true).FirstOrDefault();
                        if (tmp != null)
                            tmp.IsChecked = false;
                    }
                }
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
        /// <summary>
        /// 選取文字大小
        /// </summary>
        [Browsable(true), Category("附加属性"), Description("選取項目字型變大級數"), DefaultValue(0)]
        public double SelectedFontSize { get; set; }
        void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            rb.FontSize += SelectedFontSize;
            rb.FontWeight = FontWeights.Bold;
            SelectedValue = rb.Tag;
        }
        void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            rb.FontSize -= SelectedFontSize;
            rb.FontWeight = FontWeights.Normal;
        }
        bool _IsReadOnly = false;
        /// <summary>
        /// 是否唯讀
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
    }
}
