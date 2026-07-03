using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Member:GymUser
    {
        public string? Photo { get; set; }

        #region RelationShips
        public HealthRecord HealthRecord { get; set; } = default!; //Navigation Prop

        public ICollection<Membership> MembershipPlans { get; set; } = default!;

        public ICollection<Booking> MemberSession { get; set; } = default!;
        #endregion
        //JoinDate ==createedAt of BaseEntity
    }
}
