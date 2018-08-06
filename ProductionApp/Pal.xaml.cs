using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
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
    public sealed partial class Pal : Page
    {
        bool jjjj = false;
        bool MassSaver = false;
        bool OffSaver = false;
        bool FirstRun = true;
        bool DialogOpen = false;
        bool timerStart = false;
        int sz = 15;
        bool[] breakTime = new bool[15];
        ThreadPoolTimer PeriodicTimer;
        bool[] LineStatus = new bool[15];
        bool[] firstCheck = new bool[15];
        DateTime[] Starter = new DateTime[15];
        TextBlock[] StartBoxes = new TextBlock[15];
        TextBlock[] TotalBoxes = new TextBlock[15];
        TextBlock[] DownBoxes = new TextBlock[15];
        TextBlock[] StopBoxes = new TextBlock[15];
        //class RunSpan { public DateTime startTime; public DateTime stopTime; public int whyDown; };
        Button[] EditButtons = new Button[15];
        Button[] CheckButtons = new Button[15];
        List<MainPage.RunSpan>[] TimeTracker = new List<MainPage.RunSpan>[15];
        ContentDialog1 Activer;
        DateTime[] Stopper = new DateTime[15];
        TimeSpan[] RunTime = new TimeSpan[15];
        ToggleSwitch[] Runner = new ToggleSwitch[15];
        CheckBox[] MassRunner = new CheckBox[15];
        String[] downtimeWhy =
        {
            ("D01 - Bad Material"),
            ("D02 - White Tag"),
            ("D03 - Changeover"),
            ("D04 - Setup"),
            ("D05 - Adjust"),
            ("D06 - Machine Failure"),
            ("D07 - Material Shortage"),
            ("D08 - Label Machine"),
            ("D09 - Tool Room"),
            ("D10 - No Operator"),
            ("D11 - Operator Moved"),
            ("D99 - Break")
        };
        string[] MaterialList = new string[] { "WM", "T", "HG", "BE", "HV", "CP", "CTC", "PF", "ALMZ" };
        string[] GaugeList = new string[] { "090", "107", "112", "173" };
        string[] OptionList = new string[] { "None", "Filled", "Frame", "Cup", "Bottom" };
        public Pal()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            for (int i = 0; i < 15; i++)
            {
                TimeTracker[i] = new List<MainPage.RunSpan>();
                StartBoxes[i] = new TextBlock();
                TotalBoxes[i] = new TextBlock();
                DownBoxes[i] = new TextBlock();
                StopBoxes[i] = new TextBlock();
                EditButtons[i] = new Button();
                CheckButtons[i] = new Button();

                EditButtons[i].Name = i.ToString() + ",Butt";
                EditButtons[i].Content = "Edit";
                EditButtons[i].Click += EditClick;
                EditButtons[i].Height = 28;
                EditButtons[i].FontSize = CheckButtons[i].FontSize = 10;

                CheckButtons[i].Name = i.ToString() + ",Check";
                CheckButtons[i].Content = "Check";

                CheckButtons[i].Click += CheckClick;
                CheckButtons[i].Height = 28;

                StartBoxes[i].Text = "-";
                TotalBoxes[i].Text = "--";
                DownBoxes[i].Text = "---";
                StopBoxes[i].Text = "~~~";

                breakTime[i] = false;
                LineStatus[i] = false;
                firstCheck[i] = true;
                MassRunner[i] = new CheckBox();
                Runner[i] = new ToggleSwitch();
                Runner[i].Name = i.ToString() + ",";
                Runner[i].Toggled += LineTT;
                Runner[i].PointerPressed += Offsaves;
                Runner[i].Height = 30;
                MassRunner[i].Height = 20;
                Thickness thiccc = new Thickness
                {
                    Top = -5,
                    Bottom = -5
                };
                MassRunner[i].BorderThickness = thiccc;
                TogglePanel.Children.Add(Runner[i]);
                StartPanel.Children.Add(StartBoxes[i]);
                MassPanel.Children.Add(MassRunner[i]);
                TotalPanel.Children.Add(TotalBoxes[i]);
                DownPanel.Children.Add(DownBoxes[i]);
                StopPanel.Children.Add(StopBoxes[i]);
                EditPanel.Children.Add(EditButtons[i]);
                CheckPanel.Children.Add(CheckButtons[i]);

                //StartBoxes[i].Width = Double.NaN;
                StartBoxes[i].Height = 28;

                //TotalBoxes[i].Width = Double.NaN;
                TotalBoxes[i].Height = 28;

                //DownBoxes[i].Width = Double.NaN;
                DownBoxes[i].Height = 28;

                //StopBoxes[i].Width = Double.NaN;
                StopBoxes[i].Height = 28;

                Thickness thicc = StartBoxes[i].Padding;
                thicc.Bottom = 2;
                StartBoxes[i].Margin = StopBoxes[i].Margin = TotalBoxes[i].Margin = DownBoxes[i].Margin = CheckButtons[i].Margin = EditButtons[i].Margin = thicc;
                //thicc.Top = 8;

                thicc.Top = 1;



                thicc.Top = 25;
                thicc.Bottom = 2;
                //MassRunner[i].Padding = thicc;
                thicc.Top = 0;
                thicc.Bottom = -2;
                MassRunner[i].Margin = thicc;

                //thicc.Top = -.2; thicc.Bottom = 1;
                // EditButtons[i].Margin = CheckButtons[i].Margin = thicc;
                EditButtons[i].IsEnabled = CheckButtons[i].IsEnabled = false;




            }
            this.UpdateLayout();
            Ticker();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                //fun.Text = $"Hi";
                

            }
            else
            {
                List<MainPage.RunSpan>[] fun = (List<MainPage.RunSpan>[])e.Parameter;
               
                // tit.ToString();
                //fun.Text = tit.startTime.ToString();
                //fun.Text = "Hi!";
            }
            base.OnNavigatedTo(e);
        }
        
        public void Ticker()
        {
            TimeSpan period = TimeSpan.FromSeconds(1);
            PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //DBug.Text = Pickers.Time.ToString();
                        for (int i = 0; i < 15; i++)
                        {
                            if (LineStatus[i])
                            {
                                TimeSpan updater = DateTime.Now - Starter[i];
                                if (firstCheck[i])
                                {

                                    //updater = updater + RunTime[i];
                                    TotalBoxes[i].Text = updater.ToString(@"hh\:mm");
                                }
                                else
                                {
                                    TotalBoxes[i].Text = (SumTime(i) + updater).ToString(@"hh\:mm");
                                }
                            }
                            else
                            {
                                TotalBoxes[i].Text = SumTime(i).ToString(@"hh\:mm");
                            }

                        }
                        if (DateTime.Now.Minute == 00 && DateTime.Now.Hour == 9)
                        {
                            if (jjjj == false && !timerStart) { breaker(); autoDown(10); }
                        }
                        if (DateTime.Now.Minute == 30 && DateTime.Now.Hour == 11)
                        {
                            if (jjjj == false && !timerStart) { breaker(); autoDown(30); }

                        }
                        if (DateTime.Now.Minute == 50 && DateTime.Now.Hour == 1)
                        {
                            if (jjjj == false && !timerStart) { breaker(); autoDown(10); }
                        }
                        this.UpdateLayout();
                    });

            }, period);
        }
        public void Offsaves(object sender, RoutedEventArgs e)
        {
            OffSaver = false;
        }

        private async void CheckClick(object sender, RoutedEventArgs e)
        {
            if (!DialogOpen)
            {
                DialogOpen = true;
                Button obj = sender as Button;
                String objname = obj.Name;
                String[] objnamer = objname.Split(',');
                objname = objnamer[0];
                //DBug.Text = objname.ToString() + " Check";
                int s = Convert.ToInt16(objname);
                TextBlock Namer = LinePanel.Children[s] as TextBlock;
                var Namer2 = Namer.Text;
                ContentDialog1 CheckDialog = new ContentDialog1
                {
                    Title = "Checking " + Namer2,
                    PrimaryButtonText = "Back"
                };

                TextBlock[] CheckTexts = new TextBlock[TimeTracker[s].Count];
                Button[] EditCheckButtons = new Button[TimeTracker[s].Count];
                Button[] DelCheckButtons = new Button[TimeTracker[s].Count];
                //CheckTexts[0].Height = 500;
                //CheckDialog.Content = CheckTexts[0];

                int u = 0;
                foreach (MainPage.RunSpan y in TimeTracker[s])
                {
                    CheckTexts[u] = new TextBlock();

                    EditCheckButtons[u] = new Button();
                    EditCheckButtons[u].Content = "Edit";
                    EditCheckButtons[u].Name = s.ToString() + "," + u.ToString();

                    DelCheckButtons[u] = new Button();
                    DelCheckButtons[u].Content = "Delete";
                    DelCheckButtons[u].Name = s.ToString() + "," + u.ToString() + ",!";

                    EditCheckButtons[u].Click += ListEdit;
                    DelCheckButtons[u].Click += ListDel;
                    String checkString = y.startTime.ToString("t") + " -> " + y.stopTime.ToString("t") + " : " + downtimeWhy[y.whyDown];
                    CheckTexts[u].Text = checkString;
                    u++;



                }
                CheckDialog.getInfo(CheckTexts, EditCheckButtons, DelCheckButtons);
                //CheckTexts[0].Text = checkString;d
                this.UpdateLayout();
                Activer = CheckDialog;
                ContentDialogResult result = await CheckDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {

                }
                else
                {
                    CheckDialog.Hide();
                }
                DialogOpen = false;
            }

        }
        private async void ListDel(object sender2, RoutedEventArgs ee)
        {
            Activer.Hide();
            Button obj3 = sender2 as Button;
            String objname2 = obj3.Name;
            String[] objnamer2 = objname2.Split(',');
            objname2 = objnamer2[0];
            String objtime = objnamer2[1];
            int s = Convert.ToInt16(objname2);
            int u = Convert.ToInt16(objtime);
            TextBlock Namerr = LinePanel.Children[s] as TextBlock;
            var Namerr2 = Namerr.Text;
            ContentDialog DeleteDialog = new ContentDialog
            {
                Title = "Delete " + Namerr2 + " Run Block?",
                PrimaryButtonText = "Cancel",
                SecondaryButtonText = "OK"
            };
            List<MainPage.RunSpan> theseSpan = TimeTracker[s];
            MainPage.RunSpan thisSpan = theseSpan[u];
            TextBlock deleteInfo = new TextBlock();
            String checkString = thisSpan.startTime.ToString() + " , " + thisSpan.stopTime.ToString() + " : " + downtimeWhy[thisSpan.whyDown];
            deleteInfo.Text = checkString;
            DeleteDialog.Content = deleteInfo;
            ContentDialogResult result = await DeleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DeleteDialog.Hide();
                Activer.ShowAsync();

            }
            else
            {
                theseSpan.RemoveAt(u);
                if (theseSpan.Count == 0)
                {
                    firstCheck[s] = true;
                }
                else
                {

                }

                DownBoxes[s].Text = "---";
                StopBoxes[s].Text = "~~~";
                if (!LineStatus[s])
                {
                    StartBoxes[s].Text = "-";
                }
                if (TimeTracker[s].Count==0)
                {
                    CheckButtons[s].IsEnabled = false;
                }
                TimeTracker[s] = theseSpan;
            }

        }
        private async void ListEdit(object sender2, RoutedEventArgs ee)
        {
            Activer.Hide();
            Button obj3 = sender2 as Button;
            String objname2 = obj3.Name;
            String[] objnamer2 = objname2.Split(',');
            objname2 = objnamer2[0];
            String objtime = objnamer2[1];
            int s = Convert.ToInt16(objname2);
            int u = Convert.ToInt16(objtime);
            TextBlock Namerr = LinePanel.Children[s] as TextBlock;
            var Namerr2 = Namerr.Text;
            List<MainPage.RunSpan> thisone = TimeTracker[s];
            MainPage.RunSpan thisEdit = thisone[u];

            ContentDialog2 CheckDialog2 = new ContentDialog2
            {
                Title = "Editing " + Namerr2,
                PrimaryButtonText = "Cancel"
            };
            CheckDialog2.getTimes(thisEdit.startTime.TimeOfDay, thisEdit.stopTime.TimeOfDay, thisEdit.whyDown, downtimeWhy);

            ContentDialogResult result = await CheckDialog2.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

            }
            else
            {
                ContentDialog2.RunSpan newSpan = CheckDialog2.Changer();
                thisEdit.startTime = newSpan.startTime;
                thisEdit.stopTime = newSpan.stopTime;
                thisEdit.whyDown = newSpan.whyDown;
                thisone[u] = thisEdit;
                TimeTracker[s] = thisone;

                TimeSpan duration = thisEdit.stopTime - thisEdit.startTime;
                TotalBoxes[s].Text = duration.ToString(@"hh\:mm");
                if (!LineStatus[s])
                {
                    StopBoxes[s].Text = thisEdit.stopTime.ToString("hh:mm");
                    StartBoxes[s].Text = thisEdit.startTime.ToString("hh:mm");
                    DownBoxes[s].Text = downtimeWhy[thisEdit.whyDown];
                }


            }
        }
        public async void EditClick(object sender, RoutedEventArgs e)
        {
            if (!DialogOpen)
            {
                DialogOpen = true;
                Button obj = sender as Button;
                String objname = obj.Name;
                String[] objnamer = objname.Split(',');
                objname = objnamer[0];
                //DBug.Text = objname.ToString() + " Edit";
                int s = Convert.ToInt16(objname);
                TextBlock Namer = LinePanel.Children[s] as TextBlock;
                var Namer2 = Namer.Text;
                ContentDialog EditDialog = new ContentDialog
                {
                    Title = "Editing " + Namer2,
                    PrimaryButtonText = "Cancel",
                    SecondaryButtonText = "Accept"

                };
                TimePicker EditTime = new TimePicker();
                EditTime.TimeChanged += EditUpdater;
                EditTime.Header = "Choose New Start Time";
                EditDialog.Content = EditTime;
                EditDialog.UpdateLayout();
                ContentDialogResult result = await EditDialog.ShowAsync();

                if (result == ContentDialogResult.Secondary)
                {
                    Starter[s] = Convert.ToDateTime(EditTime.Time.ToString());
                    //DBug.Text = Starter[s].ToString();
                    StartBoxes[s].Text = Starter[s].ToString("hh:mm");
                }
                else
                {

                }
                this.UpdateLayout();
                DialogOpen = false;
            }

        }
        private void EditUpdater(object sender, TimePickerValueChangedEventArgs e)
        {
            TimePicker timePicker = sender as TimePicker;
            TimeSpan editor = timePicker.Time;
            //DBug.Text = editor.ToString();

        }
        public async void breaker()
        {
            timerStart = true;
            if (!DialogOpen)
            {
                jjjj = true;
                DialogOpen = true;
                ContentDialog ResetDialog = new ContentDialog
                {
                    Title = "It Is Break Time",
                    PrimaryButtonText = "Back",
                    SecondaryButtonText = "Accept"

                };
                ContentDialogResult result = await ResetDialog.ShowAsync();
                if (result == ContentDialogResult.Secondary)
                {


                }
                DialogOpen = false;
            }
        }
        public void LineTT(object sender, RoutedEventArgs e)
        {
            if (FirstRun)
            {
                FirstRun = false;
                for (int i = 0; i < 15; i++)
                {
                    MassRunner[i].IsEnabled = false;
                }
                SetButton.IsEnabled = false;
            }
            if (!MassSaver && !OffSaver)
            {

                ToggleSwitch obj = sender as ToggleSwitch;

                String objname = obj.Name;
                String[] objnamer = objname.Split(',');
                objname = objnamer[0];
                //DBug.Text = objname.ToString();
                int s = Convert.ToInt16(objname);
                if (!LineStatus[s])
                {
                    if (Stopper[s] != null)
                    {
                        StopBoxes[s].Text = "~~~";
                    }
                    Starter[s] = DateTime.Now;
                    DownBoxes[s].Text = "---";
                    LineStatus[s] = true;
                    StartBoxes[s].Foreground = new SolidColorBrush(Windows.UI.Colors.LimeGreen);
                    StartBoxes[s].Text = Starter[s].ToString("hh:mm");
                    EditButtons[s].IsEnabled = true;

                }
                else
                {
                    Downtimer(s);
                    //SheetMaker sheet = new SheetMaker();
                   // DateTimeOffset pee = sheet.deadhead();
                }
            }
        }
        private async void autoDown(int c)
        {
            timerStart = true;
            for (int i = 0; i < 15; i++)
            {
                if (LineStatus[i])
                {
                    Stopper[i] = DateTime.Now;
                    LineStatus[i] = false;
                    StopBoxes[i].Text = Stopper[i].ToString("hh:mm");
                    TimeSpan duration = Stopper[i] - Starter[i];
                    RunTime[i] += duration;
                    TotalBoxes[i].Text = RunTime[i].ToString(@"hh\:mm");
                    DownBoxes[i].Text = downtimeWhy[11].ToString();
                    MainPage.RunSpan thisSpan = new MainPage.RunSpan();
                    thisSpan.startTime = Starter[i]; thisSpan.stopTime = Stopper[i]; thisSpan.whyDown = 11;
                    List<MainPage.RunSpan> SpanList = new List<MainPage.RunSpan>();
                    SpanList = TimeTracker[i];
                    SpanList.Add(thisSpan);
                    TimeTracker[i] = SpanList;
                    breakTime[i] = true;
                    EditButtons[i].IsEnabled = false;
                }
            }
            TimeSpan delay = TimeSpan.FromMinutes(c);
            ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreateTimer(
            (source) =>
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    for (int i = 0; i < 15; i++)
                    {
                        if (breakTime[i])
                        {
                            breakTime[i] = false;
                            Starter[i] = DateTime.Now;
                            DownBoxes[i].Text = "---";
                            LineStatus[i] = true;
                            StartBoxes[i].Foreground = new SolidColorBrush(Windows.UI.Colors.LimeGreen);
                            StartBoxes[i].Text = Starter[i].ToString("hh:mm");
                            StopBoxes[i].Text = "~~~";
                            EditButtons[i].IsEnabled = true;
                        }
                    }
                });
            }, delay);
        }
        private async void Downtimer(int s)
        {
            if (!DialogOpen)
            {
                ComboBox Why = new ComboBox();
                TextBlock Namer = LinePanel.Children[s] as TextBlock;
                var Namer2 = Namer.Text;
                DialogOpen = true;
                ContentDialog ResetDialog = new ContentDialog
                {

                    Title = "Why is " + Namer2 + " down?",
                    PrimaryButtonText = "Cancel",
                    SecondaryButtonText = "Accept"

                };
                foreach (var reason in downtimeWhy)
                {
                    Why.Items.Add(reason);
                }
                Why.SelectedIndex = 11;
                ResetDialog.Content = Why;
                ContentDialogResult result = await ResetDialog.ShowAsync();
                if (result == ContentDialogResult.Secondary)
                {
                    Stopper[s] = DateTime.Now;
                    LineStatus[s] = false;
                    StopBoxes[s].Text = Stopper[s].ToString("hh:mm");
                    TimeSpan duration = Stopper[s] - Starter[s];
                    RunTime[s] += duration;
                    TotalBoxes[s].Text = RunTime[s].ToString(@"hh\:mm");
                    DownBoxes[s].Text = Why.SelectedItem.ToString();
                    //Runner[s].IsOn = false;
                    MainPage.RunSpan thisSpan = new MainPage.RunSpan();
                    thisSpan.startTime = Starter[s]; thisSpan.stopTime = Stopper[s]; thisSpan.whyDown = Why.SelectedIndex;
                    List<MainPage.RunSpan> SpanList = new List<MainPage.RunSpan>();
                    SpanList = TimeTracker[s];
                    if (firstCheck[s])
                    {
                        SpanList.Add(thisSpan);
                        TimeTracker[s] = SpanList;
                        firstCheck[s] = false;
                        CheckButtons[s].IsEnabled = true;
                        

                    }
                    else
                    {
                        SpanList = (List<MainPage.RunSpan>)TimeTracker[s];
                        SpanList.Add(thisSpan);
                        TimeTracker[s] = SpanList;
                        
                    }
                    StartBoxes[s].Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    EditButtons[s].IsEnabled = false;
                    if (thisSpan.whyDown == 2)
                    {
                        ResetDialog.Hide();
                        int lineZZ = s;
                        ChangeMe(lineZZ);
                        
                    }

                }


                else
                {
                    OffSaver = true;
                    Runner[s].IsOn = true;
                    LineStatus[s] = true;
                    OffSaver = false;

                }
                DialogOpen = false;
            }
        }
        private async void ChangeMe(int liner)
        {
            TextBlock Namer = LinePanel.Children[liner] as TextBlock;
            String Namer2 = Namer.Text;
            ContentDialog ChangeDialog = new ContentDialog
            {

                Title = "Go To Changeover?",
                PrimaryButtonText = "Stay?",
                SecondaryButtonText = "Go?"

            };
            ContentDialogResult result = await ChangeDialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                this.Frame.Navigate(typeof(Change), Namer2);
                DialogOpen = false;
            }
            else
            {
                ChangeDialog.Hide();
            }
        }
        private TimeSpan SumTime(int s)
        {
            List<MainPage.RunSpan> totalList = TimeTracker[s];
            TimeSpan totalTime = new TimeSpan();
            foreach (MainPage.RunSpan thisLine in totalList)
            {
                TimeSpan temp = thisLine.stopTime - thisLine.startTime;
                totalTime += temp;
                //DBug.Text = totalTime.ToString(@"hh\:mm");
            }
            return totalTime;
        }
        private void SetStart(object sender, RoutedEventArgs e)
        {

            MassSaver = true;
            FirstRun = false;
            SetButton.IsEnabled = false;
            for (int i = 0; i < 15; i++)
            {
                if (MassRunner[i].IsChecked == true)
                {
                    Starter[i] = DateTime.Now;
                    LineStatus[i] = true;
                    StartBoxes[i].Text = Starter[i].ToString("hh:mm");
                    EditButtons[i].IsEnabled = true;
                    CheckButtons[i].IsEnabled = true;
                    Runner[i].IsOn = true;
                    StartBoxes[i].Foreground = new SolidColorBrush(Windows.UI.Colors.LimeGreen);

                }
                MassRunner[i].IsEnabled = false;
            }
            MassSaver = false;
        }
        

        private void Bak(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage),TimeTracker);
        }
    }
}
