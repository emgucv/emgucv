//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

namespace MauiDemoApp
{
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
                    fonts.AddFont("Inter-Light.ttf", "InterLight");
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                    fonts.AddFont("InterDisplay-ExtraBold.ttf", "InterDisplayExtraBold");
                    fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
                });

            // Entries marked DemoTheme.BorderlessEntry (e.g. the home search field)
            // sit inside their own rounded pill, so drop the native border.
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
                if (view is Entry e && e.ClassId == DemoTheme.BorderlessEntry)
                {
#if __IOS__ || __MACCATALYST__
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
#elif __ANDROID__
                    handler.PlatformView.Background = null;
#elif WINDOWS
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
                }
            });

            return builder.Build();
        }
    }
}