using System;

namespace Enxaquecapp.WebApi.ViewModels
{
    public abstract class EntityViewModelBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt{ get; set; }
    }
}