using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WpfAppTest.AP;
using WpfAppTest.Base;

namespace WpfAppTest
{
    /// <summary>
    /// UcFiveStart3.xaml 的互動邏輯
    /// </summary>
    public partial class UcFiveStart3 : UserControl
    {
        public UcFiveStart3()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否為第一次載入
        /// </summary>
        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetData();
                IsFirstTime = false;
            }
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        void SetData()
        {
            cbl012.ItemsSource = DB.CombinationNumber(5, 0, 2).OrderBy(x => x.Code);
            //cbl012.WrapRow(10);
        }

        #region 外部呼叫
        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cbl012.Clear();
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            /*CheckBoxList*/
            cbl012.SelectedAll();
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            //殺012路
            tmp = Calculation.DivThreeRemainder(tmp, cbl012, false);

            return tmp;
        }
        #endregion
    }
}
