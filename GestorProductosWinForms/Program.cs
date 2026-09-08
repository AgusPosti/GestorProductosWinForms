using GestorProductosWinForms.Controllers;
using GestorProductosWinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorProductosWinForms
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //INSTANCIAS   
            IRepository<Producto> repositorio =
                new JsonRepository<Producto>("datos/producto.json");

            ProductoController pController =
                new ProductoController(repositorio);

            Application.Run(new Form1(pController));
        }
    }
}
