using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLDBCon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("中文呢？");
            Console.ReadLine();
            string constr = "server=localhost;User Id=root;password=;Database=mysqldb1";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            MySqlCommand mycmd = new MySqlCommand("insert into users values(2,'程序进入','OK了')", mycon);
            if (mycmd.ExecuteNonQuery() > 0)
            {
                Console.WriteLine("数据插入成功！");
            }
            Console.ReadLine();
            mycon.Close();
        }
    }
}
