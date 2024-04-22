namespace net23_asp_net_labb3.Models.DTOs;

public class InterestDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<LinkDTO> Links { get; set; } = [];
}
