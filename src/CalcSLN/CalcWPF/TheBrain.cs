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
		private bool		_memInUse;

		// Opperation in use
		private CalcInput	_currentOpp;
		#endregion


		public TheBrain()
		{
			_reg00	= 0d;
			_reg01	= 0d;
			_regM	= 0d;

			_clearReg01OnNumInput	= false;
			_pointUsed				= false;
			_decimalLocation		= 0;

			_memInUse	= false;
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
				{
					_reg01				= 0d;
					_pointUsed			= false;
					_decimalLocation	= 0;
					_clearReg01OnNumInput = false;
				}

				double newValue = (int) input;
				if (_pointUsed)
				{
					_decimalLocation++;
					newValue *= Math.Pow(0.1, _decimalLocation);
				}
				else
				{
					_reg01 *= 10;
				}

				newValue *= Math.Sign(_reg01);
				_reg01 += newValue;

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
				{
					_reg01				= 0d;
					_pointUsed			= false;
					_decimalLocation	= 0;
					_clearReg01OnNumInput = false;
				}

				if (_pointUsed)
					return;

				_pointUsed = true;
				_decimalLocation = 0;
				break;
			
			case CalcInput.KeyInvertSign:
				_reg01 *= -1;
				break;

			case CalcInput.KeyPercent:
				_reg01 = _reg00 * _reg01 / 100d;
				break;

			case CalcInput.KeySquareRoot:
				_reg01 = Math.Sqrt(_reg01);
				break;

			case CalcInput.KeyEquals:
				CommitCurrentOperation();
				_reg01 = _reg00;
				break;

			case CalcInput.KeyClear:
				_reg00 = 0d;
				_reg01 = 0d;

				_pointUsed			= false;
				_decimalLocation	= 0;
				_currentOpp			= CalcInput.KeyAdd;
				_clearReg01OnNumInput = false;
				break;


			case CalcInput.KeyMemoryClear:
				_regM = 0d;
				_memInUse = false;
				break;

			case CalcInput.KeyMemoryRetrieve:
				_reg01 = _regM;
				_memInUse = true;
				break;

			case CalcInput.KeyMemoryAdd:
				_regM += _reg01;
				_memInUse = true;
				break;

			case CalcInput.KeyMemorySubtract:
				_regM -= _reg01;
				_memInUse = true;
				break;

			default:
				throw new InvalidEnumArgumentException();
			}
		}

		private void CommitCurrentOperation()
		{
			switch (_currentOpp)
			{
			case CalcInput.KeyAdd:
				_reg00 += _reg01;
				break;
			case CalcInput.KeySubtract:
				_reg00 -= _reg01;
				break;
			case CalcInput.KeyMultiply:
				_reg00 *= _reg01;
				break;
			case CalcInput.KeyDivide:
				_reg00 /= _reg01;
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
