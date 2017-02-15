namespace MVC.Core.Entities
{
    using System;
    using Data;

    public abstract class BaseEntity<Key> : IEntity<Key> where Key : IConvertible
    {
        public virtual Key Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }
    }

    /*
    public abstract class BaseEntity : IEntity
	{
		public int Id { get; set; }

		public DateTimeOffset Created { get; set; }

		public DateTimeOffset Updated { get; set; }
	}
    */
}
