using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CybersecurityChatbotWPF
{
    public class SentimentToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sentiment = value as string ?? "neutral";
            
            return sentiment switch
            {
                "worried" => new SolidColorBrush(Colors.OrangeRed),
                "frustrated" => new SolidColorBrush(Colors.Red),
                "curious" => new SolidColorBrush(Colors.DodgerBlue),
                "happy" => new SolidColorBrush(Colors.Green),
                "sad" => new SolidColorBrush(Colors.Purple),
                _ => new SolidColorBrush(Colors.Black)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}