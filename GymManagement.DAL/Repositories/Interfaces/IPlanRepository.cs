using Gym_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        //GetAll Plans 
        Task<IEnumerable<Plan>> GetAllAsync(bool tracking=false , CancellationToken ct = default); //GETALL cummunication with the database to get all the plans
        //GetPlanById
        Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default); //GETBYID cummunication with the database to get a specific plan by its id
        //ADD
        Task<int> AddAsync(Plan plan, CancellationToken ct = default); //ADD cummunication with the database to add a new plan
        //UPDATE
        Task UpdateAsync(Plan plan, CancellationToken ct = default); //UPDATE cummunication with the database to update an existing plan
        //DELETE
        Task<int> DeleteAsync(Plan plan, CancellationToken ct = default); //DELETE cummunication with the database to delete a plan by its id
    }
}
