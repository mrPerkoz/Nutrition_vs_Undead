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

namespace Nutrition_vs_Undead
{
	/// <summary>
	/// Logika interakcji dla klasy PrzyciskKratki.xaml
	/// </summary>
	public partial class PrzyciskKratki : UserControl	// Niewidzialny przycisk na każdej kratce planszy bez rośliny umożliwiający sadzenie ich
	{
		public PrzyciskKratki()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)	// Przy kliknięciu sprawdza czy:
		{
			if (MainWindow.WybranaRoslina > 0 &&											// Wybrana jest roślina
				MainWindow.WybranaRoslina <= MainWindow.CenyRoslin.Count() - 1 &&			// Czy wybrana roślina nie wychodzi z zakresu
				MainWindow.Sloneczka >= MainWindow.CenyRoslin[MainWindow.WybranaRoslina])	// Czy stać gracza na nią
			{
				((Grid)Parent).Children.Add(new Nutrition(Grid.GetRow(this), Grid.GetColumn(this), MainWindow.WybranaRoslina));		// Pojawia roślinę z wybranym id
				((Grid)Parent).Children.Remove(this);
				MainWindow.Sloneczka -= MainWindow.CenyRoslin[MainWindow.WybranaRoslina];

				MainWindow.WybranaRoslina = -1;
			}
		}
	}
}
