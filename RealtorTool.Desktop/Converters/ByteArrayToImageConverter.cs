using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace RealtorTool.Desktop.Converters;

public class ByteArrayToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is byte[] bytes && bytes.Length > 0)
        {
            try
            {
                using var stream = new System.IO.MemoryStream(bytes);
                return new Bitmap(stream);
            }
            catch
            {
                return GetDefaultImage();
            }
        }
        
        return GetDefaultImage();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static Bitmap? GetDefaultImage()
    {
        try
        {
            // Можно вернуть дефолтное изображение из ресурсов
            // или просто null для отображения плейсхолдера
            return null;
        }
        catch
        {
            return null;
        }
    }
}