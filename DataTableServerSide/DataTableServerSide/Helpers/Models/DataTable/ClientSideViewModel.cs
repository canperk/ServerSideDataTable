using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Helpers
{
    public class ClientSideViewModel
    {
        public ClientSideViewModel()
        {
            Models = new List<ClientSideModel>();
        }
        public string FormId { get; set; }
        public string ContainerId { get; set; }
        public List<ClientSideModel> Models { get; set; }
        public string TableName { get; set; }
        public string GetAddress { get; set; }
        public string SaveAction { get; set; }
        public string FormTab { get; set; }
    }
}
