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
	public partial class KeyPadView 
		: UserControl
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

			CalcInput inputValue = ConvertInput(btnString);

			var calculator = this.DataContext as TheBrain;
			if (calculator == null)
				throw new MemberAccessException("Unable to find TheBrain instance in DataContext of KeyPadView.");
			calculator.NewInput(inputValue);
		}

		private CalcInput ConvertInput(string input)
		{
			CalcInput result;

			switch (input)
			{
			// NumPad keys
			case "0":
				result = CalcInput.NumKey0;
				break;
			case "1":
				result = CalcInput.NumKey1;
				break;
			case "2":
				result = CalcInput.NumKey2;
				break;
			case "3":
				result = CalcInput.NumKey3;
				break;
			case "4":
				result = CalcInput.NumKey4;
				break;
			case "5":
				result = CalcInput.NumKey5;
				break;
			case "6":
				result = CalcInput.NumKey6;
				break;
			case "7":
				result = CalcInput.NumKey7;
				break;
			case "8":
				result = CalcInput.NumKey8;
				break;
			case "9":
				result = CalcInput.NumKey9;
				break;
			case ".":
				result = CalcInput.KeyPoint;
				break;

			// Invert number sign
			case "\u00B1":
				result = CalcInput.KeyInvertSign;
				break;

			// Add Operation
			case "\u002B":
				result = CalcInput.KeyAdd;
				break;
			// Subract Operation
			case "\u2212":
				result = CalcInput.KeySubtract;
				break;
			// Multiply Operation
			case "\u00D7":
				result = CalcInput.KeyMultiply;
				break;
			// Divide Operation
			case "\u00F7":
				result = CalcInput.KeyDivide;
				break;

			// Memory Functions
			case "MC":
				result = CalcInput.KeyMemoryClear;
				break;
			case "MR":
				result = CalcInput.KeyMemoryRetrieve;
				break;
			case "M+":
				result = CalcInput.KeyMemoryAdd;
				break;
			case "M-":
				result = CalcInput.KeyMemorySubtract;
				break;

			// Clear
			case "C":
				result = CalcInput.KeyClear;
				break;
			// Clear Entry
			case "CE":
				result = CalcInput.KeyClearEntry;
				break;

			// Square Root
			case "\u221A":
				result = CalcInput.KeySquareRoot;
				break;

			// Percent
			case "\u0025":
				result = CalcInput.KeyPercent;
				break;

			// Case Equals
			case "=":
				result = CalcInput.KeyEquals;
				break;

			default:
				throw new ArgumentException(string.Format("\"{0}\" is not an accepted input."));
			}
			
			return result;
		}
	}
}
