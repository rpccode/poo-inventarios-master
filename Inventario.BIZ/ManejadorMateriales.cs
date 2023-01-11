using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManejadorMateriales:IManejadorMateriales
    {
        IRepositorio<Material> repositorio;
        public ManejadorMateriales(IRepositorio<Material> repositorio)
        {
            this.repositorio = repositorio;
        }

        public List<Material> Listar => repositorio.Read;

        public bool Agregar(Material entidad)
        {
            return repositorio.Create(entidad);
        }

        public Material BuscarPorId(ObjectId id)
        {
            return Listar.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Eliminar(ObjectId id)
        {
            return repositorio.Delete(id);
        }

        public List<Material> MaterialesDeCategoria(string categoria)
        {
            return Listar.Where(e => e.Categoria == categoria).ToList();
        }

        public bool Modificar(Material entidad)
        {
            return repositorio.Update(entidad);
        }
    }
}
