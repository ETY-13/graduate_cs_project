using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent2Color : Agent
{   
    [SerializeField] private GameObject Agent1;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject theOtherBall;
    public override void Initialize()
    {
        ball.GetComponent<Renderer>().material.color = Color.white;
    }
    private int move;
    public override void CollectObservations(VectorSensor sensor){
        Vector4 message =  Agent1.GetComponent<Agent1Color>().Send();
        sensor.AddObservation(new Vector2 (message[0], message[1]));
        sensor.AddObservation(new Vector2 (message[1], message[3]));
    }
    public override void OnActionReceived(ActionBuffers actions){
      move = actions.DiscreteActions[0];
      if(move == 0){
        ball.GetComponent<Renderer>().material.color = Color.yellow;
      }
      else if(move == 1){
        ball.GetComponent<Renderer>().material.color = Color.green;
      }
      else if(move == 2){
        ball.GetComponent<Renderer>().material.color = Color.red;
      }
      else if(move == 3){
        ball.GetComponent<Renderer>().material.color = Color.blue;
      }

      ManageReward();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 5;
        
        if(Input.GetKey(KeyCode.UpArrow)){
            actions[0] = 0;
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            actions[0] = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            actions[0] = 2;
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            actions[0] = 3;
        }
        else if(Input.GetKey(KeyCode.Space)){
            actions[0] = 4;
        }
    }
    public void ManageReward(){
       
        if(theOtherBall.GetComponent<Renderer>().material.color == ball.GetComponent<Renderer>().material.color &&
            theOtherBall.GetComponent<Renderer>().material.color != Color.white){
            AddReward(10.0f);
            Agent1.GetComponent<Agent1Color>().AddReward(10.0f);
            EndEpisode();
            Agent1.GetComponent<Agent1Color>().EndEpisode();
        }
        else{
            AddReward(-0.1f);
            Agent1.GetComponent<Agent1Color>().AddReward(-0.1f);
        }
    }
}
