using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using static System.Console;

namespace MyHashTableProject
{
    class HashTable
    {
        private Node[] addresses;

        public HashTable(int wantedSize = 11)
        {
            if(wantedSize < 1)
            {
                throw new InvalidOperationException(@"Hashtable must at leat have a size of one.");
            }

            addresses = new Node[wantedSize];
            for (int i = 0; i < addresses.Length; i++)
            {
                addresses[i] = null;
            }
        }

        public void Insert(int key, string name)
        {
            if(key < 0)
            {
                WriteLine("The value of the key can\'t be negative.");
                return;
            }

            int addressToAddNewRecord = key % addresses.Length; //Hashing the key
            Node temp = new Node(key, name);

            if(addresses[addressToAddNewRecord] is null) //No nodes are currently at this address
            {
                addresses[addressToAddNewRecord] = temp;
                WriteLine($"The record with key #{key} has been added to the hashtable.");
                return;
            }

            Node previous, current;

            //Loop to determine if, and where, the new node will be placed in the linked list
            //Records will be ordered by their key value in ascending order.
            for(previous = null, current = addresses[addressToAddNewRecord];
                current != null;
                previous = current, current = current.link)
            {
                if(current.key == key) //Record with specified key already exists
                {
                    WriteLine($"The key value, {key}, is already in the hashtable.");
                    return;
                }

                if(key < current.key) //The location to insert the new record isn't at the end of the linked list
                {
                    temp.link = current;

                    if(previous is null) //Inserting new node at the beginning of the linked list
                    {
                        addresses[addressToAddNewRecord] = temp;
                    }
                    else //Inserting new node somewhere in between the first and last node of the address' linked list
                    {
                        previous.link = temp;
                    }

                    WriteLine($"The record with key #{key} has been added to the hashtable.");
                    return;
                }
            }

            //If the value of the key is larger than any of the key-values at the selected address
            previous.link = temp; //Adds record to the end of the address' linked list
            WriteLine($"The record with key #{key} has been added to the hashtable.");
        }

        public void Delete(int key)
        {
            if (key < 0)
            {
                WriteLine("The value of the key can\'t be negative.");
                return;
            }

            int addressOfRecordToDelete = key % addresses.Length; //Hashing the key
            Node previous, current;

            //Loop to search for record we're looking to delete.
            for (previous = null, current = addresses[addressOfRecordToDelete];
                 current != null && key >= current.key;
                 previous = current, current = current.link)
            {
                if (current.key == key) //Record with specified key has been found
                {
                    if(previous is null) //Record to delete is first node at the address
                    {
                        addresses[addressOfRecordToDelete] = current.link;
                    }
                    else //Otherwise
                    {
                        previous.link = current.link;
                    }

                    WriteLine($"The record with the key value, {key}, has been deleted from the hashtable.");
                    return;
                }
            }

            //Record with specified key value couldn't be found.
            WriteLine($"No record with the key value, {key}, exists in the hashtable.");
        }

        public void Search(int key)
        {
            if (key < 0)
            {
                WriteLine("The value of the key can\'t be negative.");
                return;
            }

            int addressOfRecordToFind = key % addresses.Length; //Hashing the key

            //Loop to search for a record with the specified key
            for(Node p = addresses[addressOfRecordToFind]; p != null && key >= p.key; p = p.link)
            {
                if(key == p.key)
                {
                    WriteLine($"A record with the key value, {key}, has been found at address {addressOfRecordToFind}.");
                    WriteLine($"The record contains the name {p.name}.");
                    return;
                }
            }

            WriteLine(@$"A record with the key value, {key}, doesn't exist.");
        }

        public void Display()
        {
            Node p;

            //Prints the hashtable
            for(int i = 0; i < addresses.Length; i++)
            {
                Write($"{i,3}: ");
                if (addresses[i] != null) //Address is pointing to a node
                {
                    Write($"({addresses[i].key}, {addresses[i].name})");

                    for (p = addresses[i].link; p != null; p = p.link)
                    {
                        Write($"---({p.key}, {p.name})");
                    }
                }

                WriteLine();
            }
        }
    }
}
