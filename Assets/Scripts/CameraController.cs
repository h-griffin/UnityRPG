using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    public Transform target; // position target/player

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.transform;

        bottomLeftLimit = theMap.localBounds.min;
        topRightLimit = theMap.localBounds.max;
    }

    // LateUpdate is called once per frame AFTER Update
    void LateUpdate() 
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // keep camera inside map bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z);
    }
}
