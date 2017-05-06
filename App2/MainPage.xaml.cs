using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App2.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly GpioController gpio;
        private GpioPin redPin;
        private GpioPin yellowPin;
        private StartConversationResponse _startConversationResponse;
        private DirectLineAPIClient _directLineApi;
        private string _waterMark;
        public MainPage()
        {
            this.InitializeComponent();
            gpio = GpioController.GetDefault();
        }

        private async Task StartConversation()
        {
            const string secret = "PYI2PavkIbs.cwA.bYo.mHxTGJCXPbKECNeyIN30aFIfLh-V1MEPWUMDKkSmuJg";
            _directLineApi = new DirectLineAPIClient(secret);
            _startConversationResponse = await _directLineApi.StartConversationAsync();
        }
        private async void BtnSend_OnClick(object sender, RoutedEventArgs e)
        {
            await _directLineApi.SendMessageAsync(_startConversationResponse.ConversationId, "me", TxtMessage.Text);
            var response = await _directLineApi.GetMessagesAsync(_startConversationResponse.ConversationId, _waterMark);
            _waterMark = response.Watermark;
            var command = response.Messages.FirstOrDefault(msg => msg.From != "me").Text;
            if (command == null)
                return;
            var cmdArray = command.Split(';');
            if(cmdArray[0].ToLower() == "red")
                TurnRedLedOnOff(cmdArray[1]);
            if(cmdArray[0].ToLower() == "yellow")
                TurnYellowLedOnOff(cmdArray[1]);
        }

        private void TurnYellowLedOnOff(string ledState)
        {
            if (yellowPin == null)
                yellowPin = gpio.OpenPin(6);
            if (ledState.ToLower() == "on")
            {
                yellowPin.Write(GpioPinValue.High);
                yellowPin.SetDriveMode(GpioPinDriveMode.Output);
            }
            else
            {
                yellowPin.Write(GpioPinValue.Low);
            }
           
        }

        private void TurnRedLedOnOff(string ledState)
        {
            if (redPin == null)
                redPin = gpio.OpenPin(17);
            if (ledState.ToLower() == "on")
            {
                redPin.Write(GpioPinValue.High);
                redPin.SetDriveMode(GpioPinDriveMode.Output);
            }
            else
            {
                redPin.Write(GpioPinValue.Low);
            }
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await StartConversation();
        }
    }
}
