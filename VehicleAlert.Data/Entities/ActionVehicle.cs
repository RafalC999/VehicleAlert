using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleAlert.Data.Entities
{
    public class ActionVehicle
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Vehicle Vehicle { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        public int LastActionCourse { get; set; }

        public int Interval { get; set; }

        public int KmToServis { get; set; }

        public bool Alert { get; set; }

        public decimal progress { get; set; }

    }
}
