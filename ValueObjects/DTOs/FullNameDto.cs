namespace FClub.Backend.Common.ValueObjects.DTOs
{
    public sealed record FullNameDto(
        string FirstName,
        string SecondName,
        string? Patronymic
    );
}
