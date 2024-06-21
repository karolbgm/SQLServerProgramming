using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerLib;
public class Order
{
    public int Id { get; set; } = 0; //all numbers default to 0
    public int? CustomerId { get; set; } = null; //we have to do null otherwise, it will give us an error if we try to pass a null value
    public DateTime Date { get; set; } = default(DateTime);
    public string Description { get; set; } = string.Empty;

    public override string ToString() //we can use the ToString method to ANY data type
    {
        return $"{Id,3} {CustomerId,3} {Date,-25}, {Description}";
    }
}
//any column that is allowed to be null in the database, I have to add a question mark to the type in C# in case we get null. 