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

namespace Biblioteca.Vistas
{
    /// <summary>
    /// Interaction logic for DetallePeriodo.xaml
    /// </summary>
    public partial class DetallePeriodo : Window
    {
        public DetallePeriodo()
        {
            InitializeComponent();
        }

        private void SeleccionarHora(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            var periodo = new Modelos.Periodo
            {
                FechaInicio = Inicio.SelectedDate.Value,
                FechaFin = Fin.SelectedDate.Value,
                Numero = Int32.Parse(Numero.Text)
            };
            Modelos.Periodo.Coleccion.InsertOne(periodo);

            Close();
        }

        private void Validar()
        {
            int dummy;

            var editable = true;
            editable = editable && Inicio.SelectedDate != null;
            editable = editable && Fin.SelectedDate != null;
            editable = editable && Inicio.SelectedDate.Value.CompareTo(Fin.SelectedDate.Value) == -1;
            editable = editable && Int32.TryParse(Numero.Text, out dummy);

            BotonAceptar.IsEnabled = editable;
        }

        private void LlenarCampos()
        {

        }
    }
}
