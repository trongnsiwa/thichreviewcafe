using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Locations")]
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentLocationId { get; set; }
        public Location? ParentLocation { get; set; }
        public List<Location> ChildLocations { get; set; } = new List<Location>();
    }
}