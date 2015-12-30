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
		public event PropertyChangedEventHandler PropertyChanged;

		private double	_reg00;
		private double	_reg01;
		private double	_regM;
		private bool	_clearReg01OnNumInput;
		private bool	_pointUsed;


		public TheBrain()
		{
			_reg00	= 0d;
			_reg01	= 0d;
			_regM	= 0d;
			_clearReg01OnNumInput = false;
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

				if (_clearReg01OnNumInput)
				{
					_reg01 = 0d;
					_clearReg01OnNumInput = false;
				}

				if (!_pointUsed)
				{
					_reg01 *= 10;
					_reg01 += Math.Sign(_reg01) * ((int) input);
				}
				else
				{
					throw new NotImplementedException();
				}
				break;

			case CalcInput.KeyAdd:
				_reg00 += _reg01;
				_clearReg01OnNumInput = true;
				break;
			case CalcInput.KeySubtract:
				_reg00 -= _reg01;
				_clearReg01OnNumInput = true;
				break;
			case CalcInput.KeyMultiply:
				_reg00 *= _reg01;
				_clearReg01OnNumInput = true;
				break;
			case CalcInput.KeyDivide:
				_reg00 /= _reg01;
				_clearReg01OnNumInput = true;
				break;

			case CalcInput.KeyPoint:
				_pointUsed = true;
				break;
			case CalcInput.KeyInvertSign:
				_reg01 *= -1;
				break;

			case CalcInput.KeyPercent:
			case CalcInput.KeySquareRoot:
			case CalcInput.KeyEquals:
			case CalcInput.KeyClear:
			case CalcInput.KeyMemoryClear:
			case CalcInput.KeyMemoryRetrieve:
			case CalcInput.KeyMemoryAdd:
			case CalcInput.KeyMemorySubtract:
				throw new NotImplementedException();
				break;

			default:
				throw new InvalidEnumArgumentException();
			}
			throw new NotImplementedException();
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
