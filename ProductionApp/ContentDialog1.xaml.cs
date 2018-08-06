using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductionApp
{
    public sealed partial class ContentDialog1 : ContentDialog
    {
        public ContentDialog1()
        {

            this.InitializeComponent();
        }
        public void getInfo(TextBlock[] StringBlocks, Button[] EditList, Button[] DeleteList)
        {

            foreach (var q in StringBlocks)
            {
                q.Height = 32;
                q.FontSize = 13;
                Thickness joo = new Thickness();
                joo.Top = 8;
                q.Margin = joo;


                StringPanel.Children.Add(q);

            }
            foreach (var qq in EditList)
            {

                qq.Height = 26;
                qq.FontSize = 10;
                Thickness joo = new Thickness();
                joo.Top = 7;
                joo.Bottom = 7;
                qq.Margin = joo;

                EditPan.Children.Add(qq);

            }
            foreach (var qqq in DeleteList)
            {

                qqq.Height = 26;
                qqq.FontSize = 10;
                Thickness joo = new Thickness();
                joo.Top = 7;
                joo.Bottom = 7;
                qqq.Margin = joo;

                DelPan.Children.Add(qqq);

            }
            StringPanel.Height = ButtonPanel.Height = Double.NaN;
            this.Height = Double.NaN;
            this.UpdateLayout();
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
