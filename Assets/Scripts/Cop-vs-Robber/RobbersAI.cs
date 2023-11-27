using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RobbersAI : Agent {
    public Transform moneyTrans; 
    public Transform hideOutTrans;
    public Transform bodyTrans;
    public GameObject partner;
    public GameObject cops1;
    public GameObject cops2;
    public bool communication = true;
    public Vector3 CommunicationToken = new Vector3(0,0,0);
    private float distance_from_goal;
    public override void OnEpisodeBegin(){
        if(gameObject.GetComponent<Control>().carryBall){
          gameObject.GetComponent<Control>().ball.gameObject.GetComponent<BallControl>().collect();
        }
        gameObject.GetComponent<Control>().carryBall = false;
        gameObject.GetComponent<Control>().gotRich = false;
        gameObject.GetComponent<Control>().foundMoney = false;
        gameObject.GetComponent<Control>().gotCap = false;
    }
    public override void CollectObservations(VectorSensor sensor){
        if(gameObject.GetComponent<Control>().carryBall){
             CommunicationToken = new Vector3(1,1,1);
             distance_from_goal = - Mathf.Abs(Vector3.Distance(transform.localPosition, hideOutTrans.localPosition)/10f);
        }
        else{
            if(!partner.GetComponent<Control>().carryBall){
              distance_from_goal = - Vector3.Distance(transform.localPosition, moneyTrans.localPosition)/10f;
            }
            if(communication) {
              CommunicationToken = partner.GetComponent<RobbersAI>().CommunicationToken;
            }
            else{
              CommunicationToken = new Vector3(0,0,0);
            }
        }

        sensor.AddObservation(hideOutTrans.localPosition);
        sensor.AddObservation(moneyTrans.localPosition);
        sensor.AddObservation(transform.localPosition); 
        sensor.AddObservation(cops1.transform.localPosition);
        sensor.AddObservation(cops2.transform.localPosition); 
        sensor.AddObservation(CommunicationToken);
    }
    public override void OnActionReceived(ActionBuffers actions){
        int move = actions.DiscreteActions[0];
        gameObject.GetComponent<Control>().movePlayer(moveDir:move);
       
      if(gameObject.GetComponent<Control>().foundMoney){
        gameObject.GetComponent<Control>().foundMoney = false;
        AddReward(0.10f);
      }
      
      if(gameObject.GetComponent<Control>().gotRich){
        gameObject.GetComponent<Control>().gotRich = false;
        AddReward(1.0f);
        partner.GetComponent<RobbersAI>().AddReward(1.0f);
        cops1.GetComponent<CopsAI>().AddReward(-0.10f);
        cops2.GetComponent<CopsAI>().AddReward(-0.10f);
        KeepScore.Scorekeeper.RobScore();
        EndEpisode();
      }
      if(gameObject.GetComponent<Control>().gotCap){
        gameObject.GetComponent<Control>().gotCap = false;
        partner.GetComponent<RobbersAI>().AddReward(-0.50f);
        AddReward(-0.50f);
        EndEpisode();
      }
      if(!(gameObject.GetComponent<Control>().carryBall || partner.gameObject.GetComponent<Control>().carryBall)){
        distance_from_goal -= 0.5f;
      }
      AddReward(distance_from_goal);
      if(gameObject.GetComponent<Control>().child.transform.localPosition.y < -1.0f || transform.localPosition.y < 0.0f){
          gameObject.GetComponent<Control>().child.transform.localPosition = new Vector3(0.0f,0.0f,1f);
          transform.localPosition = new Vector3(0f,1.16f, 0f);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = 0;

        if(Input.GetKey(KeyCode.S)){
          actions[0] = 1;
        }
        else if(Input.GetKey(KeyCode.D)){
          actions[0] = 2;
        }
        else if(Input.GetKey(KeyCode.A)){
          actions[0] = 3;
        }
        else if(Input.GetKey(KeyCode.W)){
          actions[0] = 4;
        }
        else if(Input.GetKey(KeyCode.E)){
          actions[0] = 5;
        }
        else if(Input.GetKey(KeyCode.R)){
          actions[0] = 6;
        }
    }
}
