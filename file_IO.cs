using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class File_IO : MonoBehaviour
{
    // Constants

    // Members
        // Writers
    System.IO.StreamWriter ResultsReport;
    System.IO.StreamWriter OfficialReport;
        // Readers
    System.IO.StreamReader BasicDetailsReader;
    System.IO.StreamReader SuppressionFileReader;
    System.IO.StreamReader OrientationFileReader;
    System.IO.StreamReader SideOfSupressionFileReader;
    System.IO.StreamReader BreakPointFileReader;
    System.IO.StreamReader ResultsReader;
        // Directories (writers)
    string resultsDirectory;
    string officialDirectory;
        // Paths (Readers)
    string pathBasicDetails;
    string pathSupression;
    string pathOrientation;
    string pathSideOfSupression;
    string pathBreakPoint;
        // Basic Details variables
    string userID;
    int numOfTrials;
    int numBlocksOfTrials;
    string userGender;
    int userAge;

    // Constructors
    File_IO(string localDirectory, DateTime TrialDateTime)
    {
        assignPaths(localDirectory);
        constructGeneralReaders(localDirectory);
        readBasicDetails();
        assignDirectories(localDirectory, TrialDateTime);
        constructGeneralWriters();
        makeHeading()
    }

    // Private Methods
    private void assignPaths(string localDirectory)
    {
        pathBasicDetails = directory + "subjectInput.txt";
        pathSupression = directory + "inputFile_NoiseVsNoNoise.txt";
        pathOrientation = directory + "inputFile_UpVsDown.txt";
        pathSideOfSupression = directory + "inputFile_LeftVsRight.txt";
        pathBreakPoint = directory + "inputFile_BlockVsNoBlock.txt";
    }

    private void assignDirectories(string localDirectory, DateTime dateOfTrial)
    {
        resultsDirectory = directory + "(Lab_Results).csv";
        officialDirectory = formattedReportDirectory = directory + userID + "_" + Convert.ToString(dateOfTrial.Month) + "." +
            Convert.ToString(dateOfTrial.Day) + "." + Convert.ToString(dateOfTrial.Year)
            + "_(" + Convert.ToString(dateOfTrial.Hour) + ";" + Convert.ToString(dateOfTrial.Minute) + ";" + Convert.ToString(dateOfTrial.Second) + ").csv";
    }

    private void constructGeneralReaders()
    {
        BasicDetailsReader = new System.IO.StreamReader(pathBasicDetails);
        SuppressionFileReader = new System.IO.StreamReader(pathSupression);
        OrientationFileReader = new System.IO.StreamReader(pathOrientation);
        SideOfSupressionFileReader = new System.IO.StreamReader(pathSideOfSupression);
        BreakPointFileReader = new System.IO.StreamReader(pathBreakPoint);
    }

    private void constructGeneralWriters()
    {
        ResultsReport = new System.IO.StreamWriter(resultsDirectory);
        OfficialReport = new System.IO.StreamWriter(officialDirectory);
    }

    private void readBasicDetails()
    {
        userID = BasicDetailsReader.ReadLine();
        numOfTrials = System.Convert.ToInt32(BasicDetailsReader.ReadLine());
        numBlocksOfTrials = Convert.ToInt32(BasicDetailsReader.ReadLine());
        userGender = BasicDetailsReader.ReadLine();
        userAge = Convert.ToInt32(BasicDetailsReader.ReadLine());
    }

    private void reverseResultsFileDataFlow()
    {
        ResultsReport.close();
        ResultsReader = new System.IO.StreamReader(resultsDirectory);
    }

    private void closeGeneralReaders()
    {
        BasicDetailsReader.Close();
        SuppressionFileReader.Close();
        OrientationFileReader.Close();
        SideOfSupressionFileReader.Close();
        BreakPointFileReader.Close();
    }

    private void closeGeneralWriters()
    {
        if(ResultsReport != null)
        {
            ResultsReport.close();
        }
        OfficialReport.close();
    }

    // Public Methods

    public void transferBasicDetails(ref userID, ref numOfTrials, ref numBlocksOfTrials, ref userGender, ref userAge)
    {
        userID = this.userID;
        numOfTrials = this.numOfTrials;
        numBlocksOfTrials = this.numBlocksOfTrials;
        userGender = this.userGender;
        userAge = this.userAge;
    }

    public void printHeadingForResults()
    {
        ResultsReport.Write("Subject ID:, " + userID + ", Age:, " + userAge +
            ", Gender:, " + userGender);
        ResultsReport.WriteLine();
        ResultsReport.Write("Total Trials:,");
        ResultsReport.WriteLine(numOfTrials);
        ResultsReport.WriteLine("");
        ResultsReport.WriteLine("Trial #, Supression?, SideOfSupp?, " +
            "User Response, Response time, Were they correct?");
        ResultsReport.Flush();
    }

    public void printTrialResult(int timesRun, bool isSuppressed, string sideOfSuppression, string userReturned, int timeUntilResponse, string isUserCorrect)
    {
        ResultsReport.Write(Convert.ToString(timesRun) + ",");
        ResultsReport.Write(Convert.ToString(isSuppressed) + ",");
        ResultsReport.Write(sideOfSuppression + ",");
        ResultsReport.Write(userReturned + ",");
        if (timeUntilResponse < 900)
        {
            ResultsReport.Write(Convert.ToString(timeUntilResponse) + ",");
        }
        else
        {
            ResultsReport.Write("NaN,");
        }
        ResultsReport.WriteLine(isUserCorrect);
        ResultsReport.Flush();
    }

    public void copyResultsFile()
    {
        string scannedLine;
        reverseResultsFileDataFlow();
        while((scannedLine = ResultsReader.ReadLine()) != null)
        {
            OfficialReport.WriteLine(scannedLine);
        }
        closeGeneralReaders();
        closeGeneralWriters();
    }

    public void printError(string error)
    {
        ResultsReport.WriteLine(error);
    }
}
