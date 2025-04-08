using FClub.Backend.Common.Exceptions;
using System.Text.RegularExpressions;

namespace FClub.Backend.Common.ValueObjects
{
    public sealed record FullName
    {
        private static readonly Regex _firstNamePattern = new(@"^([А-ЯЁ][а-яё]{1,}|[A-Z][a-z]{1,})$");
        private static readonly Regex _secondNamePattern = new(@"^([А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)*|[A-Z][a-z]+(?:['-][A-Z][a-z]+)*)$");
        private static readonly Regex _patronymicPattern = new(@"^([А-ЯЁ][а-яё]{4,}(ович|овна|евич|евна|ич|инична))?$");

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? Patronymic { get; set; }

        private FullName(string firstName, string secondName, string? patronymic)
        {
            FirstName = firstName;
            SecondName = secondName;
            Patronymic = patronymic;
        }

        public static FullName Create(string firstName, string secondName, string? patronymic)
        {
            if (string.IsNullOrWhiteSpace(firstName) || !_firstNamePattern.IsMatch(firstName))
                throw new DomainException($"Invalid value for AppUser[firstName]. Entered value {firstName}");
            if (string.IsNullOrWhiteSpace(secondName) || !_secondNamePattern.IsMatch(secondName))
                throw new DomainException($"Invalid value for AppUser[secondName]. Entered value {secondName}");
            if (patronymic != null && !_patronymicPattern.IsMatch(patronymic))
                throw new DomainException($"Invalid value for AppUser[patronymic]. Entered value {patronymic}");

            return new FullName(firstName, secondName, patronymic);
        }

        public override string ToString()
        {
            return SecondName + " " + FirstName + (Patronymic == null ? "" : (" " + Patronymic));
        }
    }
}
