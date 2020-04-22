using System;
using Enxaquecapp.Domain.Base;

namespace Enxaquecapp.Domain
{
    public class User : Entity
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public DateTime BirthDate { get; protected set; }

        public Sex Sex { get; set; }

        protected User()
        {
        }

        public User(string name, string email, string password, DateTime birthDate, Sex sex)
        {
            SetName(name);
            SetEmail(email);
            SetPassword(password);
            SetBirthDate(birthDate);
            Sex = sex;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"O campo nome precisa possuir um valor");

            Name = name;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException($"O campo e-mail precisa possuir um valor");

            Email = email;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException($"O campo senha precisa possuir um valor");

            Password = password;
        }

        public void SetBirthDate(DateTime birthDate)
        {
            if (birthDate >= DateTime.Today)
                throw new ArgumentException("Data de nascimento não pode ser igual ou maior ao dia atual");

            BirthDate = birthDate;
        }
    }

    public enum Sex
    {
        NotDisclosed,
        Male,
        Female
    }
}