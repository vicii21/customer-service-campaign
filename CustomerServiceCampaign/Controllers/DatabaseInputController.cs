using Bugsnag.Payload;
using CustomerServiceCampaign.DataAccess;
using CustomerServiceCampaign.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Extensions;
using SOAPWebService;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Packaging.Signing;

namespace CustomerServiceCampaign.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseInputController : Controller
    {
        private readonly CustomerServiceCampaignContext _context;

        public DatabaseInputController(CustomerServiceCampaignContext context)
        {
            _context = context;
        }

        // POST: Import data from SOAP Web Service into Database
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ImportDBDataFromSOAP()
        {
            try
            {
                var client = new SOAPWebService.SOAPDemoSoapClient();

                var colors = ExtractColors(client);
                SaveColors(colors);

                var states = ExtractStates(client);
                SaveStates(states);

                var cities = ExtractCities(client);
                SaveCities(cities);

                var people = ExtractPeople(client);
                SavePeople(people);

                var agent = ExtractAgents(people, client);
                SaveAgents(agent);

                var customer = ExtractCustomers(people, client);
                SaveCustomers(customer);

                SaveServices();
                AssignUseCase(1, 101);

                return Ok("Data successfully imported into database");
            } catch (System.Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }

        }

        private HashSet<string> ExtractColors(SOAPDemoSoapClient client)
        {
            var colors = new HashSet<string>();
            for (int i = 1; ; i++)
            {
                var person = client.FindPersonAsync(i.ToString()).Result;
                if (person == null) break;
                if (person.FavoriteColors != null)
                {
                    foreach (var color in person.FavoriteColors)
                    {
                        if (_context.Color.Any(e => e.ColorName == color)) continue;
                        colors.Add(color);
                    }
                }
            }

            return colors;
        }

        private void SaveColors(IEnumerable<string> colors)
        {
            foreach (var color in colors)
            {
                _context.Color.Add(new Domain.Entities.Color { ColorName = color });
            }
            _context.SaveChanges();
        }

        private HashSet<string> ExtractStates(SOAPDemoSoapClient client)
        {
            var states = new HashSet<string>();
            for (int i = 1; ; i++)
            {
                var person = client.FindPersonAsync(i.ToString()).Result;
                if (person == null) break;
                if (person.Home?.State != null)
                {
                    if (_context.State.Any(e => e.StateName == person.Home.State)) continue;
                    states.Add(person.Home.State);
                }
                if (person.Office?.State != null)
                {
                    if (_context.State.Any(e => e.StateName == person.Office.State)) continue;
                    states.Add(person.Office.State);
                }
            }

            return states;
        }

        private void SaveStates(IEnumerable<string> states)
        {
            foreach (var state in states)
            {
                _context.State.Add(new State { StateName = state });
            }
            _context.SaveChanges();
        }

        private HashSet<string> ExtractCities(SOAPDemoSoapClient client)
        {
            var cities = new HashSet<string>();
            for (int i = 1; ; i++)
            {
                var person = client.FindPersonAsync(i.ToString()).Result;
                if (person == null) break;
                if (person.Home?.City != null)
                {
                    if (_context.City.Any(e => e.CityName == person.Home.City)) continue;
                    cities.Add(person.Home.City);
                }
                if (person.Office?.City != null)
                {
                    if (_context.City.Any(e => e.CityName == person.Office.City)) continue;
                    cities.Add(person.Office.City);
                }
            }

            return cities;
        }

        private void SaveCities(IEnumerable<string> cities)
        {
            try
            {
                foreach (var city in cities)
                {
                    _context.City.Add(new City { CityName = city });
                }
                _context.SaveChanges();
            } catch (System.Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private Domain.Entities.Address CreateAddress(SOAPWebService.Address address)
        {
            return new Domain.Entities.Address
            {
                Street = address.Street,
                City = _context.City.FirstOrDefault(c => c.CityName == address.City),
                State = _context.State.FirstOrDefault(c => c.StateName == address.State),
                Zip = address.Zip,
            };
        }

        private List<Domain.Entities.Person> ExtractPeople(SOAPDemoSoapClient client)
        {
            var people = new List<Domain.Entities.Person>();
            for (int i = 1; ; i++)
            {
                var person = client.FindPersonAsync(i.ToString()).Result;
                if (person == null) break;
                if (_context.Person.Any(e => e.SSN == person.SSN)) continue;
                var newPerson = new Domain.Entities.Person
                {
                    Name = person.Name,
                    SSN = person.SSN,
                    DOB = person.DOB,
                    HomeAddress = CreateAddress(person.Home),
                    OfficeAddress = CreateAddress(person.Office),
                    SpouseId = (person.Spouse != null ? _context.Person.Where(x => x.Name == person.Spouse.Name)
                    .Select(x => x.ID)
                    .FirstOrDefault() : null),
                    Age = (int)person.Age,
                };

                people.Add(newPerson);
            }
            return people;
        }

        private void SavePeople(List<Domain.Entities.Person> people)
        {
            _context.Person.AddRange(people);

            foreach (var person in people)
            {
                if (person.SpouseId.HasValue)
                {
                    var existingPerson = _context.Person.Find(person.ID);
                    if (existingPerson != null)
                    {
                        existingPerson.SpouseId = person.SpouseId;
                        _context.Entry(existingPerson).State = EntityState.Modified;
                    }
                }
            }
            _context.SaveChanges();
        }


        //private ICollection<Domain.Entities.PersonColor> ExtractPersonColors(IEnumerable<string> favoriteColors)
        //{
        //    if (favoriteColors == null) return new HashSet<Domain.Entities.PersonColor>();

        //    var personColors = new HashSet<Domain.Entities.PersonColor>();

        //    foreach (var colorName in favoriteColors)
        //    {
        //        var color = _context.Color.FirstOrDefault(c => c.ColorName == colorName);

        //        if (color == null)
        //        {
        //            color = new Domain.Entities.Color { ColorName = colorName };
        //            _context.Color.Add(color);
        //            _context.SaveChanges();
        //        }

        //        personColors.Add(new Domain.Entities.PersonColor
        //        {
        //            ColorId = color.ID,
        //        });
        //    }

        //    return personColors;
        //}

        private int? GetSpouseId(SOAPWebService.Person person)
        {
            return (person.Spouse != null ? _context.Person.Where(e => e.Name == person.Spouse.Name)
                                                                                 .Select(e => e.ID)
                                                                                 .FirstOrDefault() : null);
        }

        private List<Agent> ExtractAgents(List<Domain.Entities.Person> people, SOAPDemoSoapClient client)
        {
            var agents = new List<Agent>();

            foreach (var person in people) { 
                if (person == null) continue;

                if (person != null && !_context.Agent.Any(e => e.Person.SSN == person.SSN))
                {
                    var newAgent = new Agent
                    {
                        Person = person,
                        Title = person.Agent.Title,
                        Salary = person.Agent.Salary,
                        Notes = person.Agent.Notes
                    };

                    agents.Add(newAgent);
                }
            }
            return agents;
        }

        private void SaveAgents(List<Agent> agents)
        {
            var validPersonIds = _context.Person.Select(p => p.ID).ToHashSet();

            var validAgents = agents.Where(a => validPersonIds.Contains(a.PersonId)).ToList();

            if (validAgents.Any())
            {
                _context.Agent.AddRange(validAgents);
                _context.SaveChanges();
            }
        }

        private List<Customer> ExtractCustomers(List<Domain.Entities.Person> people, SOAPDemoSoapClient client)
        {
            var customers = new List<Customer>();
            foreach (var person in people)
            {
                var foundPerson = client.FindPersonAsync(person.ID.ToString()).Result;
                if (_context.Customer.Any(e => e.PersonId == person.ID)) continue;
                if (foundPerson is not SOAPWebService.Employee employee)
                {
                    var customer = new Customer
                    {
                        PersonId = person.ID,
                    };
                    customers.Add(customer);
                }
            }
            return customers;
        }

        private void SaveCustomers(List<Customer> customers)
        {
            _context.Customer.AddRange(customers);
            _context.SaveChanges();
        }

        private string GenerateEmail(string name)
        {
            var parts = name.Split(new[]
            {
                ' ', ','
            },
            StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 3)
            {
                return string.Empty;
            }

            string lastName = parts[0].ToLower();
            string firstName = parts[1].ToLower();
            string middleInitial = parts[2].Substring(0, 1).ToLower();

            return $"{lastName}.{middleInitial}.{firstName}@gmail.com";
        }

        private void CreateAndSaveCredentials(Domain.Entities.Person person)
        {
            var credentials = new Credentials
            {
                Email = GenerateEmail(person.Name),
                Password = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                Person = person,
            };

            _context.Credentials.Add(credentials);
            _context.SaveChanges();
        }

        private void SaveServices()
        {
            var services = new List<Service>
            {
                new Service { ServiceName = "Phone Line providing", Price = 100m },
                new Service { ServiceName = "Optical internet 100/50", Price = 90m },
                new Service { ServiceName = "Optical internet 200/100", Price = 130m },
                new Service { ServiceName = "Optical internet 500/200", Price = 160m },
                new Service { ServiceName = "TV channels 200", Price = 80m },
                new Service { ServiceName = "TV channels 250", Price = 110m },
                new Service { ServiceName = "TV channels 300", Price = 150m },
                new Service { ServiceName = "Mobile unlimited calls and messages, internet 5GB", Price = 30m },
                new Service { ServiceName = "Mobile unlimited calls and messages, internet 10GB", Price = 55m },
                new Service { ServiceName = "Mobile unlimited calls and messages, internet 15GB", Price = 105m },
            };
            _context.Service.AddRange(services);
            _context.SaveChanges();
        }

        private void AssignUseCase(int useCaseId, int personId)
        {
            var personUseCase = new PersonUseCase { UseCaseId = useCaseId, PersonId = personId };
            _context.PersonUseCase.Add(personUseCase);
            _context.SaveChanges();
        }
    }
}