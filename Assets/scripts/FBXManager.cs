using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FBXManager
{

    private List<GameObject> AllFBX = new List<GameObject>();

    public int IndexOfDisplayedFBX = -1;

    public GameObject GetCurrentFBX()
    {
        if (ValidateIndexOfFBX(IndexOfDisplayedFBX))
            return AllFBX[IndexOfDisplayedFBX];
        else
            return null;
    }

    public void LoadAllFBX(GameObject parent)
    {
        GameObject[] stuff = Resources.LoadAll<GameObject>("FBX");
        foreach(GameObject go in stuff)
        {
            Debug.Log("DISCOVERING : " + go.name);
            LoadFBX(parent, go);
        }
        HideAllFBX();
        IndexOfDisplayedFBX = 0;
        ShowFBX(0);
    }

    public void LoadFBX(GameObject parent, GameObject go)
    {
        GameObject load = Object.Instantiate(go) as GameObject;
        if (load != null)
        {
            load.transform.parent = parent.transform;
            load.transform.localPosition = Vector3.zero;
            //load.transform.localRotation = Quaternion.Euler(new Vector3(90, 90, 0));
            load.transform.localScale = new Vector3(20f, 20f, 20f);
            AllFBX.Add(load);
        }
    }

    public void LoadFBX(GameObject parent)
    {
        LoadFBX(parent, Resources.Load("FBX/Lesion") as GameObject);
    }

    private void HideAllFBX()
    {
        for (int i = 0; i < AllFBX.Count; i++)
        {
            AllFBX[i].SetActive(false);
        }
    }

    private void HideFBX(int index)
    {
        if (ValidateIndexOfFBX(index))
        {
            AllFBX[index].SetActive(false);
        }
    }

    private void ShowFBX(int index)
    {
        if (ValidateIndexOfFBX(index))
        {
            AllFBX[index].SetActive(true);
        }
    }

    private bool ValidateIndexOfFBX(int index)
    {
        return index < AllFBX.Count && index >= 0;
    }

    public void ShowNextFBX()
    {
        if(AllFBX.Count > 0)
        {
            HideAllFBX();
            IndexOfDisplayedFBX = (IndexOfDisplayedFBX+1) % AllFBX.Count;
            ShowFBX(IndexOfDisplayedFBX);
        }
    }

    //private void Lookat
}
