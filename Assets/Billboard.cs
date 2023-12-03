using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // rotate to look at the camera
        var lookAtThis = Camera.main.transform.position;
        lookAtThis.x = transform.position.x;
        transform.LookAt(lookAtThis);
    }

    void OnBecameVisible() 
    {
        //enabled = true;
    }

    void OnBecameInvisible() 
    {
        // don't bother with making it look at the camera if it isn't visible to the player
        // NOTE: Looks like it disappears on edges in preview, but unsure if that's me using the wrong resolution here or not
        //enabled = false;
    }
}
