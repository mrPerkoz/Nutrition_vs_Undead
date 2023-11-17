using System;
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
		public Undead()
		{
            InitializeComponent();

            TimerZombie.Interval = TimeSpan.FromSeconds(3);
            TimerZombie.Tick += timer_Tick;
            TimerZombie.Start();

            TimerSprawdzanie.Interval = TimeSpan.FromSeconds((double)1 / 30);
            TimerSprawdzanie.Tick += timer_Tick2;
            TimerSprawdzanie.Start();

            TimerSpowolnienie.Interval = TimeSpan.FromSeconds(10);
            TimerSpowolnienie.Tick += TimerSpowolnienie_Tick;
        }



		public DispatcherTimer TimerZombie = new();
		public DispatcherTimer TimerSprawdzanie = new();
		public DispatcherTimer TimerSpowolnienie = new();
		private bool Zyje = true;
		private int Zycie = 20;

		public Undead(int Rzadek, int Kolumna, int id)
		{
			InitializeComponent();

			Grid.SetRow(this, Rzadek);
			Grid.SetColumn(this, Kolumna);

			switch (id)
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
                    UmarlakZKaskiem.Visibility = Visibility.Visible;
                    Imp.Visibility = Visibility.Collapsed;
                    Zycie = 8;
                    break; 
				case 2:
                    TimerZombie.Interval = TimeSpan.FromSeconds(3);
                    Umarlak.Visibility = Visibility.Collapsed;
                    UmarlakZKaskiem.Visibility = Visibility.Collapsed;
                    Imp.Visibility = Visibility.Visible;
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
			
			TimerSpowolnienie.Interval = TimeSpan.FromSeconds(3);
			TimerSpowolnienie.Tick += TimerSpowolnienie_Tick;
		}

		private void TimerSpowolnienie_Tick(object? sender, EventArgs e)
		{
            TimerZombie.Interval = TimeSpan.FromSeconds(3);
			TimerSpowolnienie.Stop();
		}

		private void timer_Tick(object? sender, EventArgs e)
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
			if (Grid.GetColumn(this) == 0 && !WRozlince)
			{

				if (MessageBox.Show("Przegrałeś/aś!!!") == MessageBoxResult.OK)
				{
					System.Windows.Application.Current.Shutdown();
				}
				//Kys();

			}
		}
		private void timer_Tick2(object? sender, EventArgs e)
		{
			//if (MainWindow.PozycjePociskow.Contains(Grid.GetColumn(this)) && MainWindow.PozycjePociskow[MainWindow.PozycjePociskow.IndexOf(Grid.GetColumn(this)) + 1] == Grid.GetRow(this))
			//{
			//    Kys();
			//}
			//Console.WriteLine(MainWindow.PozycjePociskow);

			if (Zyje)
			{
				foreach (var i in ((Grid)Parent).Children.OfType<Pocisk>())
				{
					if (Grid.GetColumn(i) == Grid.GetColumn(this) && Grid.GetRow(i) == Grid.GetRow(this) || Grid.GetColumn(i) == Grid.GetColumn(this) + 1 && Grid.GetRow(i) == Grid.GetRow(this))
					{
						int Obrazenia = 2;
						if ((string)i.Tag == "Mrozon")
						{
							Obrazenia = 1;
                            TimerZombie.Interval = TimeSpan.FromSeconds(3);
                            //TimerSpowolnienie.Start();
						}

						Oberwij(Obrazenia);
						((Grid)i.Parent).Children.Remove(i);
						i.TimerRuchu.Stop();
						break;
					}
				}
			}
			if (Zycie <= 0)
			{
				TimerSprawdzanie.Stop();
				TimerZombie.Stop();
				Kys();
			}
		}

		private void Oberwij(int Obrazenia)
		{
			Zycie -= Obrazenia;
			SoundPlayer soundPlayer = new("./../../../audio/dmg.wav");
			soundPlayer.Load();
			soundPlayer.Play();

		}

		public void Kys()
		{
			if (Zyje)
			{
				SoundPlayer soundPlayer = new("./../../../audio/dying.wav");
				soundPlayer.Load();
				soundPlayer.Play();

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
