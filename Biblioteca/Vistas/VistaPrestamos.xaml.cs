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
    /// Interaction logic for VistaPrestamos.xaml
    /// </summary>
    public partial class VistaPrestamos : Page
    {
        private ObservableCollection<Modelos.Prestamo> Prestamos = new ObservableCollection<Modelos.Prestamo>();

        public VistaPrestamos()
        {
            InitializeComponent();
            Lista.ItemsSource = Prestamos;
            ActualizarPrestamos();
        }

        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetallePrestamo();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPrestamos();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetallePrestamo();
            ventana.Prestamo = (sender as ListViewItem).Content as Modelos.Prestamo;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPrestamos();
        }

        //Interno
        private void ActualizarPrestamos()
        {
            Prestamos.Clear();

            var prestamos = Modelos.Prestamo.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var prestamo in prestamos)
            {
                Prestamos.Add(prestamo);
            }
        }
    }
}
