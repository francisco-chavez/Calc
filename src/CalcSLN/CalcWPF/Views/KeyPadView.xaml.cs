﻿using System;
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
		}
	}
}
