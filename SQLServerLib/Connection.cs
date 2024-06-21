using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SQLServerLib;
public class Connection //Supports the whole application
{
    private string _connectionString { get; set; } = string.Empty; //connection string

    private SqlConnection? _sqlConnection { get; set; } = null; //DECLARING instance of SqlConnection (var connection = new SqlConnection(string))

    public SqlConnection? GetSqlConnection() //THIS WILL HELP ME PASS THE CONNECTION TO CUSTOMER CLASS. if it returns a null, it means connection was not created - you can throw an exception here
    {
        return _sqlConnection; //returns the actual connection and that's what we need to make the SQL calls
    }

    public void Open() //Checks to see if connection opened
    {
        _sqlConnection = new SqlConnection(_connectionString); //created the _sqlConnection instance
        _sqlConnection.Open(); //it is doing the opening itself - this one actually makes the connection to the database
        if(_sqlConnection.State != System.Data.ConnectionState.Open)
        {
            _sqlConnection = null;
            throw new Exception("Connection failed to open");
        }
    }

    public void Close()
    {
        _sqlConnection?.Close();
    }

    //When the connection string is created (in Program), we have to pass it to the instance of creating a new Connection 
    //The Connection will store it in this property called _connectionString
    public Connection(string connectionString) //we will pass our connection string to this constructor
    {
        _connectionString = connectionString;
    }



}
