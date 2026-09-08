using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorProductosWinForms.Models
{
    public interface IRepository<T>
         where T : class, IEntidad
    {
        List<T> LeerTodos();
        void GuardarTodos(
        List<T> items);
        T BuscarPorId(int id);
        void Agregar(T item);
        void Actualizar(T item);
        void Eliminar(int id);
    }
}
