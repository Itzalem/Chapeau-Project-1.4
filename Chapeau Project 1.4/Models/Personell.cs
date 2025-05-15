namespace Chapeau_Project_1._4.Models
{
    public class Personell
    {
        public int Staff_id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }

        //Constructors
        public Personell()
        {
            
        }

        public Personell(int staff_id, string username, string password, string role)
        {
            Staff_id = staff_id;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
