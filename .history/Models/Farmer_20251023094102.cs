using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy_Connect.Models
{
   public class Farmer
{
    public int FarmerId { get; set; }
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Location { get; set; }
    public string? ApplicationUserId { get; set; } // must match table
}

}
