using System;

namespace Enxaquecapp.WebApi.InputModels
{
    public class EpisodeInputModel
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int Intensity { get; set; }
        public bool ReleafWorked { get; set; }

        public Guid? LocalId { get; set; }
        public Guid? CauseId { get; set; }
        public Guid? ReliefId { get; set; }
    }
}