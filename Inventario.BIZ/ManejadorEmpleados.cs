using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManejadorEmpleados:IManejadorEmpleados
    {
        IRepositorio<Empleado> repositorio;
        public ManejadorEmpleados(IRepositorio<Empleado> repo)
        {
            repositorio = repo;
        }

        public List<Empleado> Listar => repositorio.Read.OrderBy(p=>p.Nombre).ToList();

        public bool Agregar(Empleado entidad)
        {
            return repositorio.Create(entidad);
        }

        

        public Empleado BuscarPorId(MongoDB.Bson.ObjectId id)
        {
            return Listar.Where(e => e.Id == id).SingleOrDefault();
        }

       

        public bool Eliminar(MongoDB.Bson.ObjectId id)
        {
            return repositorio.Delete(id);
        }

        public bool Eliminar(LiteDB.ObjectId id)
        {
            throw new NotImplementedException();
        }

        public List<Empleado> EmpleadosPorArea(string area)
        {
            return Listar.Where(e => e.Area == area).ToList();
        }

        public bool Modificar(Empleado entidad)
        {
            return repositorio.Update(entidad);
        }
    }
}
