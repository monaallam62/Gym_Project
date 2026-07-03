using GymManagement.DAL.Models;

namespace Gym_Project.Models
{
    public class Plan:BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }

        #region Relationship
        public ICollection<Membership> Membership  { get; set; } = default!;
        #endregion
    }
}
