using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestorProductosWinForms.Controllers;

namespace GestorProductosWinForms
{
    public partial class Form1 : Form
    {
        private ProductoController productoController;
        public Form1()
        {
            InitializeComponent();

            productoController = new ProductoController();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            decimal precio = decimal.Parse(txtPrecio.Text);
            int stock = int.Parse(txtStock.Text);

            productoController.agregarProducto(nombre, precio, stock);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productoController.ObtenerProductos();
        }
    }
}
