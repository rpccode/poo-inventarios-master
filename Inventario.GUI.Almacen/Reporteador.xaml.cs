using System;
using System.Collections.Generic;
using Microsoft.Reporting.WinForms;
using System.Windows;

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para Reporteador.xaml
    /// </summary>
    public partial class Reporteador : Window
    {
        string reporte;
        List<ReportDataSource> origenes;
        bool cargado;
        public Reporteador(string nombreReporte, List<ReportDataSource> datos)
        {
            InitializeComponent();
            reporte = nombreReporte;
            origenes = datos;
            Contenedor.Load += Contenedor_Load;
        }

        private void Contenedor_Load(object sender, EventArgs e)
        {
            if (!cargado)
            {
                Contenedor.LocalReport.ReportEmbeddedResource = reporte;
                foreach (var item in origenes)
                {
                    Contenedor.LocalReport.DataSources.Add(item);
                }
                Contenedor.RefreshReport();
                cargado = true;
            }
        }
    }
}
