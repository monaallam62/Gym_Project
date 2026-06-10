using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class HealthRecord:BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } 
        public string? Note  { get; set; }
        #region Realationships 1-1(TT)
        public Member Member { get; set; } = default!; //Navigation property
        public int MemberId { get; set; } //FK name of prop+Id 

        #endregion

        //UpdatedAt of BaseEntity => LastUpdated
    }
}
