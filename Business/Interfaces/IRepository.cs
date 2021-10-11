using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetList(); // получение всех объектов
        Task<T> Get(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        Task<int> Save();  // сохранение изменений
    }
}
