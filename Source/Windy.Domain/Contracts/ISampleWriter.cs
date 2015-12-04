using System.Threading.Tasks;

namespace Windy.Domain.Contracts
{
    public interface ISampleWriter<TEntity> where TEntity : class
    {
        Task Write(TEntity entity);
    }
}
