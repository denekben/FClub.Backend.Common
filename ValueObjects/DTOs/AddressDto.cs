namespace FClub.Backend.Common.ValueObjects.DTOs
{
    public sealed record AddressDto(
        string? Country,
        string? City,
        string? Street,
        string? HouseNumber
    );
}
