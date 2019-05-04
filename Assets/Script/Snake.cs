using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAgents;

public class Snake : Agent
{

    private AgentArea myArea;

    // Did the snake eat something?
    bool ate = false;

    //Did user died?
    public bool isDied = false;

    // Tail Prefab
    public GameObject tailPrefab;
    int prevTailcount = 0;

    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;

    public int move = -1;

    // Keep Track of Tail
    public List<Transform> tail = new List<Transform>();
    public Vector2 forward;
    GameObject[] foods;

    #region Observation variables 

    RaycastHit2D hitUp;
    RaycastHit2D hitDown;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;

    bool didHitUp = false;
    bool didHitDown = false;
    bool didHitRight = false;
    bool didHitLeft = false;

    float upDistance = -1;
    float downDistance = -1;
    float rightDistance = -1;
    float leftDistance = -1;

    int upHitObjectName = -2;
    int downHitObjectName = -2;
    int rightHitObjectName = -2;
    int leftHitObjectName = -2;

    RaycastHit2D hitUpRight;
    RaycastHit2D hitDownRight;
    RaycastHit2D hitRightUp;
    RaycastHit2D hitLeftDown;

    bool didHitUpRight = false;
    bool didHitDownRight = false;
    bool didHitRightUp = false;
    bool didHitLeftDown = false;

    float upRightDistance = -1;
    float downRightDistance = -1;
    float rightUpDistance = -1;
    float leftDownDistance = -1;

    int upRightHitObjectName = -1;
    int downRightHitObjectName = -1;
    int rightUpHitObjectName = -1;
    int leftDownHitObjectName = -1;
    #endregion


