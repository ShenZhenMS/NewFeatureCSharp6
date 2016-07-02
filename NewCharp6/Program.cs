#define ASYNC
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
namespace NewCharp6
{
    class Program
    {

        static void Main(string[] args)
        {

            //Initilizers for auto properties and function with lambda
            var salesOrder = new SalesOrder();
            Console.WriteLine("Begin to Test New In C# 6.0");
            Console.WriteLine("=====================Auto Init Property  lambda In A class=======================");
            Console.WriteLine("OrderNo:" + salesOrder.OrderNo);
            Console.WriteLine("OrderCode:" + salesOrder.OrderCode);
            Console.WriteLine("OrderInfo:" + salesOrder.OrderInfo);
            Console.WriteLine("GetOrderInfo:" + salesOrder.GetOrderInfo());

            Console.WriteLine("=====================Test Null Conditional Operators=======================");
            Console.WriteLine("Here Will Output Null Value.............");
            SalesInovice salesInvoice = new SalesInovice();
            Console.WriteLine(salesInvoice.OrderList?[0].OrderNo);//sample Operator
            Console.WriteLine(salesInvoice.salesReturn?.returnCode);//sample Operator

            int i = 1;
            //Initializers the Orders And Retutn
            Console.WriteLine("Here Will Output Actual Value After Initializer the Object.............");
            salesOrder.OrderNo = Guid.NewGuid();
            salesOrder.OrderCode = "OS0001";
            salesInvoice.OrderList = new List<SalesOrder>();
            salesInvoice.OrderList.Add(salesOrder);

            salesInvoice.salesReturn = new SalesReturn();
            salesInvoice.salesReturn.returnNo = Guid.NewGuid();
            salesInvoice.salesReturn.returnCode = "RT0001";

            Console.WriteLine(salesInvoice.OrderList?[0].OrderNo + ":" + salesInvoice.OrderList?[0].OrderCode);
            Console.WriteLine(salesInvoice.salesReturn?.returnNo + ":" + salesInvoice.salesReturn?.returnCode);

            //Index Initializers
            Console.WriteLine("Initializer An Collections With Index .............");
            var returnDictionary = new Dictionary<int, SalesReturn>() { [0] = new SalesReturn() { returnNo = Guid.NewGuid(), returnCode = "RT001" }, [1] = new SalesReturn { returnNo = Guid.NewGuid(), returnCode = "RT002" } };
            Console.WriteLine("Here Will Output the Dictionary：" + returnDictionary[0]?.returnNo + returnDictionary[0]?.returnCode);

            var returnStrDictionary = new Dictionary<String, SalesReturn>() { ["One"] = new SalesReturn { returnNo = Guid.NewGuid(), returnCode = "RT001" }, ["Two"] = new SalesReturn { returnNo = Guid.NewGuid(), returnCode = "RT002" } };
            Console.WriteLine("Here Will Output the Dictionary：" + returnStrDictionary["One"]?.returnNo + returnStrDictionary["One"]?.returnCode);

            //Here Will Output All Object in the Dictionarys 
            foreach (KeyValuePair<string, SalesReturn> item in returnStrDictionary)
            {
                Console.WriteLine("Key:" + item.Key + string.Format("   {0}-{1}", item.Value.returnNo, item.Value.returnCode));
            }


            //Here will Output the math function Directly Before we Will use System.Math.abs But Now Just use abs 
            Console.WriteLine("=====================Using static NameSpace=======================");
            Console.WriteLine("using static System.Math at the head firse");
            Console.WriteLine("Output the abs value:" + Abs(-7));

            //Exception Filter In C# 6.0

#if ASPNET
            Console.WriteLine("=====================Exception Filter In C# 6.0=======================");
            try
            {
                try
                {
                    Console.WriteLine("begin to throw ASPNET EXCEPTION.........");
                    throw new ExceptionHander("ASP.Net Exception");
                }
                catch (ExceptionHander ex) when (ExceptionHander.CheckEx(ex))
                {//ASPNET EXCEPTION WILL NOT CATCH HERE
                    Console.WriteLine("Exception Type:" + ex.Message + "Was Catch the Other was Missing .......");
                }
            }
            catch (ExceptionHander ex)
            {//ASPNET EXCEPTION WILL CATCH HERE
                Console.WriteLine(ex.Message);
            }
#elif DATABASE

            try
            {
                try
                {
                    Console.WriteLine("begin to throw DATABASE EXCEPTION.........");
                    throw new ExceptionHander((int)ExType.DATABASE, "DataBase Exception");
                }
                catch (ExceptionHander ex) when (ExceptionHander.CheckEx(ex))
                {
                    Console.WriteLine("Exception Type:" + ex.Message + "Was Catch the Other was Missing .......");
                }
            }
            catch (ExceptionHander ex)
            {//DATABASE EXCEPTION WILL CATCH HERE
                Console.WriteLine(ex.Message);
            }

#elif ASYNC
            Console.WriteLine("========Begin To Test async in catch and finally blocks(C#6.0 Only)===============");
            try
            {
                throw new Exception("Error Occur");
            }
            catch (Exception ex)
            {
                //TODO can't use the async In Mian 
                // var returnAsync = await ProcessWrite(ex.Message);
            }
#endif

            Console.ReadKey();
        }

        protected static SalesOrder GetSales()
        {
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.OrderCode = "SO001";
            return salesOrder;
        }


        //async read and write
        public async void ProcessWrite(string text)
        {
            string filePath = System.Environment.CurrentDirectory + "commonLog.txt";
            await WriteTextAsync(filePath, text);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        public async void ProcessRead()
        {
            string filePath = @"temp2.txt";

            if (File.Exists(filePath) == false)
            {
                Debug.WriteLine("file not found: " + filePath);
            }
            else
            {
                try
                {
                    string text = await ReadTextAsync(filePath);
                    Debug.WriteLine(text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }

    }

    public class SalesOrder
    {
        public Guid OrderNo { get; set; } = new Guid();
        public String OrderCode { get; set; }

        public Int32 Quanlity { get; set; }

        public double UnitPrice { get; set; }

        public String OrderInfo => string.Format("lambda to property demo {0}:{1}", OrderNo, OrderCode);//lambda for properyies 

        public String GetOrderInfo() => string.Format("lambda to function demo {0}:{1}", OrderNo, OrderCode);//lambda for functions 
    }




    public class SalesInovice
    {
        public Guid InvoiceNo { get; set; }
        public String InvoiceCode { get; set; }
        public List<SalesOrder> OrderList { get; set; }

        public SalesReturn salesReturn { get; set; }
    }

    public class SalesReturn
    {
        public Guid returnNo { get; set; }
        public String returnCode { get; set; }
    }

    public class ExceptionHander : Exception
    {
        public int TypeofException { get; set; } = (int)ExType.ASPNET;
        public string Msg { get; set; }

        public ExceptionHander(int Type, string Message) : this(Message)
        {
            TypeofException = Type;
        }

        public ExceptionHander(String msg)
        {
            Msg = msg;
        }

        public static Boolean CheckEx(ExceptionHander ex)
        {
            if (ex.TypeofException.Equals(ExType.DATABASE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public enum ExType
    {
        DATABASE = 0,
        ASPNET = 1
    }
}
