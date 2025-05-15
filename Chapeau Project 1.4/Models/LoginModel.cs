namespace Chapeau_Project_1._4.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        
        //Constructors
        public LoginModel() { }

        public LoginModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
