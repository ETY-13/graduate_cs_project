using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent1Color3 : Agent
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject theOtherBall;
    private Vector4 message = Vector4.zero;
    private Vector4 interpretedMessage = Vector4.zero;
    private Color [] colors = {Color.red, Color.blue, Color.yellow, Color.green};
    private System.Random rand = new System.Random();
    public override void Initialize()
    {
        ball.GetComponent<Renderer>().material.color = Color.white;
        theOtherBall.GetComponent<Renderer>().material.color = Color.white;
    }
    public override void OnEpisodeBegin(){
        colors = colors.OrderBy(x=>rand.Next()).ToArray();
        ball.GetComponent<Renderer>().material.color = colors[1];
        theOtherBall.GetComponent<Renderer>().material.color = colors[1];
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
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
      
    }
    public void SetMessage(Vector4 message){
        interpretedMessage = message;
        WriteLogToFile();
    }
    public Vector4 Send(){
        return message;
    }

    public void WriteLogToFile(){
        string bitString = interpretedMessage[0].ToString() + message[1].ToString() + message[2].ToString() + message[3].ToString();
        string colorEncodeNum = System.Convert.ToInt32(bitString,2).ToString();
        
        string color = "white";
        if(ball.GetComponent<Renderer>().material.color == Color.red){
            color  = "red";
        }
        else if(ball.GetComponent<Renderer>().material.color == Color.yellow){
            color  = "yellow";
        }
        else if(ball.GetComponent<Renderer>().material.color == Color.green){
            color  = "green";
        }
        else if(ball.GetComponent<Renderer>().material.color == Color.blue){
            color  = "blue";
        }

        StreamWriter writer = new StreamWriter("C:\\Users\\snowf\\Desktop\\graduate_talk\\train_project2_result\\log_output\\color3_v3.txt", true);
        writer.WriteLine(CompletedEpisodes.ToString() + ',' + color +','+ colorEncodeNum);
        writer.Close();
    }
   
}
