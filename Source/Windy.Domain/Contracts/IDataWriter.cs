using System.Threading.Tasks;

namespace Windy.Domain.Contracts
{
    public interface IDataWriter<TEntity> where TEntity : class
    {
        Task Write(TEntity entity);
    }
}
