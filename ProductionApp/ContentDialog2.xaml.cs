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
    public sealed partial class ContentDialog2 : ContentDialog
    {
        DateTime newStart;
        DateTime newStop;
        int newReason;
        public class RunSpan { public DateTime startTime; public DateTime stopTime; public int whyDown; }
        RunSpan changed = new RunSpan();
        public ContentDialog2()
        {

            this.InitializeComponent();
        }
        public void getTimes(TimeSpan start, TimeSpan stop, int reasoner, String[] whys)
        {
            StartMee.Time = start;
            StopMee.Time = stop;
            foreach (String excuse in whys)
            {
                Reasons.Items.Add(excuse);
            }
            Reasons.SelectedIndex = reasoner;
        }
        public RunSpan Changer()
        {
            return changed;
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            newStart = Convert.ToDateTime(StartMee.Time.ToString());
            newStop = Convert.ToDateTime(StopMee.Time.ToString());
            newReason = Reasons.SelectedIndex;

            changed.startTime = newStart;
            changed.stopTime = newStop;
            changed.whyDown = newReason;
        }
    }
}
