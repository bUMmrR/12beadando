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
        string path = "C:\\Users\\agocs\\Desktop\\fasz\\beadando\\images";
        ICollectionView myView;

        public MainWindow()
        {
            InitializeComponent();
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
            elso.Visibility = Visibility.Visible;
            masodik.Visibility = Visibility.Hidden;
            elso.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            elso.Visibility = Visibility.Hidden;
            masodik.Visibility = Visibility.Visible;
            elso.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
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
    }
}
