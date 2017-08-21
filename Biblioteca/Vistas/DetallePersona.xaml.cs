using System;
using System.Collections.Generic;
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
    /// Interaction logic for DetallePersona.xaml
    /// </summary>
    public partial class DetallePersona : Window
    {
        //Propiedades
        private Modelos.Persona _persona;

        public Modelos.Persona Persona
        {
            get => _persona;
            set
            {
                _persona = value;
                LlenarCampos();
                Validar();
            }
        }

        //Constructor
        public DetallePersona()
        {
            InitializeComponent();
        }

        //Eventos
        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickEstudiante(object sender, RoutedEventArgs e)
        {

        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Persona == null)
            {
                var persona = new Modelos.Persona
                {
                    Identidad = Identidad.Text,
                    Nombre = Nombre.Text,
                    Apellido = Apellido.Text,
                    Catedratico = Catedrático.IsChecked.Value
                };

                Modelos.Persona.Coleccion.InsertOneAsync(persona).Wait();
            }
            else
            {
                Persona.Identidad = Identidad.Text;
                Persona.Nombre = Nombre.Text;
                Persona.Apellido = Apellido.Text;
                Persona.Catedratico = Catedrático.IsChecked.Value;

                Modelos.Persona.Coleccion.FindOneAndReplaceAsync(arg => arg.Id == Persona.Id, Persona).Wait();
            }

            Close();
        }

        //Interno
        private void Validar()
        {
            var editable = true;
            editable = editable && Identidad.Text != "";
            editable = editable && Nombre.Text != "";
            editable = editable && Apellido.Text != "";

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            if (Persona == null) { return; }
            Identidad.Text = Persona.Identidad;
            Nombre.Text = Persona.Nombre;
            Apellido.Text = Persona.Apellido;
            Catedrático.IsChecked = Persona.Catedratico;
        }
    }
}
