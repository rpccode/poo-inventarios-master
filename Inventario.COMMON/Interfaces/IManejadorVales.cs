using Inventario.COMMON.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorVales:IManejadorGenerico<Vale>
    {
        List<Vale> ValesPorLiquidar();
        List<Vale> ValesEnIntervalo(DateTime inicio, DateTime fin);
        IEnumerable BuscarNoEntregadorPorEmpleado(Empleado empleado);
    }
}
