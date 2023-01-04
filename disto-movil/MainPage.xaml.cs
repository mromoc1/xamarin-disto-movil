using System.Globalization;

namespace disto_movil;

public partial class MainPage : ContentPage
{
    private ISpeechToText stt;
    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    public Command ListenCommand { get; set; }
	public Command ListenCancelCommand { get; set; }
	public string RecognitionText { get; set; }

	public MainPage(ISpeechToText stt)
	{
		InitializeComponent();
        this.stt = stt;

		ListenCommand = new Command(Listen);
		ListenCancelCommand = new Command(ListenCancel);
        BindingContext= this;

    }

    private void ListenCancel()
    {
        tokenSource?.Cancel();
    }

    private async void Listen()
    {
        var isAuthorized = await stt.RequestPermissions();
        if (isAuthorized)
        {
            try
            {
                while (true)
                {
                    RecognitionText = await stt.Listen(CultureInfo.GetCultureInfo("en-us"),
                        new Progress<string>(partialText =>
                        {
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                RecognitionText = partialText;
                            }
                            else
                            {

                                RecognitionText += partialText + " ";
                            }
                            System.Console.WriteLine(RecognitionText);
                            OnPropertyChanged(nameof(RecognitionText));
                        }), tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        else
        {
            await DisplayAlert("Permission Error", "No microphone access", "OK");
        }
    }
}

