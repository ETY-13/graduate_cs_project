using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent2Color2 : Agent
{   
    [SerializeField] private GameObject Agent1;
    [SerializeField] private GameObject ball;
    private Vector4 message = Vector4.zero;
    private Color [] colors = {Color.red, Color.blue, Color.yellow, Color.green};
    private System.Random rand = new System.Random();

    public override void Initialize()
    {
        ball.GetComponent<Renderer>().material.color = Color.white;
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(new Vector3(
            ball.GetComponent<Renderer>().material.color.r, 
            ball.GetComponent<Renderer>().material.color.g,
            ball.GetComponent<Renderer>().material.color.b
        ));
    }
    public override void OnActionReceived(ActionBuffers actions){
    
        message = new Vector4
        (
            actions.DiscreteActions[0],
            actions.DiscreteActions[1],
            actions.DiscreteActions[2],
            actions.DiscreteActions[2]
        );

        ManageReward();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       
    }
    public void ManageReward(){
       
        if(message == Agent1.GetComponent<Agent1Color2>().Send()){
            AddReward(10.0f);
            Agent1.GetComponent<Agent1Color2>().AddReward(10.0f);
            EndEpisode();
            Agent1.GetComponent<Agent1Color2>().EndEpisode();
        }
        else{
            AddReward(-0.1f);
            Agent1.GetComponent<Agent1Color2>().AddReward(-0.1f);
        }
    }
}

