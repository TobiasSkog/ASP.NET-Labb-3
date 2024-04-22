namespace net23_asp_net_labb3.Models.DTOs;

public class PeopleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public List<InterestDTO> Interests { get; set; } = [];
}
