using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public GameObject holder = null;
    public Transform holderArm = null;
    public Transform atm;
    private float lerpSpeed = 10f;

   void Start()
    {
        collect();
    }
    void FixedUpdate()
    {
        if(holder){

            if(holder.GetComponent<Control>().gotRich){
                collect();
            }
            else{
                Vector3 moveTo = Vector3.Lerp(holderArm.position,transform.position, Time.deltaTime * lerpSpeed);
                transform.position = moveTo;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

        if( transform.localPosition.x > 12 || transform.localPosition.x < -12 || 
            transform.localPosition.z > 17 || transform.localPosition.z < -17 ||
            transform.localPosition.y < -1){
            collect();
        }
    }
     public void hold(Transform holdPosition, GameObject picker)
     {
         picker.gameObject.GetComponent<Control>().carryBall = true;
         picker.gameObject.GetComponent<Control>().foundMoney = true;
         holder = picker;
         holderArm = holdPosition;
     }
     public void drop()
     {
        GetComponent<Rigidbody>().useGravity = true;
  
        if(holder){
            holder.gameObject.GetComponent<Control>().carryBall = false;
            holder = null;
            holderArm = null;
        }
     }
     public void collect()
     {
        transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, -12.0f);
        GetComponent<Rigidbody>().useGravity = true;
        if(holder){
            holder.gameObject.GetComponent<Control>().carryBall = false;
            holder = null;
            holderArm = null;
        }
    }
     public void throwme(Vector3 direction)
     {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(direction * 3, ForceMode.Impulse);
        holder.gameObject.GetComponent<Control>().carryBall = false;

        holder = null;
        holderArm = null;
    }
}
