using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent2Color3 : Agent
{   
    [SerializeField] private GameObject Agent1;
    [SerializeField] private GameObject ball;
    private Vector4 message = Vector4.zero;
    private Dictionary<Color, Vector4> colorMemo = new Dictionary<Color, Vector4>();
    private Dictionary<Vector4, Color> colorMemoReverse = new Dictionary<Vector4, Color>();
    private Color [] colors = {Color.red, Color.blue, Color.yellow, Color.green};
    private System.Random rand = new System.Random();

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
       
        if(message == Agent1.GetComponent<Agent1Color3>().Send()){
            
            Vector4 theMessage = new Vector4(0,0,0,0);
            Color theColor = Color.black;
            bool hasColor = colorMemo.TryGetValue(   
                ball.GetComponent<Renderer>().material.color, 
                out theMessage);
            bool hasMessage = colorMemoReverse.TryGetValue(
                message, out theColor
            );

            if((hasColor && theMessage == message) || (!hasColor && !hasMessage)){

                if(!hasColor){
                    colorMemo.Add(ball.GetComponent<Renderer>().material.color, message);
                    colorMemoReverse.Add(message, ball.GetComponent<Renderer>().material.color);
                }
                else if(colorMemo.Count > 4){
                    colorMemo = new Dictionary<Color, Vector4>(); 
                    colorMemoReverse = new Dictionary<Vector4, Color>();
                }
                
                AddReward(10.0f);
                Agent1.GetComponent<Agent1Color3>().AddReward(10.0f);
                Agent1.GetComponent<Agent1Color3>().SetMessage(message);
                EndEpisode();
                Agent1.GetComponent<Agent1Color3>().EndEpisode();
            }

        }
        else{
            AddReward(-0.1f);
            Agent1.GetComponent<Agent1Color3>().AddReward(-0.1f);
        }
    }
}

