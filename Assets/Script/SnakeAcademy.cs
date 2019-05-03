using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SnakeAcademy : Academy
{
    GameObject area;

    public override void AcademyReset()
    {
        area = GameObject.Find("SnakeArea");
        area.GetComponent<AgentArea>().ResetArea();
    }
}
