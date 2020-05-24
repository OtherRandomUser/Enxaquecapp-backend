using System;
using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class EpisodeViewModel : EntityViewModelBase
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int Intensity { get; set; }
        public bool ReleafWorked { get; set; }

        LocalViewModel Local { get; set; }
        public CauseViewModel Cause { get; set; }
        public ReliefViewModel Relief { get; set; }

        public static implicit operator EpisodeViewModel(Episode episode)
        {
            if (episode == null)
                return null;

            return new EpisodeViewModel
            {
                Start = episode.Start,
                End = episode.End,
                Intensity = episode.Intensity,
                ReleafWorked = episode.ReleafWorked,
                Local = (LocalViewModel) episode.Local,
                Cause = (CauseViewModel) episode.Cause,
                Relief = (ReliefViewModel) episode.Relief
            };
        }
    }
}