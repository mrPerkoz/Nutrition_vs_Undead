using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nutrition_vs_Undead
{
	/// <summary>
	/// Logika interakcji dla klasy MainMenu.xaml
	/// </summary>
	public partial class MainMenu : Window
	{
		public SoundPlayer soundPlayer = new("./../../../audio/sun.wav");
		public MainMenu()
		{
			InitializeComponent();

			soundPlayer.Load();

			soundPlayer.PlayLooping();
		}

		private void PrzyciskStart_Click(object sender, RoutedEventArgs e)
		{
			new MainWindow().Show();
			soundPlayer.Stop();
			Close();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("AB - kod\nMM - grafika\nTN - ekran startowy, prezentacja\nTD - dźwięki, prezentacja", "Twórcy:"); 
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
