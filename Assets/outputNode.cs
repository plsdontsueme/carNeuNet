using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outputNode : node
{

    networkManager mymanager;

    public outputNode(Vector2Int index, networkManager manager)
    {
        myIndex= index;
        mymanager = manager;
    }

    public override void distributeOutput(double output)
    {
        mymanager.recieveOutput(output, this);
    }
}
