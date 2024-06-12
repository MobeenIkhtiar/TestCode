using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.TaskSetup
{
    public class ItemPostDto
    {
        public string Name { get; set; }
    }

    public class ItemPutDto : ItemPostDto
    {
        public long Id { get; set; }
    }
}
