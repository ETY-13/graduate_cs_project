using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CopsAI : Agent
{
    public GameObject partner;
    public Transform robber1;
    public Transform robber2;
    private Transform catchingTarget;
    public int searchedRobber = 1;
    public bool copP1 = false;
    public bool moveBack = false;
    public bool communication = true;
    
    public override void OnEpisodeBegin(){

        float moveTo = 4.0f;
        if(moveBack) moveTo = -5.0f;
        
        if(copP1){
            transform.localPosition = new Vector3(-7.0f,1.16f, moveTo);
        }
        else{
            transform.localPosition = new Vector3(7.0f,1.16f, moveTo);
        }
        gameObject.GetComponent<Control>().retrievedMoney = false;
        gameObject.GetComponent<Control>().shame = false;

        int coinFlip = Random.Range( 0, 10);
        if(coinFlip > 5){
            catchingTarget = robber2;
            searchedRobber = 2;
        }
        else{
            catchingTarget = robber1;
            searchedRobber = 1;
        }
    }
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(catchingTarget.localPosition);
        sensor.AddObservation(transform.localPosition);
        if(communication){
            sensor.AddObservation(partner.gameObject.GetComponent<CopsAI>().communicate());
        }
        else{
            sensor.AddObservation(Vector3.zero);
        }
    }
    public override void OnActionReceived(ActionBuffers actions){
        int move = actions.DiscreteActions[0];
        gameObject.GetComponent<Control>().movePlayer(moveDir:move);

        float dist = - Mathf.Abs(Vector3.Distance(transform.localPosition, catchingTarget.localPosition)/10);
        if(gameObject.GetComponent<Control>().retrievedMoney){
            AddReward(1.0f);
            partner.gameObject.GetComponent<CopsAI>().AddReward(1.0f);
            gameObject.GetComponent<Control>().retrievedMoney = false;
            KeepScore.Scorekeeper.CopScore();
        }
        else if(gameObject.GetComponent<Control>().shame){
            AddReward(-0.1f);
            EndEpisode();
        }
    
        if(gameObject.GetComponent<Control>().child.transform.localPosition.y < -1.0f || transform.localPosition.y < 0.0f){
           gameObject.GetComponent<Control>().child.transform.localPosition = new Vector3(0.0f,0.0f,1f);
           transform.localPosition = new Vector3(0f,1.16f, 0f);
        }
    }

    public Vector3 communicate(){
        return transform.localPosition;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 0;

        if(Input.GetKey(KeyCode.DownArrow)){
            actions[0] = 1;
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            actions[0] = 2;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            actions[0] = 3;
        }
        else if(Input.GetKey(KeyCode.UpArrow)){
            actions[0] = 4;
        }
    }
}
