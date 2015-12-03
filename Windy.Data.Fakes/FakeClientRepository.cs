using System;
using System.Collections.Generic;
using System.Linq;
using Windy.Domain.Contracts;
using Windy.Domain.Entities;

namespace Windy.Data.Fakes
{
    public class FakeClientRepository : IClientRepository
    {
        public List<Client> GetAllClients()
        {
            var millId = 1;
            var generators = Generator.Generators.ToArray();
            return new List<Client> {
                new Client { Id = 1, Name="Egersund Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Egersund", Longitude= 58.430409, Latitude =5.863582  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Egersund", Longitude= 58.430294, Latitude =5.863649  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Egersund", Longitude= 58.430213, Latitude =5.863776  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Egersund", Longitude= 58.430119, Latitude =5.863983  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Egersund", Longitude= 58.430063, Latitude =5.864110  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Egersund", Longitude= 58.430058, Latitude =5.864109  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Egersund", Longitude= 58.430053, Latitude =5.864108  } },
                    }
                },
                new Client { Id = 2, Name="Karmøy Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Karmøy", Longitude= 59.204889, Latitude =5.166571  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Karmøy", Longitude= 59.204776, Latitude =5.166563  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Karmøy", Longitude= 59.204663, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Karmøy", Longitude= 59.204528, Latitude =5.166545  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Karmøy", Longitude= 59.204402, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Karmøy", Longitude= 59.204412, Latitude =5.166552  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Karmøy", Longitude= 59.204417, Latitude =5.166548  } },
                    }
                },
                new Client { Id = 3, Name="Måløy Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Måløy", Longitude= 61.204528, Latitude =5.166545  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Måløy", Longitude= 61.204889, Latitude =5.166571  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Måløy", Longitude= 61.204776, Latitude =5.166563  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Måløy", Longitude= 61.204663, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Måløy", Longitude= 61.204402, Latitude =5.166554  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Måløy", Longitude= 61.204412, Latitude =5.166558  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Måløy", Longitude= 61.204418, Latitude =5.166562  } },
                    }
                },
                new Client { Id = 4, Name="Havøya Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Havøygavlen", Longitude= 71.021194, Latitude =24.545339  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Havøygavlen", Longitude= 71.019698, Latitude =24.545270  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Havøygavlen", Longitude= 71.090022, Latitude =24.545409  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Havøygavlen", Longitude= 71.018413, Latitude =24.545825  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Havøygavlen", Longitude= 71.017782, Latitude =24.545894  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Havøygavlen", Longitude= 71.017788, Latitude =24.545898  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Havøygavlen", Longitude= 71.017791, Latitude =24.545890  } },
                    }
                },
                new Client { Id = 5, Name="Røst Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Røst", Longitude= 67.524194, Latitude =12.118339  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Røst", Longitude= 67.524698, Latitude =12.118270  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Røst", Longitude= 67.524022, Latitude =12.118409  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Røst", Longitude= 67.524413, Latitude =12.118825  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Røst", Longitude= 67.524782, Latitude =12.118894  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Røst", Longitude= 67.524785, Latitude =12.118897  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Røst", Longitude= 67.524789, Latitude =12.118899  } },
                    }
                },
                new Client { Id = 6, Name="Løvund Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Løvund", Longitude= 66.366194, Latitude =12.359339  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Løvund", Longitude= 66.366698, Latitude =12.359270  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Løvund", Longitude= 66.366022, Latitude =12.359409  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Løvund", Longitude= 66.366413, Latitude =12.359825  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Løvund", Longitude= 66.366782, Latitude =12.359894  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Løvund", Longitude= 66.366788, Latitude =12.359897  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Løvund", Longitude= 66.366792, Latitude =12.359899  } },
                    }
                },

                new Client { Id = 7, Name="Andenes Kommune", Windmills= new List<Windmill> {
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Andenes", Longitude= 69.325759, Latitude =16.096826  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Andenes", Longitude= 69.325761, Latitude =16.096830  } },
                    new Windmill { Id = millId++, Generator = generators[2], Location = new Location { Name="Andenes", Longitude= 69.325766, Latitude =16.096835  } },
                    new Windmill { Id = millId++, Generator = generators[0], Location = new Location { Name="Andenes", Longitude= 69.325770, Latitude =16.096840  } },
                    new Windmill { Id = millId++, Generator = generators[1], Location = new Location { Name="Andenes", Longitude= 69.325775, Latitude =16.096845  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Andenes", Longitude= 69.325780, Latitude =16.096850  } },
                    new Windmill { Id = millId++, Generator = generators[3], Location = new Location { Name="Andenes", Longitude= 69.325785, Latitude =16.096855  } },
                    }
                },
            };
        }
    }
}
