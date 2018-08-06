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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Change : Page
    {
        List<TextBlock> Lines = new List<TextBlock>(); List<TextBlock> Olds = new List<TextBlock>(); List<TextBlock> News = new List<TextBlock>();
        List<TextBlock> Holds = new List<TextBlock>(); List<TextBlock> Times = new List<TextBlock>(); List<TextBlock> Dies = new List<TextBlock>();
        List<Button> Butts = new List<Button>();
        TextBox LineEntry = new TextBox(); TextBox OldEntry = new TextBox(); TextBox NewEntry = new TextBox(); TextBox DieEntry = new TextBox(); TextBox TimeEntry = new TextBox(); TextBox HoldEntry = new TextBox();
        int i = 1;
        Thickness Marge = new Thickness { Top = 10, Bottom = 10 };
        Thickness Marge2 = new Thickness { Top = 4.75, Bottom = 4.75 };
        public static String[] Getters;
        String LineName;
        SolidColorBrush PB = new SolidColorBrush(Windows.UI.Colors.PowderBlue);
        SolidColorBrush DG = new SolidColorBrush(Windows.UI.Colors.DarkGreen);
        SolidColorBrush FG = new SolidColorBrush(Windows.UI.Colors.ForestGreen);
        Button AddButton = new Button { Content = "Add Row" };
        public Change()
        {
            
            
            AddButton.Foreground = PB; AddButton.Background = DG;
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            LineEntry.FontSize = 12; OldEntry.FontSize = 12; HoldEntry.FontSize = 12; DieEntry.FontSize = 12; TimeEntry.FontSize = 12; NewEntry.FontSize = 12;
            LineEntry.Opacity = 90;
            LinePanel.Children.Add(LineEntry); OldPanel.Children.Add(OldEntry); NewPanel.Children.Add(NewEntry); DiePanel.Children.Add(DieEntry); TimePanel.Children.Add(TimeEntry); HoldPanel.Children.Add(HoldEntry);
            ButtonPanel.Children.Add(AddButton);
            LinePanel.Width = OldPanel.Width = NewPanel.Width = HoldPanel.Width = DiePanel.Width = ButtonPanel.Width = TimePanel.Width = Double.NaN;
            LinePanel.Height = OldPanel.Height = NewPanel.Height = HoldPanel.Height = DiePanel.Height = ButtonPanel.Height = TimePanel.Height = Double.NaN;
            UpdateLayout();
            AddButton.Click += Tits;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                LineName = e.Parameter as String;
                String[] anass = LineName.Split(' ');
                LineEntry.Text = anass[1];

            }
            else
            {
                

                // tit.ToString();
                //fun.Text = tit.startTime.ToString();
                //fun.Text = "Hi!";
            }
            base.OnNavigatedTo(e);
        }
        private void ClickMain(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Tits(object sender, RoutedEventArgs e)
        {
            if (i < 19)
            {
                TextBlock LineText = new TextBlock { Text = LineEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                TextBlock OldText = new TextBlock { Text = OldEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                TextBlock NewText = new TextBlock { Text = NewEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                TextBlock TimeText = new TextBlock { Text = TimeEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                TextBlock DieText = new TextBlock { Text = DieEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                TextBlock HoldText = new TextBlock { Text = HoldEntry.Text, HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge, Foreground = PB, FontWeight = Windows.UI.Text.FontWeights.SemiBold };
                Button Edits = new Button { Content = "Edit", HorizontalAlignment = HorizontalAlignment.Center, Margin = Marge2, Height = 30, Name = (i - 1).ToString() };
                Edits.Foreground = PB; Edits.Background = FG;
                Edits.Click += Tits2;
                LinePanel.Children.Insert(i, LineText); NewPanel.Children.Insert(i, NewText); HoldPanel.Children.Insert(i, HoldText);
                OldPanel.Children.Insert(i, OldText); TimePanel.Children.Insert(i, TimeText); DiePanel.Children.Insert(i, DieText);
                ButtonPanel.Children.Insert(i, Edits);
                Lines.Add(LineText); Olds.Add(OldText); News.Add(NewText); Times.Add(TimeText); Dies.Add(DieText); Holds.Add(HoldText);
                LinePanel.Width = OldPanel.Width = NewPanel.Width = HoldPanel.Width = DiePanel.Width = ButtonPanel.Width = TimePanel.Width = Double.NaN;
                LinePanel.Height = OldPanel.Height = NewPanel.Height = HoldPanel.Height = DiePanel.Height = ButtonPanel.Height = TimePanel.Height = mama.Height = dada.Height = Double.NaN;
                UpdateLayout();
                i++;
            }
            if(i==19)
            {
                AddButton.Visibility = Visibility.Collapsed;
                LineEntry.Visibility = OldEntry.Visibility = NewEntry.Visibility = HoldEntry.Visibility = TimeEntry.Visibility = DieEntry.Visibility = Visibility.Collapsed;
            }
        }
        private async void Tits2(object sender, RoutedEventArgs e)
        {
            Button thisbutt = sender as Button;
            int thisIndex = Convert.ToInt16(thisbutt.Name);
            String[] Datas = new String[6];
            Datas[0] = Lines[thisIndex].Text; Datas[1] = Olds[thisIndex].Text;
            Datas[2] = News[thisIndex].Text; Datas[3] = Dies[thisIndex].Text;
            Datas[4] = Times[thisIndex].Text; Datas[5] = Holds[thisIndex].Text;
            ChangeDialog ChickDialog = new ChangeDialog
            {
                Title = "Why is " + " down?"
            };
            ChickDialog.Pistt(Datas);
            ContentDialogResult result = await ChickDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Lines[thisIndex].Text = Getters[0];
                Olds[thisIndex].Text = Getters[1];
                News[thisIndex].Text = Getters[2];
                Dies[thisIndex].Text = Getters[3];
                Times[thisIndex].Text = Getters[4];
                Holds[thisIndex].Text = Getters[5];
            }
           
        }
        public static void setGet(String[] gott)
        {
            Change.Getters = gott;
        }
    }
}
