using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AgentArea : Area
{
    // Start is called before the first frame update
    GameObject snake;

    public override void ResetArea()
    {
        snake = GameObject.Find("Head");
        ResetAgent();
    }

    public void ResetAgent() {

        foreach (Transform tail in snake.gameObject.GetComponent<Snake>().tail) {
            Destroy(tail.gameObject);
        }

        GameObject[] allFoods = GameObject.FindGameObjectsWithTag("Food");

        foreach (GameObject food in allFoods)
        {
            Destroy(food);
        } 

        //clear the tail
        snake.gameObject.GetComponent<Snake>().tail.Clear();

        //reset to origin
        snake.gameObject.transform.position = new Vector3(0, 0, 0);

        //make snake alive
        snake.gameObject.GetComponent<Snake>().isDied = false;

    }
}
