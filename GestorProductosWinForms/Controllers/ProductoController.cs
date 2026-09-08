using GestorProductosWinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GestorProductosWinForms.Controllers
{
    public class ProductoController
    {
        private List<Producto> productos;
        private IRepository<Producto> repositorio;
        private int proximoId = 1;

        public ProductoController(IRepository<Producto> repositorio)
        {
            this.repositorio = repositorio;
            productos = repositorio.LeerTodos();

            if (productos.Count > 0)
            {
                proximoId = productos.Max(p => p.Id) + 1;
            }
            else
            {
                proximoId = 1;
            }
        }

        private void Recargar()
        {
            productos = repositorio.LeerTodos();
        }

        public void AgregarProducto(string nombre, decimal precio, int stock)
        {
            Producto producto = new Producto(nombre, precio, stock);

            producto.Id = proximoId;
            proximoId++;

            //productos.Add(producto);
            repositorio.Agregar(producto);
            Recargar();
        }

        public void Eliminar(int id)
        {
            //productos.RemoveAll(p => p.Id == id);
            repositorio.Eliminar(id);
            Recargar();
            
        }
        
        public void Modificar(Producto modificado)
        {
            /*var p = productos.Find(x => x.Id == modificado.Id);
            if (p == null) return;
            p.Nombre = modificado.Nombre;
            p.Precio = modificado.Precio;
            p.Stock = modificado.Stock;*/

            repositorio.Actualizar(modificado);
            Recargar();
        }

        public List<Producto> BuscarPorNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return ObtenerProductos().ToList();
            }

            return ObtenerProductos()
                .Where(p => p.Nombre != null &&
                            p.Nombre.IndexOf(
                                texto.Trim(),
                                StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public List<Producto> ObtenerProductos()
        {
            return productos.ToList();
        }
    }
}
