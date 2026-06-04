using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.FluentConfiguration
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(TB=>
            {
                TB.HasCheckConstraint("SessionCapacityCheck", "Capacity between 1 and 25");
                TB.HasCheckConstraint("SessionEndDateCheck", "StartDate < EndDate");
            });
        }
    }
}
