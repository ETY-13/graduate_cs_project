using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TargetControl : MonoBehaviour
{
    public void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Ball")){
            if(other.gameObject.GetComponent<BallControl>().holder){
                other.gameObject.GetComponent<BallControl>().holder.gameObject.GetComponent<Control>().gotRich = true;
            }
        }
        else if(other.gameObject.CompareTag("Robbers")){
            if(other.gameObject.GetComponent<Control>().carryBall){
                other.gameObject.GetComponent<Control>().gotRich = true;
            }
        }
    }
}
