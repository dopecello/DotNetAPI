using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Empire State Building",
                            Description = "A pretty big tower in the style of Art Deco."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Statue of Liberty",
                            Description = "A pretty big statue that a lot of immigrants first saw as they travelled from Europe."
                        },
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Baltimore",
                    Description = "That one where the Star Spangled Banner was created.",
                     PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Baltimore Aquarium",
                            Description = "A pretty awesome aquarium right by the inner harbor."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Fort McHenry",
                            Description = "This is where the Star-Spangled Banner was created."
                        },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Miami",
                    Description = "That one with all of the parties.",
                     PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "The Freedom Tower",
                            Description = "A tower in the center of downtown that is a symbol for Cuban refugees."
                        },
                         new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Villa Vizcaya",
                            Description = "A nice house right on the water where a rich railroad industrialist livd back in the day."
                        },
                    }
                }
            };
        }

    }
}
