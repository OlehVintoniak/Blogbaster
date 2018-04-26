namespace Blogbaster.Core.Data.Entities.Abstract
{
    public interface IEntity
    {
    }

    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }
}