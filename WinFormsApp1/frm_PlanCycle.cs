using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinFormsApp1
{
    public partial class frm_PlanCycle : Form
    {
        string[] numHistory = {"000, 002, 016, 019, 025, 026, 028, 029, 031, 032, 033, 035, 040, 041, 042, 048, 058, 061, 065, 074, 075, 079, 085, 089, 090, 094, 098, 100, 101, 103, 104, 106, 107, 112, 114, 121, 122, 129, 133, 134, 136, 139, 140, 148, 149, 150, 152, 159, 160, 165, 167, 171, 177, 178, 179, 181, 183, 184, 187, 191, 194, 197, 198, 199, 201, 202, 209, 210, 215, 218, 219, 223, 224, 225, 231, 232, 233, 235, 237, 244, 245, 247, 251, 252, 253, 258, 260, 266, 268, 270, 272, 276, 277, 278, 284, 286, 287, 288, 290, 291, 293, 294, 295, 297, 299, 300, 306, 318, 319, 321, 322, 326, 329, 331, 332, 335, 340, 343, 347, 348, 356, 359, 365, 374, 375, 378, 380, 391, 392, 393, 399, 400, 412, 414, 417, 418, 423, 427, 428, 429, 430, 432, 435, 438, 446, 448, 454, 455, 462, 464, 466, 467, 476, 479, 481, 482, 483, 487, 489, 491, 492, 493, 494, 495, 497, 504, 505, 507, 508, 510, 514, 515, 517, 518, 523, 528, 530, 536, 538, 539, 542, 543, 544, 547, 548, 549, 552, 554, 556, 560, 562, 564, 565, 566, 568, 569, 572, 575, 578, 579, 580, 583, 587, 589, 595, 599, 604, 613, 614, 621, 623, 624, 629, 631, 633, 634, 638, 641, 644, 645, 646, 647, 648, 649, 650, 652, 653, 661, 663, 666, 668, 671, 675, 677, 679, 680, 682, 686, 687, 688, 696, 704, 706, 707, 708, 712, 715, 717, 718, 722, 724, 725, 726, 727, 728, 733, 734, 738, 739, 742, 744, 750, 751, 755, 756, 758, 759, 762, 763, 766, 768, 772, 773, 774, 780, 782, 789, 791, 793, 801, 802, 803, 805, 807, 810, 817, 818, 820, 823, 824, 826, 829, 830, 831, 835, 837, 838, 841, 842, 844, 846, 847, 848, 850, 855, 857, 858, 859, 861, 865, 867, 876, 878, 881, 883, 887, 888, 891, 892, 893, 894, 899, 903, 904, 907, 912, 916, 917, 922, 924, 925, 928, 930, 934, 938, 939, 940, 946, 949, 953, 958, 960, 965, 967, 968, 976, 978, 979, 980, 983",
                                "003, 007, 013, 016, 017, 019, 024, 028, 029, 031, 034, 041, 042, 043, 044, 045, 049, 050, 051, 055, 056, 060, 062, 064, 066, 069, 072, 073, 079, 081, 083, 085, 086, 087, 090, 096, 098, 100, 101, 106, 108, 109, 110, 112, 113, 115, 117, 118, 121, 122, 129, 132, 136, 138, 139, 145, 153, 154, 159, 163, 164, 167, 168, 169, 171, 174, 176, 178, 183, 184, 186, 187, 193, 194, 198, 202, 209, 213, 214, 215, 220, 221, 222, 224, 225, 226, 230, 231, 235, 237, 239, 242, 244, 249, 251, 252, 254, 256, 261, 263, 264, 268, 269, 278, 279, 280, 281, 282, 284, 286, 289, 290, 293, 298, 301, 306, 308, 312, 313, 314, 316, 318, 319, 320, 322, 326, 328, 335, 337, 338, 339, 348, 349, 355, 361, 362, 372, 373, 378, 380, 383, 384, 386, 388, 391, 397, 399, 402, 403, 406, 410, 411, 413, 414, 415, 417, 418, 420, 430, 432, 443, 445, 447, 448, 449, 455, 456, 459, 460, 465, 466, 471, 479, 481, 487, 494, 495, 497, 499, 500, 501, 507, 514, 515, 519, 521, 524, 526, 527, 531, 534, 539, 542, 548, 549, 550, 551, 553, 555, 563, 574, 577, 578, 579, 589, 590, 592, 593, 595, 597, 599, 600, 602, 608, 610, 614, 615, 617, 619, 620, 622, 627, 628, 629, 630, 632, 633, 636, 637, 639, 646, 649, 658, 670, 672, 675, 676, 677, 683, 686, 687, 688, 692, 693, 694, 695, 698, 699, 700, 704, 706, 708, 710, 711, 713, 717, 718, 720, 721, 726, 727, 736, 737, 742, 743, 744, 745, 747, 750, 751, 753, 756, 761, 762, 766, 773, 776, 777, 778, 780, 781, 782, 783, 784, 794, 798, 800, 803, 807, 809, 812, 817, 819, 822, 825, 826, 828, 829, 830, 832, 835, 837, 846, 848, 851, 857, 858, 859, 860, 862, 865, 868, 869, 870, 875, 879, 880, 881, 887, 889, 893, 900, 901, 911, 913, 914, 916, 922, 923, 927, 938, 940, 941, 945, 948, 951, 954, 958, 960, 968, 969, 970, 972, 981, 982, 983, 987, 991, 993, 998",
                                "003, 004, 005, 008, 010, 014, 015, 016, 019, 021, 022, 026, 028, 029, 031, 033, 035, 039, 040, 045, 047, 052, 054, 057, 059, 060, 061, 062, 065, 067, 072, 073, 081, 086, 087, 088, 091, 092, 093, 094, 095, 104, 108, 109, 113, 115, 116, 118, 120, 122, 126, 128, 131, 135, 136, 137, 141, 143, 147, 156, 157, 163, 166, 168, 172, 173, 174, 179, 183, 188, 191, 194, 195, 199, 201, 202, 204, 205, 207, 212, 219, 221, 222, 224, 227, 228, 229, 231, 234, 238, 243, 245, 252, 256, 262, 266, 274, 275, 282, 286, 288, 290, 294, 300, 301, 302, 303, 307, 309, 310, 311, 315, 316, 319, 320, 321, 323, 324, 326, 332, 336, 341, 344, 345, 350, 351, 354, 357, 359, 371, 374, 375, 379, 385, 391, 392, 395, 398, 400, 402, 404, 406, 410, 411, 415, 416, 418, 426, 433, 440, 447, 450, 451, 457, 458, 464, 465, 467, 468, 469, 478, 480, 481, 483, 486, 489, 490, 491, 493, 495, 498, 507, 510, 512, 514, 520, 521, 523, 526, 531, 534, 536, 538, 539, 543, 544, 546, 547, 548, 549, 553, 559, 561, 570, 572, 573, 575, 576, 578, 579, 581, 585, 587, 588, 590, 593, 596, 598, 599, 606, 608, 609, 610, 613, 615, 617, 620, 624, 627, 629, 630, 635, 639, 641, 644, 646, 650, 654, 655, 656, 658, 659, 661, 662, 663, 664, 666, 669, 685, 686, 687, 690, 693, 696, 700, 702, 703, 706, 707, 709, 717, 719, 721, 724, 726, 729, 735, 737, 744, 746, 747, 752, 754, 757, 758, 759, 760, 761, 763, 769, 770, 773, 774, 781, 782, 791, 792, 794, 796, 797, 798, 802, 803, 805, 807, 808, 810, 815, 816, 821, 822, 823, 824, 825, 832, 835, 838, 839, 842, 843, 845, 849, 852, 857, 868, 870, 871, 872, 876, 879, 881, 884, 886, 894, 896, 902, 905, 919, 921, 923, 924, 926, 929, 930, 932, 940, 944, 945, 947, 953, 954, 956, 957, 959, 960, 964, 966, 968, 969, 977, 978, 979, 981, 983, 984, 986, 989, 992, 996, 998",
                                "002, 005, 006, 007, 008, 009, 011, 012, 016, 021, 027, 028, 031, 032, 034, 035, 037, 042, 048, 051, 052, 053, 057, 060, 061, 062, 067, 070, 071, 072, 074, 075, 076, 078, 080, 081, 085, 087, 088, 091, 097, 098, 099, 104, 107, 110, 115, 116, 118, 122, 123, 127, 130, 133, 134, 136, 137, 138, 140, 142, 144, 152, 155, 156, 159, 163, 164, 166, 169, 174, 175, 176, 179, 184, 187, 190, 194, 197, 198, 202, 206, 208, 209, 211, 212, 221, 222, 225, 226, 231, 233, 236, 238, 240, 242, 243, 248, 253, 254, 268, 271, 273, 275, 276, 281, 283, 285, 288, 289, 297, 299, 304, 308, 310, 318, 323, 324, 327, 329, 332, 334, 336, 337, 340, 341, 343, 345, 348, 349, 352, 353, 358, 360, 363, 366, 367, 369, 372, 373, 374, 376, 378, 385, 387, 396, 397, 398, 402, 407, 409, 413, 421, 422, 423, 424, 428, 430, 437, 442, 445, 446, 463, 466, 468, 469, 478, 482, 486, 488, 489, 494, 499, 500, 501, 502, 505, 507, 509, 510, 511, 514, 517, 518, 519, 521, 523, 524, 525, 526, 527, 530, 533, 536, 538, 540, 542, 543, 550, 551, 557, 558, 559, 563, 564, 565, 570, 571, 572, 578, 580, 586, 590, 591, 594, 597, 602, 603, 604, 606, 607, 615, 618, 619, 625, 627, 629, 630, 641, 642, 643, 645, 646, 651, 657, 659, 665, 666, 667, 668, 669, 672, 673, 681, 687, 691, 694, 701, 705, 710, 711, 712, 713, 714, 720, 721, 722, 724, 728, 730, 731, 733, 739, 741, 742, 749, 757, 759, 762, 763, 767, 769, 770, 772, 784, 786, 790, 792, 796, 797, 798, 800, 802, 803, 804, 807, 808, 810, 811, 812, 814, 823, 824, 826, 827, 828, 829, 832, 833, 835, 840, 841, 846, 850, 857, 858, 861, 862, 863, 866, 869, 870, 873, 880, 884, 885, 886, 890, 894, 895, 901, 904, 905, 907, 908, 909, 911, 917, 925, 926, 927, 929, 931, 940, 943, 945, 946, 953, 954, 955, 960, 966, 970, 973, 974, 977, 980, 981, 982, 990, 996",
                                "001, 004, 005, 009, 012, 014, 016, 019, 020, 025, 026, 035, 036, 040, 049, 052, 057, 058, 059, 060, 063, 065, 067, 068, 076, 079, 088, 094, 096, 100, 102, 105, 109, 111, 112, 115, 117, 118, 121, 126, 128, 129, 130, 133, 138, 141, 143, 149, 150, 152, 153, 155, 158, 160, 162, 165, 168, 169, 171, 172, 176, 179, 183, 184, 187, 193, 194, 198, 200, 208, 212, 213, 214, 217, 218, 223, 224, 227, 235, 239, 240, 242, 243, 248, 249, 252, 254, 256, 265, 266, 268, 270, 273, 279, 281, 282, 283, 284, 289, 293, 301, 303, 304, 309, 310, 314, 316, 319, 324, 330, 332, 333, 338, 339, 340, 344, 347, 349, 351, 352, 353, 355, 356, 358, 359, 361, 364, 368, 373, 377, 379, 380, 383, 388, 389, 390, 395, 396, 400, 402, 403, 408, 411, 413, 414, 417, 418, 419, 421, 433, 434, 435, 440, 443, 446, 449, 451, 457, 460, 461, 463, 470, 471, 472, 475, 477, 481, 486, 487, 489, 491, 492, 493, 494, 495, 497, 500, 503, 511, 517, 519, 520, 522, 524, 525, 527, 530, 531, 535, 537, 539, 543, 548, 554, 556, 558, 562, 564, 567, 577, 578, 580, 583, 588, 589, 592, 593, 596, 599, 606, 607, 608, 609, 611, 615, 616, 617, 621, 626, 629, 632, 633, 634, 637, 638, 643, 646, 651, 653, 655, 656, 657, 659, 662, 663, 665, 666, 668, 673, 674, 675, 678, 679, 683, 693, 696, 697, 704, 706, 709, 711, 712, 715, 717, 720, 721, 725, 728, 732, 736, 739, 741, 742, 745, 746, 751, 754, 757, 759, 760, 764, 766, 769, 770, 774, 775, 779, 786, 787, 789, 790, 800, 803, 807, 809, 812, 813, 816, 817, 819, 822, 824, 825, 830, 833, 834, 835, 837, 840, 843, 848, 849, 853, 854, 855, 857, 858, 859, 865, 866, 867, 870, 873, 874, 876, 877, 879, 883, 887, 892, 894, 895, 896, 897, 898, 899, 904, 906, 910, 919, 922, 928, 929, 933, 938, 942, 945, 947, 948, 953, 958, 959, 965, 968, 970, 975, 978, 991, 994, 995",
                                "001, 008, 009, 011, 012, 013, 014, 015, 017, 019, 022, 026, 027, 031, 034, 036, 037, 038, 039, 040, 046, 048, 052, 054, 056, 057, 058, 060, 062, 073, 075, 076, 078, 087, 091, 092, 094, 095, 098, 099, 100, 107, 113, 114, 117, 121, 123, 124, 126, 131, 136, 139, 140, 142, 146, 148, 151, 152, 155, 156, 158, 159, 160, 161, 162, 163, 164, 165, 167, 169, 171, 179, 183, 184, 186, 187, 192, 194, 197, 199, 200, 201, 202, 204, 205, 210, 211, 213, 217, 218, 219, 220, 223, 226, 232, 234, 235, 236, 240, 242, 247, 250, 255, 257, 260, 261, 262, 264, 271, 272, 273, 274, 280, 281, 285, 289, 292, 295, 299, 301, 302, 309, 310, 314, 315, 317, 319, 321, 326, 329, 330, 331, 337, 341, 342, 345, 348, 362, 364, 365, 366, 370, 371, 372, 373, 378, 380, 388, 394, 398, 406, 409, 413, 414, 415, 418, 421, 423, 428, 431, 432, 436, 438, 440, 443, 449, 460, 467, 469, 470, 473, 474, 475, 478, 479, 481, 482, 483, 484, 485, 490, 496, 499, 500, 504, 515, 518, 521, 526, 532, 534, 536, 539, 540, 545, 546, 552, 556, 557, 561, 562, 576, 577, 578, 580, 581, 582, 588, 592, 595, 597, 599, 602, 607, 609, 612, 614, 617, 621, 622, 624, 625, 627, 631, 633, 636, 637, 638, 639, 641, 648, 649, 650, 652, 653, 654, 660, 663, 665, 666, 667, 670, 672, 679, 681, 682, 683, 684, 686, 698, 701, 703, 706, 707, 711, 712, 715, 719, 721, 722, 723, 726, 728, 730, 732, 736, 740, 741, 742, 746, 747, 750, 753, 764, 765, 772, 773, 774, 779, 782, 786, 794, 796, 798, 802, 803, 808, 809, 811, 812, 814, 816, 826, 831, 834, 839, 844, 846, 847, 850, 857, 858, 859, 867, 868, 871, 876, 880, 882, 883, 884, 887, 888, 889, 895, 897, 900, 901, 906, 911, 912, 914, 917, 919, 923, 925, 928, 931, 933, 934, 935, 936, 939, 942, 944, 946, 948, 949, 951, 956, 963, 964, 967, 975, 976, 979, 982, 985, 987, 997" };
        
        string[] FiveNumber;

        //五星的亂數
        private void getFiveNumber()
        {
            string tmpString = "",tmpSortString = "";
            int j = 350;
            Random generator = new Random();
            String r = "";
            var resourceNames = new List<string>();

            //重組排序用
            string[] TmpSortArr;
            var resourceNamesSort = new List<string>();

            for (int i = 0; i < 6; i++) 
            {
                tmpString = "";
                //Random generator = new Random();
                for (int ii = 0; ii < j; ii++)
                {
                    r = generator.Next(0, 99999).ToString("D5");
                    if (!tmpString.Contains(r))
                    {
                        tmpString = tmpString + "," + r;
                    }
                    else
                    {
                        ii--;
                    }
                }

                resourceNames.Add(tmpString.Substring(1));
                TmpSortArr = resourceNames[i].Split(',');
                Array.Sort(TmpSortArr, 0, 349);
                tmpSortString = "";
                //排序重組
                for (int iii = 0; iii < TmpSortArr.Count(); iii++)
                {
                    tmpSortString += ", " + TmpSortArr[iii];
                }
                resourceNamesSort.Add(tmpSortString.Substring(2));
            }

            FiveNumber = resourceNamesSort.ToArray();
        }

        private class ComboboxItem
        {
            public ComboboxItem(string value, string text)
            {
                Value = value;
                Text = text;
            }
            public string Value
            {
                get;
                set;
            }
            public string Text
            {
                get;
                set;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        public frm_PlanCycle()
        {
            InitializeComponent();
            getFiveNumber();
            txtGameNum.ForeColor = Color.LightGray;
            txtGameNum.Text = "请输入奖金号";
            this.txtGameNum.Leave += new System.EventHandler(this.txtGameNum_Leave);
            this.txtGameNum.Enter += new System.EventHandler(this.txtGameNum_Enter);

            txtTimes.ForeColor = Color.LightGray;
            txtTimes.Text = "请输入倍数";
            this.txtTimes.Leave += new System.EventHandler(this.txtTimes_Leave);
            this.txtTimes.Enter += new System.EventHandler(this.txtTimes_Enter);

            cbMoney.SelectedIndex = 0;
            cbGameKind.SelectedIndex = 0;
            cbGameDirect.SelectedIndex = 0;
            cbGamePlus.SelectedIndex = 0;
            //cbGamePlan.SelectedIndex = 0;
            cbGameCycle.SelectedIndex = 0;
        }

        #region TextBox的提示
        private void txtGameNum_Leave(object sender, EventArgs e)
        {
            if (txtGameNum.Text == "")
            {
                txtGameNum.Text = "请输入奖金号";
                txtGameNum.ForeColor = Color.Gray;
            }
        }
        private void txtGameNum_Enter(object sender, EventArgs e)
        {
            if (txtGameNum.Text == "请输入奖金号")
            {
                txtGameNum.Text = "";
                txtGameNum.ForeColor = Color.Black;
            }
        }
        private void txtTimes_Leave(object sender, EventArgs e)
        {
            if (txtTimes.Text == "")
            {
                txtTimes.Text = "请输入倍数";
                txtTimes.ForeColor = Color.Gray;
            }
        }
        private void txtTimes_Enter(object sender, EventArgs e)
        {
            if (txtTimes.Text == "请输入倍数")
            {
                txtTimes.Text = "";
                txtTimes.ForeColor = Color.Black;
            }
        }
        #endregion

        #region ComboBox的切換處理
        private void cbGameKind_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cbGameKind.SelectedItem.ToString())
            {
                case "五星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("五星组合");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("30000+");
                    cbGamePlus.Items.Add("40000+");
                    cbGamePlus.Items.Add("50000+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "四星":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("四星组合");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("3000+");
                    cbGamePlus.Items.Add("4000+");
                    cbGamePlus.Items.Add("5000+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "前三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("前三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "中三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("中三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "后三":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("后三组合");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("300+");
                    cbGamePlus.Items.Add("400+");
                    cbGamePlus.Items.Add("500+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "前二":
                case "后二":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("直选复式");
                    cbGameDirect.Items.Add("直选单式");
                    cbGameDirect.Items.Add("直选和值");
                    cbGameDirect.Items.Add("直选跨度");
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    cbGamePlus.Items.Add("30+");
                    cbGamePlus.Items.Add("40+");
                    cbGamePlus.Items.Add("50+");
                    cbGamePlus.SelectedIndex = 0;
                    break;
                case "定位胆":
                    cbGameDirect.Items.Clear();
                    cbGameDirect.Items.Add("定位胆");
                    //todo: 定位胆的處理
                    cbGameDirect.SelectedIndex = 0;
                    cbGamePlus.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        private void cbGamePlus_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbGamePlus.SelectedItem.ToString().Substring(0, 1))
            {
                case "3":
                    cbGameCycle.Items.Clear();
                    cbGameCycle.Items.Add(new ComboboxItem("3", "三期一周"));
                    cbGameCycle.Items.Add(new ComboboxItem("2", "二期一周"));
                    cbGameCycle.Items.Add(new ComboboxItem("1", "一期一周"));
                    cbGameCycle.SelectedIndex = 0;
                    break;
                case "4":
                    cbGameCycle.Items.Clear();
                    cbGameCycle.Items.Add(new ComboboxItem("2", "二期一周"));
                    cbGameCycle.Items.Add(new ComboboxItem("1", "一期一周"));
                    cbGameCycle.SelectedIndex = 0;
                    break;
                case "5":
                    cbGameCycle.Items.Clear();
                    cbGameCycle.Items.Add(new ComboboxItem("2", "二期一周"));
                    cbGameCycle.Items.Add(new ComboboxItem("1", "一期一周"));
                    cbGameCycle.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void btnViewResult_Click(object sender, EventArgs e)
        {
            pnlShowPlan.Visible = false;
            if (txtGameNum.Text == "" || txtGameNum.Text == "请输入奖金号" ||
                txtTimes.Text == "" || txtTimes.Text == "请输入倍数" ||
                (ckRegularCycle.Checked == false && ckWinToNextCycle.Checked == false) ||
                cbGamePlus.SelectedItem == null ||
                cbGamePlan.SelectedItem == null)
            {
                MessageBox.Show("所有欄位都必須輸入");
                return;
            }

            int cycle_1 = 1; //列出計畫號碼的周期數
            int cycle_2 = 1; //比對開獎的周期數
            int sumBets = 0; //總投注數
            int sumWin = 0; //中獎次數

            if (cbGameKind.Text == "中三" && cbGamePlus.Text == "300+" && cbGamePlan.Text == "玉神计划" && (cbGameCycle.Text == "三期一周" || cbGameCycle.Text == "二期一周"))
            {
                pnlShowPlan.Visible = true;                
                ComboboxItem item = cbGameCycle.Items[cbGameCycle.SelectedIndex] as ComboboxItem;   
                             
                #region 顯示可看的週期
                cbPlanCycleSelect.Items.Clear();
                
                string cycleName = "";
                for (int i = frmGameMain.jArr.Count - 1; i >= 0; i--)
                {
                    cycleName = "第" + cycle_1.ToString("00") + "周期";
                    string cycleDetail = "";
                    for (int j = 0; j < Convert.ToInt16(item.Value); j++)
                    {
                        if (i < 0)
                            break;
                        cycleDetail += "" + frmGameMain.jArr[i]["Issue"].ToString() + "期 ． ";
                        i--;
                    }
                    cbPlanCycleSelect.Items.Add(new ComboboxItem(cycleDetail, cycleName));
                    cycle_1++;
                    i++;
                }
                cbPlanCycleSelect.SelectedIndex = 0;
                #endregion

                #region 驗證是否中獎
                Label lbl_1;
                ComboBox cb_1;
                Label lbl_2;
                Label lbl_3;
                flowLayoutPanel1.Controls.Clear();

                bool isWin = false; //中了沒
                int periodtWin = 0; //第幾期中
                string[] temp = { "", "", "" }; //存放combobox的值

                for (int i = frmGameMain.jArr.Count - 1; i >= 0; i--) //從歷史結果開始比
                {
                    //reset
                    isWin = false;
                    periodtWin = 0;
                    temp[0] = "";
                    temp[1] = "";
                    temp[2] = "";

                    lbl_1 = new Label();
                    lbl_1.Text = "第" + cycle_2.ToString("00") + "周期";
                    lbl_1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    lbl_1.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
                    lbl_1.Size = new System.Drawing.Size(72, 25);

                    for (int j = 0; j < Convert.ToInt16(item.Value); j++)
                    {
                        if (i < 0) break;

                        string strMatch = "";
                        switch (cbGameKind.Text)
                        {
                            case "五星":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "");
                                break;
                            case "四星":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                break;
                            case "前三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                break;
                            case "中三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                break;
                            case "后三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                break;
                            case "前二":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                break;
                            case "后二":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                break;
                        }                        
                        if (isWin == false) //還沒中
                        {
                            if (numHistory[cycle_2 - 1].IndexOf(strMatch) > -1) //中
                            {
                                temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 中";
                                if (periodtWin == 0)
                                    periodtWin = j + 1;
                                isWin = true;

                                if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                {
                                    i--;
                                    sumBets++;
                                    break;
                                }
                            }
                            else //掛
                            {
                                temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 挂";
                            }
                            sumBets++;
                        }
                        else //前面已中獎
                        {
                            temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 停";
                        }
                        i--;

                    }

                    //暫時性這樣做
                    if (cycle_2 != numHistory.Count())
                    { 
                    cycle_2++;
                    i++;

                    cb_1 = new ComboBox();
                    for (int k = 0; k < 3; k++)
                    {
                        if (temp[k] != "")
                            cb_1.Items.Add(temp[k]);
                    }
                    cb_1.Cursor = System.Windows.Forms.Cursors.Hand;
                    cb_1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                    cb_1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    cb_1.ForeColor = System.Drawing.Color.Black;
                    cb_1.FormattingEnabled = true;
                    cb_1.Margin = new System.Windows.Forms.Padding(0);
                    cb_1.Size = new System.Drawing.Size(128, 26);
                    cb_1.SelectedIndex = 0;
                    cb_1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbCycleResult1_DrawItem);

                    lbl_2 = new Label();
                    lbl_2.Text = periodtWin.ToString();
                    lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                    lbl_2.Size = new System.Drawing.Size(53, 25);
                    lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                    lbl_3 = new Label();
                    if (isWin == true)
                    {
                        lbl_3.Text = "中";
                        lbl_3.ForeColor = System.Drawing.Color.Red;
                        sumWin++;
                    }
                    else
                    {
                        lbl_3.Text = "挂";
                        lbl_3.ForeColor = System.Drawing.Color.Black;
                    }
                    lbl_3.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    lbl_3.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                    lbl_3.Size = new System.Drawing.Size(60, 25);
                    lbl_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                    flowLayoutPanel1.Controls.Add(lbl_1);
                    flowLayoutPanel1.Controls.Add(cb_1);
                    flowLayoutPanel1.Controls.Add(lbl_2);
                    flowLayoutPanel1.Controls.Add(lbl_3);
                }                    
                }

                if (ckRegularCycle.Checked == true) //规律周期
                {                  
                    
                    
                }
                else //中奖即进入下一周期
                { 
                
                }
                #endregion

                #region 計算
                //每期注数 (依計畫而定)
                lblBets.Text = "350";
                //每期注數 共?元
                lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                //目前下注?周期
                lblCurrentBetsCycle.Text = (cycle_2 - 1).ToString();
                //共下注?期
                lblSumBetsCycle.Text = sumBets.ToString();
                //總投注額?元
                lblSumBetsMoney.Text = (Convert.ToDecimal(lblBetsMoney.Text) * Convert.ToDecimal(lblSumBetsCycle.Text)).ToString();
                //獎金?元
                lblWinMoney.Text = (Convert.ToDecimal(txtGameNum.Text) * Convert.ToDecimal(txtTimes.Text)).ToString();
                //盈虧?元
                lblProfit.Text = (Convert.ToDecimal(lblSumBetsMoney.Text) - Convert.ToDecimal(lblWinMoney.Text)).ToString();
                //中獎率
                lblPlanWinOpp.Text = (sumWin * 100 / Convert.ToDecimal(lblCurrentBetsCycle.Text)).ToString(".##");
                #endregion

                rtxtPlanCycle.ReadOnly = true;
            }
            else if (cbGameKind.Text == "五星") //五星開獎
            {
                getFiveNumber();

                pnlShowPlan.Visible = true;
                ComboboxItem item = cbGameCycle.Items[cbGameCycle.SelectedIndex] as ComboboxItem;

                #region 顯示可看的週期
                cbPlanCycleSelect.Items.Clear();

                string cycleName = "";
                for (int i = frmGameMain.jArr.Count - 1; i >= 0; i--)
                {
                    cycleName = "第" + cycle_1.ToString("00") + "周期";
                    string cycleDetail = "";
                    for (int j = 0; j < Convert.ToInt16(item.Value); j++)
                    {
                        if (i < 0)
                            break;
                        cycleDetail += "" + frmGameMain.jArr[i]["Issue"].ToString() + "期 ． ";
                        i--;
                    }
                    cbPlanCycleSelect.Items.Add(new ComboboxItem(cycleDetail, cycleName));
                    cycle_1++;
                    i++;
                }
                cbPlanCycleSelect.SelectedIndex = 0;
                #endregion

                #region 驗證是否中獎
                Label lbl_1;
                ComboBox cb_1;
                Label lbl_2;
                Label lbl_3;
                flowLayoutPanel1.Controls.Clear();

                bool isWin = false; //中了沒
                int periodtWin = 0; //第幾期中
                string[] temp = { "", "", "" }; //存放combobox的值

                for (int i = frmGameMain.jArr.Count - 1; i >= 0; i--) //從歷史結果開始比
                {
                    //reset
                    isWin = false;
                    periodtWin = 0;
                    temp[0] = "";
                    temp[1] = "";
                    temp[2] = "";

                    lbl_1 = new Label();
                    lbl_1.Text = "第" + cycle_2.ToString("00") + "周期";
                    lbl_1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    lbl_1.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
                    lbl_1.Size = new System.Drawing.Size(72, 25);

                    for (int j = 0; j < Convert.ToInt16(item.Value); j++)
                    {
                        if (i < 0) break;

                        string strMatch = "";
                        switch (cbGameKind.Text)
                        {
                            case "五星":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "");
                                break;
                            case "四星":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 4);
                                break;
                            case "前三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 3);
                                break;
                            case "中三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(1, 3);
                                break;
                            case "后三":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(2, 3);
                                break;
                            case "前二":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(0, 2);
                                break;
                            case "后二":
                                strMatch = frmGameMain.jArr[i]["Number"].ToString().Replace(",", "").Substring(3, 2);
                                break;
                        }
                        if (isWin == false) //還沒中
                        {
                            if (FiveNumber[cycle_2 - 1].IndexOf(strMatch) > -1) //中
                            {
                                temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 中";
                                if (periodtWin == 0)
                                    periodtWin = j + 1;
                                isWin = true;

                                if (ckWinToNextCycle.Checked == true) //中奖即进入下一周期                                    
                                {
                                    i--;
                                    sumBets++;
                                    break;
                                }
                            }
                            else //掛
                            {
                                temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 挂";
                            }
                            sumBets++;
                        }
                        else //前面已中獎
                        {
                            temp[j] = "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + " 停";
                        }
                        i--;
                    }

                    if (cycle_2 != numHistory.Count())
                    {

                        cycle_2++;
                        i++;

                        cb_1 = new ComboBox();
                        for (int k = 0; k < 3; k++)
                        {
                            if (temp[k] != "")
                                cb_1.Items.Add(temp[k]);
                        }
                        cb_1.Cursor = System.Windows.Forms.Cursors.Hand;
                        cb_1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                        cb_1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                        cb_1.ForeColor = System.Drawing.Color.Black;
                        cb_1.FormattingEnabled = true;
                        cb_1.Margin = new System.Windows.Forms.Padding(0);
                        cb_1.Size = new System.Drawing.Size(128, 26);
                        cb_1.SelectedIndex = 0;
                        cb_1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbCycleResult1_DrawItem);

                        lbl_2 = new Label();
                        lbl_2.Text = periodtWin.ToString();
                        lbl_2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                        lbl_2.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                        lbl_2.Size = new System.Drawing.Size(53, 25);
                        lbl_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                        lbl_3 = new Label();
                        if (isWin == true)
                        {
                            lbl_3.Text = "中";
                            lbl_3.ForeColor = System.Drawing.Color.Red;
                            sumWin++;
                        }
                        else
                        {
                            lbl_3.Text = "挂";
                            lbl_3.ForeColor = System.Drawing.Color.Black;
                        }
                        lbl_3.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                        lbl_3.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
                        lbl_3.Size = new System.Drawing.Size(60, 25);
                        lbl_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                        flowLayoutPanel1.Controls.Add(lbl_1);
                        flowLayoutPanel1.Controls.Add(cb_1);
                        flowLayoutPanel1.Controls.Add(lbl_2);
                        flowLayoutPanel1.Controls.Add(lbl_3);
                    }
                }


                if (ckRegularCycle.Checked == true) //规律周期
                {


                }
                else //中奖即进入下一周期
                {

                }
                #endregion

                #region 計算
                //每期注数 (依計畫而定)
                lblBets.Text = "350";
                //每期注數 共?元
                lblBetsMoney.Text = (Convert.ToDecimal(lblBets.Text) * Convert.ToDecimal(cbMoney.SelectedItem.ToString().Replace("2元", "2").Replace("2角", "0.2").Replace("2分", "0.02").Replace("2厘", "0.002")) * Convert.ToDecimal(txtTimes.Text)).ToString(".###");
                //目前下注?周期
                lblCurrentBetsCycle.Text = (cycle_2 - 1).ToString();
                //共下注?期
                lblSumBetsCycle.Text = sumBets.ToString();
                //總投注額?元
                lblSumBetsMoney.Text = (Convert.ToDecimal(lblBetsMoney.Text) * Convert.ToDecimal(lblSumBetsCycle.Text)).ToString();
                //獎金?元
                lblWinMoney.Text = (Convert.ToDecimal(txtGameNum.Text) * Convert.ToDecimal(txtTimes.Text)).ToString();
                //盈虧?元
                lblProfit.Text = (Convert.ToDecimal(lblSumBetsMoney.Text) - Convert.ToDecimal(lblWinMoney.Text)).ToString();
                //中獎率
                lblPlanWinOpp.Text = (sumWin * 100 / Convert.ToDecimal(lblCurrentBetsCycle.Text)).ToString(".##");
                #endregion
            }
            else
            {
                MessageBox.Show("測試請先選擇:中三、300+、玉神计划、三期一周或二期一周");
                return;
            }   
        }

        private void cbPlanCycleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem item = cbPlanCycleSelect.Items[cbPlanCycleSelect.SelectedIndex] as ComboboxItem;
            lblPlanCycleSelected.Text = item.Text;
            lblPlanCycleDetail.Text = item.Value;
            //先固定350組
            //這邊是用死的寫法需修正 TODO
            if (cbGameKind.Text == "五星")
            {
                if (cbPlanCycleSelect.SelectedIndex == 0)
                    rtxtPlanCycle.Text = FiveNumber[0];
                else if (cbPlanCycleSelect.SelectedIndex == 1)
                    rtxtPlanCycle.Text = FiveNumber[1];
                else if (cbPlanCycleSelect.SelectedIndex == 2)
                    rtxtPlanCycle.Text = FiveNumber[2];
                else
                    rtxtPlanCycle.Text = FiveNumber[3];
            }
            else
            {
                if (cbPlanCycleSelect.SelectedIndex == 0)
                    rtxtPlanCycle.Text = numHistory[0];
                else if (cbPlanCycleSelect.SelectedIndex == 1)
                    rtxtPlanCycle.Text = numHistory[1];
                else if (cbPlanCycleSelect.SelectedIndex == 2)
                    rtxtPlanCycle.Text = numHistory[2];
                else
                    rtxtPlanCycle.Text = numHistory[3];
            }
        }

        private void cbCycleResult1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            Pen objPen;
            Font objFont;
            float size = 11;
            FontFamily family = new FontFamily(lblBets.Font.Name);

            if (cb.Items[e.Index].ToString().IndexOf("挂") > -1)
                objPen = new Pen(Color.Black);
            else
                objPen = new Pen(Color.Red); ;

            if (e.Index != -1)
            {
                objFont = new Font(family, size);
                e.Graphics.DrawString((string)cb.Items[e.Index], objFont, objPen.Brush, e.Bounds);
            }
        }
        //取得歷史開獎
        private void UpdateHistory()
        {
            if (rtxtHistory.Text == "") //無資料就全寫入
            {
                for (int i = 0; i < frmGameMain.jArr.Count; i++)
                {
                    if (i == 120) break; //寫120筆就好
                    rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                }
            }
            else //有資料先判斷
            {
                if ((rtxtHistory.Text.Substring(0, 11) != frmGameMain.jArr[0]["Issue"].ToString()) && (frmGameMain.strHistoryNumberOpen != "?")) //有新資料了
                {
                    rtxtHistory.Text = "";
                    for (int i = 0; i < frmGameMain.jArr.Count; i++)
                    {
                        if (i == 120) break; //寫120筆就好
                        rtxtHistory.Text += frmGameMain.jArr[i]["Issue"].ToString() + "  " + frmGameMain.jArr[i]["Number"].ToString().Replace(",", " ") + "\r\n";
                    }
                }
            }
        }

        private void picAD1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.zhcw.com/");
        }

        private void picAD2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cqcp.net/");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateHistory();
        }

        private void txtGameNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能輸入數字
            if (e.KeyChar != '\b') //後退鍵以外的字元
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) //0~9
                {
                    e.Handled = true;
                }
            }
        }

        private void txtTimes_KeyPress(object sender, KeyPressEventArgs e)
        {
            //只能輸入數字
            if (e.KeyChar != '\b') //後退鍵以外的字元
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9')) //0~9
                {
                    e.Handled = true;
                }
            }
        }

        private void ckRegularCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckRegularCycle.Checked == true)
                ckWinToNextCycle.Checked = false;
        }

        private void ckWinToNextCycle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckWinToNextCycle.Checked == true)
                ckRegularCycle.Checked = false;
        }

        private void btnEditPlanNumber_Click(object sender, EventArgs e)
        {
            rtxtPlanCycle.ReadOnly = false;
        }

        private void btnCopyPlanNumber_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(rtxtPlanCycle.Text);
            MessageBox.Show("已复制到剪贴簿");
            //Random R = new Random();

            //string[] temp = new string[350];

            //Random rand = new Random(Guid.NewGuid().GetHashCode());

            //List<int> listLinq = new List<int>(Enumerable.Range(0, 999));
            //listLinq = listLinq.OrderBy(num => rand.Next()).ToList<int>();

            //for (int i = 0; i < 350; i++)
            //{
            //    temp[i] = listLinq[i].ToString("000");
            //}
            //Array.Sort(temp);

            //rtxtPlanCycle.Text = "";
            //for (int i = 0; i < 350; i++)
            //{
            //    rtxtPlanCycle.Text += temp[i] + ", ";
            //} 
        }


    }
}
