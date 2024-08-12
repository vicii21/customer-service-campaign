using Bugsnag.Payload;
using CustomerServiceCampaign.DataAccess;
using CustomerServiceCampaign.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Extensions;
using SOAPWebService;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
                _context.Color.Add(new Color { ColorName = color });
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
                    states.Add(person.Home.State);
                }
                if (person.Office?.State != null)
                {
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
                    cities.Add(person.Home.City);
                }
                if (person.Office?.City != null)
                {
                    cities.Add(person.Office.City);
                }
            }

            return cities;
        }

        private void SaveCities(IEnumerable<string> cities)
        {
            foreach (var city in cities)
            {
                _context.City.Add(new City { CityName = city });
            }
            _context.SaveChanges();
        }

        private Domain.Entities.Address CreateAddress(SOAPWebService.Address address)
        {
            return new Domain.Entities.Address
            {
                Street = address.Street,
                City = _context.City.FirstOrDefault(c => c.CityName == address.City),
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

                var newPerson = new Domain.Entities.Person
                {
                    Name = person.Name,
                    SSN = person.SSN,
                    DOB = person.DOB,
                    HomeAddress = CreateAddress(person.Home),
                    OfficeAddress = CreateAddress(person.Office),
                    SpouseId = GetSpouseId(person),
                    Age = (int)person.Age,
                };

                _context.Person.Add(newPerson);
                _context.SaveChanges();
                people.Add(newPerson);
            }
            return people;
        }

        private void SavePeople(List<Domain.Entities.Person> people)
        {
            _context.Person.AddRange(people);
            _context.SaveChanges();
        }

        private int? GetSpouseId(SOAPWebService.Person person)
        {
            if (person.Spouse != null)
            {
                return _context.Person.FirstOrDefault(p => p.Name == person.Spouse.Name)?.ID;
            }
            return (int?)null;
        }

        private List<Agent> ExtractAgents(List<Domain.Entities.Person> people, SOAPDemoSoapClient client)
        {
            var agents = new List<Agent>();
            foreach (var person in people)
            {
                var foundPerson = client.FindPersonAsync(person.ID.ToString()).Result;
                if (foundPerson is SOAPWebService.Employee employee)
                {
                    var agent = new Agent
                    {
                        PersonId = person.ID,
                        Title = employee.Title,
                        Salary = employee.Salary,
                        Notes = employee.Notes,
                    };
                    agents.Add(agent);
                }
            }
            return agents;
        }

        private void SaveAgents(List<Agent> agents)
        {
            _context.Agent.AddRange(agents);
            _context.SaveChanges();
        }

        private List<Customer> ExtractCustomers(List<Domain.Entities.Person> people, SOAPDemoSoapClient client)
        {
            var customers = new List<Customer>();
            foreach (var person in people)
            {
                var foundPerson = client.FindPersonAsync(person.ID.ToString()).Result;
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