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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Biblioteca.Vistas
{
    /// <summary>
    /// Interaction logic for VistaPersonas.xaml
    /// </summary>
    public partial class VistaPersonas : Page
    {
        //Propiedades
        public ObservableCollection<Modelos.Persona> Personas = new ObservableCollection<Modelos.Persona>();

        //Constructor
        public VistaPersonas()
        {
            InitializeComponent();
            Lista.ItemsSource = Personas;
            ActualizarPersonas();
        }

        //Eventos
        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetallePersona();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPersonas();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetallePersona();
            ventana.Persona = (sender as ListViewItem).Content as Modelos.Persona;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPersonas();
        }

        //Interno
        private void ActualizarPersonas()
        {
            Personas.Clear();

            var personas = Modelos.Persona.Coleccion.FindAsync(_ => true).Result.ToList();
            foreach (var persona in personas)
            {
                Personas.Add(persona);
            }
        }
    }
}
