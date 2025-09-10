using Constants.Enums;

namespace RegionalRides.DAL.Entities.References;

public class RefKato : BaseEntity
{
    public string Code { get; set; }
    public string NameRu { get; set; }
    public string NameKz { get; set; }
    public int Te { get; set; }
    public int Ab { get; set; }
    public int Cd { get; set; }
    public int Ef { get; set; }
    public int Hij { get; set; }
    public int K { get; set; }
    public virtual RefKato Parent { get; set; }
    public int? ParentId { get; set; }
    public virtual RefKato Region { get; set; }
    public int? RegionId { get; set; }
    public bool IsInactive { get; set; }
    public long BnsExternalId { get; set; }
    public long BnsExternalParId { get; set; }
    public BnsLocationType BnsLocationType { get; set; }
    public virtual ICollection<RefKato> RegionalChilds { get; set; }
    public virtual ICollection<RefKato> Childs { get; set; }

    public RefKato()
    {
        this.Childs = new HashSet<RefKato>();
        this.RegionalChilds = new HashSet<RefKato>();
    }
}
