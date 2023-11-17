using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Nutrition_vs_Undead
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static int WybranaRoslina = -1;
		public static int Sloneczka = 100;
		public static int[] CenyRoslin = { 0, 100, 50, 50, 300, 225 };
		public static int Punkty = 0;

		public static DispatcherTimer TimerOdswierzanie = new();
		public static DispatcherTimer TimerSloneczka = new();
		public static DispatcherTimer TimerSpawnowania = new();
		public static DispatcherTimer TimerUtrudniania = new();
		public static DispatcherTimer TimerPrzedSpawnowaniem = new();

		private int PoziomTrudnosci = 0;

		public MainWindow()
		{
			InitializeComponent();

			IloscSloneczek.Text = Convert.ToString(Sloneczka);

			TimerOdswierzanie.Interval = TimeSpan.FromSeconds((double)1 / 30);
			TimerOdswierzanie.Tick += TimerOdswierzanie_Tick;

			TimerOdswierzanie.Start();

			TimerSloneczka.Interval = TimeSpan.FromSeconds(5);
			TimerSloneczka.Tick += TimerSloneczka_Tick;

			TimerSloneczka.Start();

			TimerSpawnowania.Interval = TimeSpan.FromSeconds(5);
			TimerSpawnowania.Tick += TimerSpawnowania_Tick;



			TimerUtrudniania.Interval = TimeSpan.FromSeconds(15);
			TimerUtrudniania.Tick += TimerUtrudniania_Tick;



			TimerPrzedSpawnowaniem.Interval = TimeSpan.FromSeconds(8);
			TimerPrzedSpawnowaniem.Tick += TimerPrzedSpawnowaniem_Tick;

			TimerPrzedSpawnowaniem.Start();

			for (int i = 0;  i < 9; i++) 
			{
				for (int j = 0; j < 5; j++)
				{
					PrzyciskKratki Guzik = new();
					Grid.SetColumn(Guzik, i);
					Grid.SetRow(Guzik, j);

					Panel.SetZIndex(this, 0);

					Plansza.Children.Add(Guzik);
				}
			}




			//Plansza.Children.Add(new PrzyciskKratki());

			//Pocisk pocisk = new(0);

			//Grid.SetRow(pocisk,1);

			//Plansza.Children.Add(new Nutrition(1, 1, 0));
			//Plansza.Children.Add(new Undead(1, 8));
			//Plansza.Children.Add(pocisk);
		}

		private void TimerPrzedSpawnowaniem_Tick(object? sender, EventArgs e)
		{
			TimerPrzedSpawnowaniem.Stop();
			TimerUtrudniania.Start();

			TimerSpawnowania.Start();
		}

		private void TimerUtrudniania_Tick(object? sender, EventArgs e)
		{
			PoziomTrudnosci += 5;
			TimerSpawnowania.Interval = TimeSpan.FromSeconds(((1 / ((double)PoziomTrudnosci + 10)) * 50) * 2);
		}

        private void TimerSpawnowania_Tick(object? sender, EventArgs e)
        {
            Random random = new();

            if (PoziomTrudnosci > 30)
            {
                Plansza.Children.Add(new Undead(random.Next(5), 8, random.Next(2)));
            }
            if (PoziomTrudnosci > 20)
            {
                Plansza.Children.Add(new Undead(random.Next(5), 8, random.Next(1)));
            }
            else
            {
                Plansza.Children.Add(new Undead(random.Next(5), 8, 0));
            }
        }

        private void TimerSloneczka_Tick(object? sender, EventArgs e)
		{
			Sloneczka += 25;
		}

		private void TimerOdswierzanie_Tick(object? sender, EventArgs e)
		{
			IloscSloneczek.Text = Convert.ToString(Sloneczka);
			IloscPunktow.Text = "Punkty: " + Convert.ToString(Punkty);
		}

		private void Lopata_Click(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 0;
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 1;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 2;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 3;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 4;
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			WybranaRoslina = 5;
		}
	}
}




