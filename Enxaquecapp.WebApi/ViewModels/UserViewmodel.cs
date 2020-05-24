using System;
using Enxaquecapp.Domain;

namespace Enxaquecapp.WebApi.ViewModels
{
    public class UserViewModel : EntityViewModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }

        public static implicit operator UserViewModel(User user)
        {
            if (user == null)
                return null;

            var sex = default(string);

            switch (user.Sex)
            {
                case Domain.Sex.Male:
                {
                    sex = "Masculino";
                    break;
                }

                case Domain.Sex.Female:
                {
                    sex = "Feminino";
                    break;
                }

                case Domain.Sex.NotDisclosed:
                {
                    sex = "Não Informado";
                    break;
                }
            }

            return new UserViewModel
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Name = user.Name,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Sex = sex
            };
        }
    }
}