using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInteractSight : MonoBehaviour
{
    public LayerMask ray_mask;
    void Start()
    {

    }

    public float timer_flag;
    public float timer_time = 0.0f;
    int id_hit;   // last unique id hitted
    int id_rec;   // last unique id recorded
    public Dictionary<string, Mesh> decorations = new Dictionary<string, Mesh>();
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,10.0f, ray_mask))
        {
            if(id_hit == hit.transform.GetInstanceID())
            {
                timer_time += Time.deltaTime;
            }
            else
            {
                id_hit = hit.transform.GetInstanceID();
            }

            if(timer_time>timer_flag && id_rec!=id_hit)
            {
                id_rec = id_hit;
                //hit.collider.; // <-- GameObject
                MeshFilter mesh_property = hit.collider.GetComponent<MeshFilter>();
                if(mesh_property != null)
                {
                    decorations[mesh_property.tag] = mesh_property.mesh;
                    Debug.Log(mesh_property.mesh.name);
                }
            }
        }
        else
        {
            timer_time = 0.0f;
            id_hit = 0;
        }
    }
}
