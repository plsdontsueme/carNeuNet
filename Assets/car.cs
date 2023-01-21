using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class car : networkOutputReciever
{
    public networkManager nwm;

    public bool autoTrain = false;

    public Transform startPosition;
    public TextMeshProUGUI iterationText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rotMultText;
    public LayerMask rayLayer;
    public int iterationCounter = 0;

    Rigidbody2D rb;
    public float driveSpeed;
    public double rotation;

    //input
    public Transform[] rayCasterPoints;

    public float rotMult = 2f;

    public bool active = false;

    float startTime = 0;
    public float bestTime = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void networkOut(double output, outputNode outNode)
    {
        rotation = output;
    }

    private void FixedUpdate()
    {
        if (!active) return;
        //ray
        double[] rch = new double[rayCasterPoints.Length];
        for (int i = 0; i < rch.Length; i++)
        {

            rch[i] = Physics2D.Raycast(rayCasterPoints[i].position, rayCasterPoints[i].up, float.PositiveInfinity, rayLayer).distance;
            Debug.DrawLine(rayCasterPoints[i].position, rayCasterPoints[i].position + rayCasterPoints[i].up * (float)rch[i], Color.red);
            
            Debug.Log(rch[i]);
        }
        nwm.processInput(rch);
        rb.SetRotation(rb.rotation + (float)rotation * rotMult * Time.fixedDeltaTime);
        rb.MovePosition(transform.position + transform.up * driveSpeed * Time.fixedDeltaTime);
    }

    public void startRace()
    {
        if (autoTrain)
        {
            nwm.mutate();
        }
        resetStuff();
        active = true;
        startTime = Time.time;
    }

    void resetStuff()
    {
        rotation = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }



    public void stopRace()
    {
        float stopTime = Time.time;
        active = false;

        resetStuff();

        float raceTime = stopTime - startTime;
        if (raceTime > bestTime)
        {
            bestTime = raceTime;
            scoreText.text = "score: "+ raceTime.ToString();
        }
        else
        {
            nwm.ResetToLastMutation();
        }

        iterationCounter++;
        iterationText.text = "iteration: " + iterationCounter.ToString();

        if (autoTrain) startRace();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stopRace();
    }

    public void toggleAutotrain()
    {
        autoTrain = !autoTrain;
    }


    public void decreaseRotMult()
    {
        rotMult -= 50;
        rotMultText.text = rotMult.ToString();
    }

    public void increaseRotMult()
    {
        rotMult += 50;
        rotMultText.text = rotMult.ToString();
    }
}
