using System;
using static System.Console;

namespace MyHashTableProject
{
    class Program
    {
        static void Main(string[] args)
        {

            HashTable theTable = new HashTable(PickSizeOfHashTable());
            int userSelection;

            while(true)
            {
                userSelection = MainMenu();

                if(userSelection is 5)
                {
                    break;
                }

                AppOperations(theTable, userSelection);
            }
            
        }

        static int PickSizeOfHashTable()
        {
            int selectedSize;

            WriteLine();
            WriteLine("Before we started, we need you to pick a size for the hashtable.");
            Write("What size would you like the hashtable to be: ");

            while (!int.TryParse(ReadLine(), out selectedSize) || selectedSize < 1)
            {
                WriteLine();
                WriteLine("Hashtable size must be a positive integer.  Please try again.");
                Write("What size would you like the hashtable to be: ");
            }

            return selectedSize;
        }

        static int MainMenu()
        {
            int choice;

            WriteLine();
            WriteLine("Please select one of the following options");
            WriteLine();
            WriteLine("1. Insert record into hashtable");
            WriteLine("2. Delete record from hashtable");
            WriteLine("3. Search for record in hashtable");
            WriteLine("4. Display the hashtable");
            WriteLine("5. Quit");
            Write("Enter your choice: ");

            while(!int.TryParse(ReadLine(), out choice) || choice < 1 || choice > 5)
            {
                WriteLine();
                WriteLine("Sorry, that option doesn\'t exist!  Try again.");
                Write("Enter your choice (1-5): ");
            }

            return choice;
        }

        static void AppOperations(HashTable table, int choice)
        {
            int enteredKey;

            WriteLine();
            switch (choice)
            {
                case 1: //Insert record into hashtable
                    Write("Enter a key for the new record: ");

                    while(!int.TryParse(ReadLine(), out enteredKey) || enteredKey < 0)
                    {
                        WriteLine();
                        WriteLine("We can only accept non-negative integers.  Please try again.");
                        Write("Enter a key for the new record: ");
                    }

                    Write($"Enter a name: ");

                    table.Insert(enteredKey, ReadLine());
                    break;
                case 2: //Delete record from hashtable
                    Write("Enter the key of the record to delete from the hashtable: ");

                    while (!int.TryParse(ReadLine(), out enteredKey) || enteredKey < 0)
                    {
                        WriteLine();
                        WriteLine("We can only accept non-negative integers.  Please try again.");
                        Write("Enter the key of the record to delete from the hashtable: ");
                    }

                    table.Delete(enteredKey);
                    break;
                case 3: //Search for record in hashtable by its key
                    Write("Enter a key to look for in the hashtable: ");

                    while (!int.TryParse(ReadLine(), out enteredKey) || enteredKey < 0)
                    {
                        WriteLine();
                        WriteLine("We can only accept integers.  Please try again.");
                        Write("Enter a key to look for in the hashtable: ");
                    }

                    table.Search(enteredKey);
                    break;
                case 4: //Display hashtable
                    table.Display();
                    break;
                default:
                    WriteLine("You somehow picked an option that doesn\'t exist.  There isn\'t anything to do here.");
                    break;
            }
        }
    }
}
