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
    /// Interaction logic for VistaLibros.xaml
    /// </summary>
    public partial class VistaLibros : Page
    {
        public ObservableCollection<Modelos.Libro> Libros = new ObservableCollection<Modelos.Libro>();

        public VistaLibros()
        {
            InitializeComponent();
            Lista.ItemsSource = Libros;
            ActualizarLibros();
        }

        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetalleLibro();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarLibros();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetalleLibro();
            ventana.Libro = (sender as ListViewItem).Content as Modelos.Libro;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarLibros();
        }

        //Interno
        private void ActualizarLibros()
        {
            Libros.Clear();

            var libros = Modelos.Libro.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var libro in libros)
            {
                Libros.Add(libro);
            }
        }
    }
}
