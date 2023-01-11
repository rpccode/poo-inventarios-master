using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Inventario.GUI.Administrador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum accion
        {
            Nuevo,
            Editar
        }

        IManejadorEmpleados manejadorEmpleados;
        IManejadorMateriales manejadorMateriales;

        accion accionEmpleados;
        accion accionMateriales;
        public MainWindow()
        {
            InitializeComponent();

            manejadorEmpleados = new ManejadorEmpleados(new RepositorioGenerico<Empleado>());
            manejadorMateriales = new ManejadorMateriales(new RepositorioGenerico<Material>());

            PonerBotonesEmpleadosEnEdicion(false);
            LimpiarCamposDeEmpleados();
            ActualizarTablaEmpleados();

            PonerBotonesMaterialesEnEdicion(false);
            LimpiarCamposDeMateriales();
            ActualizarTablaDeMateriales();
        }

        private void ActualizarTablaDeMateriales()
        {
            dtgMateriales.ItemsSource = null;
            dtgMateriales.ItemsSource = manejadorMateriales.Listar.OrderBy(p=>p.Nombre);
        }

        private void LimpiarCamposDeMateriales()
        {
            txbMaterialesCategoria.Clear();
            txbMaterialesDescripcion.Clear();
            txbMaterialesId.Text = "";
            txbMaterialesNombre.Clear();
            imgFoto.Source = null;
        }

        private void PonerBotonesMaterialesEnEdicion(bool value)
        {
            btnMaterialesCancelar.IsEnabled = value;
            btnMaterialesEditar.IsEnabled = !value;
            btnMaterialesEliminar.IsEnabled = !value;
            btnMaterialesGuardar.IsEnabled = value;
            btnMaterialesNuevo.IsEnabled = !value;
        }

        private void ActualizarTablaEmpleados()
        {
            dtgEmpleados.ItemsSource = null;
            dtgEmpleados.ItemsSource = manejadorEmpleados.Listar;
        }

        private void LimpiarCamposDeEmpleados()
        {
            txbEmpleadosApellidos.Clear();
            txbEmpleadosArea.Clear();
            txbEmpleadosId.Text = "";
            txbEmpleadosNombre.Clear();
        }

        private void PonerBotonesEmpleadosEnEdicion(bool value)
        {
            btnEmpleadosCancelar.IsEnabled = value;
            btnEmpleadosEditar.IsEnabled = !value;
            btnEmpleadosEliminar.IsEnabled = !value;
            btnEmpleadosGuardar.IsEnabled = value;
            btnEmpleadosNuevo.IsEnabled = !value;
        }

        private void btnEmpleadosNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeEmpleados();
            PonerBotonesEmpleadosEnEdicion(true);
            accionEmpleados = accion.Nuevo;
        }

        private void btnEmpleadosEditar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if (emp != null)
            {
                txbEmpleadosId.Text = emp.Id.ToString();
                txbEmpleadosApellidos.Text = emp.Apellidos;
                txbEmpleadosArea.Text = emp.Area;
                txbEmpleadosNombre.Text = emp.Nombre;
                accionEmpleados = accion.Editar;
                PonerBotonesEmpleadosEnEdicion(true);
            }
        }

        private void btnEmpleadosGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(accionEmpleados== accion.Nuevo)
            {
                Empleado emp = new Empleado()
                {
                    Nombre = txbEmpleadosNombre.Text,
                    Apellidos = txbEmpleadosApellidos.Text,
                    Area = txbEmpleadosArea.Text
                };
                if (manejadorEmpleados.Agregar(emp))
                {
                    MessageBox.Show("Empleado agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeEmpleados();
                    ActualizarTablaEmpleados();
                    PonerBotonesEmpleadosEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("El Empleado no se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Empleado emp = dtgEmpleados.SelectedItem as Empleado;
                emp.Apellidos = txbEmpleadosApellidos.Text;
                emp.Area = txbEmpleadosArea.Text;
                emp.Nombre = txbEmpleadosNombre.Text;
                if (manejadorEmpleados.Modificar(emp))
                {
                    MessageBox.Show("Empleado modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeEmpleados();
                    ActualizarTablaEmpleados();
                    PonerBotonesEmpleadosEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("El Empleado no se pudo actualizar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEmpleadosCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeEmpleados();
            PonerBotonesEmpleadosEnEdicion(false);
        }

        private void btnEmpleadosEliminar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if(emp!=null)
            {
                if(MessageBox.Show("Realmente deseas eliminar este empleado?","Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
                {
                    if (manejadorEmpleados.Eliminar(emp.Id))
                    {
                        MessageBox.Show("Empleado eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActualizarTablaEmpleados();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el empleado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

        }

        private void btnMaterialesNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            accionMateriales = accion.Nuevo;
            PonerBotonesMaterialesEnEdicion(true);
        }

        private void btnMaterialesEditar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            accionMateriales = accion.Editar;
            PonerBotonesMaterialesEnEdicion(true);
            Material m = dtgMateriales.SelectedItem as Material;
            if (m != null)
            {
                txbMaterialesCategoria.Text = m.Categoria;
                txbMaterialesDescripcion.Text = m.Descripcion;
                txbMaterialesId.Text = m.Id.ToString();
                txbMaterialesNombre.Text = m.Nombre;
                imgFoto.Source = ByteToImage(m.Fotografia);
            }
        }
        
        private void btnMaterialesGuardar_Click(object sender, RoutedEventArgs e)
        {
            
            if(accionMateriales== accion.Nuevo)
            {
                Material m = new Material()
                {
                    Categoria = txbMaterialesCategoria.Text,
                    Descripcion = txbMaterialesDescripcion.Text,
                    Nombre = txbMaterialesNombre.Text,
                    Fotografia =  ImageToByte(imgFoto.Source)
                };
                if (manejadorMateriales.Agregar(m))
                {
                    MessageBox.Show("Material correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeMateriales();
                    ActualizarTablaDeMateriales();
                    PonerBotonesMaterialesEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Material m = dtgMateriales.SelectedItem as Material;
                m.Categoria = txbMaterialesCategoria.Text;
                m.Descripcion = txbMaterialesDescripcion.Text;
                m.Nombre = txbMaterialesNombre.Text;
                m.Fotografia = ImageToByte(imgFoto.Source);
                if (manejadorMateriales.Modificar(m))
                {
                    MessageBox.Show("Material correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeMateriales();
                    ActualizarTablaDeMateriales();
                    PonerBotonesMaterialesEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnMaterialesCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeMateriales();
            PonerBotonesMaterialesEnEdicion(false);
        }

        private void btnMaterialesEliminar_Click(object sender, RoutedEventArgs e)
        {
            Material m = dtgMateriales.SelectedItem as Material;
            if (m != null)
            {
                if(MessageBox.Show("¿Realmente deseas eliminar este material?","Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
                {
                    if (manejadorMateriales.Eliminar(m.Id))
                    {
                        MessageBox.Show("Material Eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActualizarTablaDeMateriales();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public ImageSource ByteToImage(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }
            else
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource imgSrc = biImg as ImageSource;
                return imgSrc;
            }
        }

        public byte[] ImageToByte(ImageSource image)
        {
            if (image != null)
            {
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                encoder.Save(memStream);
                return memStream.ToArray();
            }
            else
            {
                return null;
            }
        }
        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Title = "Selecciona una foto";
            dialogo.Filter = "Archivos de imagen|*.jpg; *.png; *.gif";
            if (dialogo.ShowDialog().Value)
            {
                imgFoto.Source = new BitmapImage(new Uri(dialogo.FileName));
            }
        }
    }
}
