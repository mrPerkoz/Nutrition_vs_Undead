﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
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
	/// Logika interakcji dla klasy Undead.xaml
	/// </summary>
	public partial class Undead : UserControl
	{
		public DispatcherTimer TimerZombie = new();			// Timer do ruszania się umarlaków
		public DispatcherTimer TimerSprawdzanie = new();	// Timer do odświerzania 30 razy na sekunde
		private bool Zyje = true;							// Flaga mówiąca czy umarlak żyje
		private int Zycie = 20;								// Wartość życia nieumarlaka

		public Undead(int Rzadek, int Kolumna, int id)
		{
			InitializeComponent();

			Grid.SetRow(this, Rzadek);
			Grid.SetColumn(this, Kolumna);

			switch (id)	// Dobieranie odpowiuednich wartości zależnie od id nieumarlaka:
			{
				case 0:
					TimerZombie.Interval = TimeSpan.FromSeconds(3);
					Umarlak.Visibility = Visibility.Visible;
					UmarlakZKaskiem.Visibility = Visibility.Collapsed;
					Imp.Visibility = Visibility.Collapsed;
					break;
				case 1:
					TimerZombie.Interval = TimeSpan.FromSeconds(1.5);
					Umarlak.Visibility = Visibility.Collapsed;
					UmarlakZKaskiem.Visibility = Visibility.Collapsed;
					Imp.Visibility = Visibility.Visible;
					Zycie = 8;
					break;
				case 2:
					TimerZombie.Interval = TimeSpan.FromSeconds(3);
					Umarlak.Visibility = Visibility.Collapsed;
					UmarlakZKaskiem.Visibility = Visibility.Visible;
					Imp.Visibility = Visibility.Collapsed;
					Zycie = 40;
					break; 
				default:
					TimerZombie.Interval = TimeSpan.FromSeconds(3);
					Umarlak.Visibility = Visibility.Visible;
					UmarlakZKaskiem.Visibility = Visibility.Collapsed;
					Imp.Visibility = Visibility.Collapsed;
					break;
			}

			TimerZombie.Tick += timer_Tick;
			TimerZombie.Start();

			TimerSprawdzanie.Interval = TimeSpan.FromSeconds((double)1 / 30);
			TimerSprawdzanie.Tick += timer_Tick2;
			TimerSprawdzanie.Start(); 
			
		}

		private void timer_Tick(object? sender, EventArgs e)	// Sprawdzanie czy nieumarlak jest w roślince
		{
			bool WRozlince = false;
			if (Grid.GetColumn(this) > 0)
			{
				Grid.SetColumn(this, Grid.GetColumn(this) - 1);
			}
			foreach (var i in ((Grid)Parent).Children.OfType<Nutrition>())
			{
				if (Grid.GetRow(this) == Grid.GetRow(i) && Grid.GetColumn(this) == Grid.GetColumn(i) - 1)
				{
					Grid.SetColumn(this, Grid.GetColumn(this) + 1);
					WRozlince = true;
				}
				if (Grid.GetRow(this) == Grid.GetRow(i) && Grid.GetColumn(this) == Grid.GetColumn(i)) WRozlince = true;
			}
			if (Grid.GetColumn(this) == 0 && !WRozlince)	// Zatrzymanie wszystkich timerów po przegranej
			{
				foreach (var i in ((Grid)Parent).Children.OfType<Undead>())
				{
					i.TimerSprawdzanie.Stop();
					i.TimerZombie.Stop();
				}
                foreach (var i in ((Grid)Parent).Children.OfType<Pocisk>())
                {
                    i.TimerRuchu.Stop();
                }
                foreach (var i in ((Grid)Parent).Children.OfType<Nutrition>())
                {
					i.TimerObrywanie.Stop();
					i.TimerOdswierzanie.Stop();
					i.TimerPocisk.Stop();
					i.TimerSloneczko.Stop();
                }
				MainWindow.TimerOdswierzanie.Stop();
				MainWindow.TimerSloneczka.Stop();
				MainWindow.TimerSpawnowania.Stop();
				MainWindow.TimerUtrudniania.Stop();

                if (MessageBox.Show("Przegrałeś/aś!!!") == MessageBoxResult.OK)
				{
					System.Windows.Application.Current.Shutdown();
				}
			}
		}
		private void timer_Tick2(object? sender, EventArgs e)	// Sprawdzanie czy nieumarlak koliduje z pociskiem
		{
			if (Zyje)
			{
				foreach (var i in ((Grid)Parent).Children.OfType<Pocisk>())
				{
					if (Grid.GetColumn(i) == Grid.GetColumn(this) && Grid.GetRow(i) == Grid.GetRow(this) ||
						Grid.GetColumn(i) == Grid.GetColumn(this) + 1 && Grid.GetRow(i) == Grid.GetRow(this))
					{
						int Obrazenia = 2;
						if ((string)i.Tag == "Mrozon")	// Jeśli pocisk jest od Mrożona
						{
							Obrazenia = 1;
							TimerZombie.Interval = TimeSpan.FromSeconds(3);
						}

						Oberwij(Obrazenia);
						i.ZabijSie();
						break;
					}
				}
			}
			if (Zycie <= 0)	// Jeśli nieumarlak ma umrzeć
			{
				TimerSprawdzanie.Stop();
				TimerZombie.Stop();
				ZabijSie();
			}
		}

        private MediaPlayer mediaPlayer = new();
        private MediaPlayer mediaPlayer2 = new();

        private void Oberwij(int Obrazenia)
		{
			Zycie -= Obrazenia;

            mediaPlayer.Open(new Uri(File.Exists("./audio/dmg.wav") ? "./audio/dmg.wav" : "./../../../audio/dmg.wav", UriKind.Relative));
            mediaPlayer.Play();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void MediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
			mediaPlayer.Close();
        }
        private void MediaPlayer_MediaEnded2(object? sender, EventArgs e)
        {
			mediaPlayer2.Close();
        }

        public void ZabijSie()
		{
			if (Zyje)
			{
                mediaPlayer2.Open(new Uri(File.Exists("./audio/dying.wav") ? "./audio/dying.wav" : "./../../../audio/dying.wav", UriKind.Relative));

                mediaPlayer2.Play();
				mediaPlayer2.MediaEnded += MediaPlayer_MediaEnded2;

                MainWindow.Punkty++;

				Zyje = false;
				((Grid)Parent).Children.Remove(this);
			}
		}
    }
}



