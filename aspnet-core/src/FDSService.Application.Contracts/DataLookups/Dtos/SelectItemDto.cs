namespace FDSService.DataLookups.Dtos;
public class SelectItemDto
{
    public string Value { get; set; }
    public string Name { get; set; }
    public SelectItemDto()
    {

    }
    public SelectItemDto(string value, string name)
    {
        Value = value;
        Name = name;
    }


}