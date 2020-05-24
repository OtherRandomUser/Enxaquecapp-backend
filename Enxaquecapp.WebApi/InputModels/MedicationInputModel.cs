using System;

namespace Enxaquecapp.WebApi.InputModels
{
    public class MedicationInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Interval { get; set; }
        public int TotalDoses { get; set; }
    }
}