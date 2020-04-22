using System;
using Enxaquecapp.Domain.Base;

namespace Enxaquecapp.Domain
{
    public class Medication : Entity
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public DateTime Start { get; protected set; }
        public TimeSpan Interval { get; protected set; }
        public int TotalDoses { get; protected set; }
        public int ConsumedDoses { get; protected set; }

        public Guid UserId { get; protected set; }
        public User User { get; protected set; }

        protected Medication()
        {
        }

        public Medication(User user, string name, string description, DateTime start, TimeSpan interval, int totalDoses)
        {
            SetUser(user);
            SetName(name);
            SetDescription(description);
            SetStart(start);
            SetInterval(interval);
            SetTotalDoses(totalDoses);
        }

        public void SetUser(User user)
        {
            UserId = user?.Id ?? throw new ArgumentNullException("Preencha o usuário");
            User = user;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("O campo nome precisa conter um valor");

            Name = name;
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("O campo descrição precisa conter um valor");

            Description = description;
        }

        private void SetStart(DateTime start)
        {
            Start = start;
        }

        private void SetInterval(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("O intervalo não pode ser menor ou igual a zero");

            Interval = interval;
        }

        private void SetTotalDoses(int totalDoses)
        {
            if (totalDoses <= 0)
                throw new ArgumentOutOfRangeException("O número de doses não pode ser menor ou igual a zero");

            TotalDoses = totalDoses;
            ConsumedDoses = (int) Math.Truncate((DateTime.UtcNow - Start).TotalHours / Interval.TotalHours);
        }
    }
}