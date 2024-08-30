using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomType
{
    RED = 0,
    BLUE = 1,
    GREEN = 2,
}
public class MushroomBehaviour : MonoBehaviour
{
    [SerializeField]
    MushroomType _mushroomType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(_mushroomType == MushroomType.RED)
            {
                if (!other.GetComponent<PlayerController>().TransformPlayer(PlayerTransformState.NORMAL, false)) return;
            }
            else if(_mushroomType == MushroomType.BLUE)
            {
                if (!other.GetComponent<PlayerController>().TransformPlayer(PlayerTransformState.SMALL, false)) return;
            }
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, .1f);
        }
    }
}
