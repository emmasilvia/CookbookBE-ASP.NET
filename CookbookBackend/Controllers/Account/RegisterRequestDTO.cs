using System;

namespace CookbookBackend.Controllers.Account
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }
    }
}
