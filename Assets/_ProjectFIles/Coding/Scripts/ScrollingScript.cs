using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    
    [SerializeField] private float scrollSpeed;
    private float offset;
    private Material mat;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float speed;
    
    Vector2 startingPosition;
    float startingZ;
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    float zDistanceFromTarget => transform.position.z - followTarget.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    //--------------------------------------------------------//

    //getComponents
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

   

    // Update is called once per frame
    void Update()
    {

        //Changes position to follow player
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor * speed;
        transform.position = new Vector3(followTarget.transform.position.x, newPosition.y + (followTarget.transform.position.y / 10f), startingZ);

        //updates material offset to scroll
        //offset += (newPosition.x / 10f);
        mat.SetTextureOffset("_MainTex", new Vector2((newPosition.x / scrollSpeed), 0));


        
    }
}









// Maybe add a code in that changes the offset as the player moves????
//