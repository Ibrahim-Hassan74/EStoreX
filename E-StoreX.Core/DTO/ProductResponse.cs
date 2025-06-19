namespace EStoreX.Core.DTO
{
    public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal? Price,
    string CategoryName,
    IEnumerable<PhotoResponse> Photos);

}
