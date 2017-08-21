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
    /// Interaction logic for VistaPrestables.xaml
    /// </summary>
    public partial class VistaPrestables : Page
    {
        //Propiedades
        private ObservableCollection<Modelos.Prestable> Prestables = new ObservableCollection<Modelos.Prestable>();

        //Constructor
        public VistaPrestables()
        {
            InitializeComponent();
            Lista.ItemsSource = Prestables;
            ActualizarPrestables();
        }

        //Eventos
        private void ClickAgregar(object sender, RoutedEventArgs e)
        {
            var ventana = new DetallePrestable();
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPrestables();
        }

        private void Seleccionar(object sender, MouseButtonEventArgs e)
        {
            var ventana = new DetallePrestable();
            ventana.Prestable = (sender as ListViewItem).Content as Modelos.Prestable;
            ventana.Owner = Window.GetWindow(this);
            ventana.ShowDialog();
            ActualizarPrestables();
        }

        //Interno
        private void ActualizarPrestables()
        {
            Prestables.Clear();

            var prestables = Modelos.Prestable.Collection.FindAsync(_ => true).Result.ToList();
            foreach (var prestable in prestables)
            {
                Prestables.Add(prestable);
            }
        }
    }
}
