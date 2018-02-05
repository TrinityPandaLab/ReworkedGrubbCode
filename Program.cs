/* START_OF_CODE
* fileName: diablo2.cs
* author: PANDA LAB
*
* date created: February 1st, 2018
* date modified: February 5st, 2018
*/

//VERSION: 1

// Standard "using"s for c# general use.
using System;
using System.IO;


namespace PANDA
{
    class Boop // Main Class (named by Prof. Huang)
    {
        static int timesCalled = 0; //Used in beginRandom to preduce safe, random numbers.

        static void Main(string[] args)
        { // Entry point
          // get input from user, for # of Trials they want to perform:
            Console.WriteLine("Please Enter the number of Trials");
            int requestedTrials = Convert.ToInt32(Console.ReadLine()); // Gets user input as an int
            // Pass that value to method that makes the 1's and 0's; One call for each file:
            beginRandom((requestedTrials), "inputFile_NoiseVsNoNoise.txt");
            beginRandom((requestedTrials/1), "inputFile_UpVsDown.txt");
            beginRandom((requestedTrials/2), "inputFile_LeftVsRight.txt");
            // Make a TXT file for humans to read
            makeHumanFile();
            // Make a CSV file for records
            createCSVFile(requestedTrials + 1);
            //End the Program
            Console.WriteLine("Success: Type anything to close the program");
            Console.ReadLine();
            
        }
        static void beginRandom(int numOfTrials, string inputFileName)
        {
            //Incriment  timesCalled:
            timesCalled++; //This is part of the random reseeding tool
            int valueToPrint = 0; //Reset upon start of program; Used for printing random numbers.
          // declare the objects needed:
            StreamWriter writeToFile = new StreamWriter(inputFileName); //this allows us to write to a .txt (or maybe in the end .csv) file
            Records trialRecords = new Records(numOfTrials); // contructs the class "Records" with "numOfTrials" (see line 83)
            Random randomSeed = new Random(); // new System.Random object named "random" for seed
            //use the randomSeed to randomly see the other randoms
            Random random1 = new Random(randomSeed.Next()); // new System.Random object named "random" for call 1
            Random random2 = new Random(2*randomSeed.Next()); // new System.Random object named "random" for call 2
            Random random3 = new Random(7+randomSeed.Next()); // new System.Random object named "random" for call 3
            //here's the good stuff: here's where we randomly get our 0's and 1's
            for (int x = 0; x < numOfTrials; x++)
            {
                if (Records.Ones == 0)
                { // We've used up all our 1's: we need more 0's:
                    Records.decNum(0); // update our records of how many 0's we have used
                    writeToFile.WriteLine(0); //print to file
                    //Console.WriteLine(valueToPrint);
                }
                else if (Records.Zeros == 0)
                { // We've used up all our 0's: we need more 1's:
                    Records.decNum(1); // update our records of how many 1's we have used
                    writeToFile.WriteLine(1); //print to file
                    //Console.WriteLine(valueToPrint);
                }
                else
                {    // More randomness: Making sure we have NO duplicate files!
                    if (timesCalled == 1)
                    {
                        valueToPrint = random1.Next(0, 2); // get a random 0 or 1 from RNG 1
                    } else if (timesCalled == 2)
                    {
                        valueToPrint = random2.Next(0, 2); // get a random 0 or 1 from RNG 2
                    } else
                    {
                        valueToPrint = random3.Next(0, 2); // get a random 0 or 1 from RNG 3
                    }
                    Records.decNum(valueToPrint); // update our records of how many 1's and 0's we have used
                    writeToFile.WriteLine(valueToPrint); //print to file
                    //Console.WriteLine(valueToPrint);
                    
                }
            }
            writeToFile.Close(); //Close the file stream, and save the changes.
            //end of method
        }
        static void makeHumanFile(){ // Make a file the Grubb Club can easilly read
            //Objects needed:
            string line;
            var putInput = new StreamWriter("inputFile_ForHuman.txt");
            StreamReader getInput1 = new StreamReader("inputFile_NoiseVsNoNoise.txt"); 
            StreamReader getInput2 = new StreamReader("inputFile_UpVsDown.txt");
            StreamReader getInput3 = new StreamReader("inputFile_LeftVsRight.txt");
            //Code:
            putInput.WriteLine("Noise?,Up?,Left?,1 = 'Yes' and 0 = 'No'");
            for (int i = 0; (line = getInput1.ReadLine()) != null; i++)
            {
              // putInput.WriteLine(line + " , " + getInput2.ReadLine() + "," + 
              //     if((string gotLine = getInput3.ReadLine()) != null));
                putInput.Write(line + "," + getInput2.ReadLine() + ",");
                if (Convert.ToInt32(line) == 0)
                {
                    putInput.WriteLine();
                } else
                {
                    putInput.WriteLine(getInput3.ReadLine());
                }
            }
            getInput1.Close();
            getInput2.Close();
            getInput3.Close();
            putInput.Close();
        }

        static void createCSVFile(int linesToCopy)
        {
            StreamReader txtReader = new StreamReader("inputFile_ForHuman.txt");
            StreamWriter csvWriter = new StreamWriter("inputFile_ForHuman.csv");
            for (int i = 0; i<= linesToCopy; i++)
            {
                csvWriter.WriteLine(txtReader.ReadLine());
            }
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