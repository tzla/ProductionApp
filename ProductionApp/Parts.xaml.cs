
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
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Newtonsoft.Json;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Parts : Page
    {
        FileOpenPicker openPickers = new FileOpenPicker();
        List<String> Material, Gauge, Part;
        class Pan { public String name; public double length; public double width; public double depth; public int expected; public int packCount;};
        List<Pan> Pans = new List<Pan>();
        List <String> AllPart = new List<String>();
        SwapChainPanel pooter = new SwapChainPanel();
       

        private async void BigBoy(object sender, TappedRoutedEventArgs e)
        {
            Image poo = sender as Image;
            BitmapImage bigImage = new BitmapImage();
            bigImage.UriSource = new Uri(poo.Tag as String);
            ImagePopper Popps = new ImagePopper();
            Popps.bigPic(bigImage);
            ContentDialogResult result = await Popps.ShowAsync();

        }
        

        private void Clack(object sender, SelectionChangedEventArgs e)
        {
            WW.Text = " peenis";
            ListView mylist = sender as ListView;
            int choice = mylist.SelectedIndex;
            hh.Text = AllPart[choice];
            Pan thisPan = Pans[choice];
            LL.Text = thisPan.length.ToString() + " in."; WW.Text = thisPan.width.ToString() + " in."; DD.Text = thisPan.depth.ToString() + " in.";
            dailyCount.Text = thisPan.expected.ToString(); packCount.Text = thisPan.packCount.ToString();
            int bals = Pans.Count;
            BitmapImage bitmapImage = new BitmapImage(); BitmapImage bitmapImagea = new BitmapImage(); BitmapImage bitmapImageb = new BitmapImage();
            String bits = AllPart[choice];
            String addresser = "ms-appx:///Assets/" + bits + ".png"; String addresserA = "ms-appx:///Assets/" + bits + "a.png"; String addresserB = "ms-appx:///Assets/" + bits + "b.png";
            try
            {
                bitmapImage.UriSource = new Uri(addresser); bitmapImagea.UriSource = new Uri(addresserA); bitmapImageb.UriSource = new Uri(addresserB);
                img.Source = bitmapImage; imgA.Source = bitmapImagea; imgB.Source = bitmapImageb;
                img.Tag = addresser; imgA.Tag = addresserA; imgB.Tag = addresserB;

            }
            catch { }

        }

        public Parts()
        {
            
            Material = new List<String>{"WM", "BE", "HG", "CP", "T", "CCAL", "ALMZ", "PF", "CTC", "HV", "DP", "TBT" };
            Gauge = new List<String> { "090", "107", "110", "112", "143", "173" };
            Part = new List<String> { "16", "18", "30", "36", "37", "40", "41", "44", "45", "46", "47", "50", "51", "52", "55", "60", "62",
                "64", "65", "66", "68", "69", "75", "77", "79", "80", "81", "85", "86", "88", "89", "118", "160", "164", "166", "168", "180", "181" };
            //AllPart = new List<String>();
            load();
            

            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            BoxD.ItemsSource = Material;
            BoxJ.ItemsSource = Part;
            BoxT.ItemsSource = Gauge;
            BoxD.SelectedIndex = BoxJ.SelectedIndex= BoxT.SelectedIndex = 0;
            ggg.Children.Add(pooter);
            

        }
        private async void load()
        {
            try
            {
                String JsonFile = "Pans.json";
                StorageFolder localFolder = KnownFolders.MusicLibrary;
                StorageFile localFile = await localFolder.GetFileAsync(JsonFile);
                String JsonString = await FileIO.ReadTextAsync(localFile);
                Pans = JsonConvert.DeserializeObject(JsonString, typeof(List<Pan>)) as List<Pan>;
                foreach (var j in Pans)
                {
                    if (j.name == null)
                    {
                        Pans.Remove(j);
                    }
                    else
                    {
                        AllPart.Add(j.name);
                        Voo.Items.Add(j.name);
                    }
                }
            }
            catch { }
        }
        private async void Fuck(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Pans);
            hh.Text = json;
            StorageFolder storageFolder = KnownFolders.MusicLibrary;
            StorageFile newFile = await storageFolder.CreateFileAsync("Pans.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(newFile, json);
        }

        private void Bak(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));

        }

        private async void Butter(object sender, RoutedEventArgs e)
        {
            
            String bits = "";
            if (BoxD.SelectedIndex != 4)
            {
                bits = BoxD.SelectedItem.ToString() + BoxJ.SelectedItem.ToString() + "-" + BoxT.SelectedItem.ToString();
                
            }
            else
            {
                bits = BoxJ.SelectedItem.ToString() + BoxD.SelectedItem.ToString() + "-" + BoxT.SelectedItem.ToString();
            }
            if(!AllPart.Contains(bits))
            {
                AllPart.Add(bits);
                Buts.Content = bits;
                Voo.Items.Add(bits);
                try
                {
                    Pans.Add(new Pan { length = Convert.ToDouble(Lengther.Text), width = Convert.ToDouble(Widther.Text), depth = Convert.ToDouble(Depther.Text), expected = Convert.ToInt32(Expecteder.Text), packCount = Convert.ToInt32(Packer.Text)});
                    Pan thisPan = Pans[(AllPart.Count - 1)];
                    LL.Text = thisPan.length + " in."; WW.Text = thisPan.width + " in."; DD.Text = thisPan.depth + " in.";
                }
                catch { }
                BitmapImage bitmapImage = new BitmapImage(); BitmapImage bitmapImagea = new BitmapImage(); BitmapImage bitmapImageb = new BitmapImage();
                //img.Width = bitmapImage.DecodePixelWidth = 280; imgA.Width = bitmapImagea.DecodePixelWidth = 280; imgB.Width = bitmapImageb.DecodePixelWidth = 280;
                String addresser = "ms-appx:///Assets/" + bits + ".png"; String addresserA = "ms-appx:///Assets/" + bits + "a.png"; String addresserB = "ms-appx:///Assets/" + bits + "b.png";
                try
                {
                    bitmapImage.UriSource = new Uri(addresser); bitmapImagea.UriSource = new Uri(addresserA); bitmapImageb.UriSource = new Uri(addresserB);
                    img.Source = bitmapImage; imgA.Source = bitmapImagea; imgB.Source = bitmapImageb;
                    img.Tag = addresser; imgA.Tag = addresserA; imgB.Tag = addresserB;

                }
                catch { }
                if (bits != null)
                {
                    Pan newPan = new Pan();
                    try { newPan.length = Convert.ToDouble(Lengther.Text); } catch { }
                    try { newPan.width = Convert.ToDouble(Widther.Text); } catch { }
                    try { newPan.depth = Convert.ToDouble(Depther.Text); } catch { }
                    try { newPan.expected = Convert.ToInt32(Expecteder.Text); } catch { }
                    try { newPan.packCount = Convert.ToInt32(Packer.Text); } catch { }
                    try { newPan.name = bits; } catch { }


                    LL.Text = newPan.length.ToString() + " in."; WW.Text = newPan.width.ToString() + " in."; DD.Text = newPan.depth.ToString() + " in.";
                    dailyCount.Text = newPan.expected.ToString(); packCount.Text = newPan.packCount.ToString();
                    //int malls = Pans.Count;
                    if (newPan.name != null) { Pans.Add(newPan); }
                }



                //openPickers.ViewMode = PickerViewMode.Thumbnail;
                // openPickers.FileTypeFilter.Add(".jpg"); openPickers.FileTypeFilter.Add(".jpeg"); openPickers.FileTypeFilter.Add(".png");

                //var file = await openPickers.PickSingleFileAsync();
                //BitmapImage imgs = new BitmapImage();
                //FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
                //imgs.SetSource(stream);
                //img.Source = imgs;
            }
            
            
            
        }

        


        //  Weight
        //      090,107,110,112,143,173

        //  Number

        //     
    }
}
