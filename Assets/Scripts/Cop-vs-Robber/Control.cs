using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    Animator animator;
    public GameObject ball;
    public bool carryBall = false;
    public bool foundMoney = false;
    public bool gotRich = false;
    public bool gotCap = false;
    public bool shame = false;
    public bool retrievedMoney = false;
    public GameObject child;
    public Transform holdPosition;
    public float moveSpeed = 2.0f;
   

    
    void OnCollisionEnter(Collision other){

        if(other.gameObject.CompareTag("Wall")){
            if(gameObject.CompareTag("Robbers")){
                gameObject.GetComponent<RobbersAI>().AddReward(-0.1f);
                transform.localPosition = new Vector3(7.0f,1.16f, 0f);
            }
            else if(gameObject.CompareTag("Cops")){
                gameObject.GetComponent<CopsAI>().AddReward(-0.1f);
            }
        }

        else if(other.gameObject.CompareTag("Ball")){
            if(gameObject.CompareTag("Robbers")){
                if(!other.gameObject.GetComponent<BallControl>().holder){
                    ball.GetComponent<BallControl>().hold(holdPosition, gameObject);
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    StreamWriter writer = new StreamWriter("C:\\Users\\snowf\\Desktop\\graduate_talk\\train_project2_result\\log_output\\gotmoney.txt", true);
                    writer.WriteLine(gameObject.name);
                    writer.Close();
                }
            }
            
            else if(gameObject.CompareTag("Cops")){
                if(other.gameObject.GetComponent<BallControl>().holder){
                    other.gameObject.GetComponent<BallControl>().holder.gameObject.GetComponent<Control>().gotCap =true;
                }
                ball.gameObject.GetComponent<BallControl>().collect();
                retrievedMoney = true;
            }
        }

        else if(other.gameObject.CompareTag("Cops")){
            if(gameObject.CompareTag("Robbers")){
                if(carryBall){
                    ball.gameObject.GetComponent<BallControl>().collect();
                    other.gameObject.GetComponent<Control>().retrievedMoney = true;
                    gotCap = true;
                }
                else if(!other.gameObject.GetComponent<Control>().retrievedMoney){
                    other.gameObject.GetComponent<Control>().shame = true;
                }

                StreamWriter writer = new StreamWriter("C:\\Users\\snowf\\Desktop\\graduate_talk\\train_project2_result\\log_output\\gotcaught.txt", true);
                writer.WriteLine(gameObject.name);
                writer.Close();
            }
        }
    }
    private void pass(){
       
       if(carryBall){
            ball.GetComponent<BallControl>().throwme(holdPosition.forward);
            carryBall = false;
       }
    }
    public void movePlayer(int moveDir = 0){

        if (moveDir == 1){
         
            child.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            if(transform.localPosition.z < 15){
                transform.localPosition += transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        else if (moveDir == 2){
          
            child.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
            if(transform.localPosition.x > -10){
            transform.localPosition += -transform.right * Time.deltaTime * moveSpeed;
            }
        }
        else if (moveDir == 3){
      
            child.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            if(transform.localPosition.x < 10){
            transform.localPosition += transform.right * Time.deltaTime * moveSpeed;
            }
        }
        else if (moveDir == 4){
         
            child.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            if(transform.localPosition.z > -15 && gameObject.CompareTag("Robbers")){
              transform.localPosition += -transform.forward * Time.deltaTime * moveSpeed;
            }
            else if(transform.localPosition.z > -4 && gameObject.CompareTag("Cops")){
                transform.localPosition += -transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        else if(moveDir == 5){
            ball.GetComponent<BallControl>().drop();
        }
        else if(moveDir == 6){
            pass();
        }
    }
}
