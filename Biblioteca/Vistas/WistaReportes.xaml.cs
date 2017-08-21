using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Biblioteca.Vistas
{
    /// <summary>
    /// Interaction logic for VistaReportes.xaml
    /// </summary>
    public partial class VistaReportes : Page
    {
        public ObservableCollection<Modelos.Clase> Clases = new ObservableCollection<Modelos.Clase>();
        public ObservableCollection<Modelos.Seccion> Secciones = new ObservableCollection<Modelos.Seccion>();

        public VistaReportes()
        {
            InitializeComponent();
            var clases = Modelos.Clase.Coleccion.Find(_ => true).ToList();
            foreach (var clase in clases)
            {
                Clases.Add(clase);
            }
            Clase.ItemsSource = Clases;
        }

        private void SeleccionarFecha(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void GenerarDiario(object sender, RoutedEventArgs e)
        {
            var diaSiguiente = DiaReporte.SelectedDate.Value.Date.AddDays(1);
            var filtro = Builders<Modelos.Prestamo>.Filter.Gte(arg => arg.Fecha, DiaReporte.SelectedDate.Value.Date) &
                Builders<Modelos.Prestamo>.Filter.Lt(arg => arg.Fecha, diaSiguiente);
            var prestamos = Modelos.Prestamo.Coleccion.Find(filtro).ToList();
            if (prestamos.Count < 1)
            {
                MessageBox.Show("No hay registros para este día");
                return;
            }

            var ventana = new ReporteDiario();
            ventana.prestamos = prestamos;
            ventana.Construir(prestamos);
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
        }

        private void IntroducirClase(object sender, TextChangedEventArgs e)
        {
            if (Clase.SelectedItem == null)
            {
                Clase.IsDropDownOpen = true;
                Clase.Items.Filter = arg =>
                {
                    var clase = arg as Modelos.Clase;
                    return clase.Nombre.Contains(Clase.Text) || clase.Codigo.Contains(Clase.Text);
                };
            } else
            {
                Clase.Items.Filter = null;
            }
        }

        private void SeleccionarClase(object sender, SelectionChangedEventArgs e)
        {
            Seccion.IsEnabled = true;
            Seccion.ItemsSource = Modelos.Seccion.Coleccion.Find(arg => arg.Clase == (Clase.SelectedItem as Modelos.Clase).Id).ToList();
            Validar();
        }

        private void SeleccionrSeccion(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void GenerarSeccion(object sender, RoutedEventArgs e)
        {
            var prestamos = Modelos.Prestamo.Coleccion.Find(arg => arg.Seccion == (Seccion.SelectedItem as Modelos.Seccion).Id).ToList();
            if (prestamos.Count < 1)
            {
                MessageBox.Show("No hay entradas o salidas para esta sección");
                return;
            }

            var ventana = new ReporteSeccion();
            ventana.prestamos = prestamos;
            ventana.Construir(prestamos);
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
        }

        public void Validar()
        {
            BotonReporteDia.IsEnabled = DiaReporte.SelectedDate.HasValue;

            BotonReporteSeccion.IsEnabled = Clase.SelectedItem != null && Seccion.SelectedItem != null;
        }
    }
}
