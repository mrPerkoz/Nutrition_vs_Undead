﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Security.AccessControl;
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
	/// Logika interakcji dla klasy Nutrition.xaml
	/// </summary>
	public partial class Nutrition : UserControl
	{
		public DispatcherTimer TimerPocisk = new();			// Timer do pojawiania pocisków
		public DispatcherTimer TimerSloneczko = new();			// Timer do
		public DispatcherTimer TimerObrywanie = new();			// Timer do
		public DispatcherTimer TimerOdswierzanie = new();		// Timer do
		private int idRosliny = 0;								// Id rośliny
		private bool Zyje = true;								// Flaga mówiąca czy roślina żyje
		private int Zycie = 40;									// Wartość życia rośliny
		public Nutrition(int Rzadek, int Kolumna, int id)
		{
			InitializeComponent();

			TimerPocisk.Interval = TimeSpan.FromSeconds(1);
			TimerPocisk.Tick += TimerPocisk_Tick;

			TimerSloneczko.Interval = TimeSpan.FromSeconds(10);
			TimerSloneczko.Tick += TimerSloneczko_Tick;

			TimerObrywanie.Interval = TimeSpan.FromSeconds(0.5);
			TimerObrywanie.Tick += TimerObrywanie_Tick;

			TimerOdswierzanie.Interval = TimeSpan.FromSeconds((double)1 / 30);
			TimerOdswierzanie.Tick += TimerOdswierzanie_Tick;

			TimerOdswierzanie.Start();

			Grid.SetRow(this, Rzadek);
			Grid.SetColumn(this, Kolumna);

			idRosliny = id;

			switch (id)	// Dobieranie odpowiuednich wartości zależnie od id rośliny:
			{
				case 1:
					TimerPocisk.Start();
					Marchewka.Visibility = Visibility.Visible;
					Slonecznik.Visibility = Visibility.Collapsed;
					Ziemnior.Visibility = Visibility.Collapsed;
					Powtarzajaca.Visibility = Visibility.Collapsed;
					Mrozon.Visibility = Visibility.Collapsed;

					break;
				case 2:
					TimerSloneczko.Start();
					Marchewka.Visibility = Visibility.Collapsed;
					Slonecznik.Visibility = Visibility.Visible;
					Ziemnior.Visibility = Visibility.Collapsed;
					Powtarzajaca.Visibility = Visibility.Collapsed;
					Mrozon.Visibility = Visibility.Collapsed;
					break;
				case 3:
					Zycie = 300;
					Marchewka.Visibility = Visibility.Collapsed;
					Slonecznik.Visibility = Visibility.Collapsed;
					Ziemnior.Visibility = Visibility.Visible;
					Powtarzajaca.Visibility = Visibility.Collapsed;
					Mrozon.Visibility = Visibility.Collapsed;
					break;
				case 4:
					TimerPocisk.Start();
					Marchewka.Visibility = Visibility.Collapsed;
					Slonecznik.Visibility = Visibility.Collapsed;
					Ziemnior.Visibility = Visibility.Collapsed;
					Powtarzajaca.Visibility = Visibility.Visible;
					Mrozon.Visibility = Visibility.Collapsed;
					TimerPocisk.Interval = TimeSpan.FromSeconds(0.75);
					break;
				case 5:
					TimerPocisk.Interval = TimeSpan.FromSeconds(3);
					TimerPocisk.Start();
					Marchewka.Visibility = Visibility.Collapsed;
					Slonecznik.Visibility = Visibility.Collapsed;
					Ziemnior.Visibility = Visibility.Collapsed;
					Powtarzajaca.Visibility = Visibility.Collapsed;
					Mrozon.Visibility = Visibility.Visible;
					break;
				default:
					TimerPocisk.Start();
					Marchewka.Visibility = Visibility.Visible;
					Slonecznik.Visibility = Visibility.Collapsed;
					Ziemnior.Visibility = Visibility.Collapsed;
					Powtarzajaca.Visibility = Visibility.Collapsed;
					Mrozon.Visibility = Visibility.Collapsed;
					break;
			}
		}


		private void TimerOdswierzanie_Tick(object? sender, EventArgs e)	// Sprawdzanie czy roślina jest w nieumarlaku
		{
			bool ZombieW = false;
			if (Zyje)
			{
				foreach (var i in ((Grid)Parent).Children.OfType<Undead>())
				{
					if (Grid.GetColumn(i) == Grid.GetColumn(this) && Grid.GetRow(i) == Grid.GetRow(this))	// Jeśli tak, rozpocznij timer do obrywania
					{
						ZombieW = true;
						TimerObrywanie.Start();
					}
				}
				if (!ZombieW && TimerObrywanie.IsEnabled)
				{
					TimerObrywanie.Stop();
				}
			}
			if (Zycie <= 0)
			{
				ZabijSie();
			}
		}

		private void TimerObrywanie_Tick(object? sender, EventArgs e)
		{
			Zycie -= 5;
		}

		private void TimerSloneczko_Tick(object? sender, EventArgs e)
		{
			MainWindow.Sloneczka += 25;
		}

		private void TimerPocisk_Tick(object? sender, EventArgs e)	// Dodawanie pocisków
		{	
			bool wLini = false;

			foreach (var i in ((Grid)Parent).Children.OfType<Undead>())	// Sprawdzanie czy nieumarlak jest w tej samej lini co roślina
			{
				if (Grid.GetRow(this) == Grid.GetRow(i))
				{
					wLini = true;
				}
			}

			if (Parent != null && wLini)
			{
				Pocisk pocisk = new();

				if (idRosliny == 5)
				{
					pocisk.Tag = "Mrozon";
					pocisk.MarchewaBoulet.Visibility = Visibility.Collapsed;
					pocisk.MrozonBoulet.Visibility = Visibility.Visible;
				}
				Grid.SetColumn(pocisk, Grid.GetColumn(this));
				Grid.SetRow(pocisk, Grid.GetRow(this));
				((Grid)Parent).Children.Add(pocisk);
				if (idRosliny == 4)
				{
					if (TimerPocisk.Interval == TimeSpan.FromSeconds(0.3))
					{
						TimerPocisk.Interval = TimeSpan.FromSeconds(0.75);
					}
					else if (TimerPocisk.Interval == TimeSpan.FromSeconds(0.75))
					{
						TimerPocisk.Interval = TimeSpan.FromSeconds(0.3);
					}
				}
                mediaPlayer.Open(new Uri(File.Exists("./audio/shot.wav") ? "./audio/shot.wav" : "./../../../audio/shot.wav", UriKind.Relative));

                mediaPlayer.Play();
                mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            }
        }

        MediaPlayer mediaPlayer = new();
        private void MediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            mediaPlayer.Close();
        }

        private void ZabijSie()
		{
			if (Zyje)
			{
				TimerPocisk.Stop();
				TimerSloneczko.Stop();
				TimerOdswierzanie.Stop();
				TimerObrywanie.Stop();

				PrzyciskKratki NowyPrzycisk = new();
				Grid.SetColumn(NowyPrzycisk, Grid.GetColumn(this));
				Grid.SetRow(NowyPrzycisk, Grid.GetRow(this));
				((Grid)Parent).Children.Add(NowyPrzycisk);

				((Grid)Parent).Children.Remove(this);
				Zyje = false;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)	// Działanie łopaty
		{
			if (MainWindow.WybranaRoslina == 0)
			{
				ZabijSie();
				MainWindow.WybranaRoslina = -1;
			}
		}
	}
}


