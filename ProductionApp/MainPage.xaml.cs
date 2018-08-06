using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProductionApp
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int y = 0;
        RunSpan hi = new RunSpan();
        List<RunSpan>[] TimeTracker = new List<RunSpan>[15];
        public class RunSpan { public DateTime startTime; public DateTime stopTime; public int whyDown; };
        bool firstTime = true;
    public MainPage()
        {
            if (firstTime)
            {
                for (int i = 0; i < 15; i++)
                {
                    TimeTracker[i] = new List<MainPage.RunSpan>();
                }
                hi.startTime = System.DateTime.Now;
                hi.stopTime = System.DateTime.Now;
                hi.whyDown = 2;
                TimeTracker[0].Add(hi);
                this.InitializeComponent();
                this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //fun.Text = $"Hi";


            }
            else if (!firstTime)
            {
                poo.Text = e.ToString();
                try { TimeTracker = e.Parameter as List<MainPage.RunSpan>[]; }
                catch { }
                
                // tit.ToString();
                //fun.Text = tit.startTime.ToString();
                //fun.Text = "Hi!";
            }
            else
            {
                firstTime = false;
            }
            base.OnNavigatedTo(e);
        }
        private void ClickPal(object sender, RoutedEventArgs e)
        {
            
            this.Frame.Navigate(typeof(Pal),TimeTracker);
        }
        private void ClickChange(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Change));
        }

        private void ClickSheet(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Sheet));
        }

        private void ClickPress(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Press));
        }

        private void ClickPart(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Parts));
        }
    }
}
