using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ControlAgent1: Agent
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject goal;
    [SerializeField] private ControlEnv EnvControl;
    private Color [] colors = {Color.red, Color.blue, Color.yellow, Color.green};
    private System.Random rand = new System.Random();
    
    public override void Initialize()
    {
        button.GetComponent<Renderer>().material.color = Color.blue;
    }
    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(-5.0f, 0.5f, -2.0f);
        ChangeColor(Color.gray);
    }
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        if(EnvControl.IsOpenWall(room2:false)){
            sensor.AddObservation(goal.transform.localPosition);
        }
        else{
            sensor.AddObservation(button.transform.localPosition);
        }
    }
    public override void OnActionReceived(ActionBuffers actions){
    
        int move = actions.DiscreteActions[0];
   
        if(move == 0){
            transform.localPosition += transform.forward * Time.deltaTime * moveSpeed;
       }
       else if (move == 1){
            transform.localPosition += transform.right * Time.deltaTime * moveSpeed;
       }
       else if(move == 2){
            transform.localPosition += -transform.right * Time.deltaTime * moveSpeed;
       }
       else if(move == 3){
            transform.localPosition += -transform.forward * Time.deltaTime * moveSpeed;
       }
       NearButton();
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
     private void NearButton(){
        if(Vector3.Distance(transform.localPosition, button.transform.localPosition)<2 ){
            ChangeColor(button.GetComponent<Renderer>().material.color);
        }
    }
    private void ChangeColor(Color color){
        gameObject.GetComponent<Renderer>().material.color = color;
    }
    private void ManageReward(){
        Color myColor = GetComponent<Renderer>().material.color;
       
        if(myColor != Color.gray && EnvControl.IsCloseWall(room2:false)){
            AddReward(10.0f);
        }
        else{
            AddReward(-0.01f);
        }   
    }
    public void ChangeButtonLayout(){
        colors = colors.OrderBy(x=>rand.Next()).ToArray();
        button.GetComponent<Renderer>().material.color = colors[1];
        button.transform.localPosition = new Vector3(Random.Range(-27f,-5f),0.5f,Random.Range(-6f,6f));
    }
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Wall")){
            AddReward(-10.0f);
            EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other){
        AddReward(10.0f);
        ChangeButtonLayout();
        EndEpisode();
    }  
}
