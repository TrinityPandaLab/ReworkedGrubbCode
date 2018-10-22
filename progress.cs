using UnityEngine;

public class operations : MonoBehaviour
{
        private static bool allowCalculations = true;
        private static bool udImage = false; // false = upright, true = upside down


        public static bool AllowCalculations
        {
            get { return allowCalculations; }
            set { allowCalculations = value;}
        }

        public static bool UdImage
        {
            get { return udImage; }
            set { udImage = value; } // false = upright, true = upside down
        }

        public static bool checkIfCorrect(bool userInput)
        {
            if (userInput == udImage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
