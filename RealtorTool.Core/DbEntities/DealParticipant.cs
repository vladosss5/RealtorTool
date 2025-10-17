using RealtorTool.Core.Enums;

namespace RealtorTool.Core.DbEntities;

public class DealParticipant : BaseIdEntity
{
    public string DealId { get; set; } = null!;
    public Deal Deal { get; set; } = null!;

    public string ClientRequestId { get; set; } = null!;
    public ClientRequest ClientRequest { get; set; } = null!;

    public ParticipantRole Role { get; set; }
}