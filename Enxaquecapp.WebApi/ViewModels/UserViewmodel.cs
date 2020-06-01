using System;
using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class UserViewModel : EntityViewModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public static implicit operator UserViewModel(User user)
        {
            if (user == null)
                return null;

            var gender = default(string);

            switch (user.Gender)
            {
                case Domain.Gender.Male:
                {
                    gender = "Masculino";
                    break;
                }

                case Domain.Gender.Female:
                {
                    gender = "Feminino";
                    break;
                }

                case Domain.Gender.NotDisclosed:
                {
                    gender = "Não Informado";
                    break;
                }
            }

            var now = DateTime.UtcNow;
            var age = now.Year - user.BirthDate.Year;

            if ((user.BirthDate.Month == now.Month && now.Day < user.BirthDate.Day) || now.Month < user.BirthDate.Month)
                age--;

            return new UserViewModel
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Name = user.Name,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Age = age,
                Gender = gender
            };
        }
    }
}