    #region ML-Agents Initialize

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        GameObject area = GameObject.Find("SnakeArea");
        myArea = area.GetComponent<AgentArea>();
    }

    public override void AgentReset()
    {
        myArea.ResetArea();

        // Move the Snake every 300ms
        // InvokeRepeating("Move", 0.3f, 0.3f);
    }

    #endregion

    #region ML-Agents Collect Observation

    public override void CollectObservations()
    {

        SetUpObservationValues();

        SetDownObservationValues();

        SetRightObservationValues();

        SetLeftObservationValues();

        SetUpRightObservationValues();

        SetDownRightObservationValues();

        SetRightUptObservationValues();

        SetLeftDownObservationValues();

        //    Debug.Log(rightHitObjectName+"  "+leftHitObjectName+"   "+upHitObjectName+"   "+downHitObjectName+"   "+downRightHitObjectName+"   "+upRightHitObjectName+"   "+leftDownHitObjectName+"   "+rightUpHitObjectName);

    }

    private void SetUpObservationValues()
    {

        hitUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up);

        if (hitUp.collider != null)
        {
            didHitUp = true;
            upDistance = hitUp.distance;

            if (hitUp.collider.gameObject.tag == "Food")
            {
                // food 1
                upHitObjectName = 1;
                //   Debug.Log("Oho mal Food erkannt " + hitUp.distance);
            }
            else if (hitUp.collider.gameObject.tag == "border")
            {
                // border 2
                upHitObjectName = 2;
            }
            else if (hitUp.collider.gameObject.tag == "tail" && hitUp.collider.gameObject.transform != transform)
            {
                // tail 3
                upHitObjectName = 3;
            }

        }
        else
        {
            didHitUp = false;
            upDistance = 0;
            upHitObjectName = 0;
        }

        AddVectorObs(didHitUp);
        AddVectorObs(upDistance);
        AddVectorObs(upHitObjectName);

    }

    private void SetDownObservationValues()
    {

        // raycast down
        hitDown = Physics2D.Raycast(transform.position, Vector2.down);

        if (hitDown.collider != null)
        {
            didHitDown = true;
            downDistance = hitDown.distance;

            if (hitDown.collider.gameObject.tag == "Food")
            {
                // food 1
                downHitObjectName = 1;
                // Debug.Log("Oho mal Food erkannt " + hitDown.distance);
            }
            else if (hitDown.collider.gameObject.tag == "border")
            {
                // border 2
                downHitObjectName = 2;
            }
            else if (hitDown.collider.gameObject.tag == "tail" && hitDown.collider.gameObject.transform != transform)
            {
                // tail 3
                downHitObjectName = 3;
            }

        }
        else
        {
            didHitDown = false;
            downDistance = 0;
            downHitObjectName = 0;
        }

        AddVectorObs(didHitDown);
        AddVectorObs(downDistance);
        AddVectorObs(downHitObjectName);

    }

    private void SetRightObservationValues()
    {

        // raycast right
        hitRight = Physics2D.Raycast(transform.position, Vector2.right);

        if (hitRight.collider != null)
        {
            didHitRight = true;
            rightDistance = hitRight.distance;

            if (hitRight.collider.gameObject.tag == "Food")
            {
                // food 1
                rightHitObjectName = 1;
                // Debug.Log("Oho mal Food erkannt " + hitRight.distance);
            }
            else if (hitRight.collider.gameObject.tag == "border")
            {
                // border 2
                rightHitObjectName = 2;
            }
            else if (hitRight.collider.gameObject.tag == "tail" && hitRight.collider.gameObject.transform != transform)
            {
                // tail 3
                rightHitObjectName = 3;
            }

        }
        else
        {
            didHitRight = false;
            rightDistance = 0;
            rightHitObjectName = 0;
        }

        AddVectorObs(didHitRight);
        AddVectorObs(rightDistance);
        AddVectorObs(rightHitObjectName);

    }

    private void SetLeftObservationValues()
    {

        // raycast left
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left);

        if (hitLeft.collider != null)
        {
            didHitLeft = true;
            leftDistance = hitLeft.distance;

            if (hitLeft.collider.gameObject.tag == "Food")
            {
                // food 1
                leftHitObjectName = 1;
                //  Debug.Log("Oho mal Food erkannt " + hitLeft.distance);
            }
            else if (hitLeft.collider.gameObject.tag == "border")
            {
                // border 2
                leftHitObjectName = 2;
            }
            else if (hitLeft.collider.gameObject.tag == "tail" && hitLeft.collider.gameObject.transform != transform)
            {
                // tail 3
                leftHitObjectName = 3;
            }
        }
        else
        {
            didHitLeft = false;
            leftDistance = 0;
            leftHitObjectName = 0;
        }

        AddVectorObs(didHitLeft);
        AddVectorObs(leftDistance);
        AddVectorObs(leftHitObjectName);

    }

    private void SetUpRightObservationValues()
    {

        hitUpRight = Physics2D.Raycast(transform.position, new Vector2(1, -1));

        if (hitUpRight.collider != null)
        {
            didHitUpRight = true;
            upRightDistance = hitUpRight.distance;

            if (hitUpRight.collider.gameObject.tag == "Food")
            {
                // food 1
                upRightHitObjectName = 1;
                // Debug.Log("Oho mal Food erkannt " + hitUpRight.distance);
            }
            else if (hitUpRight.collider.gameObject.tag == "border")
            {
                // border 2
                upRightHitObjectName = 2;
            }
            else if (hitUpRight.collider.gameObject.tag == "tail" && hitUpRight.collider.gameObject.transform != transform)
            {
                // tail 3
                upRightHitObjectName = 3;
            }

        }
        else
        {
            didHitUpRight = false;
            upRightDistance = 0;
            upRightHitObjectName = 0;
        }

        AddVectorObs(didHitUpRight);
        AddVectorObs(upRightDistance);
        AddVectorObs(upRightHitObjectName);

    }

    private void SetDownRightObservationValues()
    {

        // raycast down
        hitDownRight = Physics2D.Raycast(transform.position, new Vector2(-1, 1));

        if (hitDownRight.collider != null)
        {
            didHitDownRight = true;
            downRightDistance = hitDownRight.distance;

            if (hitDownRight.collider.gameObject.tag == "Food")
            {
                // food 1
                downRightHitObjectName = 1;
                //   Debug.Log("Oho mal Food erkannt " + hitDownRight.distance);
            }
            else if (hitDownRight.collider.gameObject.tag == "border")
            {
                // border 2
                downRightHitObjectName = 2;
            }
            else if (hitDownRight.collider.gameObject.tag == "tail" && hitDownRight.collider.gameObject.transform != transform)
            {
                // tail 3
                downRightHitObjectName = 3;
            }

        }
        else
        {
            didHitDownRight = false;
            downRightDistance = 0;
            downRightHitObjectName = 0;

        }

        AddVectorObs(didHitDownRight);
        AddVectorObs(downRightDistance);
        AddVectorObs(downRightHitObjectName);

    }

    private void SetRightUptObservationValues()
    {

        // raycast right
        hitRightUp = Physics2D.Raycast(transform.position, new Vector2(1, 1));

        if (hitRightUp.collider != null)
        {
            didHitRightUp = true;
            rightUpDistance = hitRightUp.distance;

            if (hitRightUp.collider.gameObject.tag == "Food")
            {
                // food 1
                rightUpHitObjectName = 1;
                // Debug.Log("Oho mal Food erkannt " + hitRightUp.distance);
            }
            else if (hitRightUp.collider.gameObject.tag == "border")
            {
                // border 2
                rightUpHitObjectName = 2;
            }
            else if (hitRightUp.collider.gameObject.tag == "tail" && hitRightUp.collider.gameObject.transform != transform)
            {
                // tail 3
                rightUpHitObjectName = 3;
            }
        }
        else
        {
            didHitRightUp = false;
            rightUpDistance = 0;
            rightUpHitObjectName = 0;
        }

        AddVectorObs(didHitRightUp);
        AddVectorObs(rightUpDistance);
        AddVectorObs(rightUpHitObjectName);

    }

    private void SetLeftDownObservationValues()
    {

        // raycast left
        hitLeftDown = Physics2D.Raycast(transform.position, new Vector2(-1, -1));

        if (hitLeftDown.collider != null)
        {
            didHitLeftDown = true;
            leftDownDistance = hitLeftDown.distance;


            if (hitLeftDown.collider.gameObject.tag == "Food")
            {
                // food 1
                leftDownHitObjectName = 1;
                //  Debug.Log("Oho mal Food erkannt "+hitLeft.distance);
            }
            else if (hitLeftDown.collider.gameObject.tag == "border")
            {
                // border 2
                leftDownHitObjectName = 2;
            }
            else if (hitLeftDown.collider.gameObject.tag == "tail" && hitLeftDown.collider.gameObject.transform != transform)
            {
                // tail 3
                leftDownHitObjectName = 3;
            }

        }
        else
        {
            didHitLeftDown = false;
            leftDownDistance = 0;
            leftDownHitObjectName = 0;
        }

        AddVectorObs(didHitLeftDown);
        AddVectorObs(leftDownDistance);
        AddVectorObs(leftDownHitObjectName);

    }

    #endregion

    #region ML-Agents Action

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        move = (int)vectorAction[0];

        if (!isDied)
        {
            // Move in a new Direction?
            if (move == 0)
                dir = Vector2.right;
            else if (move == 1)
                dir = -Vector2.up;    // '-up' means 'down'
            else if (move == 2)
                dir = -Vector2.right; // '-right' means 'left'
            else if (move == 3)
                dir = Vector2.up;
        }
        else
        {
            Done();
        }

        Move();
    }

    void Move()
    {

        foods = GameObject.FindGameObjectsWithTag("Food");

        foreach (GameObject food in foods)
        {
            if (transform.position == food.transform.position)
            {
                ate = true;

                // Remove the Food
                Destroy(food.gameObject);
                AddReward(0.05f);
            }
        }

        if (!isDied)
        {
            // Save current position (gap will be here)
            Vector2 v = transform.position;

            // Move head into new direction (now there is a gap)
            transform.Translate(dir);

            // Ate something? Then insert new Element into gap
            if (ate)
            {
                // Load Prefab into the world
                GameObject g = (GameObject)Instantiate(tailPrefab,
                                  v,
                                  Quaternion.identity);

                // Keep track of it in our tail list
                tail.Insert(0, g.transform);

                // Reset the flag
                ate = false;
            }
            else if (tail.Count > 0)
            {   // Do we have a Tail?
                // Move last Tail Element to where the Head was
                tail.Last().position = v;

                // Add to front of list, remove from the back
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }
        }

        foods = GameObject.FindGameObjectsWithTag("Food");

        foreach (GameObject food in foods)
        {
            if (transform.position == food.transform.position)
            {
                ate = true;

                // Remove the Food
                Destroy(food.gameObject);
                AddReward(0.05f);
            }
        }

        if (transform.position.x >= 35 || transform.position.x <= -34 || transform.position.y >= 23 || transform.position.y <= -25)
        {
            prevTailcount = 0;
            isDied = true;
            AddReward(-40f);
            Done();
        }


        if (prevTailcount != tail.Count)
        {
            Debug.Log("Reward :D " + tail.Count);
            AddReward(3f);
        }
        else
        {
            AddReward(-0.03f);
        }

        prevTailcount = tail.Count;

    }

    #endregion

    #region Handling Collision


    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("Food"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
            if (ate)
            {
                Debug.Log("Mal was gegegessen!!!! ");
            }

            AddReward(0.05f);
        }
        else
        {   // Collided with Tail or Border
            isDied = true;
            // AddReward(-1f);
            Done();
            // Debug.Log("Gegen mauer oder schwanz gestossen!");
        }

    }

    #endregion
}
