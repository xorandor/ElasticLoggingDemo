using System;
using System.Collections.Generic;
using System.Linq;
using RandomNameGeneratorLibrary;

namespace LoggerApp.Api
{
    internal class CitizenContainer
    {
        public static IEnumerable<Citizen> GetCitizens(int count)
        {
            var random = new Random();

            var namegenerator = new PersonNameGenerator();

            return Enumerable.Range(0, count).Select(x => new Citizen
            {
                Firstname = namegenerator.GenerateRandomFirstName(),
                Cpr = random.Next(1111111111, 1999999999).ToString(),
                Lastname = namegenerator.GenerateRandomLastName()
            });
        }
    }
}