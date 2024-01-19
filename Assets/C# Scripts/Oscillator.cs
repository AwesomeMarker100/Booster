using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

     [SerializeField] Vector3 movementDirection = new Vector3();
    //the amount of time before the oscillation repeats itself, the length of one full oscillation, the higher this is the slower the oscillation
    [SerializeField] float period;

    [Range(0, 1)] float movementFactor;

    private Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        //the reason this is at start is so that we can save the landing pads current location and go back to it with the slider
        startingPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //This is the amount of movement we have 

        if (period > Mathf.Epsilon) //if i was comparing period to 0 with == than i would need to use Mathf.Epsilon to
            //represent the smallest value I can
        {
            //the amount of cycles that it does the oscillation

            //if the game time is shorter, than it has to do cycles faster
            float cycles = Time.time / period; // keeps growing from zero, autmatically frame rate independent

            const float tau = 2 * Mathf.PI; // 6.28 ish, the amount of radians around a circle

            float rawSinWave = Mathf.Sin(cycles * tau); // the angle in radians, tau = 2 pi

            //Mathf.Sin returns the y coordinate of the angle based on the radian angle that we give it using tau
            //because one tau is a full cycle of a unit circle

            //mathf.Sin calculates sine of an angle

            //for example if you set cycles to 1/2 than the rawSinWave would be pi

            movementFactor = rawSinWave / 2f + 0.5f;

            //this sets our movement factor from a range of 0 - 1

            //because our range is from 0 to 1 and the sin wave's range is -1 to 1 we need to divide by 2 and add a 0.5 
            //to get those ranges to match up together, so we don't go out of range

            //we add the starting position to the amount of movement to move!
            Vector3 offset = movementDirection * movementFactor;

            // we modify vectors by using multiplication
            this.transform.position = startingPos + offset;
        } 
        
    }
}
