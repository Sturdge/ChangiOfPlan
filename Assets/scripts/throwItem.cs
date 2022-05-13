using UnityEngine;
using System.Collections;

public class throwItem : MonoBehaviour {

	private bool noiseMade = true;

    private soundManager sound;//1314371

    void Start()
    {

        sound = GetComponent<soundManager>();//1314371

    }

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "floor" && !noiseMade ) 
		{

            sound.playSFX(0);

			makeNoise( 10 );

			Debug.Log("huh? what was that noise?");

			noiseMade = true;//1314371
		}
		else if (other.gameObject.tag == "guard") 
		{
			Destroy (gameObject);
		}
	}

    #region 1314371
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

	public void setMadeNoise( bool b )
	{

		noiseMade = b;

    }
    #endregion

}