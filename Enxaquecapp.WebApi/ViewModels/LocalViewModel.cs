using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class LocalViewModel : EntityViewModelBase
    {
        public string Description { get; set; }

        public string Icon { get; set; }

        public static implicit operator LocalViewModel(Local local)
        {
            if (local == null)
                return null;

            return new LocalViewModel
            {
                Id = local.Id,
                CreatedAt = local.CreatedAt,
                Description = local.Description,
                Icon = local.Icon
            };
        }
    }
}