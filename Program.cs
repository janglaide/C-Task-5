using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


/*
  Програмне забезпечення співробітника податкової інспекції . 
  У базі даних міститься наступна інформація: відомості про податки : шифр податку , назва податку , 
  назва нормативного документа , ставка податку; відомості про платників податків : обліковий номер, 
  вид платника податків (наприклад , приватний підприємець , юридична особа і т.д.), адресу, 
  перелік виплачуваних податків (із зазначенням суми для кожного податку).  
  Кожен платник податків може виплачувати кілька податків
*/
namespace Lab5
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();
                    Console.WriteLine("Connection!");

                    Console.WriteLine("\n\t\t\tMENU\n1. SELECT\n2. INSERT\n3. UPDATE\n4. DELETE\n5. Exit");
                    var key = Console.ReadKey().KeyChar;
                    Console.Clear();
                    switch (key)
                    {
                        case '1':
                            Select(connection);
                            break;
                        case '2':

                            break;
                        case '3':
                            break;
                        case '4':
                            break;
                        case '5':
                            connection.Close();
                            Console.WriteLine("Connection closed...");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Incorrect input...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
        }
        static void Select(SqlConnection connection)
        {
            var flag = true;
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                var expression = "SELECT * FROM ";
                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                switch (key)
                {
                    case '1':
                        expression += "Payers";
                        adapter = new SqlDataAdapter(expression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '2':
                        expression += "Taxes";
                        adapter = new SqlDataAdapter(expression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '3':
                        expression += "Register";
                        adapter = new SqlDataAdapter(expression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '4':
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
            
        }
        static void TablesOut(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine(dt.TableName); // название таблицы
                                                 // перебор всех столбцов
                for(var i = 0; i < dt.Columns.Count; i++)
                {
                    if(i == 0)
                        Console.Write(dt.Columns[i].ColumnName + "\t");
                    else
                        Console.Write(dt.Columns[i].ColumnName + "\t\t");
                }
                Console.WriteLine();
                // перебор всех строк таблицы
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    for(var j = 0; j < dt.Rows[i].ItemArray.Length; j++)
                    {
                        if (j == 0)
                            Console.Write(dt.Rows[i].ItemArray[j] + "\t");
                        else
                            Console.Write(dt.Rows[i].ItemArray[j] + "\t\t");
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Insert(SqlConnection connection)
        {
            var flag = true;
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                var expression = "INSERT INTO ";
                switch (key)
                {
                    case '1':
                        Console.WriteLine("\nName: ");
                        var name = Console.ReadLine();
                        expression += "Payers (Name, Surname, [Payer Type], Address, Income) VALUES ('" + name + "', '";
                        Console.WriteLine("\nSurname (if its company, enter '0'): ");
                        string surname = Console.ReadLine();
                        if(surname == "0")
                        {
                            surname = " ";
                        }
                        expression += surname + "', '";
                        var payerFlag = true;
                        while (payerFlag)
                        {
                            Console.WriteLine("\nPayer Type:\n1. Private entrepreneur\n2. An individual\n3. Legal entity");
                            var payerKey = Console.ReadKey().KeyChar;
                            switch (payerKey)
                            {
                                case '1':
                                    expression += "private entrepreneur', '";
                                    payerFlag = false;
                                    break;
                                case '2':
                                    expression += "an individual', '";
                                    payerFlag = false;
                                    break;
                                case '3':
                                    expression += "legal entity', '";
                                    payerFlag = false;
                                    break;
                                default:
                                    Console.WriteLine("Incorrect input");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                            }
                        }
                        Console.WriteLine("\nAddress: ");
                        var address = Console.ReadLine();
                        expression += address + "', ";
                        Console.WriteLine("\nIncome: ");
                        var income = Double.Parse(Console.ReadLine());
                        expression += income.ToString() + ")";
                        
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '2':
                        expression += "Taxes (Name, [Law Document], Percentage) VALUES (";
                        
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '3':
                        expression += "Register (PayerId, TaxId, Amount) VALUES (";
                        
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '4':
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
