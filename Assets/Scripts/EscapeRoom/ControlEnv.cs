using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnv : MonoBehaviour
{
    public GameObject theButton;
    public GameObject redButton;
    public GameObject yellowButton;
    public GameObject greenButton;
    public GameObject blueButton;
    public GameObject Agent1;
    public GameObject Agent2;
    public GameObject door1;
    public GameObject door2;
    public void Update(){
        if( 
            Agent1.GetComponent<Renderer>().material.color == Agent2.GetComponent<Renderer>().material.color && 
            Agent1.GetComponent<Renderer>().material.color != Color.gray
        ){
            OpenWall();
        }
        else{
            CloseWall();
        }
    }
    public bool IsCloseWall(bool room1 = true, bool room2 = true){
        if(room1 && room2){
            return (door1.transform.position.y > 0 && door2.transform.position.y > 0);
        }
        else if(room1){
            return (door1.transform.position.y > 0);
        }
        return (door2.transform.position.y > 0);
    }
    public bool IsOpenWall(bool room1 = true, bool room2 = true){
        return (!IsCloseWall(room1, room2));
    }
    public void OpenWall(){
        door1.transform.position = new Vector3(door1.transform.position.x,-5.0f,door1.transform.position.z);
        door2.transform.position = new Vector3(door2.transform.position.x,-5.0f,door2.transform.position.z);
        hideButton(hide:true, room1:true, room2:true);
    }
    public void CloseWall(){
        if(Agent1.GetComponent<Renderer>().material.color == Color.gray){
            door1.transform.position = new Vector3(door1.transform.position.x, 0.5f,door1.transform.position.z);
            hideButton(hide:false, room1:true, room2: false);
        }
        if(Agent2.GetComponent<Renderer>().material.color == Color.gray){
            door2.transform.position = new Vector3(door2.transform.position.x, 0.5f,door2.transform.position.z);
            hideButton(hide:false, room1:false, room2:true);
        }
    }
    private void hideButton(bool hide = false, bool room1 = true, bool room2 = true){
        if(hide){
            if(room1){
                theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x,-1f,theButton.transform.localPosition.z);
            }
            if(room2){
                redButton.transform.localPosition = new Vector3(redButton.transform.localPosition.x,-1f,redButton.transform.localPosition.z);
                blueButton.transform.localPosition = new Vector3(blueButton.transform.localPosition.x,-1f,blueButton.transform.localPosition.z);
                greenButton.transform.localPosition = new Vector3(greenButton.transform.localPosition.x,-1f,greenButton.transform.localPosition.z);
                yellowButton.transform.localPosition = new Vector3(yellowButton.transform.localPosition.x,-1f,yellowButton.transform.localPosition.z);
            }
        }
        else{
            if(room1){
                theButton.transform.localPosition = new Vector3(theButton.transform.localPosition.x,0.1f,theButton.transform.localPosition.z);
            }
            if(room2){
                redButton.transform.localPosition = new Vector3(redButton.transform.localPosition.x,0.1f,redButton.transform.localPosition.z);
                blueButton.transform.localPosition = new Vector3(blueButton.transform.localPosition.x,0.1f,blueButton.transform.localPosition.z);
                greenButton.transform.localPosition = new Vector3(greenButton.transform.localPosition.x,0.1f,greenButton.transform.localPosition.z);
                yellowButton.transform.localPosition = new Vector3(yellowButton.transform.localPosition.x,0.1f,yellowButton.transform.localPosition.z);
            }
        }
    }
}
