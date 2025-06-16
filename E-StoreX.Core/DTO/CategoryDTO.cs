namespace EStoreX.Core.DTO
{
    public record CategoryRequest(string Name, string Description);

    public record CategoryResponse(Guid Id, string Name, string Description);

    public record UpdateCategoryDTO(string Name, string Description, Guid Id);
}
