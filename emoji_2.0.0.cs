using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Emojis : MonoBehaviour
{
    // Genral Constants
    private const float VERYLARGENUMBER = 50000000f;
    private const bool UPRIGHT = false;
    private const bool DOWNRIGHT = true;
    
    // For trial_text
    public Text LeftTextBox;
    public Text RightTextBox;

    // Variables & Objects
    RawImage LeftSideImage = null;
    RawImage RightSideImage = null;
    RawImage Nimage = null; // What IS this?

    // Objects to store L/RightSideImage in
    public RawImage LeftRawImage;
    public RawImage RightRawImage;

    // Texture List
    private Texture WhiteCross;
    private Texture GreenCross;
    private Texture RedCross;
    private Texture RightSideUpEmoji;
    private Texture UpSideDownEmoji;
    private Texture InterferencePattern;
    private Texture TrainingPrompt;
    private Texture MainPrompt;
    private Texture EndPrompt;
    private Texture BreakPrompt;

    // Remote Variables
    private bool isKeysRaised = true;
    private bool isKeysDepressed = false;
    private bool isDownKeyDepressed = true;
    private bool isUpKeyDepressed = true;
    private bool areGatesInitialized = false;

    // Declare the random number genereator
    System.Random RNG;

    // IOStream Paths and Objects
    System.IO.StreamWriter ResultsReport;
    string resultsDirectory;
    System.IO.StreamWriter CsvReport;
    string CsvDirectory;

    System.IO.StreamReader BasicDetailsReader;
    string pathBasicDetails;
    System.IO.StreamReader SuppressionFileReader;
    string pathSupression;
    System.IO.StreamReader OrientationFileReader;
    string pathOrientation;
    System.IO.StreamReader SideOfSupressionFileReader;
    string pathSideOfSupression;
    System.IO.StreamReader BreakPointFileReader;
    string pathBreakPoint;
    System.IO.StreamReader ResultsReader;
    string pathResultsReader;

    // Code control variables
    private int currentMainStage = 0;
    private int currentBreakStage = 0;
    private bool isTrainingTime = true;
    private bool isTrialsTime = true;
    private bool isTrainingModeActive = true;
    private bool isInitialSetUpActive = true;
    private bool isPreTrialsTimeActive = false;
    private bool hasUserAnswered = false;
    private bool isProgramFinished = false;
    private bool isTimeForBreak = false;
    private bool freezeProgram = false;

    // DateTime Variable
    DateTime dateOfTrial;

    private bool isInitialEmptyTrialRemoved = true;

    // directory
    private string directory;
    private string directory2;

    // times run indexes
    const int TRAININGTRIALS = 15;
    int timesRunTutorial = 1;
    int timesRunRealDeal = 1;
    private int progressTracker = 0;

    // Time Management Varaibles
    private float timeSinceStart = 0.0f;
    private float timePassed = 0.0f;
    private float timeSinceAnswer = 0.0f;
    private float userResponseTime = 0.0f;
    private float timeUntilResponse = 0.0f;
    private float hz = 0.1f;
    //    //    //    //
    private const float FIXATION = 1000.0f; //in milliseconds
    private const float DISPLAY = 500.0f;
    private const float COLLECTRESPONSE = 5000.0f;
    //    //    //    //
    private float trialPrepTime;
    private float trialDisplayTime;
    private float timeZone2;
    //    //    //    //
    // Record keeping Variables
    private int numberCorrect = 0;
    private int numberIncorrect = 0;
    private int numberNotAnswered = 0;

    // We will input a dataFile with following info:
    private bool isRightSideSuppressed = false;  // 0 for left, 1 for right scramble
    private bool isEmojiUpsideDown = false;       // 0 for "up" emoji, 1 for "down" emoji
    private bool isSuppressed = false;         // 0 for no supression, 1 for suppression
    private string userID = "error";
    private string userGender = "error";
    private int numOfTrials = 0;
    private int numBlocksOfTrials = 2;
    private int userAge = 0;
    private string stringSideOfSupression = "";

    // Output info
    private string userReturned = "error";
    private string isUserCorrect = "error";

    // for getting random scramble images
    private int previousNameOfFile;
    private int currentNameOfFile;

    // Properties
    string UserID
    {
        get { return userID; }
        set { userID = value; }
    }

    int NumOfTrials
    {
        get { return numOfTrials; }
        set { numOfTrials = value; }
    }

    void Start() // entry point; when it Start() finishes, Update is called
    {
        // Start up the file stream bois
        //********************************************************************************************
        //directory = @"C:\Users\smokejump.pc\Desktop\GRUBB_files\";
        //directory = @"C:\Users\smokejump.pc\Desktop\old_repo\";
        //directory = @"C:\Users\OculusRift2\Desktop\CFS\";
        //directory = @"C:\Users\smokejump.pc\Desktop\TestDirectory\";

        directory = Application.dataPath;
        directory = directory.Replace("eye_test_Data","");

        //********************************************************************************************
        dateOfTrial = DateTime.Now;

        resultsDirectory = directory + "(Lab_Results).csv";
        ResultsReport = new System.IO.StreamWriter(resultsDirectory);

        pathBasicDetails = directory + "subjectInput.txt";
        pathSupression = directory + "inputFile_NoiseVsNoNoise.txt";
        pathOrientation = directory + "inputFile_UpVsDown.txt";
        pathSideOfSupression = directory + "inputFile_LeftVsRight.txt";
        pathBreakPoint = directory + "inputFile_BlockVsNoBlock.txt";

        // Get the objects oriented...get it?

        BasicDetailsReader = new System.IO.StreamReader(pathBasicDetails);
        SuppressionFileReader = new System.IO.StreamReader(pathSupression);
        OrientationFileReader = new System.IO.StreamReader(pathOrientation);
        SideOfSupressionFileReader = new System.IO.StreamReader(pathSideOfSupression);
        BreakPointFileReader = new System.IO.StreamReader(pathBreakPoint);

        // Init the RNG
        RNG = new System.Random();

        // Start writing the output document
        setBasicDetails();
        makeHeading();

        //////////////////////////////////////////////////
        setTextBoxValue("", LeftTextBox, RightTextBox);
        //LeftTextBox.text = "";
        //RightTextBox.text = LeftTextBox.text;
        //////////////////////////////////////////////////////
        // Boot Resources
        RightSideUpEmoji = Resources.Load("up") as Texture;
        UpSideDownEmoji = Resources.Load("down") as Texture;
        WhiteCross = Resources.Load("bg2_gray") as Texture;
        GreenCross = Resources.Load("bg2_green") as Texture;
        RedCross = Resources.Load("bg2_red") as Texture;
        TrainingPrompt = Resources.Load("bg2_training") as Texture;
        MainPrompt = Resources.Load("bg2_start") as Texture;
        EndPrompt = Resources.Load("bg2_end") as Texture;
        BreakPrompt = Resources.Load("bg2_break") as Texture;

        // Init the LEFT EYE
        LeftSideImage = LeftRawImage;
        LeftSideImage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

        // Init RIGHT EYE
        RightSideImage = RightRawImage;
        RightSideImage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

        // Init the on-screen text
        //LeftTextBoxeft.text = "";
        //RightTextBoxight.text = "";
        //LeftTextBoxeft.fontSize = 32;
        //RightTextBoxight.fontSize = 32;


        // Init timeZones (in seconds)
        trialPrepTime = FIXATION / 1000.0f;
        trialDisplayTime = (FIXATION + DISPLAY) / 1000.0f;
        timeZone2 = (trialDisplayTime + COLLECTRESPONSE) / 1000.0f;

    }  // END start()

    void Update() // Note Bene (N.B.): Update is called once per frame, and
                  //keeps going until the program ends
    {
        // Compesate headset rotations/position
        transform.position = -InputTracking.GetLocalPosition(VRNode.CenterEye);
        transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(VRNode.CenterEye));

        // Gather Time
        timeSinceStart += Time.deltaTime;
        timePassed += Time.deltaTime;
        timeSinceAnswer += Time.deltaTime;
        userResponseTime += Time.deltaTime;

        //Controll Flow statements
        if (!isProgramFinished)
        {
            if (isInitialSetUpActive)
            {
                getStartSignalFromUser(0);
            }
            else if (isTrainingTime)
            {
                displayWhiteCross();
                bufferTheNextStage(ref isTrainingTime);
            }
            else if (timesRunTutorial < TRAININGTRIALS)
            {
                if (hasUserAnswered) {
                    timeZone2 = VERYLARGENUMBER;
                }
                checkStates(isSuppressed, isRightSideSuppressed, isEmojiUpsideDown);
                if (timesRunTutorial == TRAININGTRIALS)
                {
                    isPreTrialsTimeActive = true;
                }
            }
            else
            {
                if (isPreTrialsTimeActive)
                {
                    timeZone2 = (trialDisplayTime + COLLECTRESPONSE) / 1000.0f;
                    getStartSignalFromUser(1);
                }
                else if (isTrialsTime)
                {
                    displayWhiteCross();
                    bufferTheNextStage(ref isTrialsTime);
                }
                else if (isTimeForBreak)
                {
                    performBreak(timesRunRealDeal);
                }
                else
                {
                    if (timesRunRealDeal == NumOfTrials + 1)
                    {
                        beginEndOfProgram();
                    }
                    else if (timesRunRealDeal < NumOfTrials + 1)
                    {
                        checkStates(isSuppressed, isRightSideSuppressed, isEmojiUpsideDown);
                        if (!isInitialEmptyTrialRemoved)
                        {
                            timeZone2 = VERYLARGENUMBER;
                        }
                    }
                }

            }
        }
    }



    //-------------------------------------------------------------------------------------------------------------------------------------------



    void checkStates(bool suppressionCheck, bool sideSurpressedCheck, bool emojiFlipCheck) //check the progress of the test
    {
        if (hasUserAnswered)
        {
            if (timeSinceAnswer < 0.75)
            {
            }
            else
            {
                timeSinceStart = 0;
                currentMainStage = 0;
                displayWhiteCross();
                if (isTrainingModeActive)
                {
                    timesRunTutorial++;
                    hasUserAnswered = false;
                }
                else
                {
                    writeToFile();
                    checkIfBreakIsNeeded();
                    hasUserAnswered = false;
                }
            }

        }
        else
        {
            if (timeSinceStart <= trialPrepTime) // TimeZone 0: Start of test progress
            {
                if (currentMainStage == 0) // Stage 0
                {
                    if (isTrainingModeActive)
                    {
                        trainingInit();
                    }
                    else
                    {
                        actualInit();
                    }
                    displayWhiteCross();
                    currentMainStage++;
                } // end State 0

            } // end of timeZone 0
            else if (timeSinceStart <= trialDisplayTime) // timeZone 1: diplay the test
            {
                if (currentMainStage == 1) // Stage 1
                {
                    // Set up the user feedback
                    allowCalculations(emojiFlipCheck);

                    // Reset the timer and prepare to engage the next phase
                    timePassed = 0;
                    currentMainStage++;

                    // Now, actually show everything
                    randomizeSupressionImage(suppressionCheck, sideSurpressedCheck);
                    setEmoji(suppressionCheck, sideSurpressedCheck, emojiFlipCheck);

                    // The participant is on the clock now:
                    userResponseTime = 0;

                } // end State 1

                getUserInteraction(); // Check for user feedback

                if (timePassed >= hz) // Change out the supression image
                {
                    if (operations.AllowCalculations == true)
                    {
                        randomizeSupressionImage(suppressionCheck, sideSurpressedCheck);
                        timePassed = 0;
                    }
                }

            } // end TimeZone 1
            else if (timeSinceStart <= timeZone2) // TimeZone 2: Get feedback
            {

                if (currentMainStage == 2)
                {
                    if (operations.AllowCalculations == true)
                    {
                        displayWhiteCross();
                    }
                    currentMainStage++;
                }

                getUserInteraction(); // Keep checking
            }
            else
            {
                timeSinceStart = 0;
                currentMainStage = 0;
                displayWhiteCross();
                if (isTrainingModeActive)
                {
                    timesRunTutorial++;
                }
                else
                {
                    timeUntilResponse = 999999;
                    isUserCorrect = "NaN";
                    writeToFile();
                    checkIfBreakIsNeeded();

                }
            }
        }
    }

    void performBreak(int trialsCompleted)
    {
        if (!freezeProgram)
        {
            if (currentBreakStage == 0)
            {
                displayProgressPrompt();
                currentBreakStage++;
            }

            if (currentBreakStage == 1)
            {
                getStartSignalFromUser(2);
            }

            if (currentBreakStage == 2)
            {
                if (timePassed < 0.75)
                {
                    setTextBoxValue("", LeftTextBox, RightTextBox);
                }
                else
                {
                    timeSinceStart = 0;
                    timePassed = 0;
                    timeSinceStart = 0;
                    currentMainStage = 0;

                    freezeProgram = true;
                    currentBreakStage = 0;
                    isTimeForBreak = false;
                    freezeProgram = false;
                }
            }
        }
    }

    void randomizeSupressionImage(bool suppressionExists, bool sideOfSuppression)
    {
        if (suppressionExists)
        {
            var random = new System.Random();

            do
            {
                currentNameOfFile = random.Next(0, 16);
                random = new System.Random(random.Next());
            }
            while (currentNameOfFile == previousNameOfFile);

            string scrambleFile = "m" + currentNameOfFile;
            previousNameOfFile = currentNameOfFile;
            displaySupressionImage(sideOfSuppression, scrambleFile);
        }
    }


    void displaySupressionImage(bool suppressionSide, string mFile)
    {
        if (isSuppressed)
        {
            InterferencePattern = Resources.Load(mFile) as Texture;
            if (suppressionSide)
            {
                LeftSideImage.texture = InterferencePattern;
            }
            else
            {
                RightSideImage.texture = InterferencePattern;
            }
        }

    }

    void setEmoji(bool suppressionExist, bool whichSideSuppression, bool emojiFlipState)
    {
        if (suppressionExist)
        {
            if (whichSideSuppression)
            {
                if (emojiFlipState)
                {
                    RightSideImage.texture = RightSideUpEmoji;
                }
                else
                {
                    RightSideImage.texture = UpSideDownEmoji;
                }
            }
            else
            {
                if (emojiFlipState)
                {
                    LeftSideImage.texture = RightSideUpEmoji;
                }
                else
                {
                    LeftSideImage.texture = UpSideDownEmoji;
                }
            }
        }
        else
        {
            if (emojiFlipState)
            {
                RightSideImage.texture = RightSideUpEmoji;
                LeftSideImage.texture = RightSideUpEmoji;
            }
            else
            {
                LeftSideImage.texture = UpSideDownEmoji;
                RightSideImage.texture = UpSideDownEmoji;
            }
        }
    }


    void displayWhiteCross()
    {
        LeftSideImage.texture = WhiteCross;
        RightSideImage.texture = WhiteCross;
    }

    void displayGreenCross()
    {
        LeftSideImage.texture = GreenCross;
        RightSideImage.texture = GreenCross;
    }

    void displayRedCross()
    {
        LeftSideImage.texture = RedCross;
        RightSideImage.texture = RedCross;
    }

    void displayMainStartPrompt()
    {
        LeftSideImage.texture = MainPrompt;
        RightSideImage.texture = MainPrompt;
    }

    void displayTrainingStartPrompt()
    {
        LeftSideImage.texture = TrainingPrompt;
        RightSideImage.texture = TrainingPrompt;
    }

    void displayEndOfProgramPrompt()
    {
        LeftSideImage.texture = EndPrompt;
        RightSideImage.texture = EndPrompt;
    }

    void displayProgressPrompt()
    {
        var progressText = "Blocks completed: " + progressTracker.ToString() + " out of " + (numBlocksOfTrials + 1).ToString(); // progressTracker, numBlocksOfTrials;
        progressTracker++;
        setTextBoxValue(progressText, LeftTextBox, RightTextBox);
        LeftSideImage.texture = BreakPrompt;
        RightSideImage.texture = BreakPrompt;
    }

    void getUserInteraction()
    {
        if ((Input.GetAxis("LeftTrigger") >= 0.85) || Input.GetKeyUp("down"))
        {
            if (operations.AllowCalculations == true)
            {
                timeUntilResponse = userResponseTime;
                userReturned = "Upright";
                if (operations.checkIfCorrect(UPRIGHT))
                {
                    displayGreenCross();
                    isUserCorrect = "Correct";
                }
                else
                {
                    displayRedCross();
                    isUserCorrect = "Incorrect";
                }
                timeSinceAnswer = 0;
                hasUserAnswered = true;
                operations.AllowCalculations = false;
            }
        }
        else if ((Input.GetAxis("RightTrigger") >= 0.85) || Input.GetKeyUp("up"))
        {
            if (operations.AllowCalculations == true)
            {
                timeUntilResponse = userResponseTime;
                userReturned = "Upside-Down";
                if (operations.checkIfCorrect(DOWNRIGHT))
                {
                    displayGreenCross();
                    isUserCorrect = "Correct";
                }
                else
                {
                    displayRedCross();
                    isUserCorrect = "Incorrect";
                }
                timeSinceAnswer = 0;
                hasUserAnswered = true;
                operations.AllowCalculations = false;
            }
        }
    }

    void writeToFile()  // here we need to write data to a csv file:
                        //Trial #, if supression was used, user's response,
                        //user response time, and boolean correct or incorrect
    {
        if (isInitialEmptyTrialRemoved == true)
        {
            isInitialEmptyTrialRemoved = false;
        }
        else
        {
            ResultsReport.Write(Convert.ToString(timesRunRealDeal) + ",");
            ResultsReport.Write(Convert.ToString(isSuppressed) + ",");
            calculateStringSideOfSupression();
            ResultsReport.Write(stringSideOfSupression + ",");
            ResultsReport.Write(userReturned + ",");
            if (timeUntilResponse < 900)
            {
                ResultsReport.Write(timeUntilResponse);
                ResultsReport.Write(",");
            }
            else
            {
                ResultsReport.Write("NaN,");
            }

            ResultsReport.WriteLine(isUserCorrect);
            ResultsReport.Flush();
        }
        timesRunRealDeal++;
    }

    public void makeHeading()
    {
        ResultsReport.Write("Subject ID:, " + userID + ", Age:, " + userAge + ", Gender:, " + userGender);
        ResultsReport.WriteLine();
        ResultsReport.Write("Total Trials:,");
        ResultsReport.WriteLine(NumOfTrials);
        ResultsReport.WriteLine("");
        ResultsReport.WriteLine("Trial #, Supression?, SideOfSupp?, User Response, Response time, Were they correct?");
        ResultsReport.Flush();
    }

    public bool setSuppression()
    {
        int getSuppression;
        if (SuppressionFileReader.ReadLine() == "1")
        {
            getSuppression = 1;
        }
        else
        {
            getSuppression = 0;
        }
        if (getSuppression == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool calculateIsEmojiFlipped()
    {
        // is emoji upside down?
        int emojiState= 2; //Error code "2", signals something is wrong, we shouldnt get twos
        if (OrientationFileReader.ReadLine() == "1")
        {
            emojiState= 1;
            return true;
        }
        else
        {
            emojiState= 0;
            return false;
        }
    }

    public bool setSideOfSuppression()
    {
        if (isSuppressed == true)
        {
            int getSideOfSupression = 0;
            if (SideOfSupressionFileReader.ReadLine() == "1")
            {
                getSideOfSupression = 1;
            }
            else
            {
                getSideOfSupression = 0;
            }
            if (getSideOfSupression == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            ResultsReport.WriteLine("We got issues!");
            return false;
        }
    }

    public void setBasicDetails()
    {
        userID = BasicDetailsReader.ReadLine();
        NumOfTrials = System.Convert.ToInt32(BasicDetailsReader.ReadLine());
        numBlocksOfTrials = Convert.ToInt32(BasicDetailsReader.ReadLine());
        userGender = BasicDetailsReader.ReadLine();
        userAge = Convert.ToInt32(BasicDetailsReader.ReadLine());
    }

    // TODO
    // make this double squeeze -> both released
    void getStartSignalFromUser(int index)
    {
        if (!areGatesInitialized)
        {
            isKeysRaised = true;
            isKeysDepressed = false;
            isDownKeyDepressed = true;
            isUpKeyDepressed = true;
            areGatesInitialized = true;
        }

        if (index == 0)
        {
            displayTrainingStartPrompt();
        }
        else if (index == 1)
        {
            displayMainStartPrompt();
        }

        if ((((Input.GetAxis("LeftTrigger") > 0.8 || Input.GetKeyUp("up")) && ((Input.GetAxis("RightTrigger")) > 0.8 || Input.GetKeyUp("down")))) || Input.GetButton("Start"))
        {
            isKeysRaised = false;
        }

        if (!isKeysRaised) // trigger are depressed
        {
            isKeysDepressed = true;

            if (((Input.GetAxis("LeftTrigger") < 1)) || Input.GetKeyUp("down"))
            {
                isDownKeyDepressed = false;
            }
            if ((Input.GetAxis("RightTrigger") < 1) || Input.GetKeyUp("up"))
            {
                isUpKeyDepressed = false;
            }
            if (Input.GetButtonUp("Start"))
            {
                isDownKeyDepressed = false;
                isUpKeyDepressed = false;
            }

        }

        if (isKeysDepressed && (!(isDownKeyDepressed || isUpKeyDepressed)))
        {
            displayWhiteCross();
            if (index == 0)
            {
                isInitialSetUpActive = false;
                isKeysRaised = true;
                areGatesInitialized = false;
                timePassed = 0;
            }
            else if (index == 1)
            {
                isPreTrialsTimeActive = false;
                isTrainingModeActive = false;
                isKeysRaised = true;
                areGatesInitialized = false;
                timePassed = 0;
            }
            else
            {
                currentBreakStage++;
                timePassed = 0;
                isKeysRaised = true;
                areGatesInitialized = false;
            }
            timePassed = 0;
        }
    }

    void bufferTheNextStage(ref bool check)
    {
        if (check == true)
        {
            if (timePassed >= 1)
            {
                check = false;
                timePassed = 0;
            }
        }
    }

    void trainingInit()
    {
        isSuppressed = false;
        isRightSideSuppressed = false;
        isEmojiUpsideDown = System.Convert.ToBoolean(RNG.Next(0, 2));
        RNG = new System.Random(RNG.Next());
    }

    void actualInit()
    {
        isSuppressed = setSuppression();
        if (isSuppressed == true)
        {
            isRightSideSuppressed = setSideOfSuppression();
        }
        isEmojiUpsideDown = calculateIsEmojiFlipped();
    }

    void allowCalculations(bool theEmoji)
    {
        // Translate that "set" into actually usable code
        if (!theEmoji)
        {
            operations.UdImage = UPRIGHT;
            operations.AllowCalculations = true;
        }
        else
        {
            operations.UdImage = DOWNRIGHT;
            operations.AllowCalculations = true;
        }
    }

    void beginEndOfProgram()
    {
        displayWhiteCross();
        OrientationFileReader.Close();
        SideOfSupressionFileReader.Close();
        SuppressionFileReader.Close();
        BasicDetailsReader.Close();
        ResultsReport.Close();
        string newLine = "";
        CsvDirectory = directory + UserID + "_" + Convert.ToString(dateOfTrial.Month) + "." +
            Convert.ToString(dateOfTrial.Day) + "." + Convert.ToString(dateOfTrial.Year)
            + "_(" + Convert.ToString(dateOfTrial.Hour) + ";" + Convert.ToString(dateOfTrial.Minute) + ";" + Convert.ToString(dateOfTrial.Second) + ").csv";
        ResultsReader = new System.IO.StreamReader(resultsDirectory);
        CsvReport = new System.IO.StreamWriter(CsvDirectory); //

        while ((newLine = ResultsReader.ReadLine()) != null)
        {
            CsvReport.WriteLine(newLine);
        }
        ResultsReader.Close();
        CsvReport.Close();

        displayEndOfProgramPrompt();
        isProgramFinished = true;
    }

    void checkIfBreakIsNeeded()
    {
        if (Convert.ToInt32(BreakPointFileReader.ReadLine()) == 1)
        {
            isTimeForBreak = true;
        }
        else
        {
            isTimeForBreak = false;
        }
    }

    void calculateStringSideOfSupression()
    {
        if (isSuppressed)
        {
            if (!isRightSideSuppressed)
            {
                stringSideOfSupression = "Right Side";
            }
            else
            {
                stringSideOfSupression = "Left Side";
            }
        }
        else
        {
            stringSideOfSupression = "Null";
        }

    }

    // Temporaily unused
    void setTextBoxValue(string message, Text left, Text right) {
        left.text = message;
        right.text = message;
    }

}
