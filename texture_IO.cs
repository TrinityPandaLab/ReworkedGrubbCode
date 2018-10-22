using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Texture_IO : MonoBehaviour
{
    // Constants

    // Members
    public Texture WhiteCross;
    public Texture GreenCross;
    public Texture RedCross;
    public Texture RightSideUpEmoji;
    public Texture UpSideDownEmoji;
    public Texture TrainingPrompt;
    public Texture MainPrompt;
    public Texture EndPrompt;
    public Texture BreakPrompt;
    public Texture InterferencePattern;

    // Constructors
    Texture_IO()
    {
        WhiteCross = Resources.Load("bg2_gray") as Texture;
        GreenCross = Resources.Load("bg2_green") as Texture;
        RedCross = Resources.Load("bg2_red") as Texture;
        RightSideUpEmoji = Resources.Load("up") as Texture;
        UpSideDownEmoji = Resources.Load("down") as Texture;
        TrainingPrompt = Resources.Load("bg2_training") as Texture;
        MainPrompt = Resources.Load("bg2_start") as Texture;
        EndPrompt = Resources.Load("bg2_end") as Texture;
        BreakPrompt = Resources.Load("bg2_break") as Texture;
    }
    // Private Methods

    // Public Methods
    public void LoadInterferencePattern(string textureFile)
    {
        InterferencePattern = Resources.Load(textureFile) as Texture;
    }

    public void displayWhiteCross(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(WhiteCross);
    }

    public void displayGreenCross(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(GreenCross);
    }

    public void displayRedCross(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(RedCross);
    }

    public void displayMainStartPrompt(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(MainPrompt);
    }

    public void displayTrainingStartPrompt(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(TrainingPrompt);
    }

    public void displayEndOfProgramPrompt(ref Image_IO ImageManager)
    {
        ImageManager.setBothSidesTextures(EndPrompt);
    }

    public void displayProgressPrompt(ref Image_IO ImageManager,
        ref Textbox_IO TextboxManager,
        ref int completionCount, int numBlocksOfTrials)
    {
        string progressText = "Blocks completed: " + completionCount.ToString()
         + " out of " + (numBlocksOfTrials + 1).ToString();
        completionCount++;
        TextboxManager.setTextBoxValue(progressText);
        ImageManager.setBothSidesTextures(BreakPrompt);
    }
}
