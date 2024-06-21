using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;


namespace SQLServerLib;
public class OrderController
{
    private SqlConnection? _sqlConnection { get; set; } = null;
    private static string OrderGetAllSql = "SELECT * FROM ORDERS;";
    private static string OrderGetByPkSql = "SELECT * FROM ORDERS WHERE Id = @Id;";
    private static string OrderCreateSql = "INSERT ORDERS (CustomerId, Date, Description) VALUES (@CustomerId, @Date, @Description);";
    private static string OrderChangeSql = "UPDATE ORDERS SET CustomerId=@CustomerId, Date=@Date, Description=@Description Where Id = @Id;";
    private static string OrderRemoveSql = "DELETE ORDERS Where Id=@Id;";

    public List<Order> GetAll()
    {
        var sql = "SELECT * FROM ORDERS;";
        var cmmd = new SqlCommand(sql, _sqlConnection);
        var reader = cmmd.ExecuteReader();

        List<Order> orders = new List<Order>();

        while (reader.Read())

        {
            var order = SqlConvertToClass(reader);
            //Order order = new Order();
            //order.Id = Convert.ToInt32(reader["Id"]);
            //order.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) //RETURNS TRUE IF NULL, FALSE IF NOT -
            //                                                                    //GetOrdinal evaluates to the integer num that DBNull is looking for
            //? null : Convert.ToInt32(reader["CustomerId"]);
            //order.Date = Convert.ToDateTime(reader["Date"]);
            //order.Description = Convert.ToString(reader["Description"])!;

            orders.Add(order);

        }
        reader.Close();
        return orders;
    }

    public Order? GetByPK(int id)
    {
        var sql = OrderGetByPkSql;
        var cmd = new SqlCommand(sql, _sqlConnection);
        //SET UP PARAMETERS BEFORE YOU EXECUTE THE READER!!
        cmd.Parameters.AddWithValue("@Id", id);
        var reader = cmd.ExecuteReader();
        if(!reader.HasRows)
        {
            reader.Close(); //close the reader
            return null; //not finding a PK is not a good reason to throw an exception
        }
        //if there is one row
        reader.Read();
        var order = SqlConvertToClass(reader);
        reader.Close();
        return order;
    }

    public bool Create(Order newOrder)
    {
        var sql = OrderCreateSql;
        var cmd = new SqlCommand(sql, _sqlConnection);
        cmd.Parameters.AddWithValue("@CustomerId", newOrder.CustomerId); //ADD TO PARAMETERS OF CMD, it will assign the value to the @ (parameter)
        cmd.Parameters.AddWithValue("@Date", newOrder.Date);
        cmd.Parameters.AddWithValue("@Description", newOrder.Description);
        var rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected == 1 ? true : false;

    }

    public bool Change(Order orderToUpdate)
    {
        var sql = OrderChangeSql;
        var cmd = new SqlCommand(sql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", orderToUpdate.Id);
        cmd.Parameters.AddWithValue("@CustomerId", orderToUpdate.CustomerId);
        cmd.Parameters.AddWithValue("@Date", orderToUpdate.Date);
        cmd.Parameters.AddWithValue("@Description", orderToUpdate.Description);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false;
    }

    public bool Remove(int id)
    {
        var sql = OrderRemoveSql;
        var cmd = new SqlCommand(sql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false;

    }

    private Order SqlConvertToClass(SqlDataReader reader)
    {
        Order order = new Order();
        order.Id = Convert.ToInt32(reader["Id"]);
        order.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) //RETURNS TRUE IF NULL, FALSE IF NOT -
                                                                            //GetOrdinal evaluates to the integer num that DBNull is looking for
        ? null : Convert.ToInt32(reader["CustomerId"]);
        order.Date = Convert.ToDateTime(reader["Date"]);
        order.Description = Convert.ToString(reader["Description"])!;
        return order;
    }

    public OrderController(Connection connection)
    {
        if (connection.GetSqlConnection() != null)
        {
            _sqlConnection = connection.GetSqlConnection()!;
        }
    }
}
