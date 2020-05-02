using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace MyBubbleAndQuickSortProject
{
    static class MySortMethods
    {
        public static void BubbleSort(this int[] numberList)
        {
            if(numberList.Length is 0)
            {
                WriteLine(@"Can't sort an empty list.");
                return;
            }

            int swapsMade,    //How many position swaps occured during current pass
                firstNumber,
                secondNumber,
                temp;
            
            //Increments the number of passes made through the list
            for(int onPassNumber = 1; onPassNumber < numberList.Length; onPassNumber++)
            {
                swapsMade = 0;
                firstNumber = 0;
                secondNumber = 1;

                while(firstNumber < numberList.Length - onPassNumber) 
                {
                    if(numberList[firstNumber] > numberList[secondNumber])
                    {
                        temp = numberList[firstNumber];
                        numberList[firstNumber] = numberList[secondNumber];
                        numberList[secondNumber] = temp;
                        swapsMade++;
                    }

                    firstNumber++;
                    secondNumber++;
                }

                if(swapsMade is 0) //List is completely sorted
                {
                    break;
                }

                //Details on the completed pass
                Write($"After Pass {onPassNumber}: ");
                numberList.PrintList();
                WriteLine();
            }

            WriteLine("List is completely sorted.");
        }

        public static void QuickSort(this int[] numberList)
        {
            numberList.QuickSort(0, numberList.Length - 1); 
        }

        private static void QuickSort(this int[] numberList, int startIndex, int endIndex)
        {
            if(endIndex <= startIndex) //If there's less than 2 elements in an array
            {
                return;
            }

            int pivot = numberList[startIndex],
                low = startIndex,
                high = endIndex + 1,
                temp;
            
            //Prints the list passed to the method
            Write("The list is: ");
            numberList.PrintList(startIndex, endIndex);

            //Shift elements to their proper side and finds where pivot should be placed.
            while(high >= low)
            {
                low++;
                high--;

                while(low <= endIndex)
                {
                    if(numberList[low] > pivot)
                    {
                        break;
                    }

                    low++;
                }

                while(high >= low)
                {
                    if(numberList[high] < pivot)
                    {
                        break;
                    }

                    high--;
                }

                if(high < low) //Identified the likely location to move pivot to
                {
                    if (numberList[high] < pivot)
                    {
                        numberList[startIndex] = numberList[high];
                        numberList[high] = pivot;
                    }
                }
                else //Identified two numbers that need to be swapped
                {
                    temp = numberList[low];
                    numberList[low] = numberList[high];
                    numberList[high] = temp;
                }
            }
            
            //More details about the list
            WriteLine($"{pivot} is the pivot.");
            Write("After shifting around the pivot, the list looks like: ");
            numberList.PrintList(startIndex, endIndex);
            Write("Left Sublist: ");
            numberList.PrintList(startIndex, high - 1);
            Write("Right Sublist: ");
            numberList.PrintList(low, endIndex);
            WriteLine();

            //Recursive calls to quick sort the list
            numberList.QuickSort(startIndex, high - 1); //Quick sorts the left sublist
            numberList.QuickSort(low, endIndex);  //Quick sorts the right sublist
        }

        public static void PrintList(this int[] numberList)
        {
            for (int i = 0; i < numberList.Length; i++)
            {
                Write(numberList[i]);
                if (i != numberList.Length - 1)
                {
                    Write(", ");
                }
            }

            WriteLine();
        }

        public static void PrintList(this int[] numberList, int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                Write(numberList[i]);
                if (i != endIndex)
                {
                    Write(", ");
                }
            }

            WriteLine();
        }
    }
}
