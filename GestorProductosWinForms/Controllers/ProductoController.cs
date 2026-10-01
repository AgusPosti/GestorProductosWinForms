using GestorProductosWinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorProductosWinForms.Controllers
{
    public class ProductoController
    {
        private readonly IRepository<Producto> repositorio;

        public ProductoController(IRepository<Producto> repositorio)
        {
            this.repositorio = repositorio;
        }

        public void AgregarProducto(string nombre, decimal precio, int stock)
        {
            Producto producto = new Producto(nombre, precio, stock);

            repositorio.Agregar(producto);
        }

        public void Eliminar(int id)
        {
            repositorio.Eliminar(id);
        }

        public void Modificar(Producto modificado)
        {
            repositorio.Actualizar(modificado);
        }

        public List<Producto> BuscarPorNombre(string texto)
        {
            List<Producto> productos = repositorio.LeerTodos();

            if (string.IsNullOrWhiteSpace(texto))
            {
                return productos;
            }

            return productos
                .Where(p =>
                    p.Nombre != null &&
                    p.Nombre.IndexOf(
                        texto.Trim(),
                        StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public List<Producto> ObtenerProductos()
        {
            return repositorio.LeerTodos();
        }
    }
}