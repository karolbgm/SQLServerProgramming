using Microsoft.Data.SqlClient;
using SQLServerLib;
using System.Reflection.Metadata;

namespace SQLConsole;

internal class Program
{
    static void Main(string[] args)
    {
        //LearningCode(); //if you want to run it again

        //********CONNECTION********
        var connectionString = "server=localhost\\sqlexpress;" +  //where is the server/what instance
            "database=SalesDb;" + //which database within the instance
            "trusted_connection=true;" +
            "trustServerCertificate=true;";

        Connection connection = new Connection(connectionString);
        connection.Open();




        //********CUSTOMERS********

        //GETTING ALL CUSTOMERS
        CustomerController controller = new CustomerController(connection);

        //foreach(var custy in controller.GetAll())
        //{
        //    Console.WriteLine(custy.Name);
        //}

        //GETTING CUSTOMER BY PRIMARY KEY
        var customer = controller.GetByPK(14);
        //Console.WriteLine($"Id 1 is {customer!.Name}");

        //INSERTING A NEW CUSTOMER
        //var customer2 = new SQLServerLib.Customer { Id = 0, Name = "MAX", City = "Cincinnati", State = "OH", Sales = 1000, Active = true };
        //var added = controller.Create(customer2);
        //Console.WriteLine($"Insert succeed: {added}");
        //connection.Close();

        //UPDATING A CUSTOMER 
        //customer.City = "Karoland";
        //customer.State = "CA";
        //var changed = controller.Update(customer);
        //Console.WriteLine($"Did it succeed? {changed}");
        //customer = controller.GetByPK(15);
        //Console.WriteLine($"Id {customer.Id}, Name: {customer.Name}, City: {customer.City}, State: {customer.State}");


        //REMOVING CUSTOMER
        //var deleted = controller.Remove(customer);
        //Console.WriteLine($"Did it succeed? {deleted}");

        ////GETTING ALL CUSTOMERS filtering by String
        //CustomerController controller2 = new CustomerController(connection);

        //foreach (var custy in controller2.FindByStr("K"))
        //{
        //    Console.WriteLine(custy.Name);
        //}






        //********ORDERS********

        //GETTING ALL ORDERS
        OrderController orderController = new OrderController(connection);

        //foreach (var order in orderController.GetAll())
        //{
        //    //Console.WriteLine($"Order Id: {order.Id}, Customer Id: {order.CustomerId}, Date: {order.Date}, Description: {order.Description}");
        //    Console.WriteLine(order);
        //}


        //GETTING ORDER BY PRIMARY KEY
        var order2 = orderController.GetByPK(28);
        if (order2 == null)
        {
            Console.WriteLine("Order not found");
        }
        Console.WriteLine(order2);


        //INSERTING A NEW ORDER
        //var newOrder = new Order
        //{
        //    Id = 0,
        //    CustomerId = 1,
        //    Date = new DateTime(2024, 6, 21),
        //    Description = "Karol's order"
        //};
        //var rc = orderController.Create(newOrder);
        //if(rc)
        //{
        //    Console.WriteLine("Created.");
        //} else
        //{
        //    Console.WriteLine("Failed to create.");
        //}


        //UPDATING AN ORDER
        //order2.CustomerId = 2;
        //order2.Date = new DateTime(2024, 7, 1);
        //order2.Description = "Karol Dev - Order";
        //var changed = orderController.Change(order2);
        //if (changed)
        //{
        //    Console.WriteLine($"Order {order2.Id} updated!");
        //} else
        //{
        //    Console.WriteLine("Failed to update.");
        //}


        //REMOVING ORDER
        //var deleted = orderController.Remove(order2.Id);
        //Console.WriteLine($"did it succeed? {deleted}");


    }
    static void LearningCode() { 
        var connectionString = "server=localhost\\sqlexpress;" +  //where is the server/what instance
            "database=SalesDb;" + //which database within the instance
            "trusted_connection=true;" + 
            "trustServerCertificate=true;";
        //WHERE IS THE DB - localhost(my machine) "server="
        //WHAT INSTANCE - "\\sqlexpress;"
        //DATABASE - "database=SalesDb;"
        //AUTHENTICATION - "trusted_connection=true;" OR User Id = [username]; Password = [password]; 

        var connection = new SqlConnection(connectionString); //connection instance

        connection.Open();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            throw new Exception("The connection didn't open!");
        }

        Console.WriteLine("Connection opened...");

        //ExecuteReader - for SELECT statements
        //ExecuteScalar - for ANY OTHER statement that IS NOT SELECT
        //ExecuteNonQuery - same as before

        //1 - INSTANTIATE THE QUERY
        var sql = "SELECT * FROM Customers;";
        var sqlcommand = new SqlCommand(sql, connection);
        //2 - EXECUTE
        var reader = sqlcommand.ExecuteReader(); //reader is a CLASS, not just a piece of data -- it will return the data
        //Check if data came back
        if (!reader.HasRows)
        {
            Console.WriteLine("The Customer returned no rows...");
        }

        //Create a dictionary instance to store all customers 
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();

        //A while loop allows us to read EACH ROW at a time until there's no more data and loop ends
        while (reader.Read()) //
        {
            Customer customer = new Customer();
            //var id = reader["Id"]; // [COLUMN NAME] -- it returns an object (parent of all types, you can store anything in it)
            customer.Id = Convert.ToInt32(reader["Id"]);
            customer.Name = Convert.ToString(reader["Name"])!; //we have to add a ! to say "this might be null"
            customer.City = Convert.ToString(reader["City"])!;
            customer.State = Convert.ToString(reader["State"])!;
            customer.Sales = Convert.ToDecimal(reader["Sales"]);
            customer.Active = Convert.ToBoolean(reader["Active"]);

            customers.Add(customer.Id, customer);

            //Console.WriteLine($"Id: {customer.Id} | Name: {customer.Name} | Sales: {customer.Sales:C}");

            //Customer customer = new Customer { Id = id, Name = name, Sales = sales };
            //customers.Add(customer.Id, customer);

        }


        reader.Close(); //only one is allowed at a time, so we need to close it

        connection.Close(); //we need to close it because it takes a lot of resource
    }
}


//DBNull.Value ---> representation of SQL null values in C#
//if in your result set, there's a column that might be null, YOU HAVE TO CHECK THEM INDIVIDUALLY!

//student.MajorId = reader.IsDbNull("MajorId")
//? (int?) null
//: Convert.ToInt32(reader["MajorId"]);

//executeScalar() - everything except SELECT, it reads only one row and one column
//executeNonQuery() - it's NOT a select statement (insert, update, delete, create, etc) - it returns an integer number of how many rows got affected

//EXAMPLE

//Update Customer Set
// Name = 'ACME'
//Where Id = 123

//var affected = sqlCmd.ExecuteNonQuery()
//if(affected != 1) 
//throw new Exception("Update Failed");