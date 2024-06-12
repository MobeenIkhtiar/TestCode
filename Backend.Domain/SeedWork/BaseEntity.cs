using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.SeedWork
{
  public class BaseEntity : Trackable
  {
    [Key]
    public long Id { get; set; }
    public Nullable<int> SrNo { get; set; }
   
    public bool IsActive { get; set; }
    public Nullable<bool> IsDeleted { get; set; }
  }
}