//            ⢀⡴⠑⡄⠀⠀⠀⠀⠀⠀⠀⣀⣀⣤⣤⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀ 
//            ⠸⡇⠀⠿⡀⠀⠀⠀⣀⡴⢿⣿⣿⣿⣿⣿⣿⣿⣷⣦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠑⢄⣠⠾⠁⣀⣄⡈⠙⣿⣿⣿⣿⣿⣿⣿⣿⣆⠀⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⢀⡀⠁⠀⠀⠈⠙⠛⠂⠈⣿⣿⣿⣿⣿⠿⡿⢿⣆⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⢀⡾⣁⣀⠀⠴⠂⠙⣗⡀⠀⢻⣿⣿⠭⢤⣴⣦⣤⣹⠀⠀⠀⢀⢴⣶⣆ 
//            ⠀⠀⢀⣾⣿⣿⣿⣷⣮⣽⣾⣿⣥⣴⣿⣿⡿⢂⠔⢚⡿⢿⣿⣦⣴⣾⠁⠸⣼⡿ 
//            ⠀⢀⡞⠁⠙⠻⠿⠟⠉⠀⠛⢹⣿⣿⣿⣿⣿⣌⢤⣼⣿⣾⣿⡟⠉⠀⠀⠀⠀⠀ 
//            ⠀⣾⣷⣶⠇⠀⠀⣤⣄⣀⡀⠈⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀ 
//            ⠀⠉⠈⠉⠀⠀⢦⡈⢻⣿⣿⣿⣶⣶⣶⣶⣤⣽⡹⣿⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⠀⠉⠲⣽⡻⢿⣿⣿⣿⣿⣿⣿⣷⣜⣿⣿⣿⡇⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣷⣶⣮⣭⣽⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⣀⣀⣈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀ 
//            ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠻⠿⠿⠿⠿⠛⠉             
