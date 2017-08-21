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
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Biblioteca.Vistas
{
    /// <summary>
    /// Interaction logic for DetalleCopia.xaml
    /// </summary>
    public partial class DetalleCopia : Window
    {
        private Modelos.Prestable _prestable;

        public Modelos.Prestable Prestable
        {
            get => _prestable;
            set
            {
                _prestable = value;
                LlenarCampos();
                Validar();
            }
        }

        private ObservableCollection<Modelos.Libro> Libros = new ObservableCollection<Modelos.Libro>();

        public DetalleCopia()
        {
            InitializeComponent();
            var libros = Modelos.Libro.Coleccion.Find(_ => true).ToList();
            foreach (var libro in libros)
            {
                Libros.Add(libro);
            }
            Libro.ItemsSource = Libros;
        }

        private void SeleccionarLibro(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirLibro(object sender, TextChangedEventArgs e)
        {
            if (Libro.SelectedItem == null)
            {
                Libro.IsDropDownOpen = true;
                Libro.Items.Filter = arg =>
                {
                    var libro = arg as Modelos.Libro;
                    return libro.Titulo.Contains(Libro.Text) || libro.AutoresConcat.Contains(Libro.Text);
                };
            } else
            {
                Libro.Items.Filter = null;
            }
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            var copia = new Modelos.Copia
            {
                Libro = (Libro.SelectedItem as Modelos.Libro).Id,
                Numero = Int32.Parse(Numero.Text)
            };
            Prestable.Copia = copia;
            Modelos.Prestable.Collection.FindOneAndReplace(arg => arg.Id == Prestable.Id, Prestable);
            Close();
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {

        }

        private void Validar()
        {
            int dummy;

            var editable = true;
            editable = editable && Libro.SelectedItem != null;
            editable = editable && Int32.TryParse(Numero.Text, out dummy);

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            if (Prestable.Copia == null) { return; }
            foreach (var item in Libro.Items)
            {
                var libro = item as Modelos.Libro;
                if (libro.Id == Prestable.Copia.Libro)
                {
                    Libro.SelectedItem = item;
                    break;
                }
            }
            Numero.Text = Prestable.Copia.Numero + "";
        }
    }
}
