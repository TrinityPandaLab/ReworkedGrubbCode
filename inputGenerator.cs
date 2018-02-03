/* START_OF_CODE
* fileName: diablo2.cs
* author: PANDA LAB
*
* date created: February 1st, 2018
* date modified: February 2st, 2018
*/

// Standard "using"s for c# general use.
using System;
using System.IO;
//We're not using these yet:
/*
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/

namespace PANDA
{
    class Boop // Main Class (named by Prof. Huang)
    {
        static void Main(string[] args)
        { // Entry point
          // get input from user, for # of Trials they want to perform:
            Console.WriteLine("Please Enter the number of Trials");
            int requestedTrials = Convert.ToInt32(Console.ReadLine()); // Gets user input as an int
            // Pass that value to method that makes the 1's and 0's; One call for each file:
            beginRandom((requestedTrials), "inputFile_NoiseVsNoNoise.txt");
            beginRandom((requestedTrials/2), "inputFile_Noise_UpVsDown.txt");
            beginRandom((requestedTrials/2), "inputFile_NoNoise_UpVsDown.txt");
            beginRandom((requestedTrials/4), "inputFile_Up_LeftVsRight.txt");
            beginRandom((requestedTrials / 4), "inputFile_Down_LeftVsRight.txt");
            // After all the calculations are done, make sure we have equal amounts of 0's and 1's:
            Console.WriteLine("Success: Type anything to close the program");
            Console.ReadLine();
        }
        static void beginRandom(int numOfTrials, string inputFileNum)
        {
          // declare the objects needed:
            StreamWriter writeToFile = new StreamWriter(inputFileNum); //this allows us to write to a .txt (or maybe in the end .csv) file
            Records trialRecords = new Records(numOfTrials); // contructs the class "Records" with "numOfTrials" (see line 84)
            var random = new Random(); // new System.Random object named "random"
            //here's the good stuff: here's where we randomly get our 0's and 1's
            for (int x = 0; x < numOfTrials; x++)
            {
                if (Records.Ones == 0)
                { // We've used up all our 1's: we need more 0's:
                    int valueToPrint = 0;
                    Records.decNum(valueToPrint); // update our records of how many 1's and 0's we have used
                    writeToFile.WriteLine(valueToPrint); //print to file
                    //Console.WriteLine(valueToPrint);
                }
                else if (Records.Zeros == 0)
                { // We've used up all our 0's: we need more 1's:
                    int valueToPrint = 1;
                    Records.decNum(valueToPrint); // update our records of how many 1's and 0's we have used
                    writeToFile.WriteLine(valueToPrint); //print to file
                    //Console.WriteLine(valueToPrint);
                }
                else
                {
                    int valueToPrint = random.Next(0, 2); // get a random 0 or 1
                    Records.decNum(valueToPrint); // update our records of how many 1's and 0's we have used
                    writeToFile.WriteLine(valueToPrint); //print to file
                    //Console.WriteLine(valueToPrint);
                    
                }
            }
            writeToFile.Close(); //Close the file stream, and save the changes.
            //end of method
        }


    }
    class Records
    {
        //Constructors
        public Records()
        {
            // empty! we don't want to do anything with this constructor
        }

        public Records(int reqTrials)
        { // Sets the class and all members to have the # of trials requested
            setStart(reqTrials);
        }

        // class variables:
        private static int ones = 0;
        private static int zeros = 0;


        // class methods:
        public static int Ones
        {
            get { return ones; }
            set { ones = value; }
        }
        public static int Zeros
        {
            get { return zeros; }
            set { zeros = value; }
        }
        public static void setStart(int startingNum)
        { // sets the maximum number of 1's and 0's 
            ones = startingNum / 2;
            zeros = startingNum / 2;
        }

        public static void decNum(int index)
        { // decriments 1's or 0's after they've been printed
            if (index == 1)
            {
                --ones;
            }
            else
            {
                --zeros;
            }
        }



    }
}

//END_OF_CODE
