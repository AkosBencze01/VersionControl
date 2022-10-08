using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Entities
{
    class User
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
    }
}
