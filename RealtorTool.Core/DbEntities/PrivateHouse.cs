namespace RealtorTool.Core.DbEntities;

public class PrivateHouse : Realty
{
    public int? RoomsCount { get; set; }

    public decimal? TotalArea { get; set; }

    public int? FloorsCount { get; set; }

    public bool? HasGarage { get; set; }

    public bool? HasBasement { get; set; }

    public string HeatingTypeId { get; set; }
    
    public DictionaryValue? HeatingType { get; set; }
    
    public string ConstructionMaterialId { get; set; }

    public DictionaryValue? ConstructionMaterial { get; set; }

    public int? YearBuilt { get; set; }
}