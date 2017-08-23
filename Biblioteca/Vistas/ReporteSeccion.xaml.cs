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
    /// Interaction logic for ReporteSeccion.xaml
    /// </summary>
    public partial class ReporteSeccion : Window
    {
        public List<Modelos.Prestamo> prestamos = new List<Modelos.Prestamo>();

        public ReporteSeccion()
        {
            InitializeComponent();
        }

        public void Construir(List<Modelos.Prestamo> prestamos)
        {
            this.prestamos = prestamos;

            var info = prestamos.First();
            Nombre.Text = info.ObjetoSeccion.ObjetoClase.Codigo + " " +
                info.ObjetoSeccion.ObjetoClase.Nombre + ", " +
                "Periodo " + info.ObjetoSeccion.ObjetoPeriodo.ToString() + ", " +
                info.ObjetoSeccion.ObjetoCatedratico.NombreCompleto;

            var horas = new DateTime();
            foreach (var prestamo in prestamos)
            {
                if (prestamo.ObjetoFechaDevolucion.HasValue)
                {
                    horas += (prestamo.FechaDevolucion - prestamo.Fecha);
                    var texto = new TextBlock();
                    texto.Text = prestamo.Fecha.ToLocalTime() + " " +
                        prestamo.ObjetoPrestable.ToString() + " " +
                        prestamo.FechaDevolucion.ToLocalTime().ToLongTimeString();
                    Contenido.Children.Add(texto);
                }
            }

            var conclusion = new TextBlock();
            conclusion.MaxWidth = 700;
            conclusion.TextWrapping = TextWrapping.Wrap;
            conclusion.Text = "El catedrático " + info.ObjetoSeccion.ObjetoCatedratico.NombreCompleto + " ha impartido " +
                horas.ToString("HH:MM") + " horas de clase a los estudiantes de la clase " +
                info.ObjetoSeccion.ObjetoClase.Codigo + " " + info.ObjetoSeccion.ObjetoClase.Nombre + "" +
                " en la sección " + info.ObjetoSeccion.Letra + " durante el periodo " + info.ObjetoSeccion.ObjetoPeriodo.ToString() + ".";
            conclusion.Margin = new Thickness(0, 30, 0, 0);
            Contenido.Children.Add(conclusion);
        }
    }
}
