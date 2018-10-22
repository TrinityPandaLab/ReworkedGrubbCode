using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Image_IO : MonoBehaviour
{
    // Constants
    // Members
    RawImage LeftSideImage;
    RawImage RightSideImage;
    RawImage Nimage = null; // What IS this?

    // Constrcutors
    Image_IO(RawImage LeftRawImage, RawImage RightRawImage){
        LeftSideImage = LeftRawImage;
        RightSideImage = RightRawImage;
        LeftSideImage.GetComponent<RectTransform>().sizeDelta
            = new Vector2(300.0f, 350.0f);
        RightSideImage.GetComponent<RectTransform>().sizeDelta
            = new Vector2(300.0f, 350.0f);
    }
    
    // Public Methods
    public void setLeftSideTexture(Texture newTexture)
    {
        LeftSideImage.texture = newTexture;
    }

    public void setRightSideTexture(Texture newTexture)
    {
        RightSideImage.texture = newTexture;
    }

    public setBothSidesTextures(Texture newTexture)
    {
        LeftSideImage.texture = newTexture;
        RightSideImage.texture = newTexture;
    }
}
