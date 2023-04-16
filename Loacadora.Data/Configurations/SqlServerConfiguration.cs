using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loacadora.Data.Configurations
{
    public class SqlServerConfiguration
    {
        public static string ConnectionString
            => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BD_locadora;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        //public static string ConnectionString 
        //=> @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BD_locadora;Integrated Security=True;Connect Timeout=30;Encrypt=False";
    }
}
