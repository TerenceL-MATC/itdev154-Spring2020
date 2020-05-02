using System;
using static System.Console;
using System.Collections.Generic;
using System.Threading;

namespace MyBubbleAndQuickSortProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int decision;
            int[] createdList;

            while(true)
            {
                decision = MainMenu();

                if(decision is 3)
                {
                    break;
                }

                createdList = CreateList();
                AppOperations(createdList, decision);

                WriteLine("Press any key to clear the screen");
                ReadKey();
                Clear();
            }
        }

        static int MainMenu()
        {
            int choice;

            WriteLine();
            WriteLine("This app will let you see the inner workings of a bubble sort and a quick sort.");
            WriteLine();
            WriteLine("Please select an option!");
            WriteLine("1. Bubble Sort");
            WriteLine("2. Quick Sort");
            WriteLine("3. Quit");
            Write("Your choice: ");

            while(!int.TryParse(ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                WriteLine(@"Sorry, that option doesn't exist!  Please try again.");
                Write("Your choice (1-3): ");
            }

            return choice;
        }

        static int[] CreateList()
        {
            int enteredNumber;
            string enteredValue;
            List<int> newList = new List<int>();

            WriteLine("\nEnter a list of numbers below, one at a time");
            WriteLine("Hit the Enter key without typing anything signal that you done entering the list.");

            for(int i = 1; ; i++)
            {
                Write($"{i}: ");
                enteredValue = ReadLine();

                while(!int.TryParse(enteredValue, out enteredNumber) && enteredValue != "")
                {
                    WriteLine("Only whole numbers can be accepted!  Please try again.");
                    Write($"{i}: ");
                    enteredValue = ReadLine();
                }

                if(enteredValue is "")
                {
                    WriteLine("List has been finalized.");
                    Write(@"Let's see how this sort works!");
                    Thread.Sleep(1000);
                    Write('.');
                    Thread.Sleep(1000);
                    Write('.');
                    Thread.Sleep(1000);
                    Write('.');
                    Thread.Sleep(1000);
                    Clear();
                    return newList.ToArray();
                }
                else
                {
                    newList.Add(enteredNumber);
                }
            }
        }

        static void AppOperations(int[] userList, int choice)
        {
            WriteLine();

            switch (choice)
            {
                case 1: //Bubble Sort
                    Write("Your list started as: ");
                    userList.PrintList();
                    WriteLine();
                    userList.BubbleSort();
                    break;
                case 2: //Quick Sort
                    userList.QuickSort();
                    Write($"The sorted list looks like: ");
                    userList.PrintList();
                    break;
                default:
                    WriteLine(@"You somehow picked an option that doesn't exist.  There isn't any to do here.");
                    break;
            }

            WriteLine();
        }
    }
}
