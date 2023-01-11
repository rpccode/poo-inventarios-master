using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Empleado:Base
    {
        
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Area { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", Nombre, Apellidos);
        }
    }
}
