﻿using System;
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
    /// multiple.xaml 的互動邏輯
    /// </summary>
    public partial class multiple : UserControl
    {
        public multiple()
        {
            InitializeComponent();
            InitCompent();
        }

        private void InitCompent()
        {
            cbPlan.SelectedIndex = 0;
            cbCost.SelectedIndex = 0;
            cbCount.SelectedIndex = 0;
            cbMoney.SelectedIndex = 0;
            tbFiexed.Text = "10";
            cbPercent.SelectedIndex = 0;
        }

        bool isFirstTime = true;
        private void setRadioBT()
        {
            if (!isFirstTime)
            {
                if (rbFiexed.IsChecked == true)
                {
                    rbPercent.IsChecked = false;
                    rbProgress.IsChecked = false;

                    cbPercent.IsEnabled = false;
                    tbProgressStart.IsEnabled = false;
                    tbProgressAdd.IsEnabled = false;
                    tbFiexed.IsEnabled = true;
                }
                else if (rbPercent.IsChecked == true)
                {
                    rbFiexed.IsChecked = false;
                    rbProgress.IsChecked = false;

                    cbPercent.IsEnabled = true;
                    tbProgressStart.IsEnabled = false;
                    tbProgressAdd.IsEnabled = false;
                    tbFiexed.IsEnabled = false;
                }
                else if (rbProgress.IsChecked == true)
                {
                    rbFiexed.IsChecked = false;
                    rbPercent.IsChecked = false;

                    cbPercent.IsEnabled = false;
                    tbProgressStart.IsEnabled = true;
                    tbProgressAdd.IsEnabled = true;
                    tbFiexed.IsEnabled = false;
                }
            }
            isFirstTime = false;
        }

        private void rbFiexed_Checked(object sender, RoutedEventArgs e)
        {
            setRadioBT();
        }

        private void rbProgress_Checked(object sender, RoutedEventArgs e)
        {
            setRadioBT();
        }

        private void rbPercent_Checked(object sender, RoutedEventArgs e)
        {
            setRadioBT();
        }

        private void btCal_Click(object sender, RoutedEventArgs e)
        {
            List<content> lt = new List<content>();
            if (rbFiexed.IsChecked == true)
            {
                int multiple = 1;//倍數
                int issue = int.Parse(cbPlan.Text);//期數
                int count = int.Parse(cbCount.Text);//注數
                double oneCost = double.Parse(cbCost.Text);//單注成本
                double oneMoney = double.Parse(cbMoney.Text);//單注獎金
                double sumMoneyTemp = 0;
                //tbFiexed

                while (((((oneMoney * multiple) - sumMoneyTemp) / sumMoneyTemp) * 100) < double.Parse(tbFiexed.Text))
                {
                    multiple++;
                }

                bool haveError = false;

                for (int i = 0; i < issue; i++)
                {
                    if (i == 0)
                    {
                        while ((oneMoney * multiple) - (oneCost * count * multiple) <= double.Parse(tbFiexed.Text) && !haveError)
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                    }
                    else
                    {
                        while ((oneMoney * multiple) - (sumMoneyTemp + (oneCost * count * multiple)) < double.Parse(tbFiexed.Text))
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                    }
                    if (haveError)
                    {
                        MessageBox.Show("此计划不适合倍投。");
                        break;
                    }

                    sumMoneyTemp += (oneCost * count * multiple);

                    lt.Add(new content()
                    {
                        issue = i + 1,
                        multiple = multiple,
                        currentMoney = oneCost * count * multiple,//當期投入 = 單注成本*注數*倍數
                        sumMoney = sumMoneyTemp,
                        income = oneMoney * multiple,//收益 = 單注獎金*倍數
                        profit = (oneMoney * multiple) - sumMoneyTemp,//利潤 = 收益 - 累計投入
                        returns = ((((oneMoney * multiple) - sumMoneyTemp) / sumMoneyTemp) * 100).ToString() + "%" //回報率 = 利潤/累計投入
                    });
                }

            }
            else if (rbPercent.IsChecked == true)
            {
                int multiple = 1;//倍數(不知道邏輯先給2)
                int issue = int.Parse(cbPlan.Text);//期數
                int count = int.Parse(cbCount.Text);//注數
                double oneCost = double.Parse(cbCost.Text);//單注成本
                double oneMoney = double.Parse(cbMoney.Text);//單注獎金
                double sumMoneyTemp = 0;
                bool haveError = false;

                for (int i = 0; i < issue; i++)
                {

                    if (i == 0)
                    {
                        while ((((oneMoney * multiple) - (oneCost * count * multiple)) / (oneCost * count * multiple)) * 100 < double.Parse(cbPercent.Text))
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                    }
                    else
                    {
                        while ((((oneMoney * multiple) - (sumMoneyTemp + (oneCost * count * multiple))) / (sumMoneyTemp + (oneCost * count * multiple))) * 100 < double.Parse(cbPercent.Text))
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                    }


                    if (haveError)
                    {
                        MessageBox.Show("此计划不适合倍投。");
                        break;
                    }


                    sumMoneyTemp += (oneCost * count * multiple);
                    lt.Add(new content()
                    {
                        issue = i + 1,
                        multiple = multiple,
                        currentMoney = oneCost * count * multiple,//當期投入 = 單注成本*注數*倍數
                        sumMoney = sumMoneyTemp,
                        income = oneMoney * multiple,//收益 = 單注獎金*倍數
                        profit = (oneMoney * multiple) - sumMoneyTemp,//利潤 = 收益 - 累計投入
                        returns = ((((oneMoney * multiple) - sumMoneyTemp) / sumMoneyTemp) * 100).ToString() + "%" //回報率 = 利潤/累計投入
                    });
                }
            }
            else if (rbProgress.IsChecked == true)
            {
                int multiple = 1;//倍數
                int issue = int.Parse(cbPlan.Text);//期數
                int count = int.Parse(cbCount.Text);//注數
                double oneCost = double.Parse(cbCost.Text);//單注成本
                double oneMoney = double.Parse(cbMoney.Text);//單注獎金
                double sumMoneyTemp = 0;
                //tbFiexed
                bool haveError = false;
                double progress = 0.0;
                for (int i = 0; i < issue; i++)
                {
                    if (i == 0)
                    {
                        while ((oneMoney * multiple) - (oneCost * count * multiple) < double.Parse(tbProgressStart.Text) + double.Parse(tbProgressAdd.Text) && !haveError)//第一次 收益小於 起步加累進
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                        progress = (oneMoney * multiple) - (oneCost * count * multiple);
                    }
                    else
                    {
                        while ((oneMoney * multiple) - (sumMoneyTemp + (oneCost * count * multiple)) < progress+double.Parse(tbProgressAdd.Text))//收益小於 上筆利潤 加累進
                        {
                            multiple++;
                            if (multiple > 10000)
                                haveError = true;
                        }
                        if ((oneMoney * multiple) - (oneCost * count * multiple) > progress + double.Parse(tbProgressAdd.Text))
                            progress = (oneMoney * multiple) - (sumMoneyTemp + (oneCost * count * multiple));
                        else
                            progress += double.Parse(tbProgressAdd.Text);
                    }
                    if (haveError)
                    {
                        MessageBox.Show("此计划不适合倍投。");
                        break;
                    }

                    sumMoneyTemp += (oneCost * count * multiple);

                    lt.Add(new content()
                    {
                        issue = i + 1,
                        multiple = multiple,
                        currentMoney = oneCost * count * multiple,//當期投入 = 單注成本*注數*倍數
                        sumMoney = sumMoneyTemp,
                        income = oneMoney * multiple,//收益 = 單注獎金*倍數
                        profit = (oneMoney * multiple) - sumMoneyTemp,//利潤 = 收益 - 累計投入
                        returns = ((((oneMoney * multiple) - sumMoneyTemp) / sumMoneyTemp) * 100).ToString() + "%" //回報率 = 利潤/累計投入
                    });
                }

            }
            GDMaster.ItemsSource = lt;
        }

        class content
        {
            //期號
            public int issue { get; set; }
            //倍數
            public int multiple { get; set; }
            //當前投入
            public double currentMoney { get; set; }
            //累計投入
            public double sumMoney { get; set; }
            //收益
            public double income{ get; set; }
            //利潤
            public double profit { get; set; }
            //回報
            public string returns{get;set;}
        }
        
        //檢查cb key的內容 聚寶盆沒有做 會有null
        //private void cbPlan_KeyDown(object sender, KeyEventArgs e)
        //{
           
        //}
    }
    
}