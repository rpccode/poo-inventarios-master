using Inventario.COMMON.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorEmpleados:IManejadorGenerico<Empleado>
    {
        List<Empleado> EmpleadosPorArea(string area);
        
    }
}
