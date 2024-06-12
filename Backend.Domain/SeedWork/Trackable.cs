using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.SeedWork
{
  public class Trackable
  {
    public Nullable<DateTime> CreatedAt { get; set; }
    public Nullable<DateTime> UpdatedAt { get; set; }
    public Nullable<DateTime> DeletedAt { get; set; }
    public Nullable<long> CreatedBy { get; set; }
    public Nullable<long> UpdatedBy { get; set; }
    public Nullable<long> DeletedBy { get; set; }
  }
}
