using MQTTnet.Client; //MQTT messaging libraries
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;
//using MQTTnet.ManagedClient;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System; //windows libraries
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks; //system libraries
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.Foundation;
using Windows.Security.Cryptography.Certificates;
using Windows.UI.Core;//UI libraries
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.System.Threading;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.Security.ExchangeActiveSyncProvisioning;
using MQTTnet;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProductionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Press : Page
    {
        private readonly ConcurrentQueue<MqttNetLogMessage> _traceMessages = new ConcurrentQueue<MqttNetLogMessage>();//initiate mqtt
        private IMqttClient _mqttClient; //mqtt client-retrieves messages
        private IMqttServer _mqttServer; //mqtt broker-distributes messages
        int[] Count = new int[40] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Rectangle[] Liner = new Rectangle[40];
        TextBlock[] Texter = new TextBlock[40];
        public Press()
        {
            Thickness marginer = new Thickness();
            marginer.Bottom = marginer.Left = marginer.Right = marginer.Top = 2;
            Thickness Tmarginer = new Thickness();
            Tmarginer.Left = Tmarginer.Right = 32;
            Tmarginer.Top = Tmarginer.Bottom = 17;
            _mqttServer = new MqttFactory().CreateMqttServer();
            var options = new MqttServerOptions();
            options.DefaultEndpointOptions.Port = 1883;
            Starter(options);

            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            for (int i = 0; i < 40; i++)
            {
                Texter[i] = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    Margin = Tmarginer,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Indigo),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = Windows.UI.Text.FontWeights.Bold,
                    FontSize = 16

                };
                Liner[i] = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Height = 50,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 78,
                    Fill = new SolidColorBrush(Windows.UI.Colors.LightSteelBlue),
                    Margin = marginer
                };
                Liner[i].Fill.Opacity = .67;
                if (i < 8)
                {
                    Line1.Children.Add(Liner[i]);
                    Line1Text.Children.Add(Texter[i]);
                }
                else if (i < 12)
                {
                    Line2.Children.Add(Liner[i]);
                    Line2Text.Children.Add(Texter[i]);
                }
                else if (i == 12)
                {
                    Line2A.Children.Add(Liner[i]);
                    Line2AText.Children.Add(Texter[i]);
                }
                else if (i < 21)
                {
                    Line3.Children.Add(Liner[i]);
                    Line3Text.Children.Add(Texter[i]);
                }
                else if (i < 25)
                {
                    Line4.Children.Add(Liner[i]);
                    Line4Text.Children.Add(Texter[i]);
                }
                else if (i < 29)
                {
                    Line5.Children.Add(Liner[i]);
                    Line5Text.Children.Add(Texter[i]);
                }
                else if (i == 29)
                {
                    Line5A.Children.Add(Liner[i]);
                    Line5AText.Children.Add(Texter[i]);
                }
                else if (i == 30)
                {
                    Line5B.Children.Add(Liner[i]);
                    Line5BText.Children.Add(Texter[i]);
                }
                else if (i < 35)
                {
                    Line6.Children.Add(Liner[i]);
                    Line6Text.Children.Add(Texter[i]);
                }
                else if (i == 35)
                {
                    Line6B.Children.Add(Liner[i]);
                    Line6BText.Children.Add(Texter[i]);
                }
                else
                {
                    Line7.Children.Add(Liner[i]);
                    Line7Text.Children.Add(Texter[i]);
                }
                //< Rectangle x: Name = "p1" HorizontalAlignment = "Center" Height = "78" VerticalAlignment = "Center" Width = "78" Fill = "#FFDFC6C6" Margin = "1,1,1,1" />
            }

        }
        public async void Starter(MqttServerOptions options)
        {
            EasClientDeviceInformation CurrentDeviceInfor = new EasClientDeviceInformation();
            var machiner = CurrentDeviceInfor.FriendlyName;
            await _mqttServer.StartAsync(options);
            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options2 = new MqttClientOptions { ClientId = "" };


            options2.ChannelOptions = new MqttClientTcpOptions
            {
                Server = machiner,
                Port = 1883,
                TlsOptions = tlsOptions
            };

            if (options2.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            /*options.Credentials = new MqttClientCredentials
            {
                Username = User.Text,
                Password = Password.Text
            };*/

            options2.CleanSession = true;
            options2.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            try
            {
                if (_mqttClient != null)
                {
                    await _mqttClient.DisconnectAsync();
                    _mqttClient.ApplicationMessageReceived -= OnApplicationMessageReceived;
                }

                var factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient();
                _mqttClient.ApplicationMessageReceived += OnApplicationMessageReceived;

                await _mqttClient.ConnectAsync(options2);
            }
            catch (Exception exception)
            {
                //Trace.Text += exception + Environment.NewLine;
            }

            if (_mqttClient == null)
            {
                return;
            }
            var qos = MqttQualityOfServiceLevel.ExactlyOnce;
            await _mqttClient.SubscribeAsync(new TopicFilter("luxProto2", qos));
        }
        private async void OnApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                var etem = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                String message = etem.ToString();
                String[] item = message.Split(',');
                int ind = Convert.ToInt16(item[0]) - 1;
                Count[ind]++;
                Texter[ind].Text = Count[ind].ToString();
                Texter[ind].Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                Liner[ind].Fill = new SolidColorBrush(Windows.UI.Colors.Azure);
                for (int i = 0; i < 40; i++)
                {
                    if (i != ind)
                    {
                        Liner[i].Fill = new SolidColorBrush(Windows.UI.Colors.Gainsboro);
                    }
                }

            });
        }

        private void ClickBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
