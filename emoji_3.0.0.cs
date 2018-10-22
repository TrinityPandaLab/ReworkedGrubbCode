using System;
using System.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Emojis : MonoBehaviour
{
    // Genral Constants
    private const float VERYLARGENUMBER = 50000000f;
    private const bool UPRIGHT = false;
    private const bool DOWNRIGHT = true;
    private const float FIXATION = 1000.0f;         //in milliseconds
    private const float DISPLAY = 500.0f;           //in milliseconds
    private const float COLLECTRESPONSE = 5000.0f;  //in milliseconds

    // DateTime Variable
    DateTime DateOfTrial;

    // For trial_text
    //public Text LeftTextBox;
    //public Text RightTextBox;
    Textbox_IO TextboxManager;

    // Variables & Objects
    //RawImage LeftSideImage = null;
    //RawImage RightSideImage = null;
    //RawImage Nimage = null; // What IS this?
    Image_IO ImageManager;
    public RawImage LeftRawImage;
    public RawImage RightRawImage;

    // Texture List
    //private Texture WhiteCross;
    //private Texture GreenCross;
    //private Texture RedCross;
    //private Texture RightSideUpEmoji;
    //private Texture UpSideDownEmoji;
    //private Texture InterferencePattern;
    //private Texture TrainingPrompt;
    //private Texture MainPrompt;
    //private Texture EndPrompt;
    //private Texture BreakPrompt;
    Texture_IO TextureManager;

    // Remote Variables
    private bool isKeysRaised = true;
    private bool isKeysDepressed = false;
    private bool isDownKeyDepressed = true;
    private bool isUpKeyDepressed = true;
    private bool areGatesInitialized = false;

    // Declare the random number genereator
    Random RNG;

    // IOStream Paths and Objects
    File_IO FileManager;
    /*
    System.IO.StreamWriter ResultsReport;
    string resultsDirectory;
    System.IO.StreamWriter OfficialReport;
    string formattedReportDirectory;

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
*/
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

    private bool isInitialEmptyTrialRemoved = true;

    // directory
    private string directory;

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
    private string stringSideOfSuppression = "";

    // Output info
    private string userReturned = "error";
    private string isUserCorrect = "error";

    // for getting random scramble images
    private int previousNameOfFile;
    private int currentNameOfFile;

    void Start() // entry point; when it Start() finishes, Update is called
    {
        // Start up the file stream bois
        //********************************************************************************************
        //directory = @"C:\Users\smokejump.pc\Desktop\GRUBB_files\";
        //directory = @"C:\Users\smokejump.pc\Desktop\old_repo\";
        //directory = @"C:\Users\OculusRift2\Desktop\CFS\";
        //directory = @"C:\Users\smokejump.pc\Desktop\TestDirectory\";

        //directory = Application.dataPath;
        //directory = directory.Replace("eye_test_Data","");

        //********************************************************************************************
        TextboxManager = new Textbox_IO();
        RNG = new System.Random();

        DateOfTrial = DateTime.Now;
        directory = (Application.dataPath).Replace("eye_test_Data", ""); // May not work, so seprate into two linesif thats the case
        /**/TextboxManager.setTextBoxValue(directory);
        FileManager = new File_IO(directory, DateOfTrial);
        FileManager.transferBasicDetails(ref userID, ref numOfTrials, ref numBlocksOfTrials, ref userGender, ref userAge);
        //////////////////////////////////////////////////
        //TextboxManager.setTextBoxValue("");
        //LeftTextBox.text = "";
        //RightTextBox.text = LeftTextBox.text;
        //////////////////////////////////////////////////////
        // Boot Resources
        TextureManager = new Texture_IO();

        ImageManager = new Image_IO(LeftRawImage, RightRawImage);
        // Init the LEFT EYE
        //LeftSideImage = LeftRawImage;
        //LeftSideImage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

        // Init RIGHT EYE
        //RightSideImage = RightRawImage;
        //RightSideImage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

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
                TextureManager.displayWhiteCross(ref ImageManager);
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
                    TextureManager.displayWhiteCross(ref ImageManager);
                    bufferTheNextStage(ref isTrialsTime);
                }
                else if (isTimeForBreak)
                {
                    performBreak(timesRunRealDeal);
                }
                else
                {
                    if (timesRunRealDeal == numOfTrials + 1)
                    {
                        beginEndOfProgram();
                    }
                    else if (timesRunRealDeal < numOfTrials + 1)
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
            if (!(timeSinceAnswer < 0.75))
            {
                timeSinceStart = 0;
                currentMainStage = 0;
                TextureManager.displayWhiteCross(ref ImageManager);
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
                    TextureManager.displayWhiteCross(ref ImageManager);
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
                        TextureManager.displayWhiteCross(ref ImageManager);
                    }
                    currentMainStage++;
                }

                getUserInteraction(); // Keep checking
            }
            else
            {
                timeSinceStart = 0;
                currentMainStage = 0;
                TextureManager.displayWhiteCross(ref ImageManager);
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
                TextureManager.displayProgressPrompt(ref ImageManager, ref TextboxManager, ref progressTracker, numBlocksOfTrials);;
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
                    TextboxManager.setTextBoxValue("");
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

    void randomizeSupressionImage(bool isSuppressionHappening, bool sideOfSuppression)
    {
        if (isSuppressionHappening)
        {
            do
            {
                currentNameOfFile = RNG.Next(0, 16);
                RNG = new System.Random(random.Next());
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
            //InterferencePattern = Resources.Load(mFile) as Texture;
            TextureManager.LoadInterferencePattern(mFile);
            if (suppressionSide)
            {
                //LeftSideImage.texture = InterferencePattern;
                ImageManager.setLeftSideTexture(TextureManager.InterferencePattern);
            }
            else
            {
                //RightSideImage.texture = InterferencePattern;
                ImageManager.setRightSideTexture(TextureManager.InterferencePattern);
            }
        }
    }

    void setEmoji(bool isSuppressionHappening, bool whichSideSuppression, bool emojiFlipState)
    {
        if (isSuppressionHappening)
        {
            if (whichSideSuppression)
            {
                if (emojiFlipState)
                {
                    //RightSideImage.texture = RightSideUpEmoji;
                    ImageManager.setRightSideTexture(TextureManager.RightSideUpEmoji);
                }
                else
                {
                    //RightSideImage.texture = UpSideDownEmoji;
                    ImageManager.setRightSideTexture(TextureManager.UpSideDownEmoji);
                }
            }
            else
            {
                if (emojiFlipState)
                {
                    //LeftSideImage.texture = RightSideUpEmoji;
                    ImageManager.setLeftSideTexture(TextureManager.RightSideUpEmoji);
                }
                else
                {
                    ImageManager.setLeftSideTexture(TextureManager.UpSideDownEmoji);
                }
            }
        }
        else //!isSuppressionHappening
        {
            if (emojiFlipState)
            {
                //RightSideImage.texture = RightSideUpEmoji;
                //LeftSideImage.texture = RightSideUpEmoji;
                ImageManager.setBothSidesTextures(TextureManager.RightSideUpEmoji);
            }
            else
            {
                //LeftSideImage.texture = UpSideDownEmoji;
                //RightSideImage.texture = UpSideDownEmoji;
                ImageManager.setBothSidesTextures(TextureManager.UpSideDownEmoji);
            }
        }
    }
/*
    void displayWhiteCross()
    {
        //LeftSideImage.texture = WhiteCross;
        //RightSideImage.texture = WhiteCross;
        ImageManager.setBothSidesTextures(WhiteCross);
    }

    void displayGreenCross()
    {
        //LeftSideImage.texture = GreenCross;
        //RightSideImage.texture = GreenCross;
        ImageManager.setBothSidesTextures(GreenCross);
    }

    void displayRedCross()
    {
        //LeftSideImage.texture = RedCross;
        //RightSideImage.texture = RedCross;
        ImageManager.setBothSidesTextures(RedCross);
    }

    void displayMainStartPrompt()
    {
        //LeftSideImage.texture = MainPrompt;
        //RightSideImage.texture = MainPrompt;
        ImageManager.setBothSidesTextures(MainPrompt);
    }

    void displayTrainingStartPrompt()
    {
        //LeftSideImage.texture = TrainingPrompt;
        //RightSideImage.texture = TrainingPrompt;
        ImageManager.setBothSidesTextures(TrainingPrompt);
    }

    void displayEndOfProgramPrompt()
    {
        //LeftSideImage.texture = EndPrompt;
        //RightSideImage.texture = EndPrompt;
        ImageManager.setBothSidesTextures(EndPrompt);
    }

    void displayProgressPrompt()
    {
        var progressText = "Blocks completed: " + progressTracker.ToString() +
            " out of " + (numBlocksOfTrials + 1).ToString();
            // progressTracker, numBlocksOfTrials;
        progressTracker++;
        TextboxManager.setTextBoxValue(progressText);
        //LeftSideImage.texture = BreakPrompt;
        //RightSideImage.texture = BreakPrompt;
        ImageManager.setBothSidesTextures(BreakPrompt);
    }
*/
    void getUserInteraction()
    {
        if (Input.GetKeyUp("down"))
        {
            if (operations.AllowCalculations == true)
            {
                timeUntilResponse = userResponseTime;
                userReturned = "Upright";
                if (operations.checkIfCorrect(UPRIGHT))
                {
                    TextureManager.displayGreenCross(ref ImageManager);
                    isUserCorrect = "Correct";
                }
                else
                {
                    TextureManager.displayRedCross(ref ImageManager);
                    isUserCorrect = "Incorrect";
                }
                timeSinceAnswer = 0;
                hasUserAnswered = true;
                operations.AllowCalculations = false;
            }
        }
        else if (Input.GetKeyUp("up"))
        {
            if (operations.AllowCalculations == true)
            {
                timeUntilResponse = userResponseTime;
                userReturned = "Upside-Down";
                if (operations.checkIfCorrect(DOWNRIGHT))
                {
                    TextureManager.displayGreenCross(ref ImageManager);
                    isUserCorrect = "Correct";
                }
                else
                {
                    TextureManager.displayRedCross(ref ImageManager);
                    isUserCorrect = "Incorrect";
                }
                timeSinceAnswer = 0;
                hasUserAnswered = true;
                operations.AllowCalculations = false;
            }
        }
    }

    void writeToFile()
    {
        if (isInitialEmptyTrialRemoved == true)
        {
            isInitialEmptyTrialRemoved = false;
        }
        else
        {
            calculateStringSideOfSuppression();
            // here we need to write data to a csv file:
            //Trial #, if supression was used, user's response,
            //user response time, and "boolean" correct or incorrect
            FileManager.printTrialResult(timesRunRealDeal, isSuppressed,
                stringSideOfSupression, userReturned, timeUntilResponse,
                isUserCorrect);
        }
        timesRunRealDeal++;
    }

    public bool setSuppression()
    {
        int getSuppression;
        if (SuppressionFileReader.ReadLine() == "1")
        {
            getSuppression = 1;
            return true;
        }
        else
        {
            getSuppression = 0;
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
                return true;
            }
            else
            {
                getSideOfSupression = 0;
                return false;
            }
        }
        else
        {
            FileManager.printError("Emoji: Line 660, Supression fault!");
            return false;
        }
    }
/*
    public void setBasicDetails()
    {
        userID = BasicDetailsReader.ReadLine();
        numOfTrials = System.Convert.ToInt32(BasicDetailsReader.ReadLine());
        numBlocksOfTrials = Convert.ToInt32(BasicDetailsReader.ReadLine());
        userGender = BasicDetailsReader.ReadLine();
        userAge = Convert.ToInt32(BasicDetailsReader.ReadLine());
    }
*/
    // TODO
    // make this double squeeze -> both released
    void getStartSignalFromUser(int programCurrentStage)
    {
        if (!areGatesInitialized)
        {
            isKeysRaised = true;
            isKeysDepressed = false;
            isDownKeyDepressed = true;
            isUpKeyDepressed = true;
            areGatesInitialized = true;
        }

        if (programCurrentStage == 0)
        {
            TextureManager.displayTrainingStartPrompt(ref ImageManager);
        }
        else if (programCurrentStage == 1)
        {
            TextureManager.displayMainStartPrompt(ref ImageManager);
        }

        if ((Input.GetKeyUp("up") && Input.GetKeyUp("down")) || Input.GetButton("Start"))
        {
            isKeysRaised = false;
        }

        if (!isKeysRaised) // trigger are depressed
        {
            isKeysDepressed = true;

            if (Input.GetKeyUp("down"))
            {
                isDownKeyDepressed = false;
            }
            if (Input.GetKeyUp("up"))
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
            TextureManager.displayWhiteCross(ref ImageManager);
            if (programCurrentStage == 0)
            {
                isInitialSetUpActive = false;
                isKeysRaised = true;
                areGatesInitialized = false;
                timePassed = 0;
            }
            else if (programCurrentStage == 1)
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
        TextureManager.displayWhiteCross(ref ImageManager);
        FileManager.copyResultsFile();
        TextureManager.displayEndOfProgramPrompt(ref ImageManager);
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

    void calculateStringSideOfSuppression()
    {
        if (isSuppressed)
        {
            if (!isRightSideSuppressed)
            {
                stringSideOfSuppression = "Right Side";
            }
            else
            {
                stringSideOfSuppression = "Left Side";
            }
        }
        else
        {
            stringSideOfSuppression = "Null";
        }

    }


//    void setTextBoxValue(string message, Text left, Text right) {
//        left.text = message;
//        right.text = message;
//    }

}
