namespace Doodle.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}