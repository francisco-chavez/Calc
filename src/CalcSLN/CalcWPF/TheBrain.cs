using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unv.CalcWPF
{
	public class TheBrain
		: INotifyPropertyChanged
	{
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion


		#region Attributes & Properties
		// Registers
		private string		_reg00;
		private string		_reg01;
		private string		_regM;

		// Flags
		private bool		_error;
		private bool		_disReg01;
		private bool		_clearReg01OnNumInput;


		private CalcInput CurrentOpp
		{
			get { return n_currentOpp; }
			set
			{
				switch (value)
				{
				case CalcInput.KeyAdd:
				case CalcInput.KeySubtract:
				case CalcInput.KeyMultiply:
				case CalcInput.KeyDivide:
					n_currentOpp = value;
					break;

				default:
					throw new InvalidEnumArgumentException(string.Format("\"{0}\" is an invalid operation.", value));
				}
			}
		}
		private CalcInput	n_currentOpp;

		public string EntryDisplay
		{
			get { return n_entryDisplay; }
			set
			{
				if (n_entryDisplay != value)
				{
					n_entryDisplay = value;
					OnPropertyChanged("EntryDisplay");
				}
			}
		}
		private string n_entryDisplay;

		public bool MemoryInUse
		{
			get { return n_memoryInUse; }
			private set
			{
				if (n_memoryInUse != value)
				{
					n_memoryInUse = value;
					OnPropertyChanged("MemoryInUse");
				}
			}
		}
		private bool n_memoryInUse;

		#endregion


		#region Construtors
		public TheBrain()
		{
			_reg00		= "0";
			_regM		= "0";
			_error		= false;
			_disReg01	= true;

			StartNewNumber();

			MemoryInUse = false;
			CurrentOpp = CalcInput.KeyAdd;
			UpdateDisplay();
		}
		#endregion


		#region Methods
		public void NewInput(CalcInput input)
		{
			decimal reg00	= Convert.ToDecimal(_reg00);
			decimal reg01	= Convert.ToDecimal(_reg01);
			decimal regM	= Convert.ToDecimal(_regM);

			if(_error)
			switch (input)
			{
			case CalcInput.KeyClear:
				_error = false;
				break;
			case CalcInput.KeyMemoryClear:
				break;
			default:
				return;
			}

			switch (input)
			{
			case CalcInput.NumKey0:
			case CalcInput.NumKey1:
			case CalcInput.NumKey2:
			case CalcInput.NumKey3:

			case CalcInput.NumKey4:
			case CalcInput.NumKey5:
			case CalcInput.NumKey6:
			case CalcInput.NumKey7:

			case CalcInput.NumKey8:
			case CalcInput.NumKey9:
				if (_disReg01)
				{

				}

				// If starting new number
				if (_clearReg01OnNumInput)
					StartNewNumber();

				if (_reg01 == "0")
				{
					if (input == CalcInput.NumKey0)
						break;
					_reg01 = "";
				}

				_reg01 += ((int) input).ToString();

				break;


			case CalcInput.KeyAdd:
			case CalcInput.KeySubtract:
			case CalcInput.KeyMultiply:
			case CalcInput.KeyDivide:
				if (_disReg01)
					CommitCurrentOperation(reg00, reg01);
				else
					StartNewNumber();
				CurrentOpp = input;
				_disReg01 = true;
				break;


			case CalcInput.KeyPoint:
				// If starting new number
				if (_clearReg01OnNumInput)
					StartNewNumber();

				if (_reg01.Contains('.'))
					return;
				_reg01 += ".";
				break;
			
			case CalcInput.KeyInvertSign:
				// Is the value 0
				var zeroCheck = new char[] { '-', '.', '0' };
				if (_reg01.All(c => { return zeroCheck.Contains(c); }))
					return;

				// Not 0, keep going
				if (_reg01.StartsWith("-"))
					_reg01 = _reg01.Substring(1);
				else
					_reg01 = "-" + _reg01;
				break;

			case CalcInput.KeyPercent:
				if (!_disReg01)
					RaiseError();
				else
					_reg01 = (reg00 * reg01 / 100M).ToString();
				break;

			case CalcInput.KeySquareRoot:
				if (reg01 < 0M)
					RaiseError();
				else
					_reg01 = Math.Sqrt((double) reg01).ToString();
				break;

			case CalcInput.KeyEquals:
				CommitCurrentOperation(reg00, reg01);
				_disReg01 = false;
				break;

			case CalcInput.KeyClear:
				_disReg01 = true;
				_reg00 = "0";
				StartNewNumber();
				CurrentOpp = CalcInput.KeyAdd;
				break;

			case CalcInput.KeyClearEntry:
				_disReg01 = true;
				StartNewNumber();
				break;


			case CalcInput.KeyMemoryClear:
				_regM = "0";
				MemoryInUse = false;
				break;

			case CalcInput.KeyMemoryRetrieve:
				_disReg01 = true;
				_reg01 = _regM;
				MemoryInUse = true;
				break;

			case CalcInput.KeyMemoryAdd:
				_regM = (regM + reg01).ToString();
				MemoryInUse = true;
				break;

			case CalcInput.KeyMemorySubtract:
				_regM = (regM - reg01).ToString();
				MemoryInUse = true;
				break;

			default:
				throw new InvalidEnumArgumentException();
			}
			UpdateDisplay();
		}

		private void Clear()
		{
			this._clearReg01OnNumInput = false;
			this._disReg01		= true;
			this._error			= false;
			this._reg00			= "0";
			this._reg01			= "0";
			this.CurrentOpp		= CalcInput.KeyAdd;
			this.EntryDisplay	= "0";
		}

		private void RaiseError()
		{
			_reg00 = "0";
			_reg01 = "0";
			_error = true;
		}

		private void UpdateDisplay()
		{
			if (_error)
			{
				EntryDisplay = "Error!";
				return;
			}

			string[] subStrings = _reg01.Split(new char[] { '.' });
			string formatString = "{0:N0}";
			if (subStrings.Length > 1)
				formatString = "{0:N" + subStrings[1].Length.ToString() + "}";

			try
			{
				EntryDisplay = string.Format(formatString, Convert.ToDecimal(_disReg01 ? _reg01 : _reg00));
			}
			catch (OverflowException)
			{
				RaiseError();
				UpdateDisplay();
			}
		}

		private void StartNewNumber()
		{
			_reg01 = "0";
			_clearReg01OnNumInput = false;
		}

		private void CommitCurrentOperation(decimal value1, decimal value2)
		{
			try
			{
				switch (CurrentOpp)
				{
				case CalcInput.KeyAdd:
					_reg00 = (value1 + value2).ToString();
					break;
				case CalcInput.KeySubtract:
					_reg00 = (value1 - value2).ToString();
					break;
				case CalcInput.KeyMultiply:
					_reg00 = (value1 * value2).ToString();
					break;
				case CalcInput.KeyDivide:
					_reg00 = (value1 / value2).ToString();
					break;
				default:
					break;
				}
				_clearReg01OnNumInput = true;
			}
			catch (DivideByZeroException)
			{
				RaiseError();
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
