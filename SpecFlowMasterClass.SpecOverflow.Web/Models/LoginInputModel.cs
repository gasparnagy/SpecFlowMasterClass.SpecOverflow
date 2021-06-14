namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    public class LoginInputModel
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public override string ToString()
            => $"Name: {Name}, Password: {Password}";
    }
}
