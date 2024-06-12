using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Dto.BackendSetup
{
    public class BackendTaskPostDto
    {
        public string Name { get; set; }
    }
    public class BackendTaskPutDto : BackendTaskPostDto
    {
        public long Id { get; set; }
    }
}
