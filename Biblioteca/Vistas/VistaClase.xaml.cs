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
    /// Interaction logic for VistaClase.xaml
    /// </summary>
    public partial class VistaClase : Page
    {
        //Propiedades
        public ObservableCollection<Modelos.Clase> Clases = new ObservableCollection<Modelos.Clase>();

        //Constructor
        public VistaClase()
        {
            InitializeComponent();
            Lista.ItemsSource = Clases;
            ActualizarClases();
        }

        //Eventos
        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetalleClase();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarClases();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetalleClase();
            ventana.Clase = (sender as ListViewItem).Content as Modelos.Clase;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarClases();
        }

        //Interno
        private void ActualizarClases()
        {
            Clases.Clear();

            var clases = Modelos.Clase.Coleccion.FindAsync<Modelos.Clase>(_ => true).Result.ToList();
            foreach (var clase in clases)
            {
                Clases.Add(clase);
            }
        }
    }
}
