using System;

namespace Enxaquecapp.Domain.Base
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}