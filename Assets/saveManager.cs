using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class saveManager : MonoBehaviour
{
    public static saveManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void saveCurrentNetwork(networkManager nwM)
    {
        networkSave nwSave = new networkSave();

        string savePath = Path.Combine(Application.dataPath, "networkSaves");
        int dirFileCount = 0;
        try 
        {
            dirFileCount = Directory.GetFiles(savePath).Length;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        nwSave.saveData(nwM, savePath, "networkSave" + dirFileCount+ ".json");
    }

    public void loadNetwork(networkManager nwM, int nr)
    {
        networkSave nwSave = new networkSave();

        nwSave.loadData(nwM, Path.Combine(Application.dataPath, "networkSaves"), "networkSave" + nr+ ".json");
    }
}
