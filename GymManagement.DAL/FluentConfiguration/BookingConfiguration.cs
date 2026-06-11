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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(X => X.Id);
            builder.Property(X => X.CreatedAt)
              .HasColumnName("BookinDate")
              .HasDefaultValueSql("GETDATE()");
            builder.HasKey(X => new { X.MemberId ,X.SessionId}); //Composite PK
        }
    }
}
