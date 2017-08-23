using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ReporteDiario.xaml
    /// </summary>
    public partial class ReporteDiario : Window
    {
        public List<Modelos.Prestamo> prestamos = new List<Modelos.Prestamo>();

        public ReporteDiario()
        {
            InitializeComponent();
        }

        public void Construir(List<Modelos.Prestamo> prestamos)
        {
            this.prestamos = prestamos;

            var cultura = new CultureInfo("es-HN");
            var info = prestamos.First();
            Nombre.Text = "Fecha: " + info.Fecha.ToLocalTime().ToString("D", cultura);

            var devueltas = 0;
            foreach (var prestamo in prestamos)
            {
                var devuelto = prestamo.ObjetoFechaDevolucion.HasValue;
                devueltas++;
                var devolucion = !devuelto ? "El artículo aún no ha sido devuelto" : " devuelto en " +
                    prestamo.FechaDevolucion.ToLocalTime().ToShortDateString() + " a las " +
                    prestamo.FechaDevolucion.ToLocalTime().ToLongTimeString();

                var texto = new TextBlock();
                texto.Text = prestamo.Fecha.ToLocalTime().ToLongTimeString() + " " +
                    prestamo.ObjetoPrestable.ToString() + " " +
                    prestamo.ObjetoPersona.NombreCompleto + " " +
                    devolucion;
                Contenido.Children.Add(texto);
            }

            var conclusion = new TextBlock();
            conclusion.TextWrapping = TextWrapping.Wrap;
            conclusion.MaxWidth = 700;
            conclusion.Text = "El día " + info.Fecha.ToLocalTime().ToString("D", cultura) +
                " se han registrado " +
                prestamos.Count + " transacciones bibliotecarias de las cuales " + devueltas +
                " han sido completadas y " + (prestamos.Count - devueltas) + " están pendientes.";
            conclusion.Margin = new Thickness(0, 30, 0, 0);
            Contenido.Children.Add(conclusion);
        }
    }
}
