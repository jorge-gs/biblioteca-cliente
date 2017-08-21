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
using MongoDB.Bson;
using MongoDB.Driver;

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Cargar(object sender, RoutedEventArgs e)
        {
            Contenido.Navigated += ContenidoNavegar;
            Contenido.Navigate(new Vistas.VistaPrestamos());
        }

        private void Descargar(object sender, RoutedEventArgs e)
        {
            Contenido.Navigated -= ContenidoNavegar;
        }

        private void ContenidoNavegar(object sender, NavigationEventArgs e)
        {
            Contenido.NavigationService.RemoveBackEntry();
        }

        private void ClickCarreras(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaCarrera());
        }

        private void ClickClases(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaClase());
        }

        private void ClickPersonas(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaPersonas());
        }

        private void ClickPrestables(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaPrestables());
        }

        private void ClickPrestamos(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaPrestamos());
        }

        private void ClickLibros(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaLibros());
        }

        private void ClickReportes(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(new Vistas.VistaReportes());
        }
    }
}
