using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb3.Models;

public class People
{
    public int Id { get; set; }
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }
    [Range(0, 150, ErrorMessage = "Age must be between 0 and 150")]
    public int Age { get; set; }
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Enter a phone number with 10 digits")]
    public string PhoneNumber { get; set; }
    public ICollection<PeopleInterest> PeopleInterests { get; set; } = [];
}
