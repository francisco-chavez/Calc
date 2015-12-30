using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Unv.CalcWPF.Views
{
	/// <summary>
	/// Interaction logic for KeyPadView.xaml
	/// </summary>
	public partial class KeyPadView : UserControl
	{
		public KeyPadView()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button buttonClicked = sender as Button;

			if (buttonClicked == null)
				throw new Exception("Button Click sender wasn't a Button object.");

			var btnString = buttonClicked.Content.ToString();

			CalcInput convertedValue = CalcInput.NumKey0;

			switch (btnString)
			{
			// NumPad keys
			case "0":
				convertedValue = CalcInput.NumKey0;
				break;
			case "1":
				convertedValue = CalcInput.NumKey1;
				break;
			case "2":
				convertedValue = CalcInput.NumKey2;
				break;
			case "3":
				convertedValue = CalcInput.NumKey3;
				break;
			case "4":
				convertedValue = CalcInput.NumKey4;
				break;
			case "5":
				convertedValue = CalcInput.NumKey5;
				break;
			case "6":
				convertedValue = CalcInput.NumKey6;
				break;
			case "7":
				convertedValue = CalcInput.NumKey7;
				break;
			case "8":
				convertedValue = CalcInput.NumKey8;
				break;
			case "9":
				convertedValue = CalcInput.NumKey9;
				break;
			case ".":
				convertedValue = CalcInput.KeyPoint;
				break;

			// Invert number sign
			case "\u00B1":
				convertedValue = CalcInput.KeyInvertSign;
				break;

			// Add Operation
			case "\u002B":
				convertedValue = CalcInput.KeyAdd;
				break;
			// Subract Operation
			case "\u2212":
				convertedValue = CalcInput.KeySubtract;
				break;
			// Multiply Operation
			case "\u00D7":
				convertedValue = CalcInput.KeyMultiply;
				break;
			// Divide Operation
			case "\u00F7":
				convertedValue = CalcInput.KeyDivide;
				break;

			// Memory Functions
			case "MC":
				convertedValue = CalcInput.KeyMemoryClear;
				break;
			case "MR":
				convertedValue = CalcInput.KeyMemoryRetrieve;
				break;
			case "M+":
				convertedValue = CalcInput.KeyMemoryAdd;
				break;
			case "M-":
				convertedValue = CalcInput.KeyMemorySubtract;
				break;

			// Clear
			case "C":
				convertedValue = CalcInput.KeyClear;
				break;
			// Clear Entry
			case "CE":
				convertedValue = CalcInput.KeyClearEntry;
				break;

			// Square Root
			case "\u221A":
				convertedValue = CalcInput.KeySquareRoot;
				break;

			// Percent
			case "\u0025":
				convertedValue = CalcInput.KeyPercent;
				break;

			// Case Equals
			case "=":
				convertedValue = CalcInput.KeyEquals;
				break;

			default:
				throw new ArgumentException(string.Format("\"{0}\" is not an accepted input."));
			}
		}
	}
}
