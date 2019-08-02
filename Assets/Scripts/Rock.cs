using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boo.Lang.Environments;

public class Rock : MonoBehaviour
{
	private Rigidbody2D myRb2D;
	private float itemPos;

    // Start is called before the first frame update
    void Start()
    {
		myRb2D = GetComponent<Rigidbody2D>();
		itemPos = transform.localPosition.x;

    }

    // Update is called once per frame
    void Update()
    {
		print(myRb2D.velocity);
		if (itemPos >= itemPos + 1f || itemPos <= -1f  ) {
			SetStatic();
		}
    }

	void SetStatic(){
		myRb2D.isKinematic = false;
	}

	void SetKenimatic(){
		myRb2D.isKinematic = true;
	}
}
