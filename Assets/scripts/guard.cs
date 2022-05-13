//File: guard.cs
//Author: 1314371
//Date Created: 2APR2015 @ 08:34
//Latest Revision: 24APR2015 @ 11:58
//Description: File for the guard AI

using UnityEngine;
using System.Collections;

public class guard : MonoBehaviour {

    //Author's Note: I wanted to make this a master script and have 2 other scripts inherit from this but apparently C# doesn't support that. :(

    [SerializeField]
    private bool patrols;

    [SerializeField]
    private bool rotates;

    private bool chasing;

    [SerializeField]
    private int index;

    private int viewRange;

    private int timer;

    private int current;

    [SerializeField]
    private string[] patrolTag = new string[2];

    private Vector3 originalLocation;

    private Vector3 originalRot;

    private Vector2 xy;

    private GameObject target;

	[SerializeField]
    private GameObject[] patrol;

    private GameObject[] nearbyGuards;

    private NavMeshAgent agent;

    private Ray ray;

    private RaycastHit hit;

    private soundManager sound;

	void Start () 
    {

        originalLocation = transform.position;

        originalRot = transform.eulerAngles;

        agent = GetComponent<NavMeshAgent>();

        chasing = false;

        viewRange = 10;

        timer = 60;

        //patrol = GameObject.FindGameObjectsWithTag( patrolTag[index] );

        current = 0;

        sound = GetComponent<soundManager>();

        if( index > patrolTag.Length )
        {

            index = 0;

        }

	}
	
	void FixedUpdate () 
    {

        Debug.DrawRay( ray.origin, ray.direction * viewRange, Color.green );

        xy = new Vector2( transform.position.x, transform.position.z );

        ray = new Ray(transform.position, transform.forward);

        checkPlayerHidden();

        if( transform.position == originalLocation && !rotates && !patrols )
        {

            transform.eulerAngles = originalRot;

        }

        if( target != null )
        {

            agent.SetDestination(target.transform.position);

            raycastToPLayer();

        }
        else
        {

            if( patrols )
            {

                if ( xy == new Vector2( patrol[current].transform.position.x, patrol[current].transform.position.z ) )
                {

                    if (current < patrol.Length - 1)
                    {

                        current++;

                    }
                    else
                    {

                        current = 0;

                    }

                }

                agent.SetDestination(patrol[current].transform.position);

            }
            else
            {

                agent.SetDestination( originalLocation );

            }

        }

        if( rotates && !chasing )
        {

            if( timer > 0 )
            {

                timer -= 1;

            }
            else
            {

                StartCoroutine(rotate());

            }

        }

	}

    void checkPlayerHidden()
    {

        if( Physics.Raycast(ray, out hit, viewRange ) )
        {

            if (hit.collider.tag == "Player")
            {

                target = hit.collider.gameObject;

                if (!target.GetComponent<playerController>().getCaught())
                {

                    sound.playSFX(0);

                    target.GetComponent<soundManager>().playMusic(1);

                    target.GetComponent<playerController>().setCaught(true);

                }

                chasing = true;

				agent.speed=7;

                alertOthers( target );

                
            
            }

        }

    }

    void raycastToPLayer()
    {

        Vector3 dir = target.transform.position - transform.position;

        if( Physics.Raycast( transform.position, dir, out hit ) )
        {

            if( hit.collider.tag != "Player" )
            {

                StartCoroutine( lookTimer() );

            }

        }

    }

    IEnumerator rotate()
    {

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z), Time.deltaTime);

        yield return new WaitForSeconds(0.98f);

        timer = 60;

    }

    void alertOthers( GameObject t )
    {

        //play sound

        Collider[] guards = Physics.OverlapSphere(transform.position, 100);

        foreach (Collider g in guards)
        {

            if (g.tag == "guard")
            {

			
				g.GetComponent<guard>().setTarget(t);
            }

        }

    }

    IEnumerator lookTimer()
    {

        yield return new WaitForSeconds( 10 );

        target.GetComponent<soundManager>().playMusic(0);

        target.GetComponent<playerController>().setCaught(false);

        target = null;

        chasing = false;

        StopCoroutine(lookTimer());

    }

	public void setTarget( GameObject t )
	{

		target = t;

        chasing = true;

	}
	void OnTriggerEnter( Collider other )
	{

		if (other.gameObject.tag == "Player") 
		{
			Application.LoadLevel("GameOver");
		}

	}

}
