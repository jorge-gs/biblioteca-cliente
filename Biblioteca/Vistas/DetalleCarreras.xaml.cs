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

namespace Biblioteca.Vistas
{
    /// <summary>
    /// Interaction logic for DetalleCarreras.xaml
    /// </summary>
    public partial class DetalleCarreras : Window
    {
        //Propiedades
        private Modelos.Carrera _carrera;

        public Modelos.Carrera Carrera
        {
            get => _carrera;
            set
            {
                _carrera = value;
                LlenarCampos();
                Validar();
            }
        }

        //Constructor
        public DetalleCarreras()
        {
            InitializeComponent();
        }

        //Eventos
        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Carrera == null)
            {
                var carrera = new Modelos.Carrera()
                {
                    Codigo = Codigo.Text,
                    Nombre = Nombre.Text
                };

                Modelos.Carrera.Coleccion.InsertOneAsync(carrera).Wait();
                Carrera = carrera;
            } else
            {
                Carrera.Codigo = Codigo.Text;
                Carrera.Nombre = Nombre.Text;

                var filtro = MongoDB.Driver.Builders<Modelos.Carrera>.Filter.Eq(arg => arg.Id, Carrera.Id);
                Modelos.Carrera.Coleccion.FindOneAndReplaceAsync<Modelos.Carrera>(filtro, Carrera).Wait();
            }

            Close();
        }

        //Interno
        private void Validar()
        {
            var regex = new Regex("^[A-Z]{3}$");

            bool editable = true;
            editable = editable && regex.IsMatch(Codigo.Text);
            editable = editable && Nombre.Text != "";

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {
            if (Carrera == null) { return; }
            Codigo.Text = Carrera.Codigo;
            Nombre.Text = Carrera.Nombre;
        }
    }
}
