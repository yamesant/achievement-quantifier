using System.ComponentModel;
using System.Globalization;

namespace AQ.Console;

public sealed class DateOnlyTypeConverter : TypeConverter
{
    private const string Format = "dd/MM/yyyy";
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string dateString && DateOnly.TryParseExact(dateString, Format, out DateOnly date))
        {
            return date;
        }
        
        throw new FormatException($"Cannot convert '{value}' to DateOnly. Expected format is '{Format}'.");
    }
}