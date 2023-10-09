using Microsoft.EntityFrameworkCore;
using PeopleManager.Core;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.Model;
using PeopleManager.Services.Extensions;

namespace PeopleManager.Services
{
    public class VehicleService
    {
        private readonly PeopleManagerDbContext _dbContext;

        public VehicleService(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Find
        public async Task<IList<VehicleResult>> FindAsync()
        {
            return await _dbContext.Vehicles
                .ProjectToResults()
                .ToListAsync();
        }

        //Get by id
        public async Task<VehicleResult?> GetAsync(int id)
        {
            return await _dbContext.Vehicles
                .ProjectToResults()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        //Create
        public async Task<VehicleResult?> CreateAsync(VehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Type = request.Type,
                ResponsiblePersonId = request.ResponsiblePersonId
            };

            _dbContext.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            return await GetAsync(vehicle.Id);
        }

        //Update
        public async Task<VehicleResult?> UpdateAsync(int id, VehicleRequest vehicle)
        {
            var dbVehicle = await _dbContext.Vehicles.FindAsync(id);
            if (dbVehicle is null)
            {
                return null;
            }

            dbVehicle.LicensePlate = vehicle.LicensePlate;
            dbVehicle.Brand = vehicle.Brand;
            dbVehicle.Type = vehicle.Type;
            dbVehicle.ResponsiblePersonId = vehicle.ResponsiblePersonId;

            await _dbContext.SaveChangesAsync();

            return await GetAsync(id);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var vehicle = new Vehicle
            {
                Id = id,
                LicensePlate = string.Empty
            };
            _dbContext.Vehicles.Attach(vehicle);

            _dbContext.Vehicles.Remove(vehicle);

            await _dbContext.SaveChangesAsync();
        }
    }
}
