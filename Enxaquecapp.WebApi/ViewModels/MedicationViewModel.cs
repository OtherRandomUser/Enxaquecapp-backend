using System;
using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class MedicationViewModel : EntityViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Interval { get; set; }
        public int TotalDoses { get; set; }
        public int ConsumedDoses { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator MedicationViewModel(Medication medication)
        {
            if (medication == null)
                return null;

            return new MedicationViewModel
            {
                Id = medication.Id,
                CreatedAt = medication.CreatedAt,
                Name = medication.Name,
                Description = medication.Description,
                Start = medication.Start,
                Interval = medication.Interval,
                TotalDoses = medication.TotalDoses,
                ConsumedDoses = medication.ConsumedDoses,
                IsActive = medication.IsActive
            };
        }
    }
}