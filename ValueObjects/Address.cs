using FClub.Backend.Common.Exceptions;
using System.Text.RegularExpressions;

namespace FClub.Backend.Common.ValueObjects
{
    public sealed record Address
    {
        private static readonly Regex _countryPattern = new(@"^[A-Za-zА-Яа-я\s\-']{2,}$");
        private static readonly Regex _cityPattern = new(@"^[A-Za-zА-Яа-я\s\-']{2,}$");
        private static readonly Regex _streetPattern = new(@"^[A-Za-zА-Яа-я0-9\s\.,\-']{3,}$");
        private static readonly Regex _houseNumberPattern = new(@"^[0-9]+[A-Za-zА-Яа-я]*(?:[\/\-кК][0-9]+[A-Za-zА-Яа-я]*)?$");

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }

        private Address(string? country, string? city, string? street, string? houseNumber)
        {
            Country = country;
            City = city;
            Street = street;
            HouseNumber = houseNumber;
        }

        public static Address Create(string? country, string? city, string? street, string? houseNumber)
        {
            if (country != null && !_countryPattern.IsMatch(country))
                throw new DomainException($"Invalid value for Address[country]. Entered value {country}");
            if (city != null && !_cityPattern.IsMatch(city))
                throw new DomainException($"Invalid value for Address[city]. Entered value {city}");
            if (street != null && !_streetPattern.IsMatch(street))
                throw new DomainException($"Invalid value for Address[street]. Entered value {street}");
            if (houseNumber != null && !_houseNumberPattern.IsMatch(houseNumber))
                throw new DomainException($"Invalid value for Address[houseNumber]. Entered value {houseNumber}");

            return new(country, city, street, houseNumber);
        }

        public override string ToString()
        {
            var addressParts = new List<string>();

            if (Country != null) addressParts.Add(Country);
            if (City != null) addressParts.Add(City);
            if (Street != null) addressParts.Add(Street);
            if (HouseNumber != null) addressParts.Add(HouseNumber.ToString());

            string result = string.Join(", ", addressParts);
            return result.TrimEnd(',', ' ');
        }
    }
}
