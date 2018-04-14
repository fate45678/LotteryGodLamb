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
            int col1Width = 80; //期號的寬度
            int col2Width = 60; //開獎號碼的寬度
            int digitWidth = 16; //每位數的寬度
            int rowHeight = 23; //每列高度
            int fontSize = 12; //字體大小
            #endregion

            Label lbl;
            Ellipse elli;
            Line ln;
            Rectangle rec;
            canvas1.Height = (rowHeight * 8) + (period * rowHeight);

            int matchDig = 0; //0萬 1千 2百 3十 4個
            int[] number = new int[5]; //開獎號碼 0萬 1千 2百 3十 4個
            int[,] number_count = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //出現總次數 0萬 1千 2百 3十 4個 5分布
            int[,] number_miss = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //遺漏值的累加
            int[,] number_missAvg = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //平均遺漏值
            int[,] number_missSum = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //遺漏值的總和
            int[,] number_missMax = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; //最大遺漏值
            int[,] number_streak = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //連擊數  
            int[,] number_streakMax = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //最大連擊數

            int x_Temp = 0; int x_TempDig = col1Width + col2Width - 1; //目前畫到X軸的哪個位置
            int[] x_Start = new int[5] { 0, 0, 0, 0, 0 }; int[] y_Start = new int[5] { 0, 0, 0, 0, 0 }; //畫直線的起點
            int[] x_End = new int[5] { 0, 0, 0, 0, 0 }; int[] y_End = new int[5] { 0, 0, 0, 0, 0 }; //畫直線的終點
            int[,] rec_Start = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //矩形起點
            int[,] rec_Len = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ; //矩形長度

            #region 產生Header
            //期号
            lbl = new Label(); lbl.Content = "期号"; lbl.Width = col1Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp); 
            x_Temp += col1Width - 1;
            //开奖号码
            lbl = new Label(); lbl.Content = "开奖号码"; lbl.Width = col2Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp); 
            x_Temp += col2Width - 1;
            //万位~个位
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
                    lbl = new Label(); lbl.Content = headerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight); Canvas.SetLeft(lbl, x_TempDig); 
                    x_TempDig += digitWidth;
                }
            }
            //号码分布
            lbl = new Label(); lbl.Content = "号码分布"; lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, 0); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += digitWidth * 10 + 2;
            for (int headerDig2 = 0; headerDig2 < 10; headerDig2++)
            {
                lbl = new Label(); lbl.Content = headerDig2.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight); Canvas.SetLeft(lbl, x_TempDig);
                x_TempDig += digitWidth;
            }
            //設定最後Canvas的寬度
            canvas1.Width = x_Temp;
            #endregion

            #region 產生Data
            for (int rowData = 100; rowData > 0; rowData--)//100
            {
                x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
                //期數
                if (rowData <= period)
                {
                    lbl = new Label(); lbl.Content = frmGameMain.jArr[rowData - 1]["Issue"].ToString(); lbl.Width = col1Width; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 0);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2 - 1) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += col1Width - 1;
                }
                //開獎號碼
                if (rowData <= period)
                {
                    lbl = new Label(); lbl.Content = frmGameMain.jArr[rowData - 1]["Number"].ToString().Replace(",", " "); lbl.Width = col2Width; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.Tomato; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 0, 1, 0);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (rowHeight * 2 - 1) + rowHeight * (period - rowData)); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += col2Width - 1;
                }
                //記開獎號碼
                number[0] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(0, 1));
                number[1] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(2, 1));
                number[2] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(4, 1));
                number[3] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(6, 1));
                number[4] = Convert.ToInt16(frmGameMain.jArr[rowData - 1]["Number"].ToString().Substring(8, 1));
                //判斷位數
                for (int i = 0; i < 5; i++)
                {                    
                    if (i == 0)
                    {
                        if (gamekind == "m3") matchDig = 1; //比對千位
                        else if (gamekind == "b3") matchDig = 2; //比對百位
                        else if (gamekind == "b2") matchDig = 3; //比對十位
                        else matchDig = 0; //比對萬位
                    }
                    else if (i == 1)
                    {
                        if (gamekind == "m3") matchDig = 2;
                        else if (gamekind == "b3") matchDig = 3;
                        else if (gamekind == "b2") matchDig = 4;
                        else matchDig = 1;
                    }
                    else if (i == 2)
                    {
                        if (gamekind == "m3") matchDig = 3;
                        else if (gamekind == "b3") matchDig = 4;
                        else if (gamekind == "b2" || gamekind == "f2") break;
                        else matchDig = 2;
                    }
                    else if (i == 3)
                    {
                        if (gamekind == "b3" || gamekind == "m3" || gamekind == "f3") break;
                        else matchDig = 3;
                    }
                    else if (i == 4)
                    {
                        if (gamekind == "f4") break;
                        else matchDig = 4;
                    }
                    //遺漏累加
                    for (int dig = 0; dig < 10; dig++)
                    {
                        if (dig == number[matchDig]) //符合
                            number_miss[matchDig, dig] = 0;
                        else //遺漏
                            number_miss[matchDig, dig]++;
                    }                    
                    //比對開獎號碼 產生圓形&標籤&遺漏
                    if (rowData <= period)
                    {
                        for (int dig = 0; dig < 10; dig++)
                        {
                            lbl = new Label();
                            if (dig == number[matchDig]) //符合
                            {
                                if (strChartSelect != "") //要顯示走勢
                                {
                                    y_Start[i] = y_End[i];
                                    x_Start[i] = x_End[i];
                                    y_End[i] = rowHeight * (2 + (period - rowData)) + 4;
                                    x_End[i] = x_TempDig;
                                }
                                //開獎號一定顯示
                                elli = new Ellipse(); elli.Width = digitWidth; elli.Height = digitWidth; elli.Fill = Brushes.Red;
                                this.canvas1.Children.Add(elli); Canvas.SetTop(elli, rowHeight * (2 + (period - rowData)) + 4); Canvas.SetLeft(elli, x_TempDig); Canvas.SetZIndex(elli, 2);

                                lbl = new Label(); lbl.Content = dig.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0);
                                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig); Canvas.SetZIndex(lbl, 3);
                                x_TempDig += digitWidth;
                                number_count[i, dig]++;
                                //遺漏條起點
                                rec_Start[matchDig, dig] = 0;
                            }
                            else //不符合
                            {
                                lbl = new Label(); lbl.Content = ""; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.Black; lbl.Background = Brushes.White; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0);
                                                                
                                if (strMissSelect != "") //要顯示遺漏
                                {
                                    lbl.Content = number_miss[matchDig, dig]; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0);
                                }
                                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig);
                                x_TempDig += digitWidth;
                                //遺漏條起點
                                if (rec_Start[matchDig, dig] == 0) //有值就不更新
                                    rec_Start[matchDig, dig] += rowHeight * (2 + (period - rowData));
                            }
                            //最大連擊
                            if (dig == number[matchDig]) //符合
                            {
                                number_streak[i, dig]++;
                                if (number_streak[i, dig] > number_streakMax[i, dig]) //超過最大連擊就修正
                                    number_streakMax[i, dig] = number_streak[i, dig]; 
                            }
                            else //中斷
                            {
                                number_streak[i, dig] = 0; 
                            }
                        }
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
                //號碼分布
                if (rowData <= period)
                {
                    for (int dig = 0; dig < 10; dig++)
                    {
                        string numberTotal = "";
                        if (gamekind == "f5") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString() + number[3].ToString() + number[4].ToString();
                        else if (gamekind == "f4") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString() + number[3].ToString();
                        else if (gamekind == "f3") numberTotal = number[0].ToString() + number[1].ToString() + number[2].ToString();
                        else if (gamekind == "m3") numberTotal = number[1].ToString() + number[2].ToString() + number[3].ToString();
                        else if (gamekind == "b3") numberTotal = number[2].ToString() + number[3].ToString() + number[4].ToString();
                        else if (gamekind == "f2") numberTotal = number[0].ToString() + number[2].ToString();
                        else if (gamekind == "b2") numberTotal = number[3].ToString() + number[4].ToString();

                        if (numberTotal.IndexOf(dig.ToString()) > -1) //符合
                        {
                            number_count[5, dig]++;
                            elli = new Ellipse(); elli.Width = digitWidth; elli.Height = digitWidth; if (dig % 2 == 0) elli.Fill = Brushes.ForestGreen; else elli.Fill = Brushes.Purple;
                            this.canvas1.Children.Add(elli); Canvas.SetTop(elli, rowHeight * (2 + (period - rowData)) + 4); Canvas.SetLeft(elli, x_TempDig);

                            lbl = new Label(); lbl.Content = dig.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig);
                        }
                        else //不符合
                        {
                            lbl = new Label(); lbl.Content = ""; lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Transparent; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; if (dig == 9) lbl.BorderThickness = new Thickness(0, 0, 1, 0); else lbl.BorderThickness = new Thickness(0, 0, 0, 0);
                            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, rowHeight * (2 + (period - rowData))); Canvas.SetLeft(lbl, x_TempDig);
                        }
                        x_TempDig += digitWidth;
                    }
                }

            }
            //畫遺漏線 未完
            //if (strMissBarSelect != "")
            //{
            //    //判斷位數
            //    for (int i = 0; i < 5; i++)
            //    {
            //        if (i == 0)
            //        {
            //            if (gamekind == "m3") matchDig = 1; //比對千位
            //            else if (gamekind == "b3") matchDig = 2; //比對百位
            //            else if (gamekind == "b2") matchDig = 3; //比對十位
            //            else matchDig = 0; //比對萬位
            //        }
            //        else if (i == 1)
            //        {
            //            if (gamekind == "m3") matchDig = 2;
            //            else if (gamekind == "b3") matchDig = 3;
            //            else if (gamekind == "b2") matchDig = 4;
            //            else matchDig = 1;
            //        }
            //        else if (i == 2)
            //        {
            //            if (gamekind == "m3") matchDig = 3;
            //            else if (gamekind == "b3") matchDig = 4;
            //            else if (gamekind == "b2" || gamekind == "f2") break;
            //            else matchDig = 2;
            //        }
            //        else if (i == 3)
            //        {
            //            if (gamekind == "b3" || gamekind == "m3" || gamekind == "f3") break;
            //            else matchDig = 3;
            //        }
            //        else if (i == 4)
            //        {
            //            if (gamekind == "f4") break;
            //            else matchDig = 4;
            //        }
            //        for (int dig = 0; dig < 10; dig++)
            //        {
            //            rec = new Rectangle(); rec.Width = digitWidth; rec.Height = rec_Len[i, dig]; rec.Fill = Brushes.Silver;
            //            this.canvas1.Children.Add(rec); Canvas.SetTop(rec, rec_Start[i, dig]); Canvas.SetLeft(rec, 0); Canvas.SetZIndex(rec, 0);
            //        }
            //    }
            //}
            #endregion

            #region 產生Footer
            //出现总次数
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            lbl = new Label(); lbl.Content = "出现总次数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight + 1; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 1;

            for (int i = 0; i < 5; i++)
            {
                if (i == 2 && gamekind.IndexOf("2") > -1) break;
                else if (i == 3 && (gamekind.IndexOf("3") > -1 || gamekind.IndexOf("2") > -1)) break;
                else if (i == 4 && (gamekind.IndexOf("4") > -1 || gamekind.IndexOf("3") > -1 || gamekind.IndexOf("2") > -1)) break;
                for (int dig = 0; dig < 10; dig++)
                {
                    lbl = new Label(); lbl.Content = number_count[i, dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += digitWidth;
                }
            }
            for (int dig = 0; dig < 10; dig++)
            {
                lbl = new Label(); lbl.Content = number_count[5, dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 2) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                x_Temp += digitWidth;
            }
            //平均遗漏数
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            lbl = new Label(); lbl.Content = "平均遗漏数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight + 1; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 3) * rowHeight - 2); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 1;
            //最大遗漏值
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            lbl = new Label(); lbl.Content = "最大遗漏值"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight + 1; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 4) * rowHeight - 3); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 1;
            //最大连击数
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            lbl = new Label(); lbl.Content = "最大连击数"; lbl.Width = col1Width + col2Width; lbl.Height = rowHeight + 1; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 5) * rowHeight - 4); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width + col2Width - 1;

            for (int i = 0; i < 5; i++)
            {
                if (i == 2 && gamekind.IndexOf("2") > -1) break;
                else if (i == 3 && (gamekind.IndexOf("3") > -1 || gamekind.IndexOf("2") > -1)) break;
                else if (i == 4 && (gamekind.IndexOf("4") > -1 || gamekind.IndexOf("3") > -1 || gamekind.IndexOf("2") > -1)) break;
                for (int dig = 0; dig < 10; dig++)
                {
                    lbl = new Label(); lbl.Content = number_streakMax[i, dig].ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Padding = new Thickness(0); lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 5) * rowHeight - 1); Canvas.SetLeft(lbl, x_Temp);
                    x_Temp += digitWidth;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                //todo
            }
            //最末列
            x_Temp = 0; x_TempDig = col1Width + col2Width - 1; //重置
            lbl = new Label(); lbl.Content = "期号"; lbl.Width = col1Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 6) * rowHeight - 5); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col1Width - 1;
            
            lbl = new Label(); lbl.Content = "开奖号码"; lbl.Width = col2Width; lbl.Height = rowHeight * 2; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 6) * rowHeight - 5); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += col2Width - 1;

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
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 7) * rowHeight - 5); Canvas.SetLeft(lbl, x_Temp); 
                x_Temp += digitWidth * 10;

                //產生0~9
                for (int footerDig1 = 0; footerDig1 < 10; footerDig1++)
                {
                    lbl = new Label(); lbl.Content = footerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                    this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 6) * rowHeight - 4); Canvas.SetLeft(lbl, x_TempDig);
                    x_TempDig += digitWidth;
                }
            }
            //號碼分布
            for (int footerDig1 = 0; footerDig1 < 10; footerDig1++)
            {
                lbl = new Label(); lbl.Content = footerDig1.ToString(); lbl.Width = digitWidth; lbl.Height = rowHeight; lbl.FontSize = fontSize; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(0, 0, 0, 1);
                this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 6) * rowHeight - 4); Canvas.SetLeft(lbl, x_TempDig);
                x_TempDig += digitWidth;
            }
            lbl = new Label(); lbl.Content = "号码分布"; lbl.Width = digitWidth * 10 + 2; lbl.Height = rowHeight; lbl.FontSize = fontSize - 1; lbl.Foreground = Brushes.White; lbl.Background = Brushes.Black; lbl.HorizontalContentAlignment = HorizontalAlignment.Center; lbl.VerticalContentAlignment = VerticalAlignment.Center; lbl.BorderBrush = Brushes.Silver; lbl.BorderThickness = new Thickness(1, 1, 1, 1);
            this.canvas1.Children.Add(lbl); Canvas.SetTop(lbl, (period + 7) * rowHeight - 5); Canvas.SetLeft(lbl, x_Temp);
            x_Temp += digitWidth * 10;    
            #endregion
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


