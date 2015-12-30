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

		private double _reg00;
		private double _reg01;
		private double _regM;


		public TheBrain()
		{
			_reg00	= 0d;
			_reg01	= 0d;
			_regM	= 0d;
		}


		public void NewInput(CalcInput input)
		{
			throw new NotImplementedException();
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
