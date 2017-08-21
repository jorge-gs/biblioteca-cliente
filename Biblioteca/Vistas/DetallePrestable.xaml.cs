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
    /// Interaction logic for DetallePrestable.xaml
    /// </summary>
    public partial class DetallePrestable : Window
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

        public DetallePrestable()
        {
            InitializeComponent();
        }

        private void IntroducirTexto(object sender, TextChangedEventArgs e)
        {
            Validar();
        }

        private void SeleccionarCombo(object sender, SelectionChangedEventArgs e)
        {
            Validar();
        }

        private void ClickCopia(object sender, RoutedEventArgs e)
        {
            var ventana = new DetalleCopia();
            ventana.Prestable = Prestable;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
        }

        private void ClickAceptar(object sender, RoutedEventArgs e)
        {
            if (Prestable == null)
            {
                var prestable = new Modelos.Prestable
                {
                    Codigo = Codigo.Text,
                    Tipo = (Modelos.Tipo)Tipo.SelectedIndex,
                    Estado = (Modelos.Estado)Estado.SelectedIndex
                };
                if (Descripcion.Text != "") { prestable.Descripcion = Descripcion.Text; }

                Modelos.Prestable.Collection.InsertOneAsync(prestable).Wait();
            } else
            {
                Prestable.Codigo = Codigo.Text;
                Prestable.Tipo = (Modelos.Tipo)Tipo.SelectedIndex;
                Prestable.Estado = (Modelos.Estado)Estado.SelectedIndex;
                if (Descripcion.Text != "") { Prestable.Descripcion = Descripcion.Text; }

                Modelos.Prestable.Collection.FindOneAndReplaceAsync(arg => arg.Id == Prestable.Id, Prestable);
            }

            Close();
        }

        public void Validar()
        {
            var regex = new Regex(@"^[A-Z]{1,3}\d{1,3}$");

            var editable = regex.IsMatch(Codigo.Text);

            if (BotonAceptar != null) { BotonAceptar.IsEnabled = editable; }
        }

        public void LlenarCampos()
        {
            if (Prestable == null) { return; }
            Codigo.Text = Prestable.Codigo;
            Tipo.SelectedIndex = (int)Prestable.Tipo;
            Estado.SelectedIndex = (int)Prestable.Estado;
            if (Prestable.Descripcion != null)
            {
                Descripcion.Text = Prestable.Descripcion;
            }
            BotonCopia.IsEnabled = true;
        }
    }
}
