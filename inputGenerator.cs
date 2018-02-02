/* START_OF_CODE
* fileName: diablo2.cs
* author: PANDA LAB
*
* date created: February 1st, 2018
* date modified: February 1st, 2018
*/

// Standard "using"s for c# general use.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PANDA
{
	class Boop // Main Class (named by Prof. Huang)
	{
	    static void Main(string[] args){ // Entry point
	       // get input from user, for # of Trials they want to perform:
//		    Console.WriteLine("Please Enter the number of Trials");
		    int requestedTrials = Convert.ToInt32(Console.ReadLine()); // Gets user input
		    // Pass that value to methods that makes the 1's and 0's:
		    setRandom(requestedTrials); // (go to line 30)
		    // After all the calculations are done, make sure we have equal amounts of 0's and 1's:
		    Records.printValues(); // (go to line 92)
		}
	    static void setRandom(int numOfTrials){ //Called from line(s): 26
	        // declare the objects needed:
	        Records trialRecords = new Records(numOfTrials); // contructs the class "Records" with "numOfTrials" (see line 61)
		    var random = new System.Random(); // new System.Random object named "random"
		     //the good stuff: here's where we randomly get our 0's and 1's
		    for(int x = 0; x < numOfTrials; x++){
		        if(Records.Ones == 0){ // We've used up all our 1's: we need more 0's:
		            int j = 0;
		            Records.decNum(j); // update our records of how many 1's and 0's we have used
	    	        Console.WriteLine(j);
		        } else if(Records.Zeros == 0){ // We've used up all our 0's: we need more 1's:
		            int j = 1;
		            Records.decNum(j); // update our records of how many 1's and 0's we have used
	    	        Console.WriteLine(j);
		        } else {
		        int j = random.Next(0,2); // get a random 0 or 1
		        Records.decNum(j); // update our records of how many 1's and 0's we have used
	    	    Console.WriteLine(j);
	    	    // end of methodCall: go back to where it was called
		        }
		    }
		}
		

	}
	class Records{
	    //Constructors
	    public Records(){
            // empty! we don't want to do anything with this constructor
	    }
	    
	    public Records(int reqTrials){ // Sets the class and all members to have the # of trials requested
	        setStart(reqTrials);
	    }
	    
	    // class variables:
	    private static int ones = 0;
	    private static int zeros = 0;
	    
	    
	    // class methods:
	    public static int Ones{
	         get { return ones; }
             set { ones = value; }
	    }
	    public static int Zeros{
	         get { return zeros; }
             set { zeros = value; }
	    }
	    public static void setStart(int startingNum){ // sets the maximum number of 1's and 0's 
	        ones = startingNum/2;
	        zeros = startingNum-/2;
	    }
	    
	    public static void decNum(int index){ // decriments 1's or 0's after they've been printed
		    if(index == 1){
		        --ones;
		    } else {
		        --zeros;
		    }
		}
		
		public static void printValues(){
		    Console.WriteLine("The number of Ones needed for parity");
		   Console.WriteLine(ones);
		   Console.WriteLine("The number of Zeros needed for parity");
		   Console.WriteLine(zeros);
		}
	    
	    
	}
}

//END_OF_CODE
