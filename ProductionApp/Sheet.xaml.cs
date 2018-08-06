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
using Windows.Storage;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sheet : Page
    {
        String NL = "\r\n";
        String header;
        String footer;
        String allRow = "";
        int ii = 3;
        List<String> newRow = new List<String>();
        int[] columnWidths = new int[] { 80, 100, 100, 65, 65, 50, 50, 50, 50, 50, 50, 50, 50, 50, 75 };
        String[] directions = new string[] { "Bottom", "Left", "Right", "Top" };
        String[] dirs = new string[4];
        String[] thiccdirs = new string[4];
        String[] LRdirs = new string[4];
        String[] Bdirs = new string[4];
        public Sheet()
        {
            int uu = 0;
            foreach (String dir in directions)
            {
                dirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>";
                thiccdirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"3\"/>";
                if (uu == 1 || uu == 2)
                {
                    LRdirs[uu] = Bdirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"3\"/>";

                }
                else
                {
                    LRdirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>";
                    if (uu == 0)
                    {
                        Bdirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"3\"/>";
                    }
                    else
                    {
                        Bdirs[uu] = "<ss:Border ss:Position=\"" + dir + "\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>";
                    }

                }
                uu++;
            }
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
        private DateTimeOffset deadhead()
        {
            DateTimeOffset balls = dateMe.Date;
            boo.Text = balls.ToString("MM-dd-yy");
            header = "<?xml version = \"1.0\" ?>" + NL + "<ss:Workbook xmlns:ss = \"urn:schemas-microsoft-com:office:spreadsheet\" >" + NL;
            header += "<ss:Styles>" + NL + "<ss:Style ss:ID = \"1\">" + NL + "<ss:Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>" + NL;
            header += "<ss:Borders>" + NL + dirs[0] + NL + dirs[1] + NL + dirs[2] + NL + dirs[3] + NL + "</ss:Borders>" + NL + "<ss:Font ss:Bold=\"1\"/>" + NL + "</ss:Style>" + NL;
            header += "<ss:Style ss:ID = \"2\">" + NL + "<ss:Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>" + NL;
            header += "<ss:Borders>" + NL + thiccdirs[0] + NL + thiccdirs[1] + NL + thiccdirs[2] + NL + thiccdirs[3] + NL + "</ss:Borders>" + NL + "<ss:Font ss:Bold=\"1\"/>" + NL + "</ss:Style>" + NL;
            header += "<ss:Style ss:ID = \"3\">" + NL + "<ss:Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>" + NL;
            header += "<ss:Borders>" + NL + LRdirs[0] + NL + LRdirs[1] + NL + LRdirs[2] + NL + LRdirs[3] + NL + "</ss:Borders>" + NL + "<ss:Font ss:Bold=\"1\"/>" + NL + "</ss:Style>" + NL;
            header += "<ss:Style ss:ID = \"4\">" + NL + "<ss:Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>" + NL;
            header += "<ss:Borders>" + NL + Bdirs[0] + NL + Bdirs[1] + NL + Bdirs[2] + NL + Bdirs[3] + NL + "</ss:Borders>" + NL + "<ss:Font ss:Bold=\"1\"/>" + NL + "</ss:Style>" + NL;
            header += "</ss:Styles>" + NL + "<ss:Worksheet ss:Name = \"" + balls.ToString("MM-dd-yy") + "\">" + NL + "<ss:Table  ss:ExpandedColumnCount=\"15\">" + NL;
            for (int x = 0; x < 15; x++)
            {
                header += "<ss:Column ss:AutoFitWidth=\"0\" ss:Width = \"" + columnWidths[x].ToString() + "\"/>" + NL;
            }
            header += "<ss:Row>" + NL + "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"1\">" + NL + "<ss:Data ss:Type =\"String\"> Line #</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"1\">" + NL + "<ss:Data ss:Type = \"String\"> Part #</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Coils Consumed</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeAcross=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Press Counter</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeAcross=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Scrap</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeAcross=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Reject X-Pans</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeAcross=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Downtime</ss:Data>" + NL + "</ss:Cell>" + NL; //
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Run Time</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeAcross=\"1\">" + NL + "<ss:Data ss:Type = \"String\">Pans/Hr</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"1\">" + NL + "<ss:Data ss:Type = \"String\"># of Operators</ss:Data>" + NL + "</ss:Cell>" + NL + "</ss:Row>" + NL;


            header += "<ss:Row>" + NL + "<ss:Cell ss:Index=\"4\" ss:StyleID=\"2\">" + NL + "<ss:Data ss:Type =\"String\">First Press</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"5\">" + NL + "<ss:Data ss:Type =\"String\">Last Press</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"6\">" + NL + "<ss:Data ss:Type =\"String\">Reason</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"7\">" + NL + "<ss:Data ss:Type =\"String\">QTY</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"8\">" + NL + "<ss:Data ss:Type =\"String\">Reason</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"9\">" + NL + "<ss:Data ss:Type =\"String\">QTY</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"10\">" + NL + "<ss:Data ss:Type =\"String\">Reason</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"11\">" + NL + "<ss:Data ss:Type =\"String\">Time</ss:Data>" + NL + "</ss:Cell>" + NL;// + "</ss:Row>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"13\">" + NL + "<ss:Data ss:Type =\"String\">Average</ss:Data>" + NL + "</ss:Cell>" + NL;
            header += "<ss:Cell ss:StyleID=\"2\" ss:Index=\"14\">" + NL + "<ss:Data ss:Type =\"String\">Target</ss:Data>" + NL + "</ss:Cell>" + NL + "</ss:Row>" + NL;
            footer = "</ss:Table>" + NL + "</ss:Worksheet>" + NL + "</ss:Workbook>";
            return balls;
        }
        private async void Save(object sender, RoutedEventArgs e)
        {

            string path = @"C:\Users\Engineering Laptop\Documents";
            DateTimeOffset holla = deadhead();
            StorageFolder storageFolder = KnownFolders.MusicLibrary;
            //boo.Text = storageFolder.DisplayName;
            String namestring = holla.ToString("MM-dd-yy") + ".xml";
            StorageFile newFile = await storageFolder.CreateFileAsync(namestring, CreationCollisionOption.GenerateUniqueName);
            foreach (String var in newRow)
            {
                allRow += var + NL;
            }
            String one = header + allRow + footer;
            await Windows.Storage.FileIO.WriteTextAsync(newFile, one);
            newRow = new List<String>(); //clears existing list
            allRow = "";


        }
        private void rowboat(object sender, RoutedEventArgs e)
        {
            String entry = rownamer.Text;
            String entry2 = row2namer.Text;
            String entry3A = row3namerA.Text;
            String entry3B = row3namerB.Text;
            String entry3C = row3namerC.Text;
            String entry4 = row4namer.Text; String entry5 = row5namer.Text;
            String entry14 = row14namer.Text; String entry15 = row15namer.Text;
            String entry6A = row6Anamer.Text; String entry6B = row6Bnamer.Text; String entry6C = row6Cnamer.Text;
            String entry7A = row7Anamer.Text; String entry7B = row7Bnamer.Text; String entry7C = row7Cnamer.Text;
            String entry8A = row8Anamer.Text; String entry8B = row8Bnamer.Text; String entry8C = row8Cnamer.Text;
            String entry9A = row9Anamer.Text; String entry9B = row9Bnamer.Text; String entry9C = row9Cnamer.Text;
            String entry10A = row10Anamer.Text; String entry10B = row8Bnamer.Text; String entry10C = row10Cnamer.Text;
            String entry11A = row11Anamer.Text; String entry11B = row11Bnamer.Text; String entry11C = row11Cnamer.Text;
            double runtime = 7.5 - (Convert.ToDouble(row11Anamer.Text) + Convert.ToDouble(row11Bnamer.Text) + Convert.ToDouble(row11Cnamer.Text));
            String entry12 = runtime.ToString();
            row12namer.Text = runtime.ToString();
            String entry13 = Math.Floor((Convert.ToDouble(entry5) / runtime)).ToString();
            row13namer.Text = entry13;
            String newest = "<ss:Row ss:Index=\"" + ii.ToString() + "\">" + NL + "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\">" + NL + "<ss:Data ss:Type=\"String\">" + entry + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry2 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry3A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry4 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry5 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry6A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry7A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry8A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry9A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry10A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"> " + NL + "<ss:Data ss:Type=\"String\">" + entry11A + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry12 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry13 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry14 + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"2\" ss:MergeDown=\"2\" > " + NL + "<ss:Data ss:Type=\"String\">" + entry15 + "</ss:Data>" + NL + "</ss:Cell>" + NL + "</ss:Row>" + NL;  //ss:MergeAcross=\"10\"
            newest += "<ss:Row ss:Index=\"" + (ii + 1).ToString() + "\">" + NL + "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"3\">" + NL + "<ss:Data ss:Type=\"String\">" + entry3B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"6\">" + NL + "<ss:Data ss:Type=\"String\">" + entry6B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"7\">" + NL + "<ss:Data ss:Type=\"String\">" + entry7B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"8\">" + NL + "<ss:Data ss:Type=\"String\">" + entry8B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"9\">" + NL + "<ss:Data ss:Type=\"String\">" + entry9B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"10\">" + NL + "<ss:Data ss:Type=\"String\">" + entry10B + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"3\"  ss:Index=\"11\">" + NL + "<ss:Data ss:Type=\"String\">" + entry11B + "</ss:Data>" + NL + "</ss:Cell>" + NL + "</ss:Row>" + NL;
            newest += "<ss:Row ss:Index=\"" + (ii + 2).ToString() + "\">" + NL + "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"3\">" + NL + "<ss:Data ss:Type=\"String\">" + entry3C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"6\">" + NL + "<ss:Data ss:Type=\"String\">" + entry6C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"7\">" + NL + "<ss:Data ss:Type=\"String\">" + entry7C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"8\">" + NL + "<ss:Data ss:Type=\"String\">" + entry8C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"9\">" + NL + "<ss:Data ss:Type=\"String\">" + entry9C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"10\">" + NL + "<ss:Data ss:Type=\"String\">" + entry10C + "</ss:Data>" + NL + "</ss:Cell>" + NL;
            newest += "<ss:Cell ss:StyleID=\"4\"  ss:Index=\"11\">" + NL + "<ss:Data ss:Type=\"String\">" + entry11C + "</ss:Data>" + NL + "</ss:Cell>" + NL + "</ss:Row>" + NL;
            newRow.Add(newest);
            ii += 3;

        }
        private void checkNumber(object sender, TextChangedEventArgs e)
        {
            TextBox checkMe = sender as TextBox;
            String checkString = checkMe.Text;
            String goodString = "";
            if (checkString != null)
            {
                foreach (char c in checkString)
                {
                    if (char.IsDigit(c) || c == '.')
                    {
                        goodString += c;
                    }

                }
                checkMe.Text = goodString;
            }


        }
        private void ClickMain(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
