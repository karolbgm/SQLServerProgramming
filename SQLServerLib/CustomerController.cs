using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SQLServerLib;
public class CustomerController //ALL the methods we do 
    //The Controller class will make the calls to our database

{
    private SqlConnection? _sqlConnection { get; set; } = null;

    public List<Customer> GetAll()
    {
        var sql = "SELECT * FROM CUSTOMERS;";
        var command = new SqlCommand(sql, _sqlConnection);
        var reader = command.ExecuteReader();

        List<Customer> customers = new List<Customer>();
        while(reader.Read())
        {
            Customer customer = new Customer();
            ConvertToCustomer(customer, reader);
            //customer.Id = Convert.ToInt32(reader["Id"]);
            //customer.Name = Convert.ToString(reader["Name"])!;
            //customer.City = Convert.ToString(reader["City"])!;
            //customer.State = Convert.ToString(reader["State"])!;
            //customer.Sales = Convert.ToDecimal(reader["Sales"]);
            //customer.Active = Convert.ToBoolean(reader["Active"]);

            customers.Add(customer);


        }
        reader.Close(); 
        return customers;

    }

    public Customer? GetByPK(int Id)
    {
        var sql = $"SELECT * FROM CUSTOMERS WHERE Id = {Id};";
        var command = new SqlCommand(sql, _sqlConnection);
        var reader = command.ExecuteReader();

        if (!reader.HasRows)
        {
            reader.Close();
            return null;
        }

        reader.Read();
        Customer customer = new Customer();
        ConvertToCustomer(customer, reader);
        //customer.Id = Convert.ToInt32(reader["Id"]);
        //customer.Name = Convert.ToString(reader["Name"])!;
        //customer.City = Convert.ToString(reader["City"])!;
        //customer.State = Convert.ToString(reader["State"])!;
        //customer.Sales = Convert.ToDecimal(reader["Sales"]);
        //customer.Active = Convert.ToBoolean(reader["Active"]);
        reader.Close();
        return customer;
    }

    public bool Create(Customer customer)
    {
        //var sql = $" INSERT INTO Customers (Name, City, State, Sales, Active) " +
        //    $" VALUES ('{customer.Name}', '{customer.City}', '{customer.State}', {customer.Sales}, {(customer.Active ? 1 : 0)}); ";
        var sql = $" INSERT INTO Customers (Name, City, State, Sales, Active) VALUES " +
            " (@Name, @City, @State, @Sales, @Active);";
        var command = new SqlCommand(sql, _sqlConnection);
        //BETTER WAY OF DOING THIS:
        //command.Parameters.AddWithValue("@Id", customer.Id);

        CustomerSqlParemeters(command, customer);

        //command.Parameters.AddWithValue("@Name", customer.Name);
        //command.Parameters.AddWithValue("@City", customer.City);
        //command.Parameters.AddWithValue("@State", customer.State);
        //command.Parameters.AddWithValue("@Sales", customer.Sales);
        //command.Parameters.AddWithValue("@Active", customer.Active);


        var rowsAffected = command.ExecuteNonQuery(); //this will return an int

       return rowsAffected == 1 ? true : false;

    }

    public bool Update(Customer customer)
    {
        var sql = $" UPDATE Customers SET Name = @Name, City = @City, State = @State, Sales = @Sales, Active = @Active WHERE Id = @Id;";
        var command = new SqlCommand(sql, _sqlConnection);

        CustomerSqlParemeters(command, customer);
        //command.Parameters.AddWithValue("@Id", customer.Id);
        //command.Parameters.AddWithValue("@Name", customer.Name);
        //command.Parameters.AddWithValue("@City", customer.City);
        //command.Parameters.AddWithValue("@State", customer.State);
        //command.Parameters.AddWithValue("@Sales", customer.Sales);
        //command.Parameters.AddWithValue("@Active", customer.Active);

        var rowsAffected = command.ExecuteNonQuery(); //this will return an int

        return rowsAffected == 1 ? true : false;

    }

    public bool Remove(Customer customer)
    {
        var sql = $" DELETE FROM Customers WHERE Id = @Id;";
        var command = new SqlCommand(sql, _sqlConnection);

        command.Parameters.AddWithValue("@Id", customer.Id);

        var rowsAffected = command.ExecuteNonQuery(); //this will return an int

        return rowsAffected == 1 ? true : false;

    }

    public List<Customer> FindByStr(string myStr)
    {
        var sql = $" SELECT * FROM Customers WHERE Name Like '%{myStr}%'";
        var command = new SqlCommand(sql, _sqlConnection);
        var reader = command.ExecuteReader();
        List<Customer> customers = new List<Customer>();
        while (reader.Read())
        {
            Customer customer = new Customer();
            ConvertToCustomer(customer, reader);
            //customer.Id = Convert.ToInt32(reader["Id"]);
            //customer.Name = Convert.ToString(reader["Name"])!;
            //customer.City = Convert.ToString(reader["City"])!;
            //customer.State = Convert.ToString(reader["State"])!;
            //customer.Sales = Convert.ToDecimal(reader["Sales"]);
            //customer.Active = Convert.ToBoolean(reader["Active"]);

            customers.Add(customer);


        }
        reader.Close();
        return customers;


    }

    private void CustomerSqlParemeters(SqlCommand command, Customer customer)
    {
        command.Parameters.AddWithValue("@Id", customer.Id);
        command.Parameters.AddWithValue("@Name", customer.Name);
        command.Parameters.AddWithValue("@City", customer.City);
        command.Parameters.AddWithValue("@State", customer.State);
        command.Parameters.AddWithValue("@Sales", customer.Sales);
        command.Parameters.AddWithValue("@Active", customer.Active);
    }
    private void ConvertToCustomer(Customer customer, SqlDataReader reader)
    {
        customer.Id = Convert.ToInt32(reader["Id"]);
        customer.Name = Convert.ToString(reader["Name"])!;
        customer.City = Convert.ToString(reader["City"])!;
        customer.State = Convert.ToString(reader["State"])!;
        customer.Sales = Convert.ToDecimal(reader["Sales"]);
        customer.Active = Convert.ToBoolean(reader["Active"]);
    }

    public CustomerController(Connection connection)
    {
        if (connection.GetSqlConnection() != null)
        {
            _sqlConnection = connection.GetSqlConnection()!;
        }
    }
}



