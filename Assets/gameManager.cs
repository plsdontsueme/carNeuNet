using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    int loadIndex = 0;


    public networkManager mainNwm;
    public TextMeshProUGUI loadIndexText;

    public void saveNetwork()
    {
        saveManager.instance.saveCurrentNetwork(mainNwm);
    }

    public void loadNetwork()
    {
        saveManager.instance.loadNetwork(mainNwm, loadIndex);
    }

    public void loadIndexPlus()
    {
        loadIndex++;
        updateloadIndexUIText();
    }
    public void loadIndexMinus()
    {
        loadIndex--;
        updateloadIndexUIText();
    }

    void updateloadIndexUIText()
    {
        loadIndexText.text = "networkSave" + loadIndex;
    }
}
