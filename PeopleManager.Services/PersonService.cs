using Microsoft.EntityFrameworkCore;
using PeopleManager.Core;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.Model;
using PeopleManager.Services.Extensions;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;

namespace PeopleManager.Services
{
    public class PersonService
    {
        private readonly PeopleManagerDbContext _dbContext;

        public PersonService(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Find
        public async Task<IList<PersonResult>> FindAsync()
        {
            return await _dbContext.People
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ProjectToResults()
                .ToListAsync();
        }

        //Get by id
        public async Task<PersonResult?> GetAsync(int id)
        {
            return await _dbContext.People
                .ProjectToResults()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        

        //Create
        public async Task<ServiceResult<PersonResult?>> CreateAsync(PersonRequest request)
        {
            if (request.FirstName == "Bavo")
            {
                return new ServiceResult<PersonResult?>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "NoBavos",
                            Title = "We don't serve your kind here, Bavo!",
                            Type = ServiceMessageType.Error
                        }
                    }
                };
            }

            var person = new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Description = request.Description
            };
            _dbContext.Add(person);

            await _dbContext.SaveChangesAsync();

            var personResult = await GetAsync(person.Id);

            return new ServiceResult<PersonResult?>(personResult);
        }

        //Update
        public async Task<ServiceResult<PersonResult?>> UpdateAsync(int id, PersonRequest person)
        {
            var dbPerson = await _dbContext.People.FindAsync(id);
            if (dbPerson is null)
            {
                return new ServiceResult<PersonResult?>().NotFound("person");
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Email = person.Email;
            dbPerson.Description = person.Description;

            await _dbContext.SaveChangesAsync();

            var personResult = await GetAsync(id);
            return new ServiceResult<PersonResult?>(personResult);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var person = new Person
            {
                Id = id,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty
            };
            _dbContext.People.Attach(person);

            _dbContext.People.Remove(person);

            await _dbContext.SaveChangesAsync();
        }
    }
}
