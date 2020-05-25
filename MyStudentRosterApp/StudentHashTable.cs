using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MyStudentRosterApp
{
    public class StudentHashTable
    {
        private Student[] theTable;
        private bool[] hasBeenDeleted;
        private uint count;

        public StudentHashTable()
        {
            theTable = new Student[7];
            hasBeenDeleted = new bool[7];
            

            for(int i = 0; i < theTable.Length; i++)
            {
                theTable[i] = null;
                hasBeenDeleted[i] = false;
            }
                
            count = 0;
        }

        public StudentHashTable(Student[] someStudents)
        {
            uint initialTableSize = FindNextPrimeAsc(2 * (uint)someStudents.Length);
            theTable = new Student[initialTableSize];
            hasBeenDeleted = new bool[initialTableSize];

            for(int i = 0; i < theTable.Length; i++)
            {
                hasBeenDeleted[i] = false;
            }

            foreach(Student student in someStudents)
            {
                Add(student);
            }
        }

        public bool Add(Student newStudent)
        {
            if(count is 4000000000)
            {
                return false;
            }

            if((uint)theTable.Length < 4000000000 && (double)(count+1)/theTable.Length >= 0.75)
            {
                Expand();
            }

            uint keyValue = newStudent.ID,
                 firstHash = PrimaryHash(keyValue),
                 secondHash = SecondaryHash(keyValue),
                 nextHash;

            for(uint i = 0; i < theTable.Length; i++)
            {
                nextHash = (firstHash + i * secondHash) % (uint)theTable.Length;

                if (theTable[nextHash] is null) //Element isn't occupied
                {
                    theTable[nextHash] = newStudent;
                    hasBeenDeleted[nextHash] = false;
                    count++;
                    return true;
                }
            }

            return false;
        }

        public bool Remove(uint key)
        {
            if(count is 0)
            {
                return false;
            }

            uint? addressOfStudentToRemove = Search(key);

            if(addressOfStudentToRemove is null)
            {
                return false;
            }

            theTable[(uint)addressOfStudentToRemove] = null;
            hasBeenDeleted[(uint)addressOfStudentToRemove] = true;
            count--;

            if(theTable.Length > 1 && (double)count/theTable.Length <= 0.1)
            {
                Trim();
            }

            return true;
        }


        
        public uint? Search(uint key)
        {
            if(count is 0)
            {
                return null;
            }

            uint firstHash = PrimaryHash(key),
                 secondHash = SecondaryHash(key),
                 nextHash;

            for(uint i = 0; i < theTable.Length; i++)
            {
                nextHash = (firstHash + i * secondHash) % (uint)theTable.Length;

                if(theTable[nextHash] is null)
                {
                    if(hasBeenDeleted[nextHash] is false)
                    {
                        return null;
                    }
                }
                else if (theTable[nextHash].ID == key)
                {
                    return nextHash;
                }

            }

            return null;
        }

        public List<uint> Search(string first, string last)
        {
            List<uint> addressesOfMatches = new List<uint>();
            uint i = 0;

            foreach(Student student in theTable)
            {
                if(student.FirstName == first && student.LastName == last)
                {
                    addressesOfMatches.Add(i);
                }

                i++;
            }

            return addressesOfMatches;
        }

        //For initial fill and resetting of table
        public IEnumerable<Student> DataForDisplay()
        {
            foreach(Student student in theTable)
            {
                if(student != null)
                {
                    yield return student;
                }
            }
        }

        private void Expand()
        {
            uint oldCapacity = (uint)theTable.Length,
                 startingNumber = oldCapacity < 2000000000 ? 2 * oldCapacity : 4000000000,
                 newCapacity = FindNextPrimeAsc(startingNumber);

            Rehasher(newCapacity);
        }

        private void Trim()
        {
            uint oldCapacity = (uint)theTable.Length,
                 startingNumber = oldCapacity / 2,
                 newCapacity = FindNextPrimeDesc(startingNumber);

            Rehasher(newCapacity);
        }

        private void Rehasher(uint newCapacity)
        {
            Student[] newHashTable = new Student[newCapacity];
            bool[] newFlagArray = new bool[newCapacity];

            for (uint i = 0; i < newFlagArray.Length; i++)
            {
                newFlagArray[i] = false;
            }

            uint keyValue,
                 firstHash,
                 secondHash,
                 nextHash,
                 R = FindNextPrimeDesc((uint)newHashTable.Length);

            foreach (Student student in theTable)
            {
                if (student != null)
                {
                    keyValue = student.ID;
                    firstHash = keyValue % newCapacity;
                    secondHash = R - (keyValue % R);

                    for (uint i = 0; i < (uint)newHashTable.Length; i++)
                    {
                        nextHash = (firstHash + i * secondHash) % (uint)newHashTable.Length;

                        if (newHashTable[nextHash] is null) //Element isn't occupied
                        {
                            newHashTable[nextHash] = student;
                            break;
                        }
                    }
                }
            }

            theTable = newHashTable;
            hasBeenDeleted = newFlagArray;
        }

        private uint PrimaryHash(uint key)
        {
            return key % (uint)theTable.Length;
        }

        private uint SecondaryHash(uint key)
        {
            uint R = FindNextPrimeDesc((uint)theTable.Length);
            return R - (key % R);
        }

        private static uint FindNextPrimeAsc(uint number)
        {
            if(number is 4000000000)
            {
                return 4000000000;
            }

            uint nextHigherPrime;

            if(number % 2 is 0)
            {
                nextHigherPrime = ++number;
            }
            else
            {
                nextHigherPrime = number + 2;
            }

            while(!IsPrime(nextHigherPrime))
            {
                if(nextHigherPrime >= 4000000000)
                {
                    return 4000000000;
                }

                nextHigherPrime += 2;
            }

            return nextHigherPrime;
        }

        private static uint FindNextPrimeDesc(uint number)
        {
            if(number < 2)
            {
                return 1;
            }

            uint nextLowerPrime;

            if (number % 2 is 0 || number is 3)
            {
                nextLowerPrime = --number;
            }
            else
            {
                nextLowerPrime = number - 2;
            }

            
            while (!IsPrime(nextLowerPrime))
            {
                if (nextLowerPrime is 1)
                {
                    return nextLowerPrime;
                }

                nextLowerPrime -= 2;
            }

            return nextLowerPrime;
        }

        private static bool IsPrime(uint number)
        {
            if(number is 0)
            {
                throw new InvalidOperationException(@"Can't check for prime of 0.");
            }

            uint i;

            for (i = 2; i <= (uint)Math.Sqrt(number) && i != (uint)Math.Sqrt(400000000); i++)
            {
                if(number % i is 0)
                {
                    return false;
                }
            }

            if(i == (uint)Math.Sqrt(400000000))
            {
                throw new Exception("Reached the max value that we should check for prime in hashtable");
            }

            return true;
        }
    }
}
