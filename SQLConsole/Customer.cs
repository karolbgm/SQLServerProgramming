using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConsole;
public class Customer
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty; //equivalent to ""
    public decimal? Sales { get; set; } = 0; //Sales can be null so the only way to do that is add a '?' after type
    public bool Active { get; set; } = true;
}
