using System;

namespace Enxaquecapp.WebApi.InputModels
{
    public class EpisodeUpdateInputModel
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int Intensity { get; set; }
        public bool ReleafWorked { get; set; }
    }
}