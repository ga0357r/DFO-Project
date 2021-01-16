using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFO : MonoBehaviour
{

    public int N = 100; //population size
    private int D = 2;  //dimensionality
    private float delta = 0.001f;
    public int maxItr = 1000;   //max itr
    public int currItr = 0;

    //Constraints
    public float[] lowerB;
    public float[] upperB;


    public int b = 0;  //best fly
    public Fly[,] fly;
    private GameObject[] gFly;
    public float[] fitness;

    public Sprite flySprite;
    public Goal goal;
    public float waitTimeInSeconds = 0.5f;
    public float currentTimeInSeconds = 0f;
    public Person person;
    public GameObject startPoint;
    public GameObject endPoint;
    public Material greenWaypoint;
    public float moveSpeed = 0.02f;

    private float Fitness(Fly fly, Goal goal)
    {
        float fitness = 0;

        fitness = Vector2.Distance(goal.position, fly.position);

        return fitness;
    }

    private float R()
    {
        float number = Random.Range(0f, 1f);

        return number;
    }

    private void Initialize()
    {
        //fly population and fitness
        gFly = new GameObject[N];
        fitness = new float[N];

        //lowerbound and upperbound
        int lowerBound = 0;
        lowerB = new float[D];
        int upperBound = 8;
        upperB = new float[D];

        for (int d = 0; d < D; d++)
        {
            lowerB[d] = lowerBound;
            upperB[d] = upperBound;
        }

        //fly initialization
        fly = new Fly[N, D];
        for (int n = 0; n < N; n++)
        {
            GameObject g = new GameObject("Fly", typeof(Fly), typeof(SpriteRenderer));
            Fly f = g.GetComponent<Fly>();
            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
            sr.sprite = flySprite;

            for (int d = 0; d < D; d++)
            {
                fly[n, d] = new Fly();

                if (d == 0)
                {
                    fly[n, d].x = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                    f.UpdateXDimension(fly[n, d].x);
                }

                else if (d == 1)
                {
                    fly[n, d].y = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                    f.UpdateYDimension(fly[n, d].y);
                }
                print(n + "th" + " fly value : " + fly[n, d]);
            }

            f.UpdatePosition(f.x, f.y);
            gFly[n] = g;
        }
    }

    private void Loop()
    {
        currentTimeInSeconds += Time.deltaTime;

        if (currItr >= maxItr)
        {
            currItr = maxItr;
            currentTimeInSeconds = 0;
            return;
        }

        if (currentTimeInSeconds >= waitTimeInSeconds)
        {
            for (; currItr < maxItr;)
            {
                //find the best fly
                for (int n = 0; n < N; n++)
                {
                    Fly fly = gFly[n].GetComponent<Fly>();
                    fitness[n] = Fitness(fly, goal);

                    if (fitness[n] <= fitness[b])
                    {
                        b = n;
                    }
                }

                if (currItr % 100 == 0) //print result every 100 iterations
                {
                    print("Iteration : " + "itr" + "\n Best Fly Index : " + b + "\n Fitness Value : " + fitness[b]);
                }

                //update each fly individually
                for (int n = 0; n < N; n++)
                {
                    Fly f = gFly[n].GetComponent<Fly>();

                    //dont update best fly
                    if (n == b)
                    {
                        continue;
                    }

                    //find best neighbour for each fly
                    int leftN = 0;
                    int rightN = 0;
                    int bestN = 0;

                    leftN = (n - 1) % N; //index of left fly
                    if (leftN < 0)
                    {
                        leftN = N - 1;
                    }

                    rightN = (n + 1) % N; //index of right fly

                    if (fitness[rightN] < fitness[leftN])
                    {
                        bestN = rightN;
                    }
                    else
                    {
                        bestN = leftN;
                    }

                    //update each dimension separately
                    for (int d = 0; d < D; d++)
                    {
                        if (R() < delta)
                        {
                            //disturbance mechanism
                            if (d == 0)
                            {
                                //Update Equation
                                fly[n, d].x = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                                gFly[n].GetComponent<Fly>().UpdateXDimension(fly[n, d].x);

                            }

                            if (d == 1)
                            {
                                //Update Equation
                                fly[n, d].y = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                                gFly[n].GetComponent<Fly>().UpdateYDimension(fly[n, d].y);
                            }

                            gFly[n].GetComponent<Fly>().UpdatePosition(gFly[n].GetComponent<Fly>().x, gFly[n].GetComponent<Fly>().y);
                            continue;
                        }

                        if (d == 0)
                        {
                            //Update Equation
                            fly[n, d].x = fly[bestN, d].x + R() * (fly[b, d].x - fly[n, d].x);

                            gFly[n].GetComponent<Fly>().UpdateXDimension(fly[n, d].x);

                        }

                        if (d == 1)
                        {
                            //Update Equation
                            fly[n, d].y = fly[bestN, d].y + R() * (fly[b, d].y - fly[n, d].y);

                            gFly[n].GetComponent<Fly>().UpdateYDimension(fly[n, d].y);
                        }

                        //Out of Bound Control
                        if (fly[n, d] < lowerB[d] || fly[n, d] > upperB[d])
                        {
                            if (d == 0)
                            {
                                fly[n, d].x = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                                gFly[n].GetComponent<Fly>().UpdateXDimension(fly[n, d].x);
                            }

                            else if (d == 1)
                            {
                                fly[n, d].y = lowerB[d] + R() * (upperB[d] - lowerB[d]);

                                gFly[n].GetComponent<Fly>().UpdateYDimension(fly[n, d].y);

                            }
                        }
                        gFly[n].GetComponent<Fly>().UpdatePosition(gFly[n].GetComponent<Fly>().x, gFly[n].GetComponent<Fly>().y);
                    }
                    if (n == 99)
                    {
                        print("99th fly");
                        currItr++;
                        goto wait;
                    }
                }

                if (currItr >= maxItr)
                {
                    break;
                }
            }

        wait: currentTimeInSeconds = 0;

            //evaluate each fly fitness and find the best fly
            for (int n = 0; n < N; n++)
            {
                fitness[n] = Fitness(gFly[n].GetComponent<Fly>(), goal);
                if (fitness[n] < fitness[b])
                {
                    b = n;
                }
            }


            print("\n" + "Final best fitness: " + fitness[b] + "\n\n");
        }
    }

    private void DrawWaypoint()
    {
        //draw waypoint
        startPoint.transform.position = person.transform.position;
        endPoint.transform.position = gFly[b].transform.position;

        startPoint.GetComponent<LineRenderer>().SetPosition(0, person.transform.position);
        startPoint.GetComponent<LineRenderer>().SetPosition(1, endPoint.transform.position);
    }

    private void MoveToWaypoint()
    {
        //move to waypoint
        Vector2 direction = (person.transform.position - endPoint.transform.position).normalized;

        direction *= -1;

        float distanceToTarget = Vector2.Distance(person.transform.position, endPoint.transform.position);

        if (distanceToTarget <= 0.01f)
        {
            return;
        }

        person.transform.position = Vector2.Lerp(startPoint.transform.position, endPoint.transform.position, Time.deltaTime * moveSpeed);
    }

    private void Awake()
    {
        startPoint = new GameObject("Start Point", typeof(LineRenderer));
        endPoint = new GameObject("End Point", typeof(LineRenderer));

        startPoint.GetComponent<LineRenderer>().SetWidth(0.03f, 0.03f);
        endPoint.GetComponent<LineRenderer>().SetWidth(0.03f, 0.03f);

        startPoint.GetComponent<LineRenderer>().material = greenWaypoint;
        endPoint.GetComponent<LineRenderer>().material = greenWaypoint;

        for (int i = 0; i < 2; i++)
        {
            startPoint.GetComponent<LineRenderer>().SetPosition(i, Vector3.zero);
            endPoint.GetComponent<LineRenderer>().SetPosition(i, Vector3.zero);
        }

    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Loop();

        //draw a waypoint to the best fly
        if (currItr >= maxItr)
        {
            DrawWaypoint();

            MoveToWaypoint();
        }
    }   
}
