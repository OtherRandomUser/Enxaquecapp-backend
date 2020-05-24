using System;

namespace Enxaquecapp.WebApi.InputModels
{
    public class EpisodeUpdateInputModel
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
        public bool ClearEnd { get; set; } = false;

        public int? Intensity { get; set; }
        public bool? ReleafWorked { get; set; }

        public Guid? LocalId { get; set; }
        public bool ClearLocal { get; set; } = false;

        public Guid? CauseId { get; set; }
        public bool ClearCause { get; set; } = false;

        public Guid? ReliefId { get; set; }
        public bool ClearRelief { get; set; } = false;
    }
}