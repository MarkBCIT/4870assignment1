using System.ComponentModel.DataAnnotations;

public class RegisterViewModel {
  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

    [Required]
    public string Username { set; get; }

    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string Country { set; get; }
    public string MobileNumber { set; get; }

}
