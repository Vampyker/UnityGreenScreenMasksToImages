using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager 
{
    private Camera cam;

    public ImageManager (Camera camera)
    {
        cam = camera;
    }

    public void SaveImageLocation(string basePath, string baseName, int resWidth, int resHeight, int angle)
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D camTexture = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        camTexture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(rt);
        byte[] bytes = camTexture.EncodeToPNG();
        string filename = string.Format("{0}/{1}_{2}_{3}x{4}.png",
                             basePath,
                             baseName,
                             angle,
                             resWidth,
                             resHeight);

        string s = "";
        s += "BasePath : " + basePath + "\r\n";
        s += "BaseName : " + baseName + "\r\n";
        s += "Angle : " + angle + "\r\n";
        s += "ResWidth : " + resWidth + "\r\n";
        s += "ResHeight : " + resHeight + "\r\n";
        Debug.Log(s);

        System.IO.File.WriteAllBytes(filename, bytes);
    }

}
