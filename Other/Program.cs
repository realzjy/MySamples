using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Other
{
    class Program
    {
        static void Main(string[] args)
        {
            string ssss = "2017-05-03";
            DataTable t = new DataTable();
            t.Columns.Add("EventDate", typeof(string));
            var r = t.Rows.Add();
            r["EventDate"] = ssss;

            string rt = t.Rows[0].Field<string>("EventDate");
            DateTime time = Convert.ToDateTime(rt);
            DateTime eventTime = DateTime.SpecifyKind(time, DateTimeKind.Local);
            var s1 = eventTime.ToString("yyyy-MM");
            var s2 = (int)eventTime.DayOfWeek;
            var s3 = eventTime.ToString("yyyy-MM-dd");


            return;
            DateTime d = DateTime.Now;
            string ss = d.DayOfWeek.ToString();


            Console.WriteLine(decimal.Add(5m, 2m));
            Console.WriteLine(decimal.Subtract(5m, 2m));
            Console.WriteLine(decimal.Multiply(5m, 2m));
            Console.WriteLine(decimal.Divide(5m, 2m));


            Console.ReadLine();

            AlertInt(0m, 0);

            AlertInt(5m, 5);
            AlertInt(10m, 10);
            AlertInt(1.499m, 1);
            AlertInt(1.5m, 2);
            AlertInt(1.501m, 2);
            AlertInt(10.35m, 10);
            AlertInt(10.499m, 10);
            AlertInt(5.4m, 5);
            AlertInt(5.4535m, 5);
            AlertInt(5.499999999m, 5);
            AlertInt(592.5000000000000000000000001m, 593);
            AlertInt(592.4999999999999999999999999m, 592);

            AlertInt(-5m, -5);
            AlertInt(-10m, -10);
            AlertInt(-1.499m, -1);
            AlertInt(-1.5m, -2);
            AlertInt(-1.501m, -2);
            AlertInt(-10.35m, -10);
            AlertInt(-10.499m, -10);
            AlertInt(-5.4m, -5);
            AlertInt(-5.4535m, -5);
            AlertInt(-5.499999999m, -5);
            AlertInt(-592.5000000000000000000000001m, -593);
            AlertInt(-592.4999999999999999999999999m, -592);


            #region
            //Timer _acceptTimer = new Timer(
            //        (s) =>
            //        {
            //            Console.WriteLine("output");
            //        },
            //        null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            #endregion

            #region
            //ABC a = new ABC();
            //a.OKString = "asdasdasd";

            //AAA(a);

            //ABC a = new ABC();
            //var h = a.GetHashCode();
            //Console.WriteLine(h);

            //FileInfo f = new FileInfo("Other.exe.config");
            //Console.WriteLine(f.FullName + "|"+ f.Exists);

            //Console.WriteLine(f.GetHashCode());
            #endregion

            Console.ReadLine();
        }

        static void AlertInt(decimal para, int result)
        {
            string r = decimal.Parse(result.ToString()) == DecimalRound(para) ? "right" : "wrong";
            string ss = string.Format("{0} get int ought to {1}，actual is{2}，test result is “{3}”", para, result, DecimalRound(para), r);
            Console.WriteLine(ss);
        }

        static decimal DecimalRound(decimal d)
        {
            if (d > 0)
            {
                return (int)(d + 0.5m);
            }
            else if (d < 0)
            {
                return (int)(d - 0.5m);
            }
            else { return 0; }
        }
        //static decimal DecimalRound(decimal d)
        //{
        //    decimal unit = 1m;
        //    decimal rm = d % unit;
        //    decimal result = d - rm;
        //    if (rm >= unit / 2)
        //        result += unit;
        //    return result;
        //}

        static void AAA(ABC @abc)
        {
            Console.WriteLine(@abc.OKString);
        }
    }

    class ABC
    {
        public string OKString { get; set; }
    }
}
