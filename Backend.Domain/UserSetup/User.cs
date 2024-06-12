using Backend.Domain.SeedWork;

namespace Backend.Domain.UserSetup
{
  public class User : BaseEntity
  {
    

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public Nullable<bool> EmailConfirmed { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PasswordHash { get; set; }
    public string?  PhoneNumber { get; set; }


    public Nullable<DateTime> LastLoggedIn { get; set; }
    public string? SerialNumber { get; set; }


    
  }
}
