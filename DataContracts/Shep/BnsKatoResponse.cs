namespace DataContracts.Shep;

public class BnsKatoResponse
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string NameRu { get; set; }
    public string NameKz { get; set; }
    public long ParId { get; set; }
    public string ParentCode { get; set; }
    public bool IsActual { get; set; }
    public string BegDateString { get; set; }
    public string EndDateString { get; set; }
    public string ChDate { get; set; }
    public int LocationNumber { get; set; }
    public string LocationName { get; set; }
}