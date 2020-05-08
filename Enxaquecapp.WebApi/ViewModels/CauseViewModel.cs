using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class CauseViewModel : EntityViewModelBase
    {
        public string Description { get; set; }

        public string Icon { get; set; }

        public static implicit operator CauseViewModel(Cause cause)
        {
            if (cause == null)
                return null;

            return new CauseViewModel
            {
                Id = cause.Id,
                CreatedAt = cause.CreatedAt,
                Description = cause.Description,
                Icon = cause.Icon
            };
        }
    }
}