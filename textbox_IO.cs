using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Textbox_IO : MonoBehaviour
{
    // Constants
    // Members
    public Text LeftTextBox;
    public Text RightTextBox;

    // Constructors
    Textbox_IO(){}
    // Private Methods

    // Public Methods
    public void setTextBoxValue(string message) {
        LeftTextBox.text = message;
        RightTextBox.text = message;
    }
}
