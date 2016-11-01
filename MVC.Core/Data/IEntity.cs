namespace MVC.Core.Data
{
    using System;

    public interface IEntity
    {
        int Id { get; set; }

        DateTimeOffset Created { get; set; }

        DateTimeOffset Updated { get; set; }
    }
}
