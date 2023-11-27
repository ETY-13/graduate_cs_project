using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class Agent1Com : Agent
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject ear;
    private Vector4 message;
    private Vector4 agreedMessage = Vector4.zero;
    private int step_count = 0;
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(new Vector3(
            body.GetComponent<Renderer>().material.color.r, 
            body.GetComponent<Renderer>().material.color.g,
            body.GetComponent<Renderer>().material.color.b
        ));
        step_count +=1;
        WriteLogToFile();
    }
    public override void OnActionReceived(ActionBuffers actions){
    
        message = new Vector4
        (
            actions.DiscreteActions[0],
            actions.DiscreteActions[1],
            actions.DiscreteActions[2],
            actions.DiscreteActions[3]
        );
    }
    public override void Heuristic(in ActionBuffers actionsOut){
      
    }
    public Vector4 InnerVoice(){
        return message;
    }
    public Vector4 Speak(){
        return message;
    }
    public void WriteLogToFile(){
        string bitString = message[0].ToString() + message[1].ToString() + message[2].ToString() + message[3].ToString();
        string colorEncodeNum = System.Convert.ToInt32(bitString,2).ToString();
        
        string color = "white";
        if(body.GetComponent<Renderer>().material.color == Color.red){
            color  = "red";
        }
        else if(body.GetComponent<Renderer>().material.color == Color.yellow){
            color  = "yellow";
        }
        else if(body.GetComponent<Renderer>().material.color == Color.green){
            color  = "green";
        }
        else if(body.GetComponent<Renderer>().material.color == Color.blue){
            color  = "blue";
        }

        StreamWriter writer = new StreamWriter("C:\\Users\\snowf\\Desktop\\graduate_talk\\train_project2_result\\log_output\\escape_room.txt", true);
        writer.WriteLine(step_count.ToString() + ',' + color +','+ colorEncodeNum);
        writer.Close();
    }
}
