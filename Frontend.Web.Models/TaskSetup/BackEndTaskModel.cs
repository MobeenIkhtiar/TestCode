using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.TaskSetup
{
    public class BackEndTaskModel
    {
        public List<BackendTask> BackendTask { get; set; }
        public ItemPostDto PostModel { get; set; }
        public ItemPutDto PutModel { get; set; }
    }
}
