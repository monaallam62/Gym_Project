using Gym_Project.Contexts;
using Gym_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext dbContext , string seedFolderPath,ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if(!await dbContext.plans.AnyAsync(ct))
                {

                    var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                    if(plans.Any())
                    {
                        dbContext.plans.AddRange(plans);
                        logger.LogInformation($"Plans Seeded With Count = {plans.Count}");
                    }

                    if (dbContext.ChangeTracker.HasChanges())
                        await dbContext.SaveChangesAsync();
                    else
                        logger.LogInformation("Plan Already Seeded.");

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym Data Seeding Failed");
                throw;
            }
        }
            private static List<T> LoadDataFromJsonFile<T>(string folderPath , string FileName)
            {
            var filePath = Path.Combine(folderPath, FileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed Data File Not Found : {filePath}");

            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}

