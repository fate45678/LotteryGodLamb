using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using WinFormsApp1;
using Wpf.Base;
using WpfApp.Custom;

namespace WpfAppTest
{
    /// <summary>
    /// UcTwoStart.xaml 的互動邏輯
    /// </summary>
    public partial class UcTwoStart1 : UserControl
    {
        public UcTwoStart1()
        {
            InitializeComponent();
        }

        bool IsFirstTime = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsFirstTime)
            {
                SetData();
                IsFirstTime = false;
            }
            useHttpWebRequest_GetHistory();
        }

        /// <summary>
        /// 設定控件
        /// </summary>
        void SetData()
        {
            /*CheckBoxList*/
            var Data = Calculation.CreateContinueNumber();
            cblFixTen.ItemsSource = Data;
            cblFixUnit.ItemsSource = Data;
            cblAnyOne.ItemsSource = Data;

            /*RadioButtonList*/
            var Data2 = Calculation.CreateOption(1, 2, new string[2] { "保留", "排除" });
            rblFixTen.ItemsSource = Data2;
            rblFixUnit.ItemsSource = Data2;
            rblAnyOne.ItemsSource = Data2;

            /*預設值*/
            SetDefaultValue();
        }

        public static JArray jArr;
        //取得歷史開獎
        private void useHttpWebRequest_GetHistory()
        {
            DateTime dt = DateTime.Now.AddDays(-2); //最早取前2天
            string dt1 = dt.Year + dt.Month.ToString("00") + dt.Day.ToString("00");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hyqa.azurewebsites.net/DrawHistory/GetBySerialNumber?name=" + Game_Function.GameNameToCode(frm_PlanCycle.GameLotteryName) + "&startSerialNumber=" + dt1 + "&endSerialNumber=" + dt1 + "120");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            #region test in DL
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var temp = reader.ReadToEnd();
                        JArray ja = (JArray)JsonConvert.DeserializeObject(temp);

                        //處理最近開獎號碼
                        teLastFiveStart.Text = ja[0]["Number"].ToString().Replace(",", "");
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 設定預設值
        /// </summary>
        public void SetDefaultValue()
        {
            /*CheckBoxList*/
            cblFixTen.Clear();
            cblFixUnit.Clear();
            cblAnyOne.Clear();

            /*RadioButtonList*/
            rblFixTen.SelectedValue = 1;
            rblFixUnit.SelectedValue = 1;
            rblAnyOne.SelectedValue = 1;

            /*TextBox*/
            teLastFiveStart.Text = "";
        }

        /// <summary>
        /// 選取全部選項
        /// </summary>
        public void SelectAll()
        {
            /*CheckBoxList*/
            cblFixTen.SelectedAll();
            cblFixUnit.SelectedAll();
            cblAnyOne.SelectedAll();
        }

        /// <summary>
        /// 過濾數字
        /// </summary>
        public List<BaseOptions> Filter(List<BaseOptions> tmp)
        {
            tmp = Calculation.PosNumber(tmp, cblFixTen,  "0" , (int)rblFixTen.SelectedValue == 1);
            tmp = Calculation.PosNumber(tmp, cblFixUnit,  "1" , (int)rblFixUnit.SelectedValue == 1);
            tmp = Calculation.PosNumber(tmp, cblAnyOne, "*" , (int)rblAnyOne.SelectedValue == 1);
            return tmp;
        }
    }
}
