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
		private double		_reg00;
		private double		_reg01;
		private double		_regM;

		// Register 01 meta data
		private bool		_clearReg01OnNumInput;
		private bool		_pointUsed;
		private int			_decimalLocation;

		// Register M meta data
		private bool		_memoryInUse;

		// Opperation in use
		private CalcInput	_currentOpp;
		#endregion

		#region Properties
		public double Reg01
		{
			get { return _reg01; }
			private set
			{
				if (_reg01 != value)
				{
					_reg01 = value;
					OnPropertyChanged("Reg01");
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


		public TheBrain()
		{
			_reg00	= 0d;
			_regM	= 0d;

			StartNewNumber();

			MemoryInUse = false;
			_currentOpp = CalcInput.KeyAdd;
		}


		public void NewInput(CalcInput input)
		{
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

				double newValue = (int) input;
				if (_pointUsed)
				{
					_decimalLocation++;
					newValue *= Math.Pow(0.1, _decimalLocation);
				}
				else
				{
					Reg01 *= 10;
				}

				newValue *= Math.Sign(Reg01);
				Reg01 += newValue;

				break;


			case CalcInput.KeyAdd:
			case CalcInput.KeySubtract:
			case CalcInput.KeyMultiply:
			case CalcInput.KeyDivide:
				CommitCurrentOperation();
				_currentOpp = input;
				break;


			case CalcInput.KeyPoint:
				// If starting new number
				if (_clearReg01OnNumInput)
					StartNewNumber();
				if (_pointUsed)
					return;

				_pointUsed = true;
				_decimalLocation = 0;
				break;
			
			case CalcInput.KeyInvertSign:
				Reg01 *= -1;
				break;

			case CalcInput.KeyPercent:
				Reg01 = _reg00 * Reg01 / 100d;
				break;

			case CalcInput.KeySquareRoot:
				Reg01 = Math.Sqrt(Reg01);
				break;

			case CalcInput.KeyEquals:
				CommitCurrentOperation();
				Reg01 = _reg00;
				break;

			case CalcInput.KeyClear:
				_reg00		= 0d;
				StartNewNumber();
				_currentOpp = CalcInput.KeyAdd;
				break;


			case CalcInput.KeyMemoryClear:
				_regM = 0d;
				MemoryInUse = false;
				break;

			case CalcInput.KeyMemoryRetrieve:
				Reg01 = _regM;
				MemoryInUse = true;
				break;

			case CalcInput.KeyMemoryAdd:
				_regM += Reg01;
				MemoryInUse = true;
				break;

			case CalcInput.KeyMemorySubtract:
				_regM -= Reg01;
				MemoryInUse = true;
				break;

			default:
				throw new InvalidEnumArgumentException();
			}
		}

		private void StartNewNumber()
		{
			Reg01				= 0d;
			_pointUsed			= false;
			_decimalLocation	= 0;
			_clearReg01OnNumInput = false;
		}

		private void CommitCurrentOperation()
		{
			switch (_currentOpp)
			{
			case CalcInput.KeyAdd:
				_reg00 += Reg01;
				break;
			case CalcInput.KeySubtract:
				_reg00 -= Reg01;
				break;
			case CalcInput.KeyMultiply:
				_reg00 *= Reg01;
				break;
			case CalcInput.KeyDivide:
				_reg00 /= Reg01;
				break;
			default:
				break;
			}
			_clearReg01OnNumInput = true;
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
