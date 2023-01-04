using disto_movil.Platforms;
namespace disto_movil;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddTransient<MainPage>();
#if ANDROID
		builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
#endif
		return builder.Build();
	}
}
