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
		public static int WybranaRoslina = -1;										// Id wybranej rośliny (-1 - brak wybranej rośliny, 0 - łopata)
		public static int Sloneczka = 100;											// Ilość słoneczek
		public readonly static int[] CenyRoslin = { 0, 100, 50, 50, 300, 225 };     // Tablica do wglądu cen roślin
		public static int Punkty = 0;												// Ilość punktów

		public static DispatcherTimer TimerOdswierzanie = new();                    // Timer do odświerzania 30 razy na sekundę
		public static DispatcherTimer TimerSloneczka = new();                       // Timer do dodawania słoneczek
		public static DispatcherTimer TimerSpawnowania = new();                     // Timer do pojawiania nieumarlaków
		public static DispatcherTimer TimerUtrudniania = new();                     // Timer do stopniowego zmieniania poziomu trudności
		public static DispatcherTimer TimerPrzedSpawnowaniem = new();               // Timer do odczekania przed zaczęciem pojawiania nieumarlaków

		private int PoziomTrudnosci = 0;                                            // Wartość poziomu trudności

		public MainWindow()
		{
			InitializeComponent();

			IloscSloneczek.Text = Convert.ToString(Sloneczka);

			// Ustalanie interwału i rozpoczęcie timerów:

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

			// Stworzenie przycisków służących do stawiania roślinek
			// na całym gridzie:

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
		}

		private void TimerPrzedSpawnowaniem_Tick(object? sender, EventArgs e)
		{
			TimerPrzedSpawnowaniem.Stop();
			TimerUtrudniania.Start();

			TimerSpawnowania.Start();
		}

		private void TimerUtrudniania_Tick(object? sender, EventArgs e)	// Rośnięcie częstotliwości pojawiania nieumarlaków
		{
			PoziomTrudnosci += 5;
			TimerSpawnowania.Interval = TimeSpan.FromSeconds(((1 / ((double)PoziomTrudnosci + 10)) * 50) * 2);
		}

		private void TimerSpawnowania_Tick(object? sender, EventArgs e)
		{
			Random random = new();

			if (PoziomTrudnosci > 30)   // Szansa na pojawienie nieumarlaka z kaskiem, gdy poziom trudności jest większy od 30
			{
				Plansza.Children.Add(new Undead(random.Next(5), 8, random.Next(3)));
			}
			else if (PoziomTrudnosci > 20)   // Szansa na pojawienie impa, gdy poziom trudności jest większy od 20
			{
				Plansza.Children.Add(new Undead(random.Next(5), 8, random.Next(2)));
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

		private void TimerOdswierzanie_Tick(object? sender, EventArgs e)	// Aktualizowanie ilości słoneczek i punktów co 1/30 sekundy
		{
			IloscSloneczek.Text = Convert.ToString(Sloneczka);
			IloscPunktow.Text = "Punkty: " + Convert.ToString(Punkty);
		}

		// Przyciski do wybierania roślin:

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