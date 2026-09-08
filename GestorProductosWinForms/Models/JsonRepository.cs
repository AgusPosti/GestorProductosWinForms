using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace GestorProductosWinForms.Models
{
    public class JsonRepository<T> : IRepository<T>
        where T : class, IEntidad
    {
            private string rutaArchivo;
            private JsonSerializerSettings settings;

            public JsonRepository(string rutaArchivo, JsonSerializerSettings settings = null)
            {
                this.rutaArchivo = rutaArchivo;

                this.settings = settings ?? new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };
            }

            public List<T> LeerTodos()
            {
                if (!File.Exists(rutaArchivo))
                {
                    return new List<T>();
                }

                string contenidoJson = File.ReadAllText(rutaArchivo);

                if (string.IsNullOrWhiteSpace(contenidoJson))
                {
                    return new List<T>();
                }

                return JsonConvert.DeserializeObject<List<T>>(contenidoJson, settings)
                       ?? new List<T>();
            }

            public void GuardarTodos(List<T> elementos)
            {
                Persistir(elementos);
            }


            public T BuscarPorId(int id)
            {
                return LeerTodos().FirstOrDefault(item => item.Id == id);
            }


            public void Agregar(T item)
            {
                List<T> items = LeerTodos();

                int nuevoId = items.Count == 0
                    ? 1
                    : items.Max(x => x.Id) + 1;

                item.Id = nuevoId;

                items.Add(item);

                GuardarTodos(items);
            }

            public void Actualizar(T item)
            {
                List<T> items = LeerTodos();

                int indice = items.FindIndex(x => x.Id == item.Id);

                if (indice >= 0)
                {
                    items[indice] = item;
                    GuardarTodos(items);
                }
            }

            public void Eliminar(int id)
            {
                List<T> items = LeerTodos();

                T item = items.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    items.Remove(item);
                    GuardarTodos(items);
                }
            }

            private void Persistir(List<T> items)
            {
                string carpeta = Path.GetDirectoryName(rutaArchivo);

                if (!string.IsNullOrWhiteSpace(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string json = JsonConvert.SerializeObject(items, settings);

                File.WriteAllText(rutaArchivo, json);
            }


    }

}

