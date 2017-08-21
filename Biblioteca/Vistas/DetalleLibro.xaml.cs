using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DetalleLibro.xaml
    /// </summary>
    public partial class DetalleLibro : Window
    {
        private ObservableCollection<Modelos.Autor> Autores = new ObservableCollection<Modelos.Autor>();
        private ObservableCollection<Modelos.Editorial> Editoriales = new ObservableCollection<Modelos.Editorial>();
        private ObservableCollection<Modelos.Autor> AutoresLista = new ObservableCollection<Modelos.Autor>();

        private Modelos.Libro _libro;

        public Modelos.Libro Libro
        {
            get => _libro;
            set
            {
                _libro = value;
                LlenarCampos();
                Validar();
            }
        }

        public DetalleLibro()
        {
            InitializeComponent();
            var autores = Modelos.Autor.Coleccion.Find(_ => true).ToList();
            foreach (var autor in autores)
            {
                Autores.Add(autor);
            }
            var editoriales = Modelos.Editorial.Coleccion.Find(_ => true).ToList();
            foreach (var editorial in editoriales)
            {
                Editoriales.Add(editorial);
            }
            Autor.ItemsSource = Autores;
            Editorial.ItemsSource = Editoriales;
            ListaAutores.ItemsSource = AutoresLista;
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirAutor(object sender, TextChangedEventArgs e)
        {
            if (Autor.SelectedItem == null)
            {
                Autor.IsDropDownOpen = true;
                Autor.Items.Filter = arg =>
                {
                    var autor = arg as Modelos.Autor;
                    return autor.Nombre.Contains(Autor.Text);
                };
            } else
            {
                Autor.Items.Filter = null;
            }
        }

        private void SeleccionarAutor(object sender, SelectionChangedEventArgs e)
        {
            AutoresLista.Add(Autor.SelectedItem as Modelos.Autor);
            Validar();
        }

        private void AgregarAutor(object sender, RoutedEventArgs e)
        {
            var autor = new Modelos.Autor { Nombre = Autor.Text };
            Modelos.Autor.Coleccion.InsertOne(autor);
            Autores.Add(autor);
            AutoresLista.Add(autor);
        }

        private void IntroducirEditorial(object sender, TextChangedEventArgs e)
        {
            if (Editorial.SelectedItem == null)
            {
                Editorial.IsDropDownOpen = true;
                Editorial.Items.Filter = arg =>
                {
                    var editorial = arg as Modelos.Editorial;
                    return editorial.Nombre.Contains(Editorial.Text);
                };
            } else
            {
                Editorial.Items.Filter = null;
            }
        }

        private void SeleccionarEditorial(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void AgregarEditorial(object sender, RoutedEventArgs e)
        {
            var editorial = new Modelos.Editorial { Nombre = Editorial.Text };
            Modelos.Editorial.Coleccion.InsertOne(editorial);
            Editoriales.Add(editorial);
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Libro == null)
            {

                var libro = new Modelos.Libro
                {
                    ISBN = ISBN.Text,
                    Titulo = Titulo.Text,
                    Editorial = (Editorial.SelectedItem as Modelos.Editorial).Id
                };
                foreach (var autor in AutoresLista)
                {
                    libro.AgregarAutor(autor);
                }
                Modelos.Libro.Coleccion.InsertOne(libro);
            } else
            {
                Libro.ISBN = ISBN.Text;
                Libro.Titulo = Titulo.Text;
                Libro.Editorial = (Editorial.SelectedItem as Modelos.Editorial).Id;
                foreach (var autor in AutoresLista)
                {
                    Libro.AgregarAutor(autor);
                }
                Modelos.Libro.Coleccion.FindOneAndReplace(arg => arg.Id == Libro.Id, Libro);
            }
            Close();
        }

        public void Validar()
        {
            var regex = new Regex(@"^\d{9,12}\d|x$");

            var editable = true;
            editable = editable && regex.IsMatch(ISBN.Text);
            editable = editable && Titulo.Text != "";
            editable = editable && AutoresLista.Count > 0;
            editable = editable && Editorial.SelectedItem != null;

            BotonAceptar.IsEnabled = editable;
        }

        public void LlenarCampos()
        {
            if (Libro != null)
            {
                ISBN.Text = Libro.ISBN;
                Titulo.Text = Libro.Titulo;
                foreach (var autor in Libro.ListaAutores)
                {
                    AutoresLista.Add(autor);
                }
                foreach(var item in Editorial.Items)
                {
                    var editorial = item as Modelos.Editorial;
                    if (editorial.Id == Libro.Editorial)
                    {
                        Editorial.SelectedItem = item;
                        break;
                    }
                }
            }
        }
    }
}
