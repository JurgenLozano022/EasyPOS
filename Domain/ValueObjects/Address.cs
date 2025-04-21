namespace Domain.ValueObjects
{
    public partial record Address
    {
        public Address(string country, string line1, string line2, string city, string state, string zipCode)
        {
            Country = country;
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Country { get; init; } = string.Empty;
        public string Line1 { get; init; } = string.Empty;
        public string Line2 { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string ZipCode { get; init; } = string.Empty;

        public static Address? Create(string country, string line1, string line2, string city, string state, string zipCode)
        {
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(line1) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(zipCode))
                return null;

            return new Address(country, line1, line2, city, state, zipCode);
        }
    }  
}
