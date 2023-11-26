using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCntrl : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private CharacterController charCntrl;
    private Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        charCntrl.Move(direction * speed * Time.deltaTime);
    }
}
