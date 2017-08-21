using System;
using System.Collections.Generic;
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
    /// Interaction logic for DetalleClase.xaml
    /// </summary>
    public partial class DetalleClase : Window
    {
        //Propiedades
        private Modelos.Clase _clase;

        public Modelos.Clase Clase
        {
            get => _clase;
            set
            {
                _clase = value;
                LlenarCampos();
                Validar();
            }
        }

        //Constructor
        public DetalleClase()
        {
            InitializeComponent();
        }

        //Eventos Clase
        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickSecciones(object sender, RoutedEventArgs e)
        {
            var ventana = new DetalleSeccion();
            ventana.Clase = Clase;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Clase == null)
            {
                var clase = new Modelos.Clase
                {
                    Codigo = Codigo.Text,
                    Nombre = Nombre.Text,
                    Horas = Int32.Parse(Horas.Text),
                    UVs = Int32.Parse(UVs.Text)
                };

                Modelos.Clase.Coleccion.InsertOneAsync(clase).Wait();
            }
            else
            {
                Clase.Codigo = Codigo.Text;
                Clase.Nombre = Nombre.Text;
                Clase.Horas = Int32.Parse(Horas.Text);
                Clase.UVs = Int32.Parse(UVs.Text);

                Modelos.Clase.Coleccion.FindOneAndReplaceAsync(arg => arg.Id == Clase.Id, Clase).Wait();
            }

            Close();
        }

        //Interno Clase
        private void Validar()
        {
            var regex = new Regex(@"^[A-Z]{3}\d{4}$");
            int inservible;

            var editable = true;
            editable = editable && regex.IsMatch(Codigo.Text);
            editable = editable && Nombre.Text != "";
            editable = editable && Int32.TryParse(Horas.Text, out inservible);
            editable = editable && Int32.TryParse(UVs.Text, out inservible);

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            if (Clase != null)
            {
                Codigo.Text = Clase.Codigo;
                Nombre.Text = Clase.Nombre;
                Horas.Text = String.Format("{0}", Clase.Horas);
                UVs.Text = String.Format("{0}", Clase.UVs);
                Secciones.IsEnabled = true;
            }
        }
    }
}
