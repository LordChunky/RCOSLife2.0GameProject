using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * This is the base for all animal AI
 * This class does not implement AI for any specific animal
*/
public class AnimalBase : MonoBehaviour
{   
    //The State and Stats of animal
    public enum State { Idling, Walking, Running, Eating, Panicking, Dying, Following, Hungry, Pushed } //The different states the animal can be in
    public State currState = State.Idling; 
    public int awareness; //When the animal can detect objects. Is different for different animals, and can change depending on state
    public float walkspeed; //How fast the animal walks
    public float runspeed; //How fast the animal runs
    public Rigidbody2D animal;
    public float HPCap; //Max Health
    public float currHP; //Current HP of Animal
    public float currMaxSpeed; //Current possible max speed
    public float currSpeed; //Current Speed of Animal
    public float weight; //Determines how much the animal will get pushed
    
    public Animator animate;
    public Vector2 position;
    public Vector2 newposition;
    public float time;
    public float timeDelay;
    public bool reached; //Determines if the animal has reached its destination
    public GameObject player;
    public RaycastHit hit;
    public SpriteRenderer aniSprite;


    //random pos
    public void PositionChange()
    {
        currSpeed = Random.Range(0, currMaxSpeed);
        float posxmin = transform.position.x - currSpeed;
        float posxmax = transform.position.x + currSpeed;
        float posymin = transform.position.y - currSpeed;
        float posymax = transform.position.y + currSpeed;

        int gen = Random.Range(0, 2);
        if (gen == 0) 
        {
            newposition = new Vector2(Random.Range(posxmin, posxmax), transform.position.y);
        }
        else if (gen == 1)
        {
            newposition = new Vector2(transform.position.x, Random.Range(posymin, posymax));
        }
        else if(gen == 2)
        {
            newposition = new Vector2(Random.Range(posxmin, posxmax), Random.Range(posymin, posymax));
        }
        flipSprite();
    }

    public string  getAnimal()
    {
        return this.GetType().Name; //Get the animal type
    }

    public void Run()
    {
        currState = State.Running;
        currMaxSpeed = runspeed;
    }
    
    public void Walk()
    {
        currMaxSpeed = walkspeed;
        currState = State.Walking;
    }

    public void Idle()
    {
        currMaxSpeed = 0;
        currState = State.Idling;
    }

    public void Hungry()
    {
        currState = State.Hungry;
    }

    public float getHP()
    {
        return currHP;
    }

    public float getSpeed()
    {
        return currSpeed;
    }

    private void flipSprite()
    {
        Vector2 direction = newposition - position; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //flip sprites based on the direction of the target and "this"
        if (angle >= 90 || angle <= -90)
        {
            //face left
            aniSprite.flipX = false;

        }
        else
        {
            aniSprite.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        { 
                currState = State.Pushed;            
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
            newposition = transform.position;
            currState = State.Idling;   

    }
}
