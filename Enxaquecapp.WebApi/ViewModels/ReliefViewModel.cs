using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class ReliefViewModel : EntityViewModelBase
    {
        public string Description { get; set; }

        public string Icon { get; set; }

        public static implicit operator ReliefViewModel(Relief relief)
        {
            if (relief == null)
                return null;

            return new ReliefViewModel
            {
                Id = relief.Id,
                CreatedAt = relief.CreatedAt,
                Description = relief.Description,
                Icon = relief.Icon
            };
        }
    }
}