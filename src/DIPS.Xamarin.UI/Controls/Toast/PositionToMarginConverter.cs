using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Toast
{
    public class PositionToMarginConverter : IMarkupExtension, IValueConverter
    {
        private IServiceProvider m_serviceProvider;
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double positionY)
            {
                var ratio = positionY < 0 ? 0 : (positionY > 100 ? 100 : positionY);
                var size = ratio * (Application.Current.MainPage.Height - 60) / 100;
                return new Thickness(0, size, 0, 0);
            }
            
            return new Thickness(0, 10, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            // var margin = (Thickness)GetValue(PositionYProperty);
            // return margin.Top * 100 / Application.Current.MainPage.Height;
            // var ratio = margin.Top < 0 ? 0 : (margin.Top > 100 ? 100 : margin.Top);
            // var size = ratio * Application.Current.MainPage.Height / 100;
            // return margin.Top;
            throw new NotImplementedException();
        }
    }
}