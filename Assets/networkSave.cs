using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class networkSave
{
    bool isLoaded = false;

    // networkManager
    /*public int iterationCounter;
    public float driveSpeed;
    public float mutateRange;
    public float rotMult;
    public float bestTime;
    */
    public int[] blueprint;

    //temp nwM
    node[][] network;


    // nodes
    public double[][][] weight;
    public float[][] bias;
    public double[][] activation;


    public void saveData(networkManager nm, string filePath, string fileName)
    {
        getDataFromNwM(nm);

        //save to location as json
        string fullPath = Path.Combine(filePath, fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            Debug.Log(dataToStore);

            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    Debug.Log("savingData...");
                    writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void getDataFromNwM(networkManager nm)
    {
        blueprint = nm.blueprint;
        /*iterationCounter = nm.iterationCounter;
        driveSpeed = nm.driveSpeed;
        mutateRange = nm.mutateRange;
        rotMult= nm.rotMult;
        bestTime = nm.bestTime;*/

        network = nm.network.Clone() as node[][];



        //nodes
        weight = new double[network.Length][][];
        bias = new float[network.Length][];
        activation = new double[network.Length][];

        for (int l = 0; l < network.Length; l++)
        {
            weight[l] = new double[network[l].Length][];
            bias[l] = new float[network[l].Length];
            activation[l] = new double[network[l].Length];

            for (int n = 0; n < network[l].Length; n++)
            {
                weight[l][n] = network[l][n].weight.Clone() as double[];
                bias[l][n] = network[l][n].bias;
                activation[l][n] = network[l][n].activation;
            }
        }

        isLoaded = true;
    }

    public void loadData(networkManager nm, string filePath, string fileName)
    {
        string fullPath = Path.Combine(filePath, fileName);

        try
        {
            gameData newNws = Newtonsoft.Json.JsonConvert.DeserializeObject<gameData>(fullPath);

            weight = newNws.weight; 
            bias = newNws.bias; 
            activation = newNws.activation;
            blueprint = newNws.blueprint;

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }


        loadDataToNwM(nm);
    }

    public void loadDataToNwM(networkManager networkManager)
    {
        if (!isLoaded) return;

        rebuildNetworkFromData(networkManager);

        networkManager.network = network.Clone() as node[][];

        networkManager.blueprint = blueprint;
        /*networkManager.iterationCounter = iterationCounter;
        networkManager.driveSpeed = driveSpeed;
        networkManager.mutateRange = mutateRange;
        networkManager.rotMult = rotMult;
        networkManager.bestTime = bestTime;
        */
    }

    void rebuildNetworkFromData(networkManager nwM)
    {
        network = generateNetwork(blueprint, nwM);
        for (int l = 0; l < network.Length; l++)
        {
            for (int n = 0; n < network[l].Length; n++)
            {
                network[l][n].lastweight = weight[l][n].Clone() as double[];
                network[l][n].weight = weight[l][n].Clone() as double[];

                network[l][n].lastbias = bias[l][n];
                network[l][n].bias = bias[l][n];

                network[l][n].lastactivation = activation[l][n];
                network[l][n].activation = activation[l][n];
            }
        }
    }

    node[][] generateNetwork(int[] nodesBlueprint, networkManager nwM)
    {
        if (nodesBlueprint.Length < 2)
        {
            Debug.Log("cantCreateNetwork");
            return new node[0][];
        }

        node[][] returnNetwork = new node[nodesBlueprint.Length][];

        for (int l = 0; l < returnNetwork.Length; l++)
        {
            returnNetwork[l] = new node[nodesBlueprint[l]];

            for (int n = 0; n < returnNetwork[l].Length; n++)
            {
                if (l == returnNetwork.Length - 1) returnNetwork[l][n] = new outputNode(new Vector2Int(l, n), nwM);
                else returnNetwork[l][n] = new node(new Vector2Int(l, n));

            }
        }

        return returnNetwork;
    }
}
