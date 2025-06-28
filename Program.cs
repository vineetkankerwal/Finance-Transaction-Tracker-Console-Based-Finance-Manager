using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vineet_CSharp_Test
{
    public enum TaskStatus
    {
        Pending, Completed
    }
    public enum TransactionStatus
    {
        Pending, Cleared, Flagged
    }
    public enum TransactionType
    {
        Income, Expense, Investement
    }
    public enum Confirmation
    {
        yes,no
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TransactionManager transactionManager = new TransactionManager();
                Console.WriteLine("\t\t\tWelcome , Lets start it");

                do
                {
                    
                    Console.WriteLine("\n\n\t\t\tEnter 1 => add Transaction ");
                    Console.WriteLine("\t\t\tEnter 2 => update Transaction Data");
                    Console.WriteLine("\t\t\tEnter 3 => Display Transaction Data");
                    Console.WriteLine("\t\t\tEnter 4 => Delete Transaction Data ");
                    Console.WriteLine("\t\t\tEnter 5 => Filter Transaction Data By Transaction Type(Expense,Investement) ");
                    Console.WriteLine("\t\t\tEnter 6 => Filter Transaction Data By ID");
                    Console.WriteLine("\t\t\tEnter 7 => Filter Transaction Data By Email ");
                    Console.WriteLine("\t\t\tEnter 8 => Filter Transaction Data By Category ");
                    Console.WriteLine("\t\t\tEnter 9 =>filter Transaction Data By Status ");
                    Console.WriteLine("\t\t\tEnter 10 =>filter Transaction Data By Date ");
                    Console.WriteLine("\t\t\tEnter 11 =>filter Transaction Data By Amount ");
                    Console.WriteLine("\t\t\tEnter 12 =>Sending a Transaction Summary via Email");
                    Console.WriteLine("\t\t\tEnter 13 =>filter Transaction Data By Description");
                    Console.WriteLine("\t\t\tEnter 14 => Exit");
                    Console.WriteLine("\nEnter your choice");


                    if (Int32.TryParse(Console.ReadLine(), out Int32 choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                transactionManager.Add();
                                transactionManager.TransactionSummary();
                                break;
                            case 2:
                                transactionManager.Update();
                                transactionManager.TransactionSummary();
                                break;
                            case 3:
                                transactionManager.DisplayAllRecord(transactionManager.transactionslist);
                                break;
                            case 4:
                                transactionManager.DeleteRecord(transactionManager.transactionslist);
                                transactionManager.TransactionSummary();
                                break;

                            case 5:
                                transactionManager.DisplayByTransactionType(transactionManager.transactionslist);
                                break;
                            case 6:
                                transactionManager.DisplayByID(transactionManager.transactionslist);
                                break;

                            case 7:
                                transactionManager.DisplayByEmail(transactionManager.transactionslist);
                                break;
                            case 8:
                                transactionManager.DisplayByCategory(transactionManager.transactionslist);
                                break;
                            case 9:
                                transactionManager.DisplayByTransactionStatus(transactionManager.transactionslist);
                                break;
                            case 10:
                                transactionManager.DisplayByDate(transactionManager.transactionslist);
                                break;
                            case 11:
                                transactionManager.DisplayByAmount(transactionManager.transactionslist);
                                break;
                            case 12:

                                Console.WriteLine("Enter the Recipient Email for sending transaction summary:");
                                string AccountEmail = Console.ReadLine();
                                while (true)
                                {
                                    if (!Regex.IsMatch(AccountEmail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("incorrect format for email !");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                    AccountEmail = Console.ReadLine();
                                }
                                transactionManager.SendLogFile(AccountEmail);
                                break;
                            case 13:
                                transactionManager.DisplayByDescription(transactionManager.transactionslist);
                                break;
                            case 14:
                                System.Environment.Exit(0);
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nWrong choice for cases");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;

                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("enter correct format");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                } while (true);


            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString()+"we are fixing soon..");
                Console.ForegroundColor = ConsoleColor.White;
            }


        }
    }

    
}