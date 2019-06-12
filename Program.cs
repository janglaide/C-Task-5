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

                    Console.WriteLine("\n\t\t\tMENU\n1. SELECT\n2. INSERT\n3. UPDATE\n4. DELETE\n5. Queries\n6. Exit");
                    var key = Console.ReadKey().KeyChar;
                    Console.Clear();
                    switch (key)
                    {
                        case '1':
                            Select(connection);
                            break;
                        case '2':
                            Insert(connection);
                            break;
                        case '3':
                            Update(connection);
                            break;
                        case '4':
                            Delete(connection);
                            break;
                        case '5':
                            Queries(connection);
                            break;
                        case '6':
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

        static void Queries(SqlConnection connection)
        {
            var flag = true;
            var key = 'c';
            while (flag)
            {
                Console.WriteLine("1. Order by amount in register\n2. Joined tables\n3. Private entrepreneurs\n" +
                    "4. Individuals\n5. Legal entities\n6. Maximum income\n7. Law Documents and Amounts for them" +
                    "\n8. Return to main menu\n");
                key = Console.ReadKey().KeyChar;
                DataSet ds = new DataSet();
                SqlDataAdapter adapter;
                string sql;
                switch (key)
                {
                    case '1':
                        sql = "SELECT * FROM Register Order By Amount";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '2':
                        sql = "SELECT DISTINCT TaxRegister.dbo.Payers.Name, TaxRegister.dbo.Payers.Surname, TaxRegister.dbo.Payers.Income, ";
                        sql += "TaxRegister.dbo.Taxes.Percentage, TaxRegister.dbo.Register.Amount FROM TaxRegister.dbo.Payers ";
                        sql += "JOIN TaxRegister.dbo.Register ON TaxRegister.dbo.Payers.Id = TaxRegister.dbo.Register.PayerId ";
                        sql += "JOIN TaxRegister.dbo.Taxes ON TaxRegister.dbo.Register.TaxId = TaxRegister.dbo.Taxes.Id ";
                        sql += "WHERE TaxRegister.dbo.Payers.Id = TaxRegister.dbo.Register.PayerId AND TaxRegister.dbo.Register.TaxId = TaxRegister.dbo.Taxes.Id ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '3':
                        sql = "SELECT TaxRegister.dbo.Payers.Name, TaxRegister.dbo.Payers.Surname, TaxRegister.dbo.Payers.[Payer Type], ";
                        sql += "TaxRegister.dbo.Taxes.Name FROM TaxRegister.dbo.Payers ";
                        sql += "JOIN TaxRegister.dbo.Register ON TaxRegister.dbo.Payers.Id = TaxRegister.dbo.Register.PayerId ";
                        sql += "JOIN TaxRegister.dbo.Taxes ON TaxRegister.dbo.Register.TaxId = TaxRegister.dbo.Taxes.Id ";
                        sql += "WHERE TaxRegister.dbo.Payers.[Payer Type] = 'private entrepreneur' ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.ReadKey();
                        flag = false;
                        break;
                    case '4':
                        sql = "SELECT TaxRegister.dbo.Payers.Name, TaxRegister.dbo.Payers.Surname, TaxRegister.dbo.Payers.[Payer Type], ";
                        sql += "TaxRegister.dbo.Taxes.Name FROM TaxRegister.dbo.Payers ";
                        sql += "JOIN TaxRegister.dbo.Register ON TaxRegister.dbo.Payers.Id = TaxRegister.dbo.Register.PayerId ";
                        sql += "JOIN TaxRegister.dbo.Taxes ON TaxRegister.dbo.Register.TaxId = TaxRegister.dbo.Taxes.Id ";
                        sql += "WHERE TaxRegister.dbo.Payers.[Payer Type] = 'an individual' ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.ReadKey();
                        flag = false;
                        break;
                    case '5':
                        sql = "SELECT TaxRegister.dbo.Payers.Name, TaxRegister.dbo.Payers.Surname, TaxRegister.dbo.Payers.[Payer Type], ";
                        sql += "TaxRegister.dbo.Taxes.Name FROM TaxRegister.dbo.Payers ";
                        sql += "JOIN TaxRegister.dbo.Register ON TaxRegister.dbo.Payers.Id = TaxRegister.dbo.Register.PayerId ";
                        sql += "JOIN TaxRegister.dbo.Taxes ON TaxRegister.dbo.Register.TaxId = TaxRegister.dbo.Taxes.Id ";
                        sql += "WHERE TaxRegister.dbo.Payers.[Payer Type] = 'legal entity' ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.ReadKey();
                        flag = false;
                        break;
                    case '6':
                        sql = "SELECT MAX(TaxRegister.dbo.Payers.Income) FROM TaxRegister.dbo.Payers; ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '7':
                        sql = "SELECT TaxRegister.dbo.Taxes.[Law Document], TaxRegister.dbo.Taxes.Percentage, TaxRegister.dbo.Register.Amount ";
                        sql += "FROM TaxRegister.dbo.Taxes JOIN TaxRegister.dbo.Register ON TaxRegister.dbo.Taxes.Id = TaxRegister.dbo.Register.TaxId ";
                        sql += "WHERE TaxRegister.dbo.Taxes.Id = TaxRegister.dbo.Register.TaxId ORDER BY Amount ";
                        adapter = new SqlDataAdapter(sql, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '8':
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void Delete(SqlConnection connection)
        {
            var flag = true;
            var expression = "DELETE FROM Payers WHERE Id = ";
            var key = 'c';
            var foreignDelete = "";
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                string id;
                string selectExpression;
                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                switch (key)
                {
                    case '1':

                        selectExpression = "SELECT * FROM Payers";

                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.WriteLine("\nEnter Row Id to Delete:");
                        id = Console.ReadLine();
                        expression += id;

                        foreignDelete = "DELETE FROM Register WHERE PayerId = " + id;
                        flag = false;
                        break;
                    case '2':

                        selectExpression = "SELECT * FROM Taxes";

                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.WriteLine("\nEnter Row Id to Delete:");
                        id = Console.ReadLine();
                        expression += id;

                        foreignDelete = "DELETE FROM Register WHERE TaxId = " + id;
                        flag = false;
                        break;
                    case '3':

                        selectExpression = "SELECT * FROM Register";

                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        Console.WriteLine("\nEnter Row Id to Delete:");
                        id = Console.ReadLine();
                        expression += id;

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
            SqlCommand command = new SqlCommand(expression, connection);
            if (key != '4')
            {
                var number = command.ExecuteNonQuery();
                Console.WriteLine("The row was deleted");
            }
            if(key == '1' || key == '2')
            {
                SqlCommand command2 = new SqlCommand(foreignDelete, connection);
                var number2 = command2.ExecuteNonQuery();
            }
            Console.ReadKey();
        }

        static void Update(SqlConnection connection)
        {
            var flag = true;
            var expression = "";
            var key = 'c';
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();

                string selectExpression;
                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                switch (key)
                {
                    case '1':
                        expression = "UPDATE Payers SET ";

                        selectExpression = "SELECT * FROM Payers";
                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        
                        expression = UpdateCase1(expression);

                        flag = false;
                        break;
                    case '2':
                        expression = "UPDATE Taxes SET ";

                        selectExpression = "SELECT * FROM Taxes";
                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        expression = UpdateCase2(expression);
                        flag = false;
                        break;
                    case '3':
                        expression = "UPDATE Register SET ";

                        selectExpression = "SELECT * FROM Register";
                        adapter = new SqlDataAdapter(selectExpression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);

                        expression =  UpdateCase3(expression);
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
            SqlCommand command = new SqlCommand(expression, connection);
            if (key != '4')
            {
                var number = command.ExecuteNonQuery();
                Console.WriteLine("The row was updated");
            }

            Console.ReadKey();
        }

        static string UpdateCase3(string expression)
        {
            Console.WriteLine("\nEnter Row Id to Update:");
            var id = Console.ReadLine();
            var flag = true;
            var key = 'c';
            while (flag)
            {
                Console.WriteLine("\n1. PayerId\n2. TaxId\n3. Amount\n");
                key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case '1':
                        Console.WriteLine("\nNew PayerId: ");
                        var newPayerId = Console.ReadLine();
                        expression += "PayerId = " + newPayerId + " WHERE Id = " + id;
                        flag = false;
                        break;
                    case '2':
                        Console.WriteLine("\nNew TaxId: ");
                        var newTaxId = Console.ReadLine();
                        expression += "TaxId = " + newTaxId + " WHERE Id = " + id;
                        flag = false;
                        break;
                    case '3':
                        Console.WriteLine("\nNew Amount: ");
                        var newAmount = Console.ReadLine();
                        expression += "Amount = " + newAmount + " WHERE Id = " + id;
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        break;
                }
            }

            return expression;
        }
        static string UpdateCase2(string expression)
        {
            Console.WriteLine("\nEnter Row Id to Update:");
            var id = Console.ReadLine();
            var flag = true;
            while (flag)
            {
                Console.WriteLine("\n1. Name\n2. Law Document\n3. Percentage\n");
                var key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case '1':
                        Console.WriteLine("\nNew name: ");
                        var newName = Console.ReadLine();
                        expression += "Name = '" + newName + "' WHERE Id = " + id;
                        flag = false;
                        break;
                    case '2':
                        Console.WriteLine("\nNew law document: ");
                        var newLawDocument = Console.ReadLine();
                        expression += "[Law Document] = '" + newLawDocument + "' WHERE Id = " + id;
                        flag = false;
                        break;
                    case '3':
                        Console.WriteLine("\nNew percentage: ");
                        var newPercentage = Console.ReadLine();
                        expression += "Percentage = " + newPercentage + " WHERE Id = " + id;
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        break;
                }
            }
            return expression;
        }
        static string UpdateCase1(string expression)
        {
            Console.WriteLine("\nEnter Row Id to Update:");
            var id = Console.ReadLine();
            var flag = true;
            while (flag)
            {
                Console.WriteLine("\n1. Name\n2. Surname\n3. Payer Type\n4. Address\n5. Income\n");
                var key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case '1':
                        Console.WriteLine("\nNew name: ");
                        string newName = Console.ReadLine();
                        expression += "Name = '" + newName + "' WHERE Id = " + id;
                        flag = false;
                        break;
                    case '2':
                        Console.WriteLine("\nNew surname: ");
                        var newSurname = Console.ReadLine();
                        expression += "Surname = '" + newSurname + "' WHERE Id = " + id;
                        flag = false;
                        break;
                    case '3':
                        Console.WriteLine("\nNew payer type: ");
                        var newPayerType = Console.ReadLine();
                        expression += "[Payer Type] = '" + newPayerType + "' WHERE Id = " + id.ToString();
                        flag = false;
                        break;
                    case '4':
                        Console.WriteLine("\nNew Address: ");
                        var newAddress = Console.ReadLine();
                        expression += "Address = '" + newAddress + "' WHERE Id = " + id.ToString();
                        flag = false;
                        break;
                    case '5':
                        Console.WriteLine("\nNew Income: ");
                        var newIncome = Double.Parse(Console.ReadLine());
                        expression += "Income = " + newIncome.ToString() + " WHERE Id = " + id.ToString();
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        Console.ReadKey();
                        break;
                }
            }
            return expression;
        }
        static void Select(SqlConnection connection)
        {
            var flag = true;
            var key = 'c';
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                string expression;
                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                switch (key)
                {
                    case '1':
                        expression = "SELECT * FROM Payers";
                        adapter = new SqlDataAdapter(expression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '2':
                        expression = "SELECT * FROM Taxes";
                        adapter = new SqlDataAdapter(expression, connection);
                        adapter.Fill(ds);
                        TablesOut(ds);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '3':
                        expression = "SELECT * FROM Register";
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
            var expression = "INSERT INTO ";
            var key = 'c';
            while (flag)
            {
                Console.WriteLine("1. Payers\n2. Taxes\n3. Register (Payers to taxes)\n4. Return to main menu");
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (key)
                {
                    case '1':
                        expression = InsertCase1(expression);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '2':
                        expression = InsertCase2(expression);
                        Console.ReadKey();
                        flag = false;
                        break;
                    case '3':
                        expression = InsertCase3(expression, connection);
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
            SqlCommand command = new SqlCommand(expression, connection);
            if (key != '4')
            {
                var number = command.ExecuteNonQuery();
                Console.WriteLine("Added a row");
            }

            Console.ReadKey();
        }
        static string InsertCase3(string expression, SqlConnection connection)
        {
            expression += "Register (PayerId, TaxId, Amount) VALUES (";
            Console.WriteLine("PayerId: ");
            var payer = Console.ReadLine();
            Console.WriteLine("TaxId:");
            var tax = Console.ReadLine();

            var amount = CountAmount(payer, tax, connection);

            expression += payer.ToString() + ", " + tax.ToString() + ", " + amount.ToString() + ")";
            return expression;
        }

        static double CountAmount(string payer, string tax, SqlConnection connection)
        {

            double result;
            var sql1 = "SELECT Income FROM Payers WHERE Id = " + payer;
            var sql2 = "SELECT Percentage FROM Taxes WHERE Id = " + tax;

            SqlDataAdapter adapter1 = new SqlDataAdapter(sql1, connection);
            SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, connection);

            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            adapter1.Fill(ds1);
            adapter2.Fill(ds2);

            var percent = ds2.Tables[0].Rows[0].ItemArray[0];
            var income = ds1.Tables[0].Rows[0].ItemArray[0];
            var intPercent = Convert.ToDouble(percent);
            var intIncome = Convert.ToDouble(income);

            result = intIncome * (intPercent / 100);

            return result;
        }
        static string InsertCase2(string expression)
        {
            expression += "Taxes (Name, [Law Document], Percentage) VALUES ('";
            Console.WriteLine("\nName: ");
            var name = Console.ReadLine();
            expression += name + "', '";
            Console.WriteLine("\nLaw Document (Article 'number' of the Tax Code): ");
            var document = Console.ReadLine();
            expression += document + "', ";
            Console.WriteLine("\nPercentage: ");
            var percentage = Console.Read();
            expression += percentage.ToString() + ")";
            return expression;
        }
        static string InsertCase1(string expression)
        {
            Console.WriteLine("\nName: ");
            var name = Console.ReadLine();
            expression += "Payers (Name, Surname, [Payer Type], Address, Income) VALUES ('" + name + "', '";
            Console.WriteLine("\nSurname (if its company, enter '0'): ");
            string surname = Console.ReadLine();
            if (surname == "0")
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
            return expression;
        }
    }
}