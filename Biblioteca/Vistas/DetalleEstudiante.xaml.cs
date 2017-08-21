using System;
using System.Collections.Generic;
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
    /// Interaction logic for DetalleEstudiante.xaml
    /// </summary>
    public partial class DetalleEstudiante : Window
    {
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

        public DetalleEstudiante()
        {
            InitializeComponent();
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            var estudinate = new Modelos.Estudiante { NoCuenta = Numero.Text };
            Persona.Estudiante = estudinate;
            Modelos.Persona.Coleccion.FindOneAndReplace(arg => arg.Id == Persona.Id, Persona);
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {

        }

        private void Validar()
        {
            BotonAceptar.IsEnabled = Numero.Text != "";
        }

        private void LlenarCampos()
        {
            if (Persona.Estudiante == null) { return; }
            Numero.Text = Persona.Estudiante.NoCuenta;
        }
    }
}
