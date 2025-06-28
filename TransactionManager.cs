using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;


namespace Finane_Transaction_Manager
{
    public class TransactionManager : ITransactionManager
    {
        public List<FinanceTransaction> transactionslist = new List<FinanceTransaction>();
        public string Description;
        public decimal Amount;
        public TransactionType Type;
        public TransactionStatus Status;
        public string Date;
        public string Category;
        public string AccountEmail;
        public string Notes;
        TransactionType _incomeValueForComparison = TransactionType.Income;
        TransactionType _expenseValueForComparison = TransactionType.Expense;
        TransactionType _InvestementValueForComparison = TransactionType.Investement;
        TransactionStatus _PendingStatusValueForComparison = TransactionStatus.Pending;
        TransactionStatus _ClearedSatusValueForComparison = TransactionStatus.Cleared;
        TransactionStatus _FlaggedStatusValueForComparison = TransactionStatus.Flagged;
        public Confirmation confirm;
        Confirmation yes = Confirmation.yes;
        Confirmation no = Confirmation.no;
        string currentDate = $"{DateTime.Now.ToShortDateString()}";

        public void Add()
        {
            try
            {
                Console.WriteLine("Enter the Description for transaction:");
                Description = Console.ReadLine();

                while (true)
                {
                    if (Description.Length <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Description atleast 10 characters long to look good!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    else
                    {
                        break;
                    }

                    Description = Console.ReadLine();
                }

                Console.WriteLine("Enter the amount:");
                while (true)
                {
                    if (Decimal.TryParse(Console.ReadLine(), out Amount))
                    {
                        if (Amount <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Amount value can't be negative!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect format for Amount");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                bool TypeChecking = false;
                int ChoiceOfType;

                do
                {
                    Console.WriteLine("Choose the transaction Type :\n\t 1 for Income,\n\t 2 for Expense,\n\t 3 for Investement:");
                    TypeChecking = int.TryParse(Console.ReadLine(), out ChoiceOfType);

                    if (!TypeChecking || ChoiceOfType < 1 || ChoiceOfType > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your choice is invalid! Please enter 1, 2, or 3.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                } while (!TypeChecking || ChoiceOfType < 1 || ChoiceOfType > 3);

                switch (ChoiceOfType)
                {
                    case 1:
                        Type = _incomeValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose income type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Type = _expenseValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose expense type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 3:
                        Type = _InvestementValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose investement type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }


                Status = _PendingStatusValueForComparison;

                Console.WriteLine("Enter the Date of Transaction in (DD/MM/YYYY):");
                Date = Console.ReadLine();
                while (true)
                {
                    if (!Regex.IsMatch(Date, @"^\d{2}/\d{2}/\d{4}$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("incorrect format for date!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        break;
                    }

                    Date = Console.ReadLine();

                }


                Console.WriteLine("Enter the category of transaction:");
                Category = Console.ReadLine();
                while (true)
                {
                    if (!Regex.IsMatch(Category, @"^[A-Za-z]+(?:\s[A-Za-z]+)*$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("incorrect format for category!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        break;
                    }

                    Category = Console.ReadLine();
                }

                Console.WriteLine("Enter the transaction Account Email  :");
                AccountEmail = Console.ReadLine();
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

                Console.WriteLine("Enter the Notes for Transaction (optional) :");
                Notes = Console.ReadLine();

                transactionslist.Add(new FinanceTransaction(Description, Amount, Type, Status, Date, Category, AccountEmail, Notes));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nTransaction successfully added!");
                Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void DisplayTransactions(IEnumerable<FinanceTransaction> TransactionInfo)
        {
            try
            {
                if (TransactionInfo != null)
                {
                    Console.WriteLine("Here's Your transaction Details:\n\n");
                    foreach (var trans in TransactionInfo)
                    {
                        Console.WriteLine($"Transaction with ID: {trans.Id}");
                        Console.WriteLine($"Transaction with description :{trans.Description}");
                        Console.WriteLine($"Transaction with Amount :{trans.Amount}");
                        Console.WriteLine($"Transaction with Type :{trans.Type}");
                        Console.WriteLine($"Transaction with Status :{trans.Status}");
                        Console.WriteLine($"Transaction with Date :{trans.Date}");
                        Console.WriteLine($"Transaction with Category :{trans.Category}");
                        Console.WriteLine($"Transaction with AccountEmail :{trans.AccountEmail}");
                        Console.WriteLine($"Transaction with Notes :{trans.Notes}\n\n");

                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("List is empty!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }


        public void DisplayAllRecord(List<FinanceTransaction> transaction)
        {
            try
            {

                var TransactionInfo = from trans in transaction orderby trans.Id select trans;

                DisplayTransactions(TransactionInfo);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }


        }

        public void DisplayByDate(List<FinanceTransaction> transaction)
        {
            try
            {
                Console.WriteLine("\nEnter the transaction Date:");
                string Date = Console.ReadLine();
                Date = Console.ReadLine();
                while (true)
                {
                    if (!Regex.IsMatch(Date, @"^\d{2}/\d{2}/\d{4}$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("incorrect format for date!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        break;
                    }

                    Date = Console.ReadLine();

                }

                var TransactionInfo = (from trans in transaction where trans.Date == Date select trans);
                DisplayTransactions(TransactionInfo);



            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        public void DisplayByAmount(List<FinanceTransaction> transaction)
        {
            try
            {
                Console.WriteLine("\nEnter the transaction Amount:");

                if (Decimal.TryParse(Console.ReadLine(), out Amount))
                {
                    if (Amount <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Amount value can't be negative!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {

                        var TransactionInfo = (from trans in transaction where trans.Amount == Amount select trans);
                        DisplayTransactions(TransactionInfo);

                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect format for Amount");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void DisplayByDescription(List<FinanceTransaction> transaction)
        {

            try
            {

                Console.WriteLine("\nEnter the transaction Description:");
                Description = Console.ReadLine();
                var TransactionInfo = (from trans in transaction where trans.Description == Description select trans);
                DisplayTransactions(TransactionInfo);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }



        public void DisplayByID(List<FinanceTransaction> transaction)
        {
            try
            {

                Console.WriteLine("\nEnter the transaction Id:");

                if (Guid.TryParse(Console.ReadLine(), out Guid transactionID))
                {

                    var TransactionInfo = (from trans in transaction where trans.Id == transactionID select trans);
                    DisplayTransactions(TransactionInfo);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("record not found!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void Update()
        {
            try
            {

                TransactionType _incomeValueForComparison = TransactionType.Income;
                TransactionType _expenseValueForComparison = TransactionType.Expense;
                TransactionType _InvestementValueForComparison = TransactionType.Investement;
                TransactionStatus _PendingStatusValueForComparison = TransactionStatus.Pending;
                TransactionStatus _ClearedSatusValueForComparison = TransactionStatus.Cleared;
                TransactionStatus _FlaggedStatusValueForComparison = TransactionStatus.Flagged;

                if (transactionslist.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("nothing to update the record!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("\nEnter the transaction Id:");

                if (Guid.TryParse(Console.ReadLine(), out Guid transactionID))
                {

                    var TransactionToUpdate = transactionslist.FirstOrDefault(trans => trans.Id == transactionID);
                    if (TransactionToUpdate == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"transaction with ID {transactionID} not found.");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }

                    Console.WriteLine("Enter 1=> for All fields Updation");
                    Console.WriteLine("Enter 2=> For Some fields Updation");
                    Console.WriteLine("Enter 3=> Exit");

                    if (Int32.TryParse(Console.ReadLine(), out Int32 choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Enter the Updated Description for transaction:");
                                Description = Console.ReadLine();

                                while (true)
                                {
                                    if (Description.Length < 10)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Description atleast 10 characters long to look good!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }

                                    else
                                    {
                                        break;
                                    }

                                    Description = Console.ReadLine();
                                }

                                TransactionToUpdate.Description = Description;

                                Console.WriteLine("Enter the updated amount:");
                                while (true)
                                {
                                    if (Decimal.TryParse(Console.ReadLine(), out Amount))
                                    {
                                        if (Amount <= 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Amount value can't be negative!");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }

                                        else
                                        {
                                            break;
                                        }


                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Incorrect format for Amount");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                TransactionToUpdate.Amount = Amount;

                                bool TypeChecking = false;
                                int ChoiceOfType;

                                do
                                {
                                    Console.WriteLine("Choose the transaction Type :\n\t 1 for Income,\n\t 2 for Expense,\n\t 3 for Investement:");
                                    TypeChecking = int.TryParse(Console.ReadLine(), out ChoiceOfType);

                                    if (!TypeChecking || ChoiceOfType < 1 || ChoiceOfType > 3)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Your choice is invalid! Please enter 1, 2, or 3.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                } while (!TypeChecking || ChoiceOfType < 1 || ChoiceOfType > 3);

                                switch (ChoiceOfType)
                                {
                                    case 1:
                                        Type = _incomeValueForComparison;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("You choose income type");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case 2:
                                        Type = _expenseValueForComparison;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("You choose expense type");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case 3:
                                        Type = _InvestementValueForComparison;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("You choose investement type");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                }


                                TransactionToUpdate.Type = Type;

                                bool StatsChecking = false;
                                int ChoiceOfStatus;

                                do
                                {
                                    Console.WriteLine("Choose the transaction Status \n\t 1 for Cleared,\n\t 2 for Flagged:");
                                    StatsChecking = int.TryParse(Console.ReadLine(), out ChoiceOfStatus);

                                    if (!StatsChecking || ChoiceOfStatus < 1 || ChoiceOfStatus > 2)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Your choice is invalid! Please enter 1, 2.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                } while (!StatsChecking || ChoiceOfStatus < 1 || ChoiceOfStatus > 2);

                                switch (ChoiceOfStatus)
                                {
                                    case 1:
                                        Status = _ClearedSatusValueForComparison;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("You choose Cleared Status");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case 2:
                                        Status = _FlaggedStatusValueForComparison;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("You choose Flagged Status");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                }
                                TransactionToUpdate.Status = Status;

                                Console.WriteLine("Enter Updated Date of Transaction in (DD/MM/YYYY):");
                                Date = Console.ReadLine();
                                while (true)
                                {
                                    if (!Regex.IsMatch(Date, @"^\d{2}/\d{2}/\d{4}$"))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("incorrect format for date!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                    Date = Console.ReadLine();

                                }
                                TransactionToUpdate.Date = Date;

                                Console.WriteLine(" Enter updated category of transaction:");
                                Category = Console.ReadLine();
                                while (true)
                                {
                                    if (!Regex.IsMatch(Category, @"^[A-Za-z]+(?:\s[A-Za-z]+)*$"))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("incorrect format for category!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                    Category = Console.ReadLine();
                                }
                                TransactionToUpdate.Category = Category;

                                Console.WriteLine("Enter Updated Account Email for :");
                                AccountEmail = Console.ReadLine();
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
                                TransactionToUpdate.AccountEmail = AccountEmail;

                                Console.WriteLine("Enter Updated Notes for Transaction (optional) :");
                                AccountEmail = Console.ReadLine();
                                TransactionToUpdate.Notes = Notes;


                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nRecord Updated successfully!");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.WriteLine("\n----------Enter the number of fields to update--------------");
                                if (int.TryParse(Console.ReadLine(), out int fieldLength))
                                {
                                    for (int i = 0; i < fieldLength; i++)
                                    {
                                        Console.WriteLine($"Enter field {i + 1} to update (Description, Amount,Type. Status,Date,Category,AccountEmail,Notes):");
                                        string fieldToUpdate = Console.ReadLine().ToLower();
                                        string[] validFields = { "description", "amount", "type", "status", "date", "category", "accountEmail", "notes" };
                                        if (validFields.Contains(fieldToUpdate))
                                        {

                                            switch (fieldToUpdate)
                                            {
                                                case "description":
                                                    Console.WriteLine("Enter the Updated Description for transaction:");
                                                    Description = Console.ReadLine();

                                                    while (true)
                                                    {
                                                        if (Description.Length < 10)
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("Description atleast 10 characters long to look good!");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }

                                                        else
                                                        {
                                                            break;
                                                        }

                                                        Description = Console.ReadLine();
                                                    }

                                                    TransactionToUpdate.Description = Description;
                                                    break;

                                                case "amount":
                                                    Console.WriteLine("Enter the updated amount:");
                                                    while (true)
                                                    {
                                                        if (Decimal.TryParse(Console.ReadLine(), out Amount))
                                                        {
                                                            if (Amount <= 0)
                                                            {
                                                                Console.ForegroundColor = ConsoleColor.Red;
                                                                Console.WriteLine("Amount value can't be negative!");
                                                                Console.ForegroundColor = ConsoleColor.White;
                                                            }

                                                            else
                                                            {
                                                                break;
                                                            }


                                                        }
                                                        else
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("Incorrect format for Amount");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                    }
                                                    TransactionToUpdate.Amount = Amount;
                                                    break;

                                                case "type":
                                                    Console.WriteLine(" Choose Updated the transaction Type :\n\t 1 for Income,\n\t 2 for Expense,\n\t 3 for Investement:");
                                                    bool choiceUpdated = int.TryParse(Console.ReadLine(), out int ChoiceOfTypeUpdated);

                                                    switch (ChoiceOfTypeUpdated)
                                                    {
                                                        case 1:
                                                            Type = _incomeValueForComparison;
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("You choose income type");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;

                                                        case 2:
                                                            Type = _expenseValueForComparison;
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("You choose expense type");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;
                                                        case 3:
                                                            Type = _InvestementValueForComparison;
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("You choose investement type");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;
                                                        default:
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("your choice is invalid!");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;


                                                    }
                                                    TransactionToUpdate.Type = Type;
                                                    break;
                                                case "status":
                                                    StatsChecking = false;
                                                    int ChoiceOfStatusUpdated;

                                                    do
                                                    {
                                                        Console.WriteLine("Choose the transaction Status \n\t 1 for Cleared,\n\t 2 for Flagged:");
                                                        StatsChecking = int.TryParse(Console.ReadLine(), out ChoiceOfStatus);

                                                        if (!StatsChecking || ChoiceOfStatus < 1 || ChoiceOfStatus > 2)
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("Your choice is invalid! Please enter 1, 2.");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                    } while (!StatsChecking || ChoiceOfStatus < 1 || ChoiceOfStatus > 2);

                                                    switch (ChoiceOfStatus)
                                                    {
                                                        case 1:
                                                            Status = _ClearedSatusValueForComparison;
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("You choose Cleared Status");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;
                                                        case 2:
                                                            Status = _FlaggedStatusValueForComparison;
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine("You choose Flagged Status");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            break;


                                                    }
                                                    TransactionToUpdate.Status = Status;
                                                    break;
                                                case "date":
                                                    Console.WriteLine("Enter Updated Date of Transaction in (DD/MM/YYYY):");
                                                    Date = Console.ReadLine();
                                                    while (true)
                                                    {
                                                        if (!Regex.IsMatch(Date, @"^\d{2}/\d{2}/\d{4}$"))
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("incorrect format for date!");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }

                                                        Date = Console.ReadLine();

                                                    }
                                                    TransactionToUpdate.Date = Date;
                                                    break;
                                                case "category":
                                                    Console.WriteLine(" Enter updated category of transaction:");
                                                    Category = Console.ReadLine();
                                                    while (true)
                                                    {
                                                        if (!Regex.IsMatch(Category, @"^[A-Za-z]+(?:\s[A-Za-z]+)*$"))
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine("incorrect format for category!");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }

                                                        Category = Console.ReadLine();
                                                    }
                                                    TransactionToUpdate.Category = Category;
                                                    break;
                                                case "accountEmail":
                                                    Console.WriteLine("Enter Updated Account Email for :");
                                                    AccountEmail = Console.ReadLine();
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
                                                    TransactionToUpdate.AccountEmail = AccountEmail;
                                                    break;
                                                case "notes":

                                                    Console.WriteLine("Enter Updated Notes for Transaction (optional) :");
                                                    AccountEmail = Console.ReadLine();
                                                    TransactionToUpdate.Notes = Notes;
                                                    break;

                                            }


                                        }


                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nRecord Updated successfully!");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 3:
                                System.Environment.Exit(0);
                                break;


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DeleteRecord(List<FinanceTransaction> transaction)
        {
            try
            {
                string confirmation;
                if (transaction.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("nothing to delete !record is empty!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("\nEnter the transaction id");

                if (!Guid.TryParse(Console.ReadLine(), out Guid transactionId))

                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid employee ID format.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
                Console.ForegroundColor = ConsoleColor.White;

                var TransactionToRemove = transaction.FirstOrDefault(trans => trans.Id == transactionId);
                if (TransactionToRemove != null)
                {

                    Console.WriteLine(" Are You sure you want to delete\n\t\t\t 1 for yes \n\t\t\t 2 for no ");
                    bool checkingChoice = int.TryParse(Console.ReadLine(), out int ConfirmationDelete);

                    switch (ConfirmationDelete)
                    {
                        case 1:
                            confirm = yes;
                            transaction.Remove(TransactionToRemove);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nEmployee with ID {transactionId} has been deleted\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case 2:
                            confirm = no;
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("your choice is invalid!");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;


                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("your entered id is not in the database or format does not match!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }


        }



        public void DisplayByEmail(List<FinanceTransaction> transaction)
        {
            try
            {

                Console.WriteLine("\nEnter the transaction AccountEmail:");
                AccountEmail = Console.ReadLine();


                var TransactionInfo = (from trans in transaction where trans.AccountEmail == AccountEmail select trans);
                DisplayTransactions(TransactionInfo);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
        public void DisplayByCategory(List<FinanceTransaction> transaction)
        {
            try
            {
                Console.WriteLine("\nEnter the transaction Category:");
                Category = Console.ReadLine();


                var TransactionInfo = (from trans in transaction where trans.Category == Category select trans);
                DisplayTransactions(TransactionInfo);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }


        public void DisplayByTransactionStatus(List<FinanceTransaction> transaction)
        {
            try
            {


                Console.WriteLine("Choose the transaction Status  \n\t 1 for Pending, \n\t 2 for Cleared,\n\t 3 for Flagged:");
                bool checkingChoice = int.TryParse(Console.ReadLine(), out int ChoiceOfStatus);
                TransactionStatus Status;

                switch (ChoiceOfStatus)
                {
                    case 1:
                        Status = _PendingStatusValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose Pending Status");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Status = _ClearedSatusValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose Cleared Status ");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 3:
                        Status = _FlaggedStatusValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose Flagged Status");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("your choice is invalid!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                }


                var TransactionInfo = from trans in transaction
                                      where trans.Status == Status
                                      select trans;
                if (TransactionInfo != null && transaction.Count > 0)
                {
                    DisplayTransactions(TransactionInfo);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("transaction accordeing to transaction status not found!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

        }


        public void DisplayByTransactionType(List<FinanceTransaction> transaction)
        {
            try
            {
                Console.WriteLine("Choose the transaction Type :\n\t 1 for Income,\n\t 2 for Expense,\n\t 3 for Investement:");
                bool checkingChoice = int.TryParse(Console.ReadLine(), out int ChoiceOfType);
                TransactionType Type;

                switch (ChoiceOfType)
                {
                    case 1:
                        Type = _incomeValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose income type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Type = _expenseValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose expense type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 3:
                        Type = _InvestementValueForComparison;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You choose investement type");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("your choice is invalid!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                }


                var TransactionInfo = from trans in transaction
                                      where trans.Type == Type
                                      select trans;
                if (TransactionInfo != null && transaction.Count > 0)
                {
                    DisplayTransactions(TransactionInfo);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("transaction accordeing to transaction type not found!!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void TransactionSummary()
        {
            try
            {
                string filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "source",
                    "repos",
                    "Vineet_CSharp_Test",
                    "Vineet_CSharp_Test",
                    "data",
                    currentDate + ".json"
                );

                string directoryPath = Path.GetDirectoryName(filePath);

                // Ensure the directory exists BEFORE creating the file
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                try
                {
                    var jsonData = JsonConvert.SerializeObject(transactionslist, Formatting.Indented);
                    string logMessage = $"{DateTime.Now}:{Environment.NewLine}{jsonData}{Environment.NewLine}";

                    File.WriteAllText(filePath, logMessage);

                    Console.WriteLine("Transaction summary successfully saved to: " + filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving transaction summary: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void SendLogFile(string recipientEmail)
        {
            try
            {

                string filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "source",
                    "repos",
                    "Vineet_CSharp_Test",
                    "Vineet_CSharp_Test",
                    "data",
                    currentDate + ".json"
                );
                MailMessage email = new MailMessage("kankerwalvineet2003@gmail.com", recipientEmail);
                email.Subject = "Transaction Summary File";
                email.Body = "Hello \n Sir/Madam \n Please find attached the transaction summary file containing all transaction records.\n\nThank You.";

                try
                {


                    if (File.Exists(filePath))
                    {
                        email.Attachments.Add(new Attachment(filePath));
                    }
                    else
                    {
                        Console.WriteLine($"Warning: File not found at {filePath}. Email will be sent without attachment.");
                    }

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential("kankerwalvineet2003@gmail.com", "demz vvvz hsph zgrp");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;

                        smtp.Send(email);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Email Sent Successfully!!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {


                    Console.WriteLine("Error sending email: " + ex.ToString());


                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString() + "we are fixing it soon");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


    }


}









