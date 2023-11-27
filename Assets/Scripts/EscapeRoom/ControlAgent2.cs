using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ControlAgent2 : Agent
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private GameObject theRightButton;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject ears;
    [SerializeField] private GameObject redButton;
    [SerializeField] private GameObject blueButton;
    [SerializeField] private GameObject greenButton;
    [SerializeField] private GameObject yellowButton;
    [SerializeField] private ControlEnv EnvControl;
    [SerializeField] private bool logMove = false;

    private Color [] colors = {Color.red, Color.blue, Color.yellow, Color.green};
    private System.Random rand = new System.Random();
    private Vector3 [] buttonPosition = {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0)};
    public override void Initialize()
    {
        buttonPosition[0] = blueButton.transform.localPosition;
        buttonPosition[1] = redButton.transform.localPosition;
        buttonPosition[2] = greenButton.transform.localPosition;
        buttonPosition[3] = yellowButton.transform.localPosition;

        theRightButton.GetComponent<Renderer>().material.color = Color.blue;
        redButton.GetComponent<Renderer>().material.color = Color.red;
        blueButton.GetComponent<Renderer>().material.color = Color.blue;
        yellowButton.GetComponent<Renderer>().material.color = Color.yellow;
        greenButton.GetComponent<Renderer>().material.color = Color.green;
    
    }
    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(15.0f, 0.5f, -3.0f);
        ChangeColor(Color.gray);
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);

        int direction = ears.GetComponent<Agent2Com>().Heard();

        if(EnvControl.IsOpenWall(room1:false)){
            sensor.AddObservation(goal.transform.localPosition);
        }
        else if(direction == 0){
            sensor.AddObservation(yellowButton.transform.localPosition);
        }
        else if(direction == 1){
            sensor.AddObservation(redButton.transform.localPosition);
        }
        else if(direction == 2){
            sensor.AddObservation(greenButton.transform.localPosition);
        }
        else if(direction == 3){
            sensor.AddObservation(blueButton.transform.localPosition);
        }
        else{
            sensor.AddObservation(new Vector3(0,0,0));
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

        if(Input.GetKey(KeyCode.W)){
            actions[0] = 0;
        }
        else if(Input.GetKey(KeyCode.D)){
            actions[0] = 1;
        }
        else if(Input.GetKey(KeyCode.A)){
            actions[0] = 2;
        }
        else if(Input.GetKey(KeyCode.S)){
            actions[0] = 3;
        }
        else if(Input.GetKey(KeyCode.E)){
            actions[0] = 4;
        }
    }

     private void NearButton(){
        if(EnvControl.IsCloseWall(room1:false)){
            if(Vector3.Distance(transform.localPosition, redButton.transform.localPosition)<2){
                ChangeColor(redButton.GetComponent<Renderer>().material.color);
            }
            else if(Vector3.Distance(transform.localPosition, blueButton.transform.localPosition)<2){
                ChangeColor(blueButton.GetComponent<Renderer>().material.color);
            }
            else if(Vector3.Distance(transform.localPosition, greenButton.transform.localPosition)<2){
                ChangeColor(greenButton.GetComponent<Renderer>().material.color);
            }
            else if(Vector3.Distance(transform.localPosition, yellowButton.transform.localPosition)<2){
                ChangeColor(yellowButton.GetComponent<Renderer>().material.color);
            }
            else {
                ChangeColor(Color.gray);
            }
        }
    }
    private void ChangeColor(Color color){
        gameObject.GetComponent<Renderer>().material.color = color;
    }
   
   private void ManageReward(){
        Color myColor = GetComponent<Renderer>().material.color;
        Color theRightButtonColor = theRightButton.GetComponent<Renderer>().material.color;
        
        if(myColor == theRightButtonColor){
            if(EnvControl.IsCloseWall(room1:false)){
                AddReward(10.0f);
                if(logMove){
                    ChangeButtonLayout();
                    EndEpisode();
                }
            }
            else{
                AddReward(-1.0f);
                if(logMove){
                    EndEpisode();
                }
            }
            
        }
        else if(myColor == Color.gray){
            AddReward(-0.01f);
        }   
        else{
            AddReward(-10.0f);
            EndEpisode();
        }
    }
    public void ChangeButtonLayout(){
        if(logMove){
            colors = colors.OrderBy(x=>rand.Next()).ToArray();
            theRightButton.GetComponent<Renderer>().material.color = colors[1];
        }

        buttonPosition.OrderBy(x=>rand.Next()).ToArray();
       
        int count = buttonPosition.Count();
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            Vector3 tmp = buttonPosition[i];
            buttonPosition[i] = buttonPosition[r];
            buttonPosition[r] = tmp;
        }
       
        redButton.transform.localPosition = buttonPosition[0];
        blueButton.transform.localPosition = buttonPosition[1];
        greenButton.transform.localPosition = buttonPosition[2];
        yellowButton.transform.localPosition = buttonPosition[3];
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
