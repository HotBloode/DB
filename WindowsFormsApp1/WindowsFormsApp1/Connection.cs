using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Connection
    {       
        NpgsqlConnection con = new NpgsqlConnection("Server='localhost';Port=5432;User Id=postgres;Password=admin;Database=steam;");
    }
}
