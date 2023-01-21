using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    public static bool log = false;

    //last
    public double[] lastweight;
    public float lastbias;
    public double lastactivation;

    //
    public double[] weight;
    public float bias;
    public double activation = 0f;
    public Vector2Int myIndex;

    public node[] outputNodes;
    public node[] inputNodes;

    double[] setinput;
    int inputRecieved = 0;

    

    public node(Vector2Int index)
    {
        myIndex = index;
    }
    public node() { }
    public void setupNode(double[] setweight, float setbias, double setactivation, node[] setoutputNodes, node[] setinputNodes)
    {
        weight = setweight.Clone() as double[];
        bias = setbias;
        activation = setactivation;
        inputNodes = setinputNodes.Clone() as node[];
        outputNodes = setoutputNodes.Clone() as node[];

        resetOperationValues();

        if(log) Debug.Log("node " + myIndex + " set up");
    }

    public void mutate(float range)
    {
        //last
        lastactivation = activation;
        lastbias = bias;
        lastweight = weight.Clone() as double[];

        //
        bias += UnityEngine.Random.Range(-range, range);
        activation += UnityEngine.Random.Range(-range, range);
        for (int i = 0; i < weight.Length; i++)
        {
            weight[i] += UnityEngine.Random.Range(-range, range);
        }
    }

    public void redoMutation()
    {
        if (lastweight == null) return;
        activation = lastactivation;
        bias = lastbias;
        weight = lastweight.Clone() as double[];
    }

    void resetOperationValues()
    {
        inputRecieved = 0;
        setinput = new double[inputNodes.Length];
    }

    double calculateOutput(double[] input)
    {
        double output = (double)bias;

        for (int i = 0; i < input.Length; i++)
        {
            output += input[i] * weight[i];
        }

        return LogSigmoid(output);
    }

    public virtual void distributeOutput(double output)
    {
        foreach (node node in outputNodes)
        {
            if (log) Debug.Log(myIndex + " distributed to " + node.myIndex + " [" + output + "]");
            node.recieveInput(output, this);
        }
    }

    public void recieveInput(double nodeInput, node node)
    {
        int nodeIndex = -1;

        for (int i = 0; i < inputNodes.Length; i++)
        {
            if (inputNodes[i].myIndex.y == node.myIndex.y)
            {
                nodeIndex = i;
                break;
            }
        }

        setinput[nodeIndex] = nodeInput;
        inputRecieved++;

        //Debug.Log(myIndex + " recieved input from " + node.myIndex);
        if (log) Debug.Log(myIndex + " recieved " + inputRecieved + "/" + inputNodes.Length);

        if (inputRecieved >= inputNodes.Length)
        {
            distributeOutput(calculateOutput(setinput));
            resetOperationValues();
        }
    }

    public double LogSigmoid(double x)
    {
        return x;
    }
}
