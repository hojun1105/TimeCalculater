using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculator
{
    public class LoginViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
    
        public List<string> ReturnData()
        {
            return new List<string> { Id, Password };
        }
    }
}
