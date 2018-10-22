using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Emojis : MonoBehaviour
{
    // For trial_text
    public Text textl;
    public Text textr;

    // Variables & Objects
    RawImage Limage = null;
    RawImage Rimage = null;
    RawImage Nimage = null;

    // Objects to store L/Rimage in
    public RawImage L;
    public RawImage R;

    // Texture List
    private Texture grayScreen;
    private Texture greenScreen;
    private Texture redScreen;
    private Texture emojiUp;
    private Texture emojiDown;
    private Texture noise;
    private Texture trainingScreen;
    private Texture startScreen;
    private Texture endScreen;
    private Texture breakScreen;

    // Remote Variables
    private const bool leftRemote = false;
    private const bool rightRemote = true;
    private bool safetyGateLeft = true;
    private bool safetyGateRight = true;
    private bool passedFirstGate = false;
    private bool safetyGateUpLeft = true;
    private bool safetyGateUpRight = true;
    private bool initializeGates = true;

    // Declare the random number genereator
    System.Random randomNumGen;

    // IOStream Paths and Objects
    System.IO.StreamWriter outputStream;
    string pathOutput;
    System.IO.StreamWriter mommy;
    string pathMommy;

    System.IO.StreamReader basicDetailsReader;
    string pathBasicDetails;
    System.IO.StreamReader streamSupression;
    string pathSupression;
    System.IO.StreamReader streamEmoji;
    string pathEmoji;
    System.IO.StreamReader streamSide;
    string pathSide;
    System.IO.StreamReader linebacker;
    string pathLinebacker;
    System.IO.StreamReader daddy;
    string pathDaddy;

    // Code control variables
    private int currentState = 0;
    private int breakState = 0;
    private bool waitTime1 = true;
    private bool waitTime2 = true;
    private bool waitTime3 = true;
    private bool trainingState = true;
    private bool preTraining = true;
    private bool preStart = false;
    private bool hasAnswered = false;
    private bool endGameBoys = false;
    private bool blockerActive = true;
    private bool breakReady = false;
    private bool endThisBreak = false;
    private bool pauseTheMadness = false;

    // DateTime Variable
    DateTime dateOfTrial;

    //...Don't ask...
    private bool virginity = true;

    // directory
    private string directory;
    private string directory2;

    // times run indexes
    const int trainingTrials = 15;
    int timesRunTutorial = 0;
    int timesRunExp = 0;
    private int progressTracker = 0;

    // Time Management Varaibles
    private float timeSinceStart = 0.0f;
    private float timePassed2 = 0.0f;
    private float timeSinceAnswer = 0.0f;
    private float userResponseTime = 0.0f;
    private float passTime = 0.0f;
    private float hz = 0.1f;
    //    //    //    //
    private const float fixation = 1000.0f; //in milliseconds
    private const float display = 500.0f;
    private const float collectResponse = 5000.0f;
    //    //    //    //
    private float timeZone0;
    private float timeZone1;
    private float timeZone2;
    //    //    //    //
    // Record keeping Variables
    private int numberCorrect = 0;
    private int numberIncorrect = 0;
    private int numberNotAnswered = 0;

    // We will input a dataFile with following info:
    private bool sideSuppressed = false;  // 0 for left, 1 for right scramble
    private bool emojiFlip = false;       // 0 for "up" emoji, 1 for "down" emoji
    private bool suppress = false;         // 0 for no supression, 1 for suppression
    private string userID = "error";
    private string userGender = "error";
    private int numOfTrials = 0;
    private int blockAmount = 2;
    private int userAge = 0;
    private string printSide = "";

    // Output info
    private string userReturned = "error";
    private string isUserCorrect = "error";

    // for getting random scramble images
    private int lastFileNum;
    private int fileNum;



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

    //List of Methods
    //   void Start()
    //   void Update()
    //   void checkStates()
    //   void scramble()
    //   void randscram()
    //   void setEmoji()
    //   void setGray()
    //   writeToFile()

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

        pathOutput = directory + "(Lab_Results).csv";
        outputStream = new System.IO.StreamWriter(pathOutput);

        pathBasicDetails = directory + "subjectInput.txt";
        pathSupression = directory + "inputFile_NoiseVsNoNoise.txt";
        pathEmoji = directory + "inputFile_UpVsDown.txt";
        pathSide = directory + "inputFile_LeftVsRight.txt";
        pathLinebacker = directory + "inputFile_BlockVsNoBlock.txt";

        // Get the objects oriented...get it?

        basicDetailsReader = new System.IO.StreamReader(pathBasicDetails);
        streamSupression = new System.IO.StreamReader(pathSupression);
        streamEmoji = new System.IO.StreamReader(pathEmoji);
        streamSide = new System.IO.StreamReader(pathSide);
        linebacker = new System.IO.StreamReader(pathLinebacker);

        // Init the RNG
        randomNumGen = new System.Random();

        // Start writing the output document
        setBasicDetails();
        makeHeading();

        //////////////////////////////////////////////////

        // progressTracker
        // blockAmount

        textl.text = "";
        textr.text = textl.text;
        //////////////////////////////////////////////////////
        // Boot Resources
        emojiUp = Resources.Load("up") as Texture;
        emojiDown = Resources.Load("down") as Texture;
        grayScreen = Resources.Load("bg2_gray") as Texture;
        greenScreen = Resources.Load("bg2_green") as Texture;
        redScreen = Resources.Load("bg2_red") as Texture;
        trainingScreen = Resources.Load("bg2_training") as Texture;
        startScreen = Resources.Load("bg2_start") as Texture;
        endScreen = Resources.Load("bg2_end") as Texture;
        breakScreen = Resources.Load("bg2_break") as Texture;

        // Init the LEFT EYE
        Limage = L;
        Limage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

        // Init RIGHT EYE
        Rimage = R;
        Rimage.GetComponent<RectTransform>().sizeDelta = new Vector2(300.0f, 350.0f);

        // Init the on-screen text
        //textLeft.text = "";
        //textRight.text = "";
        //textLeft.fontSize = 32;
        //textRight.fontSize = 32;


        // Init timeZones
        timeZone0 = fixation / 1000.0f;
        timeZone1 = (fixation + display) / 1000.0f;
        timeZone2 = (timeZone1 + collectResponse) / 1000.0f;

    }  // Here, Unity calls Update()

    void Update() // Note Bene (NB): Update is called once per frame, and keeps going until the program dies
    {
        // Compesate headset rotations/position
        transform.position = -InputTracking.GetLocalPosition(VRNode.CenterEye);
        transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(VRNode.CenterEye));

        // Gather Time
        timeSinceStart += Time.deltaTime; //Note LOGAN!!! Learn from these!
        timePassed2 += Time.deltaTime;
        timeSinceAnswer += Time.deltaTime;
        userResponseTime += Time.deltaTime;

        //Controll Flow statements
        if (!endGameBoys)
        {
            if (preTraining)
            {
                waitingScreen(0);
            }
            else if (waitTime1)
            {
                setGray();
                gettingReady(ref waitTime1);
            }
            else if (timesRunTutorial - 1 < trainingTrials)
            {
                if (hasAnswered) {
                    timeZone2 = 5000000000f;
                }
                checkStates(suppress, sideSuppressed, emojiFlip);
                if (timesRunTutorial - 1 == trainingTrials)
                {
                    preStart = true;
                }
            }
            else
            {
                if (preStart)
                {
                    timeZone2 = (timeZone1 + collectResponse) / 1000.0f;
                    waitingScreen(1);
                }
                else if (waitTime2)
                {
                    setGray();
                    gettingReady(ref waitTime2);
                }
                else if (breakReady) // breakReady
                {
                    performBreak(timesRunExp);
                }
                else
                {
                    if (timesRunExp == NumOfTrials + 1)
                    {
                        gameOverMan();
                    }
                    else if (timesRunExp < NumOfTrials + 1)
                    {
                        checkStates(suppress, sideSuppressed, emojiFlip);
                        if (!virginity)
                        {
                            timeZone2 = 5000000000f;
                        }
                    }
                }

            }
        }
    }



    //-------------------------------------------------------------------------------------------------------------------------------------------



    void checkStates(bool suppressionCheck, bool sideSurpressedCheck, bool emojiFlipCheck) //check the progress of the test
    {
        if (hasAnswered)
        {
            if (timeSinceAnswer < 0.75)
            {
            }
            else
            {
                timeSinceStart = 0;
                currentState = 0;
                setGray();
                if (trainingState == true)
                {
                    timesRunTutorial++;
                    blockerActive = true;
                    hasAnswered = false;
                }
                else
                {
                    writeToFile();
                    breakCheck();
                    //timesRunExp++;
                    blockerActive = true;
                    hasAnswered = false;
                }
            }

        }

        else
        {
            if (timeSinceStart <= timeZone0) // TimeZone 0: Start of test progress
            {
                if (currentState == 0) // State 0
                {
                    if (trainingState == true)
                    {
                        trainingInit();
                    }
                    else
                    {
                        actualInit();
                    }
                    setGray();
                    currentState++;
                } // end State 0

            } // end of timeZone 0
            else if (timeSinceStart <= timeZone1) // timeZone 1: diplay the test
            {
                if (currentState == 1) // State 1
                {
                    // Set up the user feedback
                    allowCalculations(emojiFlipCheck);

                    // Reset the timer and prepare to engage the next phase
                    timePassed2 = 0;
                    currentState++;

                    // Now, actually display everything
                    scramble(suppressionCheck, sideSurpressedCheck);
                    setEmoji(suppressionCheck, sideSurpressedCheck, emojiFlipCheck);

                    // The participant is on the clock now:
                    userResponseTime = 0;

                } // end State 1

                userInteraction(); // Check for user feedback

                if (timePassed2 >= hz) // Change out the supression image
                {
                    if (operations.AllowCalculations == true)
                    {
                        scramble(suppressionCheck, sideSurpressedCheck);
                        timePassed2 = 0;
                    }
                }

            } // end TimeZone 1
            else if (timeSinceStart <= timeZone2) // TimeZone 2: Get feedback
            {

                if (currentState == 2)
                {
                    if (operations.AllowCalculations == true)
                    {
                        setGray();
                    }
                    currentState++;
                }

                userInteraction(); // Keep checking
            }
            else
            {
                timeSinceStart = 0;
                currentState = 0;
                setGray();
                if (trainingState == true)
                {
                    timesRunTutorial++;
                }
                else
                {
                    passTime = 999999;
                    isUserCorrect = "NaN";
                    writeToFile();
                    breakCheck();

                    if (!breakReady)
                    {
                        //timesRunExp++;
                    }

                }
            }
        }
    }

    void performBreak(int trialsCompleted)
    {
        if (pauseTheMadness)
        {

        }
        else
        {
            if (breakState == 0)
            {
                setBreak();
                breakState++;
            }

            if (breakState == 1)
            {
                waitingScreen(2);
            }

            if (breakState == 2)
            {
                if (timePassed2 < 0.75)
                {
                    textl.text = "";
                    textr.text = textl.text;
                }
                else
                {
                    timeSinceStart = 0;
                    timePassed2 = 0;
                    timeSinceStart = 0;
                    currentState = 0;

                    pauseTheMadness = true;
                    breakState = 0;
                    breakReady = false;
                    pauseTheMadness = false;
                }
            }
        }
    }












    void scramble(bool suppressionExists, bool sideOfSuppression)
    {
        if (suppressionExists)
        {
            var random = new System.Random();

            do
            {
                fileNum = random.Next(0, 16);
                random = new System.Random(random.Next());
            }
            while (fileNum == lastFileNum);

            string scrambleFile = "m" + fileNum;
            lastFileNum = fileNum;
            randscram(sideOfSuppression, scrambleFile);
        }
    }


    void randscram(bool suppressionSide, string mFile)
    {
        if (suppress)
        {
            noise = Resources.Load(mFile) as Texture;
            if (suppressionSide)
            {
                Limage.texture = noise;
            }
            else
            {
                Rimage.texture = noise;
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
                    Rimage.texture = emojiUp;
                }
                else
                {
                    Rimage.texture = emojiDown;
                }
            }
            else
            {
                if (emojiFlipState)
                {
                    Limage.texture = emojiUp;
                }
                else
                {
                    Limage.texture = emojiDown;
                }
            }
        }
        else
        {
            if (emojiFlipState)
            {
                Rimage.texture = emojiUp;
                Limage.texture = emojiUp;
            }
            else
            {
                Limage.texture = emojiDown;
                Rimage.texture = emojiDown;
            }
        }
    }


    void setGray()
    {
        Limage.texture = grayScreen;
        Rimage.texture = grayScreen;
    }

    void setGreen()
    {
        Limage.texture = greenScreen;
        Rimage.texture = greenScreen;
    }

    void setRed()
    {
        Limage.texture = redScreen;
        Rimage.texture = redScreen;
    }

    void setStart()
    {
        Limage.texture = startScreen;
        Rimage.texture = startScreen;
    }

    void setTraining()
    {
        Limage.texture = trainingScreen;
        Rimage.texture = trainingScreen;
    }

    void setEnd()
    {
        Limage.texture = endScreen;
        Rimage.texture = endScreen;
    }

    void setBreak()
    {
        progressTracker++;
        textl.text = "Blocks completed: " + progressTracker.ToString() + " out of " + (blockAmount + 1).ToString(); // progressTracker, blockAmount;
        textr.text = textl.text;
        Limage.texture = breakScreen;
        Rimage.texture = breakScreen;
    }

    void userInteraction()
    {
        if ((Input.GetAxis("LeftTrigger") >= 0.85) || Input.GetKeyUp("down"))
        {
            if (operations.AllowCalculations == true)
            {
                passTime = userResponseTime;
                userReturned = "Upright";
                if (operations.checkIfCorrect(leftRemote))
                {
                    setGreen();
                    isUserCorrect = "Correct";
                    timeSinceAnswer = 0;
                    hasAnswered = true;
                }
                else
                {
                    setRed();
                    isUserCorrect = "Incorrect";
                    timeSinceAnswer = 0;
                    hasAnswered = true;
                }
                operations.AllowCalculations = false;
            }
        }
        else if ((Input.GetAxis("RightTrigger") >= 0.85) || Input.GetKeyUp("up"))
        {
            if (operations.AllowCalculations == true)
            {
                passTime = userResponseTime;
                userReturned = "Upside-Down";
                if (operations.checkIfCorrect(rightRemote))
                {
                    setGreen();
                    isUserCorrect = "Correct";
                    timeSinceAnswer = 0;
                    hasAnswered = true;
                }
                else
                {
                    setRed();
                    isUserCorrect = "Incorrect";
                    timeSinceAnswer = 0;
                    hasAnswered = true;
                }
                operations.AllowCalculations = false;
            }
        }


    }



    void writeToFile() // here we need to write data to a csv file: Trial #, if supression was used, user's response, user response time, and boolean correct or incorrect
    {
        if (virginity == true)
        {
            virginity = false;
        }
        else
        {
            //  out:  (System.Convert.ToString(timesRunExp), System.Convert.ToString(suppress), userReturned, System.Convert.ToString(passTime), isUserCorrect);
            outputStream.Write(Convert.ToString(timesRunExp) + ",");
            outputStream.Write(Convert.ToString(suppress) + ",");
            translateSide();
            outputStream.Write(printSide + ",");
            outputStream.Write(userReturned + ",");
            if (passTime < 900)
            {
                outputStream.Write(passTime);
                outputStream.Write(",");
            }
            else
            {
                outputStream.Write("NaN,");
            }

            outputStream.WriteLine(isUserCorrect);
            outputStream.Flush();
        }
        timesRunExp++;
    }
    void writeTofile(string boop)
    {
        outputStream.WriteLine(boop);
    }

    public void makeHeading()
    {
        outputStream.Write("Subject ID:, " + userID + ", Age:, " + userAge + ", Gender:, " + userGender);
        outputStream.WriteLine();
        outputStream.Write("Total Trials:,");
        outputStream.WriteLine(numOfTrials);
        outputStream.WriteLine("");
        outputStream.WriteLine("Trial #, Supression?, SideOfSupp?, User Response, Response time, Were they correct?");
        outputStream.Flush();
    }

    public bool setSuppression()
    {
        int getSuppression;
        if (streamSupression.ReadLine() == "1")
        {
            //writeTofile("yes");
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

    public bool setEmojiFlip()
    {

        int emoj = 2;
        if (streamEmoji.ReadLine() == "1")
        {
            emoj = 1;
        }
        else
        {
            emoj = 0;
        }
        if (emoj == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool setSideOfSuppression()
    {
        if (suppress == true)
        {
            int getSideOfSupression = 0;
            if (streamSide.ReadLine() == "1")
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
            outputStream.WriteLine("Daddy we got issues!");
            return false;
        }
    }

    public void setBasicDetails()
    {
        userID = basicDetailsReader.ReadLine();
        numOfTrials = System.Convert.ToInt32(basicDetailsReader.ReadLine());
        blockAmount = Convert.ToInt32(basicDetailsReader.ReadLine());
        userGender = basicDetailsReader.ReadLine();
        userAge = Convert.ToInt32(basicDetailsReader.ReadLine());
    }

    // TODO
    // make this double squeeze -> both released
    void waitingScreen(int index)
    {
        if (initializeGates) //Reset start-section flags
        {
            safetyGateLeft = true;
            safetyGateRight = true;
            passedFirstGate = false;
            safetyGateUpLeft = true;
            safetyGateUpRight = true;
            initializeGates = false;
        }

        if (index == 0)
        {
            setTraining();
        }
        else if (index == 1)
        {
            setStart();
        }

        if ((((Input.GetAxis("LeftTrigger") > 0.8 || Input.GetKeyUp("up")) && ((Input.GetAxis("RightTrigger")) > 0.8 || Input.GetKeyUp("down")))) || Input.GetButton("Start"))
        {
            safetyGateLeft = false;
            safetyGateRight = false;
        }
        if (!(safetyGateLeft || safetyGateRight)) // trigger are depressed
        {
            passedFirstGate = true;

            if (((Input.GetAxis("LeftTrigger") < 1)) || Input.GetKeyUp("down"))
            {
                safetyGateUpLeft = false;
            }
            if ((Input.GetAxis("RightTrigger") < 1) || Input.GetKeyUp("up"))
            {
                safetyGateUpRight = false;
            }
            if (Input.GetButtonUp("Start"))
            {
                safetyGateUpLeft = false;
                safetyGateUpRight = false;
            }

        }

        if (passedFirstGate && (!(safetyGateUpLeft || safetyGateUpRight)))
        {
            setGray();
            if (index == 0)
            {
                preTraining = false;
                safetyGateLeft = true;
                safetyGateRight = true;
                initializeGates = true;
                timePassed2 = 0;
            }
            else if (index == 1)
            {
                preStart = false;
                trainingState = false;
                safetyGateLeft = true;
                safetyGateRight = true;
                initializeGates = true;
                timePassed2 = 0;
            }
            else
            {
                breakState++;
                timePassed2 = 0;
                safetyGateLeft = true;
                safetyGateRight = true;
                initializeGates = true;
            }
            timePassed2 = 0;
        }
    }

    void gettingReady(ref bool check)
    {
        if (check == true)
        {
            if (timePassed2 >= 1)
            {
                check = false;
                timePassed2 = 0;
            }
        }
    }

    void trainingInit()
    {
        suppress = false;
        sideSuppressed = false;
        emojiFlip = System.Convert.ToBoolean(randomNumGen.Next(0, 2));
        randomNumGen = new System.Random(randomNumGen.Next());
    }

    void actualInit()
    {
        suppress = setSuppression();
        if (suppress == true)
        {
            sideSuppressed = setSideOfSuppression();
        }
        emojiFlip = setEmojiFlip();
    }

    void allowCalculations(bool theEmoji)
    {
        // Translate that "set" into actually usable code
        if (!theEmoji)
        //if (emojiInput == 0)  KDM debug
        {
            operations.UdImage = leftRemote;
            operations.AllowCalculations = true;
        }
        else
        {
            operations.UdImage = rightRemote;
            operations.AllowCalculations = true;
        }
    }

    void gameOverMan()
    {
        setGray();
        streamEmoji.Close();
        streamSide.Close();
        streamSupression.Close();
        basicDetailsReader.Close();
        outputStream.Close();
        string newLine = "";
        pathDaddy = pathOutput;
        pathMommy = directory + UserID + "_" + Convert.ToString(dateOfTrial.Month) + "." + Convert.ToString(dateOfTrial.Day) + "." + Convert.ToString(dateOfTrial.Year)
            + "_(" + Convert.ToString(dateOfTrial.Hour) + ";" + Convert.ToString(dateOfTrial.Minute) + ";" + Convert.ToString(dateOfTrial.Second) + ").csv";
        daddy = new System.IO.StreamReader(pathDaddy);
        mommy = new System.IO.StreamWriter(pathMommy); //

        while ((newLine = daddy.ReadLine()) != null)
        {
            mommy.WriteLine(newLine);
        }
        daddy.Close();
        mommy.Close();

        setEnd();
        endGameBoys = true;
    }

    void breakCheck()
    {
        if (Convert.ToInt32(linebacker.ReadLine()) == 1)
        {
            breakReady = true;
        }
        else
        {
            breakReady = false;
        }
    }

    void translateSide()
    {
        if (suppress)
        {
            if (!sideSuppressed)
            {
                printSide = "Right Side";
            }
            else
            {
                printSide = "Left Side";
            }
        }
        else
        {
            printSide = "Null";
        }

    }

    void setText(string message, Text left, Text right)
    {
        left.text = message;
        right.text = message;
    }

}
