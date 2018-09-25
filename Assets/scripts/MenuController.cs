using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class MenuController : MonoBehaviour
{
    // consts
    private const int MIN_ROTATION = 120;
    private const int MAX_ROTATION = 240;
    private const int IMAGE_WIDTH = 400;
    private const int IMAGE_HEIGHT = 400;

    // parts of scene
    public Camera cam;
    public GameObject Model;

    // state management
    private enum ImageState
    {
        Idle,
        Rotating,
        RotatingGeneratingImages
    }

    private ImageState currentState = ImageState.Idle;

    // info
    public string DisplayMessage = "";
    
    // current model rotation
    private int rotationIterator = 0;
    public Vector3 modelEulerAngles = new Vector3();
    public Quaternion modelQuaternion = Quaternion.identity;

    // managers
    private FBXManager fbxm = new FBXManager();
    private ImageManager imageManager;

    public void ButtonLoadModel()
    {
        fbxm.LoadAllFBX(Model);
        LookAtCurrentModel();
    }

    public void ButtonRotateModel()
    {
        SetModelState(ImageState.Rotating, "Model not set for rotation."); 
    }

    public void ButtonGenerateImages()
    {
        SetModelState(ImageState.RotatingGeneratingImages, "Model not set for rotation and write.");
    }

    public void ButtonNextFBX()
    {
        fbxm.ShowNextFBX();
        LookAtCurrentModel();
    }

    private void LookAtCurrentModel()
    {
        GameObject cur = fbxm.GetCurrentFBX();
        if (cur != null)
        {
            cam.transform.LookAt(cur.transform);
        }
    }

    private bool ValidateGameObject(GameObject go)
    {
        return go != null;
    }

    private void SetModelState(ImageState state, string failureMessage)
    {
        if (ValidateGameObject(Model))
        {
            ResetRotation();
            currentState = state;
        }
        else
        {
            Debug.Log(failureMessage);
        }
    }

    private void ResetRotation()
    {
        rotationIterator = MIN_ROTATION;
    }

    private void IterateRotation()
    {
        rotationIterator++;
    }

    private void UpdateRotation()
    {
        if (rotationIterator >= MIN_ROTATION && rotationIterator <= MAX_ROTATION)
        {
            modelEulerAngles.x = 0f;
            modelEulerAngles.y = rotationIterator;
            modelEulerAngles.z = 0f;
            modelQuaternion.eulerAngles = modelEulerAngles;
            Model.transform.localRotation = modelQuaternion;
        }
        else
        {
            currentState = ImageState.Idle;
        }
    }

    private void CreateImage()
    {
        if(currentState != ImageState.Idle)
        {
            imageManager.SaveImageLocation(@"D:\TmpTest\", "lesion", IMAGE_WIDTH, IMAGE_HEIGHT, rotationIterator);
        }
    }

    private void Start()
    {
        imageManager = new ImageManager(cam);
    }

    private void Update()
    {
        switch(currentState)
        {
            case ImageState.Idle:
                // nothing
                break;
            case ImageState.Rotating:
                IterateRotation();
                UpdateRotation();
                break;
            case ImageState.RotatingGeneratingImages:
                IterateRotation();
                UpdateRotation();
                CreateImage();
                break;
            default:
                // nothing
                break;
        }
    }
}
