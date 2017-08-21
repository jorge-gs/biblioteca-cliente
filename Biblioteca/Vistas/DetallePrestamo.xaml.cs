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
    /// Interaction logic for DetallePrestamo.xaml
    /// </summary>
    public partial class DetallePrestamo : Window
    {
        private Modelos.Prestamo _prestamo;

        private ObservableCollection<Modelos.Persona> Personas = new ObservableCollection<Modelos.Persona>();
        private ObservableCollection<Modelos.Prestable> Prestables = new ObservableCollection<Modelos.Prestable>();
        private ObservableCollection<Modelos.Clase> Clases = new ObservableCollection<Modelos.Clase>();
        private ObservableCollection<Modelos.Seccion> Secciones = new ObservableCollection<Modelos.Seccion>();

        public Modelos.Prestamo Prestamo
        {
            get => _prestamo;
            set
            {
                _prestamo = value;
                LlenarCampos();
                Validar();
            }
        }

        public DetallePrestamo()
        {
            InitializeComponent();
            var personas = Modelos.Persona.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var persona in personas)
            {
                Personas.Add(persona);
            }
            var prestables = Modelos.Prestable.Collection.FindAsync(_ => true).Result.ToList();
            foreach (var prestable in prestables)
            {
                Prestables.Add(prestable);
            }
            var clases = Modelos.Clase.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var clase in clases)
            {
                Clases.Add(clase);
            }
            Persona.ItemsSource = Personas;
            Articulo.ItemsSource = Prestables;
            Clase.ItemsSource = Clases;
        }

        private void IntroducirPersona(object sender, TextChangedEventArgs e)
        {
            if (Persona.SelectedItem == null)
            {
                Persona.IsDropDownOpen = true;
                Persona.Items.Filter = arg =>
                {
                    var persona = arg as Modelos.Persona;
                    return persona.Identidad.Contains(Persona.Text) || persona.Nombre.Contains(Persona.Text);
                };
            } else
            {
                Persona.Items.Filter = null;
            }
        }

        private void SeleccionarPersona(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirPrestable(object sender, TextChangedEventArgs e)
        {
            if (Articulo.SelectedItem == null)
            {
                Articulo.IsDropDownOpen = true;
                Articulo.Items.Filter = arg =>
                {
                    var prestable = arg as Modelos.Prestable;
                    if (prestable.Copia != null)
                    {
                        if (prestable.Copia.ObjetoLibro.Titulo.Contains(Articulo.Text) || prestable.Copia.ObjetoLibro.AutoresConcat.Contains(Articulo.Text))
                        {
                            return true;
                        }
                    }
                    return prestable.Codigo.Contains(Articulo.Text);
                };
            } else
            {
                Articulo.Items.Filter = null;
            }
        }

        private void SeleccionarPrestable(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirClase(object sender, TextChangedEventArgs e)
        {
            if (Clase.SelectedItem == null)
            {
                Clase.IsDropDownOpen = true;
                Clase.Items.Filter = arg =>
                {
                    var clase = arg as Modelos.Clase;
                    return clase.Codigo.Contains(Clase.Text) || clase.Nombre.Contains(Clase.Text);
                };
                Seccion.IsEnabled = false;
            } else
            {
                Clase.Items.Filter = null;
            }
        }

        private void SeleccionarClase(object sender, SelectionChangedEventArgs e)
        {
            Seccion.ItemsSource = Modelos.Seccion.Coleccion.Find(arg => arg.Clase == (Clase.SelectedItem as Modelos.Clase).Id).ToList();
            Seccion.IsEnabled = true;
            Validar();
        }

        private void SeleccionarSeccion(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void SeleccionarEstado(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ClickDevolver(object sender, RoutedEventArgs e)
        {

            var estado = EstadoDevolucion.SelectedIndex;
            Console.WriteLine(estado);

            Prestamo.FechaDevolucion = DateTime.Now;
            Prestamo.EstadoDevolucion = (Modelos.Estado)estado;

            Modelos.Prestamo.Coleccion.FindOneAndReplace(arg => arg.Id == Prestamo.Id, Prestamo);
            Close();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Prestamo == null)
            {
                var prestamo = new Modelos.Prestamo
                {
                    Fecha = DateTime.Now,
                    Persona = (Persona.SelectedItem as Modelos.Persona).Id,
                    Prestable = (Articulo.SelectedItem as Modelos.Prestable).Id,
                    Estado = (Articulo.SelectedItem as Modelos.Prestable).Estado
                };
                if (Clase.SelectedItem != null && Seccion.SelectedItem != null)
                {
                    prestamo.Seccion = (Seccion.SelectedItem as Modelos.Seccion).Id;
                }
                Modelos.Prestamo.Coleccion.InsertOne(prestamo);
            } else
            {
                Prestamo.Persona = (Persona.SelectedItem as Modelos.Persona).Id;
                Prestamo.Prestable = (Articulo.SelectedItem as Modelos.Prestable).Id;
                Prestamo.Estado = (Articulo.SelectedItem as Modelos.Prestable).Estado;
                if (Clase.SelectedItem != null && Seccion.SelectedItem != null)
                {
                    Prestamo.Seccion = (Seccion.SelectedItem as Modelos.Seccion).Id;
                }
                Modelos.Prestamo.Coleccion.FindOneAndReplace(arg => arg.Id == Prestamo.Id, Prestamo);
            }

            Close();
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {

        }

        private void Validar()
        {
            bool editable = true;
            editable = editable && Persona.SelectedItem != null;
            editable = editable && Articulo.SelectedItem != null;
            editable = editable && (Clase.SelectedItem == null || Seccion.SelectedItem != null);

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            var editando = Prestamo != null;
            EstadoDevolucion.IsEnabled = editando;
            BotonDevolucion.IsEnabled = editando;
            if (!editando) { return; }

            foreach (var item in Persona.Items)
            {
                var persona = item as Modelos.Persona;
                if (persona.Id == Prestamo.Persona)
                {
                    Persona.SelectedItem = item;
                    break;
                }
            }
            foreach (var item in Articulo.Items)
            {
                var prestable = item as Modelos.Prestable;
                if (prestable.Id == Prestamo.Prestable)
                {
                    Articulo.SelectedItem = item;
                    break;
                }
            }
            if (Prestamo.Seccion != new ObjectId() && Prestamo.Seccion != null)
            {
                foreach (var item in Clase.Items)
                {
                    var clase = item as Modelos.Clase;
                    if (clase.Id == Prestamo.ObjetoSeccion.Clase)
                    {
                        Clase.SelectedItem = item;
                    }
                }
                foreach (var item in Seccion.Items)
                {
                    var seccion = item as Modelos.Seccion;
                    if (seccion.Id == Prestamo.Seccion)
                    {
                        Seccion.SelectedItem = item;
                    }
                }
            }
        }
    }
}
