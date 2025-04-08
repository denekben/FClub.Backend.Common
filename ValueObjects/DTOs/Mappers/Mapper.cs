namespace FClub.Backend.Common.ValueObjects.DTOs.Mappers
{
    public static class Mapper
    {
        public static AddressDto AsDto(this Address address)
        {
            return new(address.Country, address.City, address.Street, address.HouseNumber);
        }

        public static FullNameDto AsDto(this FullName fullName)
        {
            return new(fullName.FirstName, fullName.SecondName, fullName.Patronymic);
        }
    }
}
