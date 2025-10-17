namespace RealtorTool.Core.DbEntities;

public class ClientPhoto : Photo
{
    public string ClientId { get; set; } = null!;
    public Client Client { get; set; } = null!;
}