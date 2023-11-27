using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent2Com : Agent
{   
    [SerializeField] private GameObject speaker;
    private int move;
    private Vector4 message;
    private int step_count = 0;
    public override void CollectObservations(VectorSensor sensor){
        message = speaker.GetComponent<Agent1Com>().Speak();
        sensor.AddObservation(new Vector2 (message[0], message[1]));
        sensor.AddObservation(new Vector2 (message[1], message[3]));

        step_count +=1;
        WriteLogToFile();
    }
    public override void OnActionReceived(ActionBuffers actions){
      move = actions.DiscreteActions[0];
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       
    }
    public int Heard(){
        return move;
    }
    public void ManageReward(float amount, bool end = false){
        AddReward(amount);
        if(end){
            EndEpisode();
        }
    }
    public void WriteLogToFile(){
        string bitString = message[0].ToString() + message[1].ToString() + message[2].ToString() + message[3].ToString();
        string colorEncodeNum = System.Convert.ToInt32(bitString,2).ToString();
        
        StreamWriter writer = new StreamWriter("C:\\Users\\snowf\\Desktop\\graduate_talk\\train_project2_result\\log_output\\escape_room_moves.txt", true);
        writer.WriteLine(step_count.ToString() + ',' + move +','+ colorEncodeNum);
        writer.Close();
    }
}
