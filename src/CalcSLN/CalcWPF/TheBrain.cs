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


		#region Attributes
		// Registers
		private string		_reg00;
		private string		_reg01;
		private string		_regM;

		// Register 01 meta data
		private bool		_clearReg01OnNumInput;
		private string		_entryDisplay;

		// Register M meta data
		private bool		_memoryInUse;

		// Opperation in use
		private CalcInput	_currentOpp;

		private bool		_error;
		#endregion


		#region Properties
		public string EntryDisplay
		{
			get { return _entryDisplay; }
			set
			{
				if (_entryDisplay != value)
				{
					_entryDisplay = value;
					OnPropertyChanged("EntryDisplay");
				}
			}
		}

		public bool MemoryInUse
		{
			get { return _memoryInUse; }
			private set
			{
				if (_memoryInUse != value)
				{
					_memoryInUse = value;
					OnPropertyChanged("MemoryInUse");
				}
			}
		}
		#endregion


		#region Construtors
		public TheBrain()
		{
			_reg00	= "0";
			_regM	= "0";
			_error	= false;

			StartNewNumber();

			MemoryInUse = false;
			_currentOpp = CalcInput.KeyAdd;
			UpdateDisplay();
		}
		#endregion


		public void NewInput(CalcInput input)
		{
			decimal reg00	= Convert.ToDecimal(_reg00);
			decimal reg01	= Convert.ToDecimal(_reg01);
			decimal regM	= Convert.ToDecimal(_regM);
			bool	useReg0 = false;

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
				CommitCurrentOperation(reg00, reg01);
				_currentOpp = input;
				useReg0 = true;
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
				_reg01 = _reg00;
				break;

			case CalcInput.KeyClear:
				_reg00 = "0";
				StartNewNumber();
				_currentOpp = CalcInput.KeyAdd;
				break;

			case CalcInput.KeyClearEntry:
				StartNewNumber();
				break;


			case CalcInput.KeyMemoryClear:
				_regM = "0";
				MemoryInUse = false;
				break;

			case CalcInput.KeyMemoryRetrieve:

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
			UpdateDisplay(useReg0);
		}

		private void RaiseError()
		{
			_reg00 = "0";
			_reg01 = "0";
			_error = true;
		}

		private void UpdateDisplay(bool useReg0 = false)
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
				EntryDisplay = string.Format(formatString, Convert.ToDecimal(useReg0 ? _reg00 : _reg01));
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
				switch (_currentOpp)
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
	}
}
