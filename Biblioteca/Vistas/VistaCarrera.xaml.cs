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
    /// Interaction logic for VistaCarrera.xaml
    /// </summary>
    public partial class VistaCarrera : Page
    {
        //Propiedades
        public ObservableCollection<Modelos.Carrera> Carreras = new ObservableCollection<Modelos.Carrera>();

        //Constructor
        public VistaCarrera()
        {
            InitializeComponent();
            Lista.ItemsSource = Carreras;
            ActualizarCarreras();
        }

        //Eventos
        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetalleCarreras();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarCarreras();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetalleCarreras();
            ventana.Carrera = (sender as ListViewItem).Content as Modelos.Carrera;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarCarreras();
        }

        //Interno
        private void ActualizarCarreras()
        {
            Carreras.Clear();

            var carreras = Modelos.Carrera.Coleccion.FindAsync<Modelos.Carrera>(_ => true).Result.ToList();
            foreach (var carrera in carreras)
            {
                Carreras.Add(carrera);
            }
        }
    }
}
