namespace MVC.Core.Data
{
    using System;

    public interface IEntity<Key> where Key : IConvertible
    {
        Key Id { get; set; }

        DateTimeOffset Created { get; set; }

        DateTimeOffset Updated { get; set; }
    }

    /*
    public interface IEntity
    {
        int Id { get; set; }

        DateTimeOffset Created { get; set; }

        DateTimeOffset Updated { get; set; }
    }
    */
}