//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠛⠛⠛⠋⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠙⠛⠛⠛⠿⠻⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⠀⠀⡀⠠⠤⠒⢂⣉⣉⣉⣑⣒⣒⠒⠒⠒⠒⠒⠒⠒⠀⠀⠐⠒⠚⠻⠿⠿⣿⣿⣿⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⠏⠀⠀⠀⠀⡠⠔⠉⣀⠔⠒⠉⣀⣀⠀⠀⠀⣀⡀⠈⠉⠑⠒⠒⠒⠒⠒⠈⠉⠉⠉⠁⠂⠀⠈⠙⢿⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠔⠁⠠⠖⠡⠔⠊⠀⠀⠀⠀⠀⠀⠀⠐⡄⠀⠀⠀⠀⠀⠀⡄⠀⠀⠀⠀⠉⠲⢄⠀⠀⠀⠈⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⠋⠀⠀⠀⠀⠀⠀⠀⠊⠀⢀⣀⣤⣤⣤⣤⣀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠜⠀⠀⠀⠀⣀⡀⠀⠈⠃⠀⠀⠀⠸⣿⣿⣿⣿
//⣿⣿⣿⣿⡿⠥⠐⠂⠀⠀⠀⠀⡄⠀⠰⢺⣿⣿⣿⣿⣿⣟⠀⠈⠐⢤⠀⠀⠀⠀⠀⠀⢀⣠⣶⣾⣯⠀⠀⠉⠂⠀⠠⠤⢄⣀⠙⢿⣿⣿
//⣿⡿⠋⠡⠐⠈⣉⠭⠤⠤⢄⡀⠈⠀⠈⠁⠉⠁⡠⠀⠀⠀⠉⠐⠠⠔⠀⠀⠀⠀⠀⠲⣿⠿⠛⠛⠓⠒⠂⠀⠀⠀⠀⠀⠀⠠⡉⢢⠙⣿
//⣿⠀⢀⠁⠀⠊⠀⠀⠀⠀⠀⠈⠁⠒⠂⠀⠒⠊⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⢀⣀⡠⠔⠒⠒⠂⠀⠈⠀⡇⣿
//⣿⠀⢸⠀⠀⠀⢀⣀⡠⠋⠓⠤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠄⠀⠀⠀⠀⠀⠀⠈⠢⠤⡀⠀⠀⠀⠀⠀⠀⢠⠀⠀⠀⡠⠀⡇⣿
//⣿⡀⠘⠀⠀⠀⠀⠀⠘⡄⠀⠀⠀⠈⠑⡦⢄⣀⠀⠀⠐⠒⠁⢸⠀⠀⠠⠒⠄⠀⠀⠀⠀⠀⢀⠇⠀⣀⡀⠀⠀⢀⢾⡆⠀⠈⡀⠎⣸⣿
//⣿⣿⣄⡈⠢⠀⠀⠀⠀⠘⣶⣄⡀⠀⠀⡇⠀⠀⠈⠉⠒⠢⡤⣀⡀⠀⠀⠀⠀⠀⠐⠦⠤⠒⠁⠀⠀⠀⠀⣀⢴⠁⠀⢷⠀⠀⠀⢰⣿⣿
//⣿⣿⣿⣿⣇⠂⠀⠀⠀⠀⠈⢂⠀⠈⠹⡧⣀⠀⠀⠀⠀⠀⡇⠀⠀⠉⠉⠉⢱⠒⠒⠒⠒⢖⠒⠒⠂⠙⠏⠀⠘⡀⠀⢸⠀⠀⠀⣿⣿⣿
//⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠑⠄⠰⠀⠀⠁⠐⠲⣤⣴⣄⡀⠀⠀⠀⠀⢸⠀⠀⠀⠀⢸⠀⠀⠀⠀⢠⠀⣠⣷⣶⣿⠀⠀⢰⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠀⠁⢀⠀⠀⠀⠀⠀⡙⠋⠙⠓⠲⢤⣤⣷⣤⣤⣤⣤⣾⣦⣤⣤⣶⣿⣿⣿⣿⡟⢹⠀⠀⢸⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣧⡀⠀⠀⠀⠀⠀⠀⠀⠑⠀⢄⠀⡰⠁⠀⠀⠀⠀⠀⠈⠉⠁⠈⠉⠻⠋⠉⠛⢛⠉⠉⢹⠁⢀⢇⠎⠀⠀⢸⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⣀⠈⠢⢄⡉⠂⠄⡀⠀⠈⠒⠢⠄⠀⢀⣀⣀⣰⠀⠀⠀⠀⠀⠀⠀⠀⡀⠀⢀⣎⠀⠼⠊⠀⠀⠀⠘⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣄⡀⠉⠢⢄⡈⠑⠢⢄⡀⠀⠀⠀⠀⠀⠀⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠁⠀⠀⢀⠀⠀⠀⠀⠀⢻⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣀⡈⠑⠢⢄⡀⠈⠑⠒⠤⠄⣀⣀⠀⠉⠉⠉⠉⠀⠀⠀⣀⡀⠤⠂⠁⠀⢀⠆⠀⠀⢸⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣄⡀⠁⠉⠒⠂⠤⠤⣀⣀⣉⡉⠉⠉⠉⠉⢀⣀⣀⡠⠤⠒⠈⠀⠀⠀⠀⣸⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣶⣤⣄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣶⣶⣶⣤⣤⣤⣤⣀⣀⣤⣤⣤⣶⣾⣿⣿⣿⣿⣿