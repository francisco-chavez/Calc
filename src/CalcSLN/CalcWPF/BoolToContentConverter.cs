using System;
using System.Globalization;
using System.Windows.Data;


namespace Unv.CalcWPF
{
	/// <summary>
	/// This converter class replaces all one-way BoolToXXXXConverter classes used in WPF bindings.
	/// </summary>
	public class BoolToContentConverter
		: IValueConverter
	{
		#region Properties

		/// <summary>
		/// Get or set the content to convert to when given a True value.
		/// </summary>
		public object TrueContent	{ get; set; }

		/// <summary>
		/// Get or set the content to convert to when given a False value.
		/// </summary>
		public object FalseContent	{ get; set; }

		/// <summary>
		/// Get or set the content to convert to when given a Null value. Quite a 
		/// few properties will use boxing, leading to the posibility of a null value.
		/// </summary>
		public object NullContent	{ get; set; }

		#endregion


		#region Constructors

		public BoolToContentConverter()
		{
			TrueContent		= "True";
			FalseContent	= "False";
			NullContent		= "Null";
		}

		~BoolToContentConverter()
		{
			TrueContent		= null;
			FalseContent	= null;
			NullContent		= null;
		}

		#endregion


		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool?	boolValue = value as bool?;
			return	boolValue.HasValue ? (boolValue.Value ? TrueContent : FalseContent) : NullContent;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException("BoolToContentConverter(s) are unable to convert back to a bool value.");
		}

		#endregion
	}
}