//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣤⣤⣤⣀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣼⠟⠉⠉⠉⠉⠉⠉⠉⠙⠻⢶⣄⠀⠀⠀⠀⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⣷⡀⠀⠀⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⡟⠀⣠⣶⠛⠛⠛⠛⠛⠛⠳⣦⡀⠀⠘⣿⡄⠀⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⠁⠀⢹⣿⣦⣀⣀⣀⣀⣀⣠⣼⡇⠀⠀⠸⣷⠀⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⡏⠀⠀⠀⠉⠛⠿⠿⠿⠿⠛⠋⠁⠀⠀⠀⠀⣿⡄⣠
//             ⠀⠀⢀⣀⣀⣀⠀⠀⢠⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡇⠀
//             ⠿⠿⠟⠛⠛⠉⠀⠀⣸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀
//             ⠀⠀⠀⠀⠀⠀⠀⠀⣿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣧⠀
//             ⠀⠀⠀⠀⠀⠀⠀⢸⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⠀
//             ⠀⠀⠀⠀⠀⠀⠀⣾⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀
//             ⠀⠀⠀⠀⠀⠀⠀⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀
//             ⠀⠀⠀⠀⠀⠀⢰⣿⠀⠀⠀⠀⣠⡶⠶⠿⠿⠿⠿⢷⣦⠀⠀⠀⠀⠀⠀⠀⣿⠀
//             ⠀⠀⣀⣀⣀⠀⣸⡇⠀⠀⠀⠀⣿⡀⠀⠀⠀⠀⠀⠀⣿⡇⠀⠀⠀⠀⠀⠀⣿⠀
//             ⣠⡿⠛⠛⠛⠛⠻⠀⠀⠀⠀⠀⢸⣇⠀⠀⠀⠀⠀⠀⣿⠇⠀⠀⠀⠀⠀⠀⣿⠀
//             ⢻⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣼⡟⠀⠀⢀⣤⣤⣴⣿⠀⠀⠀⠀⠀⠀⠀⣿⠀
//             ⠈⠙⢷⣶⣦⣤⣤⣤⣴⣶⣾⠿⠛⠁⢀⣶⡟⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡟⠀
//             ⢷⣶⣤⣀⠉⠉⠉⠉⠉⠄⠀⠀⠀⠀⠈⣿⣆⡀⠀⠀⠀⠀⠀⠀⢀⣠⣴⡾⠃⠀
//             ⠀⠈⠉⠛⠿⣶⣦⣄⣀⠀⠀⠀⠀⠀⠀⠈⠛⠻⢿⣿⣾⣿⡿⠿⠟⠋⠁⠀⠀⠀