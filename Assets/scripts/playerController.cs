//File: playerController.cs
//Author: 1314371
//Date Created: 20MAR2015 @ 10:55
//Latest Revision: 11APR2015 @ 08:06
//Description: File for the playerController

using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{

	[SerializeField]
    private float speed;

    private float maxSpeed;

    private float turnSmoothing = 30f;

    private Material current;

    private GameObject objHit = null;

    private Ray ray;

    private RaycastHit hit;

	private int count;

	public GameObject obj;

	private Vector3 throwPos;

	private int levelLoader; //SID:1209432

	private soundManager sound;

    private bool caught;

    void Start () 
    {

		count = 0;

        maxSpeed = speed;

		ThrowPositionUpdate ();

		levelLoader = Application.loadedLevel;

		sound = GetComponent<soundManager>();

		sound.playMusic(0);

        caught = false;

	}

    void FixedUpdate()
    {

		Camera.main.transform.position = new Vector3 ( transform.position.x + 4, 20, transform.position.z - 8 );

        float h = Input.GetAxis( "Horizontal" );

        float v = Input.GetAxis( "Vertical" );

        movement( h, v );

		ThrowPositionUpdate ();

        ray = new Ray(transform.position, Camera.main.transform.position - transform.position);
         
        raycastToPlayer();

        Debug.DrawRay( ray.origin, ray.direction, Color.red );

        //region 1311008's
		if (Input.GetButtonDown("Throw"))
		{
			if (count > 0)
			{
				GameObject token = (GameObject)Instantiate(obj, throwPos, transform.rotation);
				token.rigidbody.AddForce( transform.forward * 400 );
				token.rigidbody.AddForce( transform.up * 300 );
				token.GetComponent<throwItem>().setMadeNoise(false);
				count--;
			}
		}
		//endregion

    }

    void movement( float h, float v )
    {

        if( h != 0f || v != 0f )
        {

            rotation( h, v );

            transform.position += transform.forward * Time.deltaTime *speed;

        }
        
        if( Input.GetButton("Sneak") )
        {

            if( speed > maxSpeed / 2 )
            {

                speed /= 2;

            }

        }
        else if( !Input.GetButton( "Sneak" ) )
        {

			makeNoise(2);
            if( speed < maxSpeed )
            {

                speed *= 2;

            }

        }

    }

    void rotation( float h, float v )
    {

        Vector3 targetDir = new Vector3( h, 0f, v );

        Quaternion targetRot = Quaternion.LookRotation( targetDir, Vector3.up );

        Quaternion newRot = Quaternion.Lerp( rigidbody.rotation, targetRot, turnSmoothing * Time.deltaTime );

        rigidbody.MoveRotation( newRot );

    }

    void raycastToPlayer()
    {

        Vector3 dir = gameObject.transform.position - Camera.main.transform.position;

        if (Physics.Raycast(Camera.main.transform.position, dir, out hit))
        {

            if (hit.collider.tag == "canTransparent")
            {

                objHit = hit.collider.gameObject;

                current = objHit.renderer.material;

                objHit.renderer.material.color = new Color( current.color.r, current.color.g, current.color.b, 0.25f );

            }
            else
            {

                if( objHit != null )
                {

                    objHit.renderer.material.color = new Color( current.color.r, current.color.g, current.color.b, 1 );

                }

            }

        }

    }

    void OnTriggerEnter(Collider other)
    {

		if (other.gameObject.tag == "throwableObj")
		{

			count++;

			Destroy (other.gameObject);

		}

		if( other.gameObject.tag == "puddle" )
		{
			if( !Input.GetButton("Sneak") )
			{

                sound.playSFX( 4 );

				makeNoise ( 10 );

			}


		}

		if (other.gameObject.tag == "guard") 
		{
			Application.LoadLevel("GameOver");
		}

		if (other.gameObject.tag == "LevelEnder") 
		{
			Application.LoadLevel(levelLoader+1); //SID1209432
		}

    }

	void makeNoise( float r )
	{
		
		Collider[] alerted = Physics.OverlapSphere( transform.position, r );
		
		foreach (Collider g in alerted )
		{
			
			if( g.tag == "guard" )
			{
				
				g.GetComponent<guard>().setTarget( this.gameObject );
				
			}
			
		}
		
	}

	void ThrowPositionUpdate()
	{
		throwPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1.3f);
	}

    public void setCaught( bool b )
    {

        caught = b;

    }

    public bool getCaught()
    {

        return caught;

    }

}
