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
    public sealed partial class ChangeDialog : ContentDialog
    {
        String[] gash = new String[6]; 
        public ChangeDialog()
        {
            this.InitializeComponent();
        }
        public void Pistt(String[] kunt)
        {
            Liners.Text = LineIn.Text = kunt[0];
            OldIn.Text = Olders.Text = kunt[1];
            NewIn.Text = Newers.Text = kunt[2];
            DieIn.Text = Diers.Text = kunt[3];
            TimeIn.Text = Timers.Text = kunt[4];
            HoldIn.Text = Holders.Text = kunt[5];
            //C3. = gash;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            gash[0] = LineIn.Text; gash[1] = OldIn.Text; gash[2] = NewIn.Text;
            gash[3] = DieIn.Text; gash[4] = TimeIn.Text; gash[5] = HoldIn.Text;
            Change.setGet(gash);

            
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        
    }
}
