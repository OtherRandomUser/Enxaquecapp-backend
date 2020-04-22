using System;
using Enxaquecapp.Domain.Base;

namespace Enxaquecapp.Domain
{
    public class Episode : Entity
    {
        public DateTime Start { get; protected set; }
        public DateTime? End { get; protected set; }
        public int Intensity { get; protected set; }

        public bool ReleafWorked { get; set; }

        public Guid UserId { get; protected set; }
        public User User { get; protected set; }

        public Guid? LocalId { get; protected set; }
        public Local Local { get; protected set; }

        public Guid? CauseId { get; protected set; }
        public Cause Cause { get; protected set; }

        public Guid? ReliefId { get; protected set; }
        public Relief Relief { get; protected set; }

        protected Episode()
        {
        }

        public Episode(User user, DateTime start, DateTime? end, int intensity, bool releafWorked, Local local = null, Cause cause = null, Relief relief = null)
        {
            SetUser(user);
            SetStart(start);
            SetEnd(end);
            SetIntensity(intensity);

            ReleafWorked = releafWorked;

            SetLocal(local);
            SetCause(cause);
            SetRelief(relief);
        }

        public void SetUser(User user)
        {
            UserId = user?.Id ?? throw new ArgumentNullException($"Preencha o usuário");
            User = user;
        }

        public void SetStart(DateTime start)
        {
            if (End.HasValue && End.Value <= start)
                throw new ArgumentOutOfRangeException("O início de um episódio não pode ser maior que o final do mesmo");

            Start = start;
        }

        public void SetEnd(DateTime? end)
        {
            if (end.HasValue && end.Value <= Start)
                throw new ArgumentOutOfRangeException("O fim de um episódio não pode ser menor que o início do mesmo");

            End = end;
        }

        public void SetIntensity(int intensity)
        {
            if (intensity < 0 || intensity > 10)
                throw new ArgumentOutOfRangeException("A intensidade de um episódio precisa ser um número entre 0 e 10");
        }

        private void SetLocal(Local local)
        {
            if (local != null && local.UserId != UserId)
                throw new ArgumentException("Local precisa pertencer ao mesmo usuário que episódio");

            LocalId = local?.Id;
            Local = local;
        }

        private void SetCause(Cause cause)
        {
            if (cause != null && cause.UserId != UserId)
                throw new ArgumentException("Causa precisa pertencer ao mesmo usuário que episódio");

            CauseId = cause?.Id;
            Cause = cause;
        }

        private void SetRelief(Relief relief)
        {
            if (relief != null && relief.UserId != UserId)
                throw new ArgumentException("Alívio precisa pertencer ao mesmo usuário que episódio");

            ReliefId = relief?.Id;
            Relief = relief;
        }
    }
}