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
        private int proximoId = 1;

        public ProductoController()
        {
            productos = new List<Producto>();
        }


        public void AgregarProducto(string nombre, decimal precio, int stock)
        {
            Producto producto = new Producto(nombre, precio, stock);

            producto.Id = proximoId;
            proximoId++;

            productos.Add(producto);
        }

        public void Eliminar(int id)
        {
            productos.RemoveAll(p => p.Id == id);
        }
        
        public void Modificar(Producto modificado)
        {
            var p = productos.Find(x => x.Id == modificado.Id);
            if (p == null) return;
            p.Nombre = modificado.Nombre;
            p.Precio = modificado.Precio;
            p.Stock = modificado.Stock;
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
            return productos;
        }
    }
}
