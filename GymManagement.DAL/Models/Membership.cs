using Gym_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Membership:BaseEntity
    {
        #region Relationships
        public Member Member { get; set; }
        public int MemberId { get; set; } //-----------int
        public Plan Plan { get; set; }
        public int PlanId { get; set; }     //-----------int

        //StartDate==CreateAt=BaseEntity
        public DateTime EndDate { get; set; }

        //Read Only Property
        //[NotMapped] //Not going to table 
        //EF Core By Default Read Only Prop not map prop to columns(Tables) in DataBase  
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;

        #endregion   
    }
    }
