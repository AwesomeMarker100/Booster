using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    bool isChanging = false;

    Rigidbody rig;
    AudioSource a;
    //GameObject lp;

    int strikes;

    CoinScript coin;

    [SerializeField] float RotationSpeed = 140f;
    [SerializeField] float ThrustSpeed = 950f;

    float rotatingSpeed;
    ScoreKeeper scorekeeper = new ScoreKeeper();

    [SerializeField] AudioClip RocketSound;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip FinishedLevel;

    [SerializeField] ParticleSystem RocketThrust;
    [SerializeField] ParticleSystem RocketExplode;
    [SerializeField] ParticleSystem RocketFinish;

    bool collisionsEnabled = true;

    [SerializeField] float Deathhits = 2;

    [SerializeField] float LevelLoadingDelay = 0;

    float maxScene;

    

    //serialize field makes this field speed changeable in the inspector

    // Start is called before the first frame update
    void Start()
    {

        coin = new CoinScript();
        //setting it once so that it won't have to create a rig variable every frame cause that would be insane
        rig = GetComponent<Rigidbody>();
        a = GetComponent<AudioSource>();

        maxScene = SceneManager.sceneCountInBuildSettings - 1;

        rotatingSpeed = RotationSpeed * Time.deltaTime;
        strikes = 0;
        //GetComponent gets a unity component from a specific gameobject that the script is attached to
    }

    // Update is called once per frame
    void Update()
    {
        //function to process all the movement of the rocket ship
        if(!isChanging)
        {
            Thrust();
            Rotate();

        }

        if (Debug.isDebugBuild)
        {

            ReturnToDebugKeys();

        }
        

    }

    void ReturnToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

            LoadNextScene();

        }else if (Input.GetKeyDown(KeyCode.P))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        } else if (Input.GetKeyDown(KeyCode.C))
        {

            collisionsEnabled = !collisionsEnabled; //this is a toggle, it can go on and off


        }
    }

    void OnCollisionEnter(Collision collision)
    {
       

        if(isChanging || !collisionsEnabled)
        {

            return;

        }

        switch (collision.gameObject.tag)
        {

            case "Friendly":   break;
            case "Finish":
                FinishSequence();
                break;
         

            
            default:
                DeathSequence();
                break;

            

        }
    }
 
    private void FinishSequence()
    {
        isChanging = true;
  

        a.Stop();

        a.PlayOneShot(FinishedLevel);

        RocketThrust.Stop();
        RocketFinish.Play();

        Invoke("LoadNextScene", LevelLoadingDelay);

        

    }
    private void DeathSequence()
    {
        if (strikes == Deathhits)
        {
            isChanging = true;

            a.Stop();
            a.PlayOneShot(DeathSound);
            

            RocketThrust.Stop();

            RocketExplode.Play();

            //while invoke is getting called, the rest of the game will keep going for the rest of that second like the Update() function and the collision function
            Invoke("LoadSameScene", LevelLoadingDelay);

        }
        else
        {
            strikes++;

        }

    }

    private void LoadNextScene()
    {

        
       if(SceneManager.GetActiveScene().buildIndex == maxScene)
        {

            Invoke("LoopBackScene", LevelLoadingDelay);

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    private void LoopBackScene()
    {
        
        SceneManager.LoadScene(0);

    }

    private void LoadSameScene()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    private void Thrust()//makes our code easier to read
    {
        RespondToThrustInput(); 
    }

    private void RespondToThrustInput()
    {
        float totalThrustSpeed = ThrustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            //The AddRelativeForce will add force relative to the ship's direction or position from the Transform component
            rig.AddRelativeForce(Vector3.up * totalThrustSpeed);
            ThrustMusicControl();
        }
        else
        {

            StopThrustMusic();
        }

    }

    private void ThrustMusicControl()
    {

        if (!a.isPlaying)
        {
            a.volume = 0.252f;
            a.PlayOneShot(RocketSound); //this will check if the audio is already playing so it won't play again and again
        }

        RocketThrust.Play();
    }

    private void StopThrustMusic()
    {

        a.Stop();//this will stop the audio when the user lets go of the space bar
        RocketThrust.Stop();

    }
    private void Rotate()
    {
        
         // this will stop the RigidBody from controlling the rotation and let us manually do it

        RotateLeft();
        RotateRight();
        

         // this will let the rigidbody handle the rotation now.

    }

    private void RotateLeft()
    {

        if (Input.GetKey(KeyCode.A))
        {
            //the direction refers to the axis, for example Vector3.forward refers to the z axis because it points forward
            rig.freezeRotation = true;
            transform.Rotate(Vector3.forward * rotatingSpeed);
            rig.freezeRotation = false;
        }

    }

    private void RotateRight()
    {

        if (Input.GetKey(KeyCode.D))
        {
            rig.freezeRotation = true;
            transform.Rotate(-Vector3.forward * rotatingSpeed);
            rig.freezeRotation = false;
        }

    }

}

    
