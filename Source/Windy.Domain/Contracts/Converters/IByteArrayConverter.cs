namespace Windy.Domain.Contracts.Converters
{
    public interface IByteArrayConverter<TEntity> where TEntity : class
    {
        byte[] ConvertToBytes(TEntity entity);

        TEntity ConvertFromBytes(byte[] byteArray);
    }
}
