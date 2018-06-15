using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1.Model
{
    public class CheckedListBoxEx : CheckedListBox
    {
        public override int ItemHeight
        {
            get
            {
                return (this.Font.Height + 23);//在这改变Item高度，（在字体高度上加上一个数值可能表示这个Item的高度高于其中的字体高度吧）
            }
            set
            {
                //base.ItemHeight=value;//在msdn中说CheckedListBox可以设，但不知道在这winform中为什么base里没有这个属性，可能在webform中会有吧
            }
        }

        //internal override bool SupportsUseCompatibleTextRendering
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}
        public string ValueMember
        {
            get
            {
                return base.ValueMember;
            }
            set
            {
                base.ValueMember = value;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            if (base.IsHandleCreated)
            {
                //base.SendMessage(0x1a0, 0, this.ItemHeight);
            }
            base.OnFontChanged(e);
        }
    }
}
