using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DetalleSeccion.xaml
    /// </summary>
    public partial class DetalleSeccion : Window
    {
        private Modelos.Clase _clase;
        private Modelos.Seccion _seccion;

        private ObservableCollection<Modelos.Seccion> Secciones = new ObservableCollection<Modelos.Seccion>();
        private ObservableCollection<Modelos.Persona> Catedraticos = new ObservableCollection<Modelos.Persona>();
        private ObservableCollection<Modelos.Periodo> Periodos = new ObservableCollection<Modelos.Periodo>();

        public Modelos.Clase Clase
        {
            get => _clase;
            set
            {
                _clase = value;

                var secciones = Modelos.Seccion.Coleccion.FindAsync(arg => arg.Clase == value.Id).Result.ToList();
                foreach (var seccion in secciones)
                {
                    Secciones.Add(seccion);
                }
                ListaSecciones.ItemsSource = Secciones;

                LlenarCampos();
                Validar();
            }
        }

        public Modelos.Seccion Seccion
        {
            get => _seccion;
            set
            {
                _seccion = value;
                LlenarCampos();
                Validar();
            }
        }

        public DetalleSeccion()
        {
            InitializeComponent();
            var catedraticos = Modelos.Persona.Coleccion.FindAsync(arg => arg.Catedratico).Result.ToList();
            foreach (var catedratico in catedraticos)
            {
                Catedraticos.Add(catedratico);
            }
            var periodos = Modelos.Periodo.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var periodo in periodos)
            {
                Periodos.Add(periodo);
            }
            Catedratico.ItemsSource = Catedraticos;
            Periodo.ItemsSource = Periodos;
            Periodo.SelectedIndex = Periodo.Items.Count - 1;
        }

        private void SeleccionarSeccion(object sender, MouseButtonEventArgs e)
        {
            Seccion = ListaSecciones.SelectedItem as Modelos.Seccion;
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirCatedratico(object sender, TextChangedEventArgs e)
        {
            if (Catedratico.SelectedItem == null)
            {
                Catedratico.IsDropDownOpen = true;
                Catedratico.Items.Filter = arg =>
                {
                    var persona = arg as Modelos.Persona;
                    return persona.Catedratico;
                };
            } else
            {
                Catedratico.Items.Filter = null;
            }
        }

        private void SeleccionarCatedratico(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void SeleccionarPeriodo(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Seccion == null)
            {
                var seccion = new Modelos.Seccion()
                {
                    Clase = Clase.Id,
                    Letra = Letra.Text,
                    Catedratico = (Catedratico.SelectedItem as Modelos.Persona).Id,
                    Periodo = (Periodo.SelectedItem as Modelos.Periodo).Id
                };
                Modelos.Seccion.Coleccion.InsertOne(seccion);
                Clase.AgregarSeccion(seccion);
                Modelos.Clase.Coleccion.FindOneAndReplace(arg => arg.Id == Clase.Id, Clase);
            } else
            {
                Seccion.Letra = Letra.Text;
                Seccion.Catedratico = (Catedratico.SelectedItem as Modelos.Persona).Id;
                Seccion.Periodo = (Periodo.SelectedItem as Modelos.Periodo).Id;
                Modelos.Seccion.Coleccion.FindOneAndReplace(arg => arg.Id == Seccion.Id, Seccion);
            }
            Close();
        }

        private void ClickAgregarPeriodo(object sender, RoutedEventArgs e)
        {
            var ventana = new DetallePeriodo();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();

            Periodos.Clear();
            var periodos = Modelos.Periodo.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var periodo in periodos)
            {
                Periodos.Add(periodo);
            }
        }

        private void Validar()
        {
            var editable = true;
            editable = editable && Letra.Text.Length == 1;
            editable = editable && Catedratico.SelectedItem != null;
            editable = editable && Periodo.SelectedItem != null;

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            if (Seccion == null) { return; }
            Letra.Text = Seccion.Letra;
            foreach (var item in Catedratico.Items)
            {
                var catedratico = item as Modelos.Persona;
                if (catedratico.Id == Seccion.Catedratico)
                {
                    Catedratico.SelectedItem = item;
                    break;
                }
            }
            foreach (var item in Periodo.Items)
            {
                var periodo = item as Modelos.Periodo;
                if (periodo.Id == Seccion.Periodo)
                {
                    Periodo.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
