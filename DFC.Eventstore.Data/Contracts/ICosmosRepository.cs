using DFC.Eventstore.Data.Models;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Eventstore.Data.Contracts
{
    public interface ICosmosRepository<T>
        where T : class, IDataModel
    {
        Task<HttpStatusCode> CreateAsync(T model);
    }
}