using System;
using System.Globalization;
using System.Windows.Data;


namespace Unv.CalcWPF
{
	public class BoolToContentConverter
		: IValueConverter
	{
		public object TrueContent	{ get; set; }
		public object FalseContent	{ get; set; }
		public object NullContent	{ get; set; }

		public BoolToContentConverter()
		{
			TrueContent		= "True";
			FalseContent	= "False";
			NullContent		= "Null";
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool? boolValue = value as bool?;
			return boolValue.HasValue ? (boolValue.Value ? TrueContent : FalseContent) : NullContent;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
