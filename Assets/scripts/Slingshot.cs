using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //from video
    [Header("Inscribed")]
   // public GameObject projectilePrefab;
    private Transform ball;

    

    //for rubber
    [SerializeField] private LineRenderer rubber;
    [SerializeField] private Transform firstPoint;
    [SerializeField] private Transform secondPoint;

    [SerializeField] private Configuration configuration;
    
    //from book
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 4f;
    public GameObject projLinePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    //from VIDEO
    /*void Start(){
        rubber.SetPosition(2, firstPoint.position);
        rubber.SetPosition(0, secondPoint.position);
    }

    void Update(){
        Vector3 ballPosition;
        if(Input.GetMouseButtonDown(0)){
            ball = Instantiate(configuration.ball).transform;
        }
        if(Input.GetMouseButton(0)){
            ballPosition = GetMousePositionInWorld();
            ball.position = ballPosition;
            rubber.SetPosition(1, ballPosition);
            if(ballPosition.y < -10){
                ballPosition.y = -10;
            }
            
        }
        if(Input.GetMouseButtonUp(0)){
            //create a vector3 for comparison
            
            Vector3 ballLauncher = transform.position;
            
            ballPosition = GetMousePositionInWorld();
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            Vector3 exec = ballPosition - ballLauncher;
            exec.x = 4*exec.x * (-1);   //Precising it
            exec.y = 4*(4-exec.y);      //precising it
            
            rigidbody.linearVelocity =  (exec);
            //FollowCam.POI = ball;
            ball = null;
        }
        //Vector3 ballPosition = Vector3.zero;
    }

    Vector3 GetMousePositionInWorld(){
        Vector3 mousePosition = Input.mousePosition;
        Vector3 slingshotPos = transform.position;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePositionInWorld.z = 0; //mouse position in world is now in he 2d place

        Transform launchPointTrans = transform.Find("LaunchPoint");  //finds launchpoint
        Vector3 otherPos = mousePositionInWorld - slingshotPos ;
        if(otherPos.magnitude > configuration.Radius){  //binds slingshot to 5 in game
            otherPos.Normalize();
            otherPos *= configuration.Radius;
        } 
        return slingshotPos + otherPos ; 

    }*/

    //from BOOK
    void Awake(){
        
        //Sets up the launchpoint 
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter(){
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        //print("slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown(){
        aimingMode = true;
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = this.transform.position;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        
    }

    void Update(){
        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //changed
        Vector3 mouseDelta = mousePos3D - launchPos /*- transform.position*/;

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        rubber.SetPosition(1, projPos);

        if(Input.GetMouseButtonUp(0)){
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.linearVelocity = -mouseDelta*velocityMult;

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

            FollowCam.POI = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }
}
