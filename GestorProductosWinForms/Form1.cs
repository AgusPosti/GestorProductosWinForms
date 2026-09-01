using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestorProductosWinForms.Controllers;
using GestorProductosWinForms.Models;
using System.IO;

namespace GestorProductosWinForms
{
    public partial class Form1 : Form
    {
        private ProductoController productoController;
        private BindingSource productosSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();

            productoController = new ProductoController();

            dataGridViewProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProductos.MultiSelect = false;
            dataGridViewProductos.AllowUserToAddRows = false;
            dataGridViewProductos.ReadOnly = true;

            dataGridViewProductos.DataSource = productosSource;

            ActualizarTabla();
            ActualizarBotones();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OcultarErorres();
            bool esValido = true;

            string nombre = txtNombre.Text;

            if (string.IsNullOrEmpty(nombre))
            {
                labelNombre.Visible = true;
                esValido = false;
            }
           
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0 || precio >= 10000000)
            {
                labelPrecio.Visible = true;
                esValido = false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock <= 0 || stock >= 10000)
            {
                labelStock.Visible = true;
                esValido = false;
            }

            if (!esValido)
            {
                return;
            }

            if (modoEdicion)
            {
                var confirmar = MessageBox.Show(
                    $"¿Está seguro de que desea modificar el producto {productoEditando.Nombre}?",
                    "Confirmar edición",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar == DialogResult.Yes)
                {
                    productoEditando.Nombre = nombre.Trim();
                    productoEditando.Precio = precio;
                    productoEditando.Stock = stock;

                    productoController.Modificar(productoEditando);
                }

                SalirModoEdicion();
            }
            else
            {
                productoController.AgregarProducto(txtNombre.Text.Trim(), decimal.Parse(txtPrecio.Text), int.Parse(txtStock.Text));
            }
               // productoController.AgregarProducto(nombre, precio, stock);

            ActualizarTabla();
            LimpiarCampos();

        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtNombre.Focus();

            OcultarErorres();
        }

        private void OcultarErorres()
        {
            labelNombre.Visible = false;
            labelPrecio.Visible = false;
            labelStock.Visible = false;
        }

        private void ActualizarTabla()
        {
            var lista = productoController.ObtenerProductos().ToList();

            /*dataGridViewProductos.DataSource = null;
            dataGridViewProductos.DataSource = lista;
                new BindingList<Producto>(lista.ToList());
            
            lblContador.Text = $"{lista.Count} productos";

            btnEditar.Visible = lista.Count > 0;
            btnEliminar.Visible = lista.Count > 0;*/
            /*productosSource.DataSource = lista;
            productosSource.ResetBindings(false);

            lblContador.Text = $"{lista.Count} productos";
            
            btnEditar.Visible = lista.Count > 0;
            btnEliminar.Visible = lista.Count > 0;*/
            MostrarProductos(lista);
        }

        private Producto ObtenerSeleccionado()
        {
            /*if (dataGridViewProductos.SelectedRows.Count == 0)
                return null;
            return dataGridViewProductos.SelectedRows[0].DataBoundItem as Producto;
            */
            /*if (dataGridViewProductos.CurrentRow == null)
                return null;

            return dataGridViewProductos.CurrentRow.DataBoundItem as Producto;*/
            return productosSource.Current as Producto;
        }

        private void dataGridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var producto = dataGridViewProductos.Rows[e.RowIndex];
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var p = ObtenerSeleccionado();
            if (p == null) return;

            var confirmar = MessageBox.Show(
                $"Eliminar {p.Nombre}?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmar == DialogResult.Yes)
            {
                productoController.Eliminar(p.Id);
                ActualizarTabla();
            }
        }

        private void SalirModoEdicion()
        {
            modoEdicion = false;
            productoEditando = null;
            ActualizarBotones();
        }

        private bool modoEdicion = false;
        private Producto productoEditando = null;

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var p = ObtenerSeleccionado();
            if (p == null) return;

            modoEdicion = true;
            productoEditando = p;

            txtNombre.Text = p.Nombre;
            txtPrecio.Text = p.Precio.ToString();
            txtStock.Text = p.Stock.ToString();

            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            btnAgregar.Text = modoEdicion ? "GUARDAR CAMBIOS" : "AGREGAR";
            btnCancelar.Visible = modoEdicion;
            btnEditar.Enabled = !modoEdicion;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            SalirModoEdicion();
            LimpiarCampos();
        }

        private void dataGridViewProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var fila = dataGridViewProductos.Rows[e.RowIndex];

            var producto = fila.DataBoundItem as Producto;

            if (producto == null)
                return;

            if (producto.Stock <= 5)
            {
                fila.DefaultCellStyle.BackColor = Color.LightCoral;
            }
            else if (producto.Stock <= 10)
            {
                fila.DefaultCellStyle.BackColor = Color.Khaki;
            }
            else
            {
                fila.DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var lista = productoController.BuscarPorNombre(txtBuscar.Text);

            MostrarProductos(lista);
        }

        private void MostrarProductos(List<Producto> lista)
        {
            productosSource.DataSource = lista;
            productosSource.ResetBindings(false);

            lblContador.Text = $"{lista.Count} productos";

            btnEditar.Visible = lista.Count > 0;
            btnEliminar.Visible = lista.Count > 0;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductos.Rows.Count == 0)
            {
                MessageBox.Show(
                    "No hay productos para exportar.",
                    "Exportar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            SaveFileDialog guardarArchivo = new SaveFileDialog();

            guardarArchivo.Filter = "Archivo de texto (*.txt)|*.txt";
            guardarArchivo.Title = "Exportar productos";
            guardarArchivo.FileName = "productos.txt";

            if (guardarArchivo.ShowDialog() == DialogResult.OK)
            {
                StringBuilder texto = new StringBuilder();

                // Encabezados
                texto.AppendLine("ID\tNombre\tPrecio\tStock");
                texto.AppendLine("---------------------------------------------");

                // Productos de la tabla
                foreach (DataGridViewRow fila in dataGridViewProductos.Rows)
                {
                    if (fila.IsNewRow)
                        continue;

                    texto.AppendLine(
                        $"{fila.Cells["Id"].Value}\t" +
                        $"{fila.Cells["Nombre"].Value}\t" +
                        $"{fila.Cells["Precio"].Value}\t" +
                        $"{fila.Cells["Stock"].Value}"
                    );
                }

                File.WriteAllText(guardarArchivo.FileName, texto.ToString());

                MessageBox.Show(
                    "Productos exportados correctamente.",
                    "Exportación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
