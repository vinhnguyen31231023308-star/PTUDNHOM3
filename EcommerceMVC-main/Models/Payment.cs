using System.ComponentModel.DataAnnotations;
using EcommerceMVC.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public string CardType { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;

    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
}
