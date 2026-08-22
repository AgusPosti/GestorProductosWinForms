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


        public void agregarProducto(string nombre, decimal precio, int stock)
        {
            Producto producto = new Producto(nombre, precio, stock);

            producto.Id = proximoId;
            proximoId++;

            productos.Add(producto);
        }

        public List<Producto> ObtenerProductos()
        {
            return productos;
        }
    }
}
