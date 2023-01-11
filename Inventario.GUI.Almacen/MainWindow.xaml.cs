using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.DAL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum Accion
        {
            Nuevo,
            Editar
        }
        ManejadorVales manejadorDeVales;
        ManejadorEmpleados manejadorEmpleados;
        ManejadorMateriales manejadorMateriales;
        Vale vale;

        Accion accionVale;

        public MainWindow()
        {
            InitializeComponent();

            manejadorDeVales = new ManejadorVales(new RepositorioGenerico<Vale>());
            manejadorEmpleados = new ManejadorEmpleados(new RepositorioGenerico<Empleado>());
            manejadorMateriales = new ManejadorMateriales(new RepositorioGenerico<Material>());


            ActualizarTablaDeVales();
            gridDetalle.IsEnabled = false;
        }

        private void ActualizarTablaDeVales()
        {
            dtgVales.ItemsSource = null;
            dtgVales.ItemsSource = manejadorDeVales.Listar;
        }

        private void btnEliminarVale_Click(object sender, RoutedEventArgs e)
        {
            Vale v = dtgVales.SelectedItem as Vale;
            if (v != null)
            {
                if(MessageBox.Show("Realmente deseas eliminar el vale?", "Almacén", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
                {
                    if (manejadorDeVales.Eliminar(v.Id))
                    {
                        MessageBox.Show("Eliminado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        ActualizarTablaDeVales();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnNuevoVale_Click(object sender, RoutedEventArgs e)
        {
            gridDetalle.IsEnabled = true;
            ActualizarCombosDetalle();
            vale = new Vale();
            vale.MaterialesPrestados = new List<Material>();
            ActualizarListaDeMaterialesEnVale();
            accionVale = Accion.Nuevo;
        }

        private void ActualizarListaDeMaterialesEnVale()
        {
            dtgMaterialesEnVale.ItemsSource = null;
            dtgMaterialesEnVale.ItemsSource = vale.MaterialesPrestados;
        }

        private void ActualizarCombosDetalle()
        {
            cmbAlmacenista.ItemsSource = null;
            cmbAlmacenista.ItemsSource = manejadorEmpleados.EmpleadosPorArea("Almacen");

            cmbMateriales.ItemsSource = null;
            cmbMateriales.ItemsSource = manejadorMateriales.Listar;

            cmbSolicitante.ItemsSource = null;
            cmbSolicitante.ItemsSource = manejadorEmpleados.Listar;
        }

        private void btnAgregarMaterial_Click(object sender, RoutedEventArgs e)
        {
            Material m = cmbMateriales.SelectedItem as Material;
            if (m != null)
            {
                vale.MaterialesPrestados.Add(m);
                ActualizarListaDeMaterialesEnVale();
            }
        }

        private void btnEliminarMaterial_Click(object sender, RoutedEventArgs e)
        {
            Material m = dtgMaterialesEnVale.SelectedItem as Material;
            if (m != null)
            {
                vale.MaterialesPrestados.Remove(m);
                ActualizarListaDeMaterialesEnVale();
            }
        }

        private void btnGuardarVale_Click(object sender, RoutedEventArgs e)
        {
            if(accionVale== Accion.Nuevo)
            {
                vale.EncargadoDeAlmacen = cmbAlmacenista.SelectedItem as Empleado;
                vale.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                vale.FechaHoraSolicitud = DateTime.Now;
                vale.Solicitante = cmbSolicitante.SelectedItem as Empleado;
                if (manejadorDeVales.Agregar(vale))
                {
                    MessageBox.Show("Vale guardado con éxito","Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    LimpiarCamposDeVale();
                    gridDetalle.IsEnabled = false;
                    ActualizarTablaDeVales();
                }
                else
                {
                    MessageBox.Show("Error al guardar el vale", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                vale.EncargadoDeAlmacen = cmbAlmacenista.SelectedItem as Empleado;
                vale.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                vale.FechaHoraSolicitud = DateTime.Now;
                vale.Solicitante = cmbSolicitante.SelectedItem as Empleado;
                vale.FechaEntregaReal = DateTime.Parse( lblFechaHoraEntregaReal.Content.ToString());
                if (manejadorDeVales.Modificar(vale))
                {
                    MessageBox.Show("Vale guardado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    LimpiarCamposDeVale();
                    gridDetalle.IsEnabled = false;
                    ActualizarTablaDeVales();
                }
                else
                {
                    MessageBox.Show("Error al guardar el vale", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LimpiarCamposDeVale()
        {
            dtpFechaEntrega.SelectedDate = DateTime.Now;
            lblFechaHoraEntregaReal.Content = "";
            lblFechaHoraPrestamo.Content = "";
            dtgMaterialesEnVale.ItemsSource = null;
            cmbAlmacenista.ItemsSource = null;
            cmbMateriales.ItemsSource = null;
            cmbSolicitante.ItemsSource = null;
        }

        private void dtgVales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vale v = dtgVales.SelectedItem as Vale;
            if (v != null)
            {
                gridDetalle.IsEnabled = true;
                vale = v;
                ActualizarListaDeMaterialesEnVale();
                accionVale = Accion.Editar;
                lblFechaHoraPrestamo.Content = vale.FechaHoraSolicitud.ToString();
                lblFechaHoraEntregaReal.Content = vale.FechaEntregaReal.ToString();
                ActualizarCombosDetalle();
                cmbAlmacenista.Text = vale.EncargadoDeAlmacen.ToString();
                cmbSolicitante.Text = vale.Solicitante.ToString();
                dtpFechaEntrega.SelectedDate = vale.FechaEntrega;
            }
        }

        private void btnEntregarVale_Click(object sender, RoutedEventArgs e)
        {
            lblFechaHoraEntregaReal.Content = DateTime.Now;
        }

        private void btnCanelarVale_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeVale();
            gridDetalle.IsEnabled = false;
        }

        private void btnReportes_Click(object sender, RoutedEventArgs e)
        {
            Reportes reportes = new Reportes();
            reportes.Show();
        }
    }
}
