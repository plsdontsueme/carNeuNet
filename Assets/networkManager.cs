using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networkManager : MonoBehaviour
{
    public networkOutputReciever networkOutputReciever;

    public node[][] network;
    public float mutateRange = .1f;
    bool stillProcessing = false;
    public int[] blueprint;

    private void Awake()
    {
        setupNewNetwork(blueprint);
    }

    void setupNewNetwork(int[] blueprint) 
    {
        network = generateNetwork(blueprint);
        //setup nodes
        for (int l = 0; l < network.Length; l++)
        {
            for (int n = 0; n < network[l].Length; n++)
            {

                if (l == 0) network[l][n].setupNode(new double[0], 0f, 0f, network[1], new node[0]);
                else
                {
                    double[] standartWeights = new double[network[l - 1].Length];
                    /*for(int i=0; i<standartWeights.Length; i++)
                    {
                        standartWeights[i] = 1;
                    }*/

                    if (l == network.Length - 1) network[l][n].setupNode(standartWeights, 0f, 0f, new node[0], network[l - 1]);
                    else network[l][n].setupNode(standartWeights, 0f, 0f, network[l + 1], network[l - 1]);
                }
            }
        }
    }
    node[][] generateNetwork(int[] nodesBlueprint)
    {
        if(nodesBlueprint.Length < 2)
        {
            Debug.Log("cantCreateNetwork");
            return new node[0][];
        }

        node[][] returnNetwork = new node[nodesBlueprint.Length][];

        for(int l=0; l< returnNetwork.Length; l++)
        {
            returnNetwork[l] = new node[nodesBlueprint[l]];

            for (int n=0; n < returnNetwork[l].Length; n++)
            {
                if (l == returnNetwork.Length-1) returnNetwork[l][n] = new outputNode(new Vector2Int(l, n), this); 
                else returnNetwork[l][n] = new node(new Vector2Int(l,n));

            }
        }

        return returnNetwork;
    }

    public void recieveOutput(double output, outputNode outNode)
    {
        stillProcessing = false;

        networkOutputReciever.networkOut(output, outNode);
    }


    public void mutate()
    {

        foreach (node[] array in network)
        {
            foreach (node node in array)
            {
                node.mutate(mutateRange);
            }
        }
    }

    public void ResetToLastMutation()
    {

        foreach (node[] array in network)
        {
            foreach (node node in array)
            {
                node.redoMutation();
            }
        }
    }

    public void processInput(double[] input)
    {
        if (stillProcessing) return;

        stillProcessing = true;

        for (int i = 0; i < network[0].Length; i++)
        {
            network[0][i].distributeOutput(input[i]);
        }
    }



    
}