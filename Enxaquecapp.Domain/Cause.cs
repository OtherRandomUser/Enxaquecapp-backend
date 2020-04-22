using System;
using Enxaquecapp.Domain.Base;

namespace Enxaquecapp.Domain
{
    public class Cause : Entity
    {
        public string Description { get; protected set; }

        public string Icon { get; set; }

        public Guid UserId { get; protected set; }
        public User User { get; protected set; }

        protected Cause()
        {
        }

        public Cause(User user, string description, string icon)
        {
            SetUser(user);
            SetDescription(description);
            Icon = icon;
        }

        public void SetUser(User user)
        {
            UserId = user?.Id ?? throw new ArgumentNullException("Preencha o usuário");
            User = user;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("O campo descrição precisa conter um valor");

            Description = description;
        }
    }
}