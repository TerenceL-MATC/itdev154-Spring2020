using static System.Console;

namespace MyBinarySearchTreeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            BinarySearchTree aTree = new BinarySearchTree();

            while(true)
            {
                choice = GetMenuChoice();

                if(choice is 4)
                {
                    break;
                }

                AppOperations(aTree, choice);
            }
        }

        static int GetMenuChoice()
        {
            int choice;
            WriteLine("\nBinary Search Tree Options");
            WriteLine("1. Add integer to search tree");
            WriteLine("2. Delete integer from search tree");
            WriteLine("3. Display the search tree");
            WriteLine("4. Quit");
            Write("Enter your choice: ");

            while (!int.TryParse(ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                WriteLine(@"Sorry!  That option doesn't exist.  Please try again.");
                Write("Enter your choice (1-4): ");
            }

            WriteLine();
            return choice;
        }

        static void AppOperations(BinarySearchTree theTree, int theChoice)
        {
            int keyValue;

            switch(theChoice)
            {
                case 1: //Add integer to search tree
                    Write("Enter a number to add to the search tree: ");
                    
                    while(!int.TryParse(ReadLine(), out keyValue))
                    {
                        WriteLine("Number entered must be a whole number.  Please try again");
                        Write("Enter a number to add to the search tree: ");
                    }

                    theTree.Insert(keyValue);
                    break;
                case 2: //Delete integer to search tree
                    Write("Enter a number to delete from the search tree: ");

                    while (!int.TryParse(ReadLine(), out keyValue))
                    {
                        WriteLine("Number entered must be a whole number.  Please try again");
                        Write("Enter a number to delete from the search tree: ");
                    }

                    theTree.Delete(keyValue);
                    break;
                case 3: //Display the search tree
                    theTree.Display();
                    break;
                default:
                    WriteLine(@"You somehow picked a option that doesn't exist.  There's nothing to do here.");
                    break;
            }

            WriteLine();
        }
    }
}
