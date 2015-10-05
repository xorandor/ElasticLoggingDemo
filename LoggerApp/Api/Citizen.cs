namespace LoggerApp.Api
{
    internal class Citizen
    {
        public string Cpr { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public override string ToString()
        {
            return $"Citizen {Cpr}, {Firstname} {Lastname}";
        }
    }
}