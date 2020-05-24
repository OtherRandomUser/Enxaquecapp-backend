using System;

namespace Enxaquecapp.WebApi.InputModels
{
    public class UserUpdateInputModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }
    }
}