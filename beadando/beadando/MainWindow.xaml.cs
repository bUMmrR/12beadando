using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Photos photos;
        string path = "C:\\Users\\agocs\\Desktop\\-\\Progi\\12beadando\\beadando\\images";
        ICollectionView myView;

        public MainWindow()
        {
            InitializeComponent();
            //ResizeMode = ResizeMode.NoResize;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var files = new DirectoryInfo(path).GetFiles().ToList();
            photos = new Photos(files);

            myView = CollectionViewSource.GetDefaultView(photos);
            this.DataContext = myView;

            //Nézi a mappa tartalmát, hogy kerül-e bele új kép.
            FileSystemWatcher fsw = new FileSystemWatcher(path);
            fsw.Created += FswCreated;
            fsw.EnableRaisingEvents = true;

            nezetValtas();
        }

        private void nezetValtas()
        {
            if (Properties.Settings.Default.mod == "1")
            {
                elsoKonfiguracio();
            }
            else if(Properties.Settings.Default.mod == "2")
            {
                masodikKonfiguracio();
            }
            else
            {
                harmadikKonfiguracio();
            }
        }

        private void FswCreated(object sender, FileSystemEventArgs e)
        {
            //Ha új képet töltünk be
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                //Diszépcserrel ellenőrzött módon tud a két szál között kommunikálni.
                Dispatcher.Invoke(new ThreadStart(() =>
                {
                    FileInfo fs = new FileInfo(e.FullPath);
                    // Ez külön szálon fut.Ezért ezt nem támogatja. Dispécsert kell használni a két szál között.
                    photos.Insert(0, fs);
                }));
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.mod = "1";
            Properties.Settings.Default.Save();
            elsoKonfiguracio();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.mod = "2";
            Properties.Settings.Default.Save();
            masodikKonfiguracio();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.mod = "3";
            Properties.Settings.Default.Save();
            harmadikKonfiguracio();
        }

        private void elsoKonfiguracio()
        {
            gomb1.Background = new SolidColorBrush(Colors.Gainsboro);
            gomb2.Background = new SolidColorBrush(Colors.GhostWhite);
            gomb3.Background = new SolidColorBrush(Colors.GhostWhite);
            elso.Visibility = Visibility.Visible;
            masodik.Visibility = Visibility.Hidden;
            harmadik.Visibility = Visibility.Hidden;
        }


        private void masodikKonfiguracio()
        {
            gomb1.Background = new SolidColorBrush(Colors.GhostWhite);
            gomb2.Background = new SolidColorBrush(Colors.Gainsboro);
            gomb3.Background = new SolidColorBrush(Colors.GhostWhite);
            elso.Visibility = Visibility.Hidden;
            masodik.Visibility = Visibility.Visible;
            harmadik.Visibility = Visibility.Hidden;
        }
        private void harmadikKonfiguracio()
        {
            gomb1.Background = new SolidColorBrush(Colors.GhostWhite);
            gomb2.Background = new SolidColorBrush(Colors.GhostWhite);
            gomb3.Background = new SolidColorBrush(Colors.Gainsboro);
            elso.Visibility = Visibility.Hidden;
            masodik.Visibility = Visibility.Hidden;
            harmadik.Visibility = Visibility.Visible;
        }


        //jobbra
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (myView != null)
            {
                if (myView.CurrentPosition < myView.Cast<object>().Count() - 1)
                {
                    myView.MoveCurrentToNext();
                }
                else
                {
                    // If the current position is the last item, move to the first item
                    myView.MoveCurrentToFirst();
                }
            }
        }

        //balra
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (myView != null)
            {
                if (myView.CurrentPosition > 0)
                {
                    myView.MoveCurrentToPrevious();
                }
                else
                {
                    // If the current position is the first item, move to the last item
                    myView.MoveCurrentToLast();
                }
            }
        }
    }
}
