using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent1Interpret : Agent
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject mouth;
    [SerializeField] private bool train = false;
    private Vector4 message;
    private Dictionary<Color, Vector4> colorMemo = new Dictionary<Color, Vector4>();
    private Dictionary<Vector4, Color> colorMemoReverse = new Dictionary<Vector4, Color>();
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(new Vector3(
            body.GetComponent<Renderer>().material.color.r, 
            body.GetComponent<Renderer>().material.color.g,
            body.GetComponent<Renderer>().material.color.b
        ));
    }
    public override void OnActionReceived(ActionBuffers actions){
    
        message = new Vector4
        (
            actions.DiscreteActions[0],
            actions.DiscreteActions[1],
            actions.DiscreteActions[2],
            actions.DiscreteActions[3]
        );

        ManageReward();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
      
    }
    private void ManageReward(){
        if (message == mouth.GetComponent<Agent1Com>().InnerVoice()){
            Vector4 theMessage = new Vector4(0,0,0,0);
            Color theColor = Color.black;
            bool hasColor = colorMemo.TryGetValue(   // Ensure that the encodeing for the coloris the same as previous 
                body.GetComponent<Renderer>().material.color, // until agent have seem all four colors.
                out theMessage);
            bool hasMessage = colorMemoReverse.TryGetValue(
                message, out theColor
            );

            if((hasColor && theMessage == message) || (!hasColor && !hasMessage) ){
               
                if(!hasColor){
                    colorMemo.Add(body.GetComponent<Renderer>().material.color, message);
                    colorMemoReverse.Add(message, body.GetComponent<Renderer>().material.color);
                }
                else if(colorMemo.Count > 4){
                    colorMemo = new Dictionary<Color, Vector4>();  // Reset the dictionary after agent have seem all four colors. 
                    colorMemoReverse = new Dictionary<Vector4, Color>();
                }
                
                AddReward(10.0f);
                mouth.GetComponent<Agent1Com>().AddReward(10.0f);

                if(train){
                    if(body.GetComponent<Renderer>().material.color != Color.gray){
                        body.GetComponent<ControlAgent1>().EndEpisode();
                        body.GetComponent<ControlAgent1>().ChangeButtonLayout();
                    }
                
                    EndEpisode();
                    mouth.GetComponent<Agent1Com>().EndEpisode();
                }
            }
        }
        else{
            AddReward(-1.0f);
        }
    }
    public Vector4 InnerVoice(){
        return message;
    }
}
