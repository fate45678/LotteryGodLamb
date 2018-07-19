using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinFormsApp1
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void DrawChart(string strGameKindSelect, string strLineSelect, string strMissSelect, string strMissBarSelect, string strChartSelect, string strPeriodSelect)
        {
            //MessageBox.Show(strGameKindSelect + "," + strLineSelect + "," + strMissSelect + "," + strMissBarSelect + "," + strChartSelect + "," + strPeriodSelect);
            this.canvas1.Children.Clear();

            #region
            string gamekind = ""; //五星 四星 前三...
            if (strGameKindSelect == "五星") gamekind = "f5";
            else if (strGameKindSelect == "四星") gamekind = "f4";
            else if (strGameKindSelect == "前三") gamekind = "f3";
            else if (strGameKindSelect == "中三") gamekind = "m3";
            else if (strGameKindSelect == "后三") gamekind = "b3";
            else if (strGameKindSelect == "前二") gamekind = "f2";
            else if (strGameKindSelect == "后二") gamekind = "b2";

            int period = 0; //顯示筆數(近幾期)
            if (strPeriodSelect == "最近30期") period = 30;
            else if (strPeriodSelect == "最近50期") period = 50;
            else if (strPeriodSelect == "最近100期") period = 100;
            #endregion

            #region 版面預設值
            int col1Width = 140; //期號的寬度
            int col2Width = 100; //開獎號碼的寬度
            int digitWidth = 25; //每位數的寬度
            int col3Width = 36; //组三 组六 豹子 对子 跨度 和值 的寬度
            int col4Width = 20; //大小型态 单双型态 质合型态 012型态 的單一值寬度
            int col5Width = 60; //和值尾数 的寬度
            int rowHeight = 30; //每列高度
            int fontSize = 13; //字體大小
            //int col1Width = 120; //期號的寬度
            //int col2Width = 60; //開獎號碼的寬度
            //int digitWidth = 16; //每位數的寬度
            //int col3Width = 42; //组三 组六 豹子 对子 跨度 和值 的寬度
            //int col4Width = 30; //大小型态 单双型态 质合型态 012型态 的單一值寬度
            //int col5Width = 70; //和值尾数 的寬度
            //int rowHeight = 33; //每列高度
            //int fontSize = 12; //字體大小
            #endregion

            Label lbl;
            Ellipse elli;
            Line ln;
            Rectangle rec;
            canvas1.Height = (rowHeight * 8) + (period * rowHeight);

            int matchDig = 0; //該比對哪個位數 0:萬 1:千 2:百 3:十 4:個
            int[] number = new int[5]; //開獎號碼 0:萬 1:千 2:百 3:十 4:個
            int[,] number_count = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //出現總次數 0:萬 1:千 2:百 3:十 4:個 5:號碼分布
            int[,] number_miss = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //遺漏值 0:萬 1:千 2:百 3:十 4:個 5:號碼分布
            int[,] number_missMax = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //最大遺漏值 0:萬 1:千 2:百 3:十 4:個 5:號碼分布
            //int[,] number_missAvg = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //平均遺漏值
            //int[,] number_missSum = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //遺漏值的總和
            int[,] number_streak = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //連擊數 0:萬 1:千 2:百 3:十 4:個 5:號碼分布
            int[,] number_streakMax = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //最大連擊數 0:萬 1:千 2:百 3:十 4:個 5:號碼分布

            int y_Temp = 0; //目前畫到Y軸的哪個位置
            int x_Temp = 0; //目前畫到X軸的哪個位置(for 整個欄位)
            int x_TempDig = col1Width + col2Width - 1; //目前畫到X軸的哪個位置(for 0~9位數)
            int[] x_Start = new int[5] { 0, 0, 0, 0, 0 }; int[] y_Start = new int[5] { 0, 0, 0, 0, 0 }; //畫直線的起點
            int[] x_End = new int[5] { 0, 0, 0, 0, 0 }; int[] y_End = new int[5] { 0, 0, 0, 0, 0 }; //畫直線的終點
            int[,] rec_Start = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //矩形起點
            int[,] rec_Len = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //矩形長度

            #region 產生Header
            //欄位:期号
            lbl = new Label(); lbl.Content = "期号"; lbl.Width = col1Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width - 1;
            //欄位:开奖号码
            lbl = new Label(); lbl.Content = "开奖号码"; lbl.Width = col2Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col2Width - 1;
            //欄位:万位~个位
            for (int i = 0; i < 5; i++)
            {
                lbl = new Label(); lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                if (i == 0)
                {
                    if (gamekind == "m3") lbl.Content = "千位";
                    else if (gamekind == "b3") lbl.Content = "百位";
                    else if (gamekind == "b2") lbl.Content = "十位";
                    else lbl.Content = "万位";
                }
                else if (i == 1)
                {
                    if (gamekind == "m3") lbl.Content = "百位";
                    else if (gamekind == "b3") lbl.Content = "十位";
                    else if (gamekind == "b2") lbl.Content = "个位";
                    else lbl.Content = "千位";
                }
                else if (i == 2)
                {
                    if (gamekind == "m3") lbl.Content = "十位";
                    else if (gamekind == "b3") lbl.Content = "个位";
                    else if (gamekind == "b2" || gamekind == "f2") break;
                    else lbl.Content = "百位";
                }
                else if (i == 3)
                {
                    if (gamekind == "b3" || gamekind == "m3" || gamekind == "f3") break;
                    else lbl.Content = "十位";
                }
                else if (i == 4)
                {
                    if (gamekind == "f4") break;
                    else lbl.Content = "个位";
                }
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth * 10;

                //產生0~9
                for (int headerDig1 = 0; headerDig1 < 10; headerDig1++)
                {
                    lbl = new Label(); lbl.Content = headerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (headerDig1 == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight); Canvas.SetLeft(lbl, x_TempDig);
                    x_TempDig += digitWidth;
                }
            }
            //欄位:号码分布
            lbl = new Label(); lbl.Content = "号码分布"; lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += digitWidth * 10;
            for (int headerDig2 = 0; headerDig2 < 10; headerDig2++)
            {
                lbl = new Label(); lbl.Content = headerDig2.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight); Canvas.SetLeft(lbl, x_TempDig);
                x_TempDig += digitWidth;
            }
            //欄位:大小型态
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "大小型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:单双型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "单双型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:质合型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "质合型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:012型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "012型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:组三
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "组三"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:组六 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "组六"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:豹子                        
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "豹子"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:对子
            if (gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "对子"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:跨度
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3" || gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "跨度"; lbl.Width = col3Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            //欄位:和值
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3" || gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "和值"; lbl.Width = col3Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            //欄位:和值尾数
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "和值尾数"; lbl.Width = col5Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width;
            }
            //設定最後Canvas的寬度
            canvas1.Width = x_Temp;
            y_Temp += rowHeight * 2 - 1;
            #endregion

            #region 產生Data
            //判斷資料數量
            int initCount = 0;
            if (frmGameMain.jArr.Count < 120) initCount = frmGameMain.jArr.Count;
            else initCount = 120;
            //一列一列加上去
            for (int rowData = initCount; rowData > 0; rowData--)
            {
                x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
                //欄位:期號
                if (rowData <= period)
                {
                    lbl = new Label(); lbl.Content = frmGameMain.jArr[rowData - 1]["Issue"].ToString(); lbl.Width = col1Width; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 0);
                    //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2 - 1) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += col1Width - 1;
                }
                //欄位:開獎號碼
                if (rowData <= period)
                {
                    lbl = new Label(); lbl.Content = frmGameMain.jArr[rowData - 1]["Number"].ToString().Replace(",", " "); lbl.Width = col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.Tomato; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 0);
                    //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2 - 1) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += col2Width - 1;
                }
                //step1 記開獎號碼
                number[0] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(0, 1));
                number[1] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(2, 1));
                number[2] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(4, 1));
                number[3] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(6, 1));
                number[4] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(8, 1));
                //step2 判斷位數
                for (int i = 0; i < 5; i++)
                {
                    if (GetWhichDigit(gamekind, i) != 99)
                    {
                        matchDig = GetWhichDigit(gamekind, i);
                        //遺漏累加
                        for (int dig = 0; dig < 10; dig++)
                        {
                            if (dig == number[matchDig]) //符合
                                number_miss[matchDig, dig] = 0;
                            else //遺漏
                                number_miss[matchDig, dig]++;
                        }
                        //已進入近30/50/100筆的範圍，開始呈現資料(比對開獎號碼 產生圓形&標籤&遺漏)
                        if (rowData <= period)
                        {
                            for (int dig = 0; dig < 10; dig++)
                            {
                                lbl = new Label();
                                if (dig == number[matchDig]) //符合
                                {
                                    if (strChartSelect != "") //要顯示走勢
                                    {
                                        y_Start[i] = y_End[i] + 5;
                                        x_Start[i] = x_End[i];
                                        //y_End[i] = rowHeight * (2 + (period - rowData)) + 4;
                                        y_End[i] = y_Temp;
                                        x_End[i] = x_TempDig;
                                    }
                                    //開獎號一定顯示
                                    elli = new Ellipse(); elli.Width = digitWidth; elli.Height = digitWidth; elli.Fill = Brushes.Red;
                                    //this.canvas1.Children.Add(elli); Canvas.SetTop(elli, rowHeight * (2 + (period - rowData)) + 4); Canvas.SetLeft(elli, x_TempDig); Canvas.SetZIndex(elli, 2);
                                    this.canvas1.Children.Add(elli); Canvas.SetTop(elli, y_Temp + 4); Canvas.SetLeft(elli, x_TempDig); Canvas.SetZIndex(elli, 2);

                                    lbl = new Label(); lbl.Content = dig.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0); lbl.Padding = new Thickness(0);
                                    //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig); Canvas.SetZIndex(lbl, 3);
                                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_TempDig); Canvas.SetZIndex(lbl, 3);
                                    x_TempDig += digitWidth;
                                    number_count[matchDig, dig]++;
                                    //遺漏條起點
                                    rec_Start[matchDig, dig] = 0;
                                }
                                else //不符合
                                {
                                    lbl = new Label(); lbl.Content = ""; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0); lbl.Padding = new Thickness(0);

                                    if (strMissSelect != "") //要顯示遺漏
                                    {
                                        lbl.Content = number_miss[matchDig, dig]; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0);
                                    }
                                    //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig); Canvas.SetZIndex(lbl, 4);
                                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_TempDig); Canvas.SetZIndex(lbl, 4);
                                    x_TempDig += digitWidth;
                                    //紀錄遺漏
                                    if (number_miss[matchDig, dig] > number_missMax[matchDig, dig]) //超過最大遺漏就修正
                                        number_missMax[matchDig, dig] = number_miss[matchDig, dig];
                                    //遺漏條起點
                                    if (rec_Start[matchDig, dig] == 0) //有值就不更新
                                    {
                                        //rec_Start[matchDig, dig] += rowHeight * (2 + (period - rowData));
                                        rec_Start[matchDig, dig] += y_Temp;
                                    }
                                }
                                //最大連擊
                                if (dig == number[matchDig]) //符合
                                {
                                    number_streak[matchDig, dig]++;
                                    if (number_streak[matchDig, dig] > number_streakMax[matchDig, dig]) //超過最大連擊就修正
                                        number_streakMax[matchDig, dig] = number_streak[matchDig, dig];
                                }
                                else //中斷
                                {
                                    number_streak[matchDig, dig] = 0;
                                }
                            }
                        }
                    }
                }
                //已進入近30/50/100筆的範圍，開始呈現資料(號碼分布 組三...等右半部的資料)
                if (rowData <= period)
                {
                    string numberTotal = "";
                    if (gamekind == "f5") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString() + number[3].ToString() + number[4].ToString();
                    else if (gamekind == "f4") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString() + number[3].ToString();
                    else if (gamekind == "f3") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString();
                    else if (gamekind == "m3") numberTotal = number[1].ToString() + number[2].ToString() + number[3].ToString();
                    else if (gamekind == "b3") numberTotal = number[2].ToString() + number[3].ToString() + number[4].ToString();
                    else if (gamekind == "f2") numberTotal = number[0].ToString() + number[1].ToString();
                    else if (gamekind == "b2") numberTotal = number[3].ToString() + number[4].ToString();

                    //欄位:號碼分布
                    for (int dig = 0; dig < 10; dig++)
                    {
                        if (numberTotal.IndexOf(dig.ToString()) > -1) //符合
                        {
                            number_count[5, dig]++;

                            elli = new Ellipse(); elli.Width = digitWidth; elli.Height = digitWidth; if (dig % 2 == 0) elli.Fill = Brushes.ForestGreen; else elli.Fill = Brushes.Purple;
                            //this.canvas1.Children.Add(elli); Canvas.SetTop(elli, rowHeight * (2 + (period - rowData)) + 4); Canvas.SetLeft(elli, x_TempDig);
                            this.canvas1.Children.Add(elli); Canvas.SetTop(elli, y_Temp + 4); Canvas.SetLeft(elli, x_TempDig);

                            lbl = new Label(); lbl.Content = dig.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0); lbl.Padding = new Thickness(0);
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_TempDig);
                            //最大遺漏
                            number_miss[5, dig] = 0;

                            //最大連擊
                            number_streak[5, dig]++;
                            if (number_streak[5, dig] > number_streakMax[5, dig]) //超過最大連擊就修正
                                number_streakMax[5, dig] = number_streak[5, dig];
                        }
                        else //不符合
                        {
                            lbl = new Label(); lbl.Content = ""; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0); lbl.Padding = new Thickness(0);
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_TempDig);
                            //最大遺漏
                            number_miss[5, dig]++;
                            if (number_miss[5, dig] > number_missMax[5, dig]) //超過最大遺漏就修正
                                number_missMax[5, dig] = number_miss[5, dig];
                            //最大連擊
                            number_streak[5, dig] = 0;
                        }
                        x_TempDig += digitWidth;
                    }
                    x_Temp = x_TempDig;

                    //欄位:大小型态
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            lbl = new Label(); lbl.Width = col4Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.CornflowerBlue; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (k == 2) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0); ;
                            lbl.Content = Game_Function.GetNumBigOrSmall(Convert.ToInt16(numberTotal.Substring(k, 1)));
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                            x_Temp += col4Width;
                        }
                        //x_Temp -= 1;
                    }
                    //欄位:单双型态 
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            lbl = new Label(); lbl.Width = col4Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.YellowGreen; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (k == 2) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0); ;
                            lbl.Content = Game_Function.GetNumOddOrEven(Convert.ToInt16(numberTotal.Substring(k, 1)));
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                            x_Temp += col4Width;
                        }
                        //x_Temp -= 1;
                    }
                    //欄位:质合型态 
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            lbl = new Label(); lbl.Width = col4Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.CornflowerBlue; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (k == 2) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0); ;
                            lbl.Content = Game_Function.GetNumPrimeOrNot(Convert.ToInt16(numberTotal.Substring(k, 1)));
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                            x_Temp += col4Width;
                        }
                        //x_Temp -= 1;
                    }
                    //欄位:012型态 
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            lbl = new Label(); lbl.Width = col4Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.YellowGreen; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (k == 2) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0); ;
                            lbl.Content = Game_Function.GetNum012(Convert.ToInt16(numberTotal.Substring(k, 1)));
                            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                            x_Temp += col4Width;
                        }
                        //x_Temp -= 1;
                    }
                    //欄位:组三
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 1, 0); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumIsSame2(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width;
                    }
                    //欄位:组六 
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 1, 0); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumIsDiff(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width;
                    }
                    //欄位:豹子                        
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 1, 0); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumAllTheSame(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width - 1;
                    }
                    //欄位:对子
                    if (gamekind == "f2" || gamekind == "b2")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 1, 0); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumIsPair(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width - 1;
                    }
                    //欄位:跨度
                    if (gamekind == "f2" || gamekind == "b2")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.CornflowerBlue; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 1); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumSubstract(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width - 1;
                    }
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.CornflowerBlue; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 1); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumSubstract(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width - 1;
                    }
                    //欄位:和值
                    if (gamekind == "f2" || gamekind == "b2")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Tomato; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 1); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumSum(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width;
                    }
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col3Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Tomato; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 1); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumSum(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col3Width;
                    }
                    //欄位:和值尾数
                    if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
                    {
                        lbl = new Label(); lbl.Width = col5Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 1, 0); lbl.Padding = new Thickness(0); ;
                        lbl.Content = Game_Function.GetNumSumTrail(Convert.ToInt16(numberTotal.Substring(0, 1)), Convert.ToInt16(numberTotal.Substring(1, 1)), Convert.ToInt16(numberTotal.Substring(2, 1)));
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += col5Width;
                    }
                }
                //畫走勢線
                for (int i = 0; i < 5; i++)
                {
                    if (y_Start[i] != 0 && x_Start[i] != 0) //畫走勢線
                    {
                        ln = new Line(); ln.X1 = x_Start[i] + digitWidth / 2; ln.Y1 = y_Start[i] + digitWidth / 2; ln.X2 = x_End[i] + digitWidth / 2; ln.Y2 = y_End[i] + digitWidth / 2; ln.Stroke = Brushes.Red; ln.StrokeThickness = 1;
                        this.canvas1.Children.Add(ln); Canvas.SetZIndex(ln, 1);
                    }
                }
                //畫輔助線
                if (strLineSelect != "")
                {
                    if (rowData % 5 == 0)
                    {
                        ln = new Line(); ln.X1 = 0; ln.Y1 = y_Temp; ln.X2 = x_Temp; ln.Y2 = y_Temp; ln.Stroke = Brushes.Red; ln.StrokeThickness = 1;
                        this.canvas1.Children.Add(ln); Canvas.SetZIndex(ln, 1);
                    }
                }
                if (rowData <= period)
                {
                    y_Temp += rowHeight;
                }
            }
            //畫遺漏線 
            if (strMissBarSelect != "")
            {
                x_Temp = col1Width + col2Width - 1; //重置
                //判斷位數
                for (int i = 0; i < 5; i++)
                {
                    if (GetWhichDigit(gamekind, i) != 99)
                    {
                        for (int dig = 0; dig < 10; dig++)
                        {
                            if (rec_Start[GetWhichDigit(gamekind, i), dig] != 0) //紀錄遺漏條長度
                            {
                                //rec_Len[GetWhichDigit(gamekind, i), dig] = period * 25 - rec_Start[GetWhichDigit(gamekind, i), dig];
                                rec_Len[GetWhichDigit(gamekind, i), dig] = y_Temp - rec_Start[GetWhichDigit(gamekind, i), dig];
                            }
                            rec = new Rectangle(); rec.Width = digitWidth - 1; rec.Height = rec_Len[GetWhichDigit(gamekind, i), dig]; rec.Opacity = 0.6;
                            if (dig % 2 == 0) rec.Fill = Brushes.Gray; else rec.Fill = Brushes.YellowGreen;
                            this.canvas1.Children.Add(rec); Canvas.SetTop(rec, rec_Start[GetWhichDigit(gamekind, i), dig]); Canvas.SetLeft(rec, x_Temp); Canvas.SetZIndex(rec, 0);
                            x_Temp += digitWidth;
                        }
                    }
                }
            }
            #endregion

            #region 產生Footer
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            #region 第一列
            //欄位:出现总次数
            lbl = new Label(); lbl.Content = "出现总次数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 2;
            //欄位:比對結果
            for (int i = 0; i < 5; i++)
            {
                if (GetWhichDigit(gamekind, i) != 99)
                {
                    for (int dig = 0; dig < 10; dig++)
                    {
                        lbl = new Label(); lbl.Content = number_count[GetWhichDigit(gamekind, i), dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                        //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += digitWidth;
                    }
                }
            }
            //欄位:號碼分布
            for (int dig = 0; dig < 10; dig++)
            {
                lbl = new Label(); lbl.Content = number_count[5, dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth;
            }
            //欄位:顯示空框框
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3") //大小型态 单双型态 质合型态 012型态 组三 组六 豹子 跨度 和值 和值尾数 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                //this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col5Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width;
            }
            if (gamekind == "f2" || gamekind == "b2") //對子 跨度 和值 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            #endregion

            #region 第二列
            //平均遗漏数
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            y_Temp += rowHeight - 1;
            lbl = new Label(); lbl.Content = "平均遗漏数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 2;
            //欄位:比對結果
            for (int i = 0; i < 5; i++)
            {
                if (GetWhichDigit(gamekind, i) != 99)
                {
                    for (int dig = 0; dig < 10; dig++)
                    {
                        lbl = new Label(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                        if (number_count[GetWhichDigit(gamekind, i), dig] == 0)
                            lbl.Content = "31";
                        else
                            lbl.Content = Math.Ceiling(Convert.ToDecimal((period - number_count[GetWhichDigit(gamekind, i), dig])) / Convert.ToDecimal(number_count[GetWhichDigit(gamekind, i), dig])) + 1;
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += digitWidth;
                    }
                }
            }
            //欄位:號碼分布
            for (int dig = 0; dig < 10; dig++)
            {
                lbl = new Label(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                if (number_count[5, dig] == 0)
                    lbl.Content = "31";
                else
                    lbl.Content = Math.Ceiling(Convert.ToDecimal((period - number_count[5, dig])) / Convert.ToDecimal(number_count[5, dig])) + 1;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth;
            }
            //欄位:顯示空框框
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3") //大小型态 单双型态 质合型态 012型态 组三 组六 豹子 跨度 和值 和值尾数 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col5Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width;
            }
            if (gamekind == "f2" || gamekind == "b2") //對子 跨度 和值 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            #endregion

            #region 第三列
            //最大遗漏值
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            y_Temp += rowHeight - 1;
            lbl = new Label(); lbl.Content = "最大遗漏值"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 2;
            //欄位:比對結果
            for (int i = 0; i < 5; i++)
            {
                if (GetWhichDigit(gamekind, i) != 99)
                {
                    for (int dig = 0; dig < 10; dig++)
                    {
                        lbl = new Label(); lbl.Content = number_missMax[GetWhichDigit(gamekind, i), dig]; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += digitWidth;
                    }
                }
            }
            //欄位:號碼分布
            for (int dig = 0; dig < 10; dig++)
            {
                lbl = new Label(); lbl.Content = number_missMax[5, dig]; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth;
            }
            //欄位:顯示空框框
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3") //大小型态 单双型态 质合型态 012型态 组三 组六 豹子 跨度 和值 和值尾数 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col5Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width;
            }
            if (gamekind == "f2" || gamekind == "b2") //對子 跨度 和值 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            #endregion

            #region 第四列
            //最大连击数
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            y_Temp += rowHeight - 1;
            lbl = new Label(); lbl.Content = "最大连击数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 2;
            //欄位:比對結果
            for (int i = 0; i < 5; i++)
            {
                if (GetWhichDigit(gamekind, i) != 99)
                {
                    for (int dig = 0; dig < 10; dig++)
                    {
                        lbl = new Label(); lbl.Content = number_streakMax[GetWhichDigit(gamekind, i), dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                        this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                        x_Temp += digitWidth;
                    }
                }
            }
            //欄位:號碼分布
            for (int dig = 0; dig < 10; dig++)
            {
                lbl = new Label(); lbl.Content = number_streakMax[5, dig]; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 0) lbl.BorderThickness = new Thickness(1, 1, 0, 1); else lbl.BorderThickness = new Thickness(0, 1, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth;
            }
            //欄位:顯示空框框
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3") //大小型态 单双型态 质合型态 012型态 组三 组六 豹子 跨度 和值 和值尾数 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col5Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width;
            }
            if (gamekind == "f2" || gamekind == "b2") //對子 跨度 和值 (直接顯示空框框)
            {
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
                lbl = new Label(); lbl.Content = ""; lbl.Width = col3Width + 1; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.DimGray; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1); lbl.Padding = new Thickness(0); ;
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            #endregion

            #region 最末列
            //欄位:期号
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            y_Temp += rowHeight - 1;
            lbl = new Label(); lbl.Content = "期号"; lbl.Width = col1Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width - 1;
            //欄位:开奖号码
            lbl = new Label(); lbl.Content = "开奖号码"; lbl.Width = col2Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col2Width - 1;
            //欄位:万位~个位
            for (int i = 0; i < 5; i++)
            {
                lbl = new Label(); lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                if (i == 0)
                {
                    if (gamekind == "m3") lbl.Content = "千位";
                    else if (gamekind == "b3") lbl.Content = "百位";
                    else if (gamekind == "b2") lbl.Content = "十位";
                    else lbl.Content = "万位";
                }
                else if (i == 1)
                {
                    if (gamekind == "m3") lbl.Content = "百位";
                    else if (gamekind == "b3") lbl.Content = "十位";
                    else if (gamekind == "b2") lbl.Content = "个位";
                    else lbl.Content = "千位";
                }
                else if (i == 2)
                {
                    if (gamekind == "m3") lbl.Content = "十位";
                    else if (gamekind == "b3") lbl.Content = "个位";
                    else if (gamekind == "b2" || gamekind == "f2") break;
                    else lbl.Content = "百位";
                }
                else if (i == 3)
                {
                    if (gamekind == "b3" || gamekind == "m3" || gamekind == "f3") break;
                    else lbl.Content = "十位";
                }
                else if (i == 4)
                {
                    if (gamekind == "f4") break;
                    else lbl.Content = "个位";
                }
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + rowHeight); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth * 10;

                //產生0~9
                for (int footerDig1 = 0; footerDig1 < 10; footerDig1++)
                {
                    lbl = new Label(); lbl.Content = footerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (footerDig1 == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 1); else lbl.BorderThickness = new Thickness(0, 0, 0, 1); lbl.Padding = new Thickness(0); ;
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_TempDig);
                    x_TempDig += digitWidth;
                }
            }
            //欄位:號碼分布
            for (int footerDig1 = 0; footerDig1 < 10; footerDig1++)
            {
                lbl = new Label(); lbl.Content = footerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + 1); Canvas.SetLeft(lbl, x_TempDig);
                x_TempDig += digitWidth;
            }
            lbl = new Label(); lbl.Content = "号码分布"; lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp + rowHeight); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += digitWidth * 10;
            //欄位:大小型态
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "大小型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:单双型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "单双型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:质合型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "质合型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:012型态 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "012型态"; lbl.Width = col4Width * 3 + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col4Width * 3;
            }
            //欄位:组三
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "组三"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:组六 
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "组六"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:豹子                        
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "豹子"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:对子
            if (gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "对子"; lbl.Width = col3Width + 1; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width;
            }
            //欄位:跨度
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3" || gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "跨度"; lbl.Width = col3Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            //欄位:和值
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3" || gamekind == "f2" || gamekind == "b2")
            {
                lbl = new Label(); lbl.Content = "和值"; lbl.Width = col3Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col3Width - 1;
            }
            //欄位:和值尾数
            if (gamekind == "f3" || gamekind == "m3" || gamekind == "b3")
            {
                lbl = new Label(); lbl.Content = "和值尾数"; lbl.Width = col5Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, y_Temp); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += col5Width - 1;
            }
            y_Temp += rowHeight * 2 + 2;
            #endregion

            #endregion
            canvas1.Height = y_Temp;
        }

        //判斷應比對哪一個位數，回傳99表示比對中止
        private int GetWhichDigit(string game, int i)
        {
            int result = 99;
            if (i == 0)
            {
                if (game == "m3") result = 1; //比對千位
                else if (game == "b3") result = 2; //比對百位
                else if (game == "b2") result = 3; //比對十位
                else result = 0; //比對萬位
            }
            else if (i == 1)
            {
                if (game == "m3") result = 2;
                else if (game == "b3") result = 3;
                else if (game == "b2") result = 4;
                else result = 1;
            }
            else if (i == 2)
            {
                if (game == "m3") result = 3;
                else if (game == "b3") result = 4;
                else if (game == "b2" || game == "f2") result = 99;
                else result = 2;
            }
            else if (i == 3)
            {
                if (game == "b2" || game == "f2" || game == "b3" || game == "m3" || game == "f3") result = 99;
                else result = 3;
            }
            else if (i == 4)
            {
                if (game == "b2" || game == "f2" || game == "b3" || game == "m3" || game == "f3" || game == "f4") result = 99;
                else result = 4;
            }
            return result;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(frm_Chart.strGameKindSelect);
            //MessageBox.Show(frmGameMain.strHistory);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="content">文字</param>
        /// <param name="height">高</param>
        /// <param name="width">寬</param>
        /// <param name="fontsize">字體大小</param>
        /// <param name="forecolor">字體顏色</param>
        /// <param name="backcolor">背景顏色</param>
        /// <param name="bordercolor">框限顏色</param>
        /// <param name="borderleft">左框寬度</param>
        /// <param name="bordertop">上框寬度</param>
        /// <param name="borderright">右框寬度</param>
        /// <param name="borderbottom">下框寬度</param>
        /// <returns></returns>
        public Label SetLabel(Label lbl, string content, int height, int width, int fontsize, Brushes forecolor, Brushes backcolor, Brushes bordercolor, int borderleft, int bordertop, int borderright, int borderbottom)
        {
            //lbl.Content = content;
            //lbl.Width = width;
            //lbl.Height = height; 
            //lbl.FontSize = fontsize;
            //lbl.Foreground = Brushes.forecolor;
            //lbl.Background = backcolor; 
            //lbl.HorizontalContentAlignment = HorizontalAlignment.Center; 
            //lbl.VerticalContentAlignment = VerticalAlignment.Center; 
            //lbl.BorderBrush = Brushes.Silver; 
            //if (dig2 == 9) 
            //    lbl.BorderThickness = new Thickness(0, 0, 1, 0); 
            //else 
            //    lbl.BorderThickness = new Thickness(0, 0, 0, 0); 
            return lbl;
        }
    }
}


