using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManejadorVales : IManejadorVales
    {
        IRepositorio<Vale> repositorio;
        public ManejadorVales(IRepositorio<Vale> repositorio)
        {
            this.repositorio = repositorio;
        }
        public List<Vale> Listar => repositorio.Read;

        public bool Agregar(Vale entidad)
        {
            return repositorio.Create(entidad);
        }

        public IEnumerable BuscarNoEntregadorPorEmpleado(Empleado empleado)
        {
            return repositorio.Read.Where(p => p.Solicitante.Id == empleado.Id && p.FechaEntregaReal==null).OrderBy(p=>p.FechaEntrega);
        }

        public Vale BuscarPorId(ObjectId id)
        {
            return Listar.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Eliminar(ObjectId id)
        {
            return repositorio.Delete(id);
        }

        public bool Modificar(Vale entidad)
        {
            return repositorio.Update(entidad);
        }

        public List<Vale> ValesEnIntervalo(DateTime inicio, DateTime fin)
        {
            throw new NotImplementedException();
        }

        public List<Vale> ValesPorLiquidar()
        {
            return repositorio.Read.Where(p => p.FechaEntregaReal == null).ToList();
        }
    }
}
