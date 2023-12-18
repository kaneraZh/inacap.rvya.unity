using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraInteractSight : MonoBehaviour
{
    public LayerMask ray_mask;
    void Start()
    {

    }
    public GameObject props;
    public Transform stand;

    public float equip_timer_flag;
    public float equip_timer_time= 0.0f;
    int id_hit;   // last unique id hitted
    int id_rec;   // last unique id recorded
    public struct Deco
    {
        public Mesh mesh;
        public Material material;
        public Deco(Mesh mesh, Material material)
        {
            this.mesh = mesh;
            this.material = material;
        }
    }
    public Dictionary<string, Deco> decorations = new Dictionary<string, Deco>();

    public float transition_timer_flag;
    public float transition_timer_time= 0.0f;
    public Vector3 mirror_normal = new Vector3 (0,0,-1);
    void Update()
    {
        stand.rotation = Quaternion.LookRotation(
            Vector3.Reflect(transform.rotation * Vector3.forward, mirror_normal), 
            Vector3.Reflect(transform.rotation * Vector3.up     , mirror_normal)
        );

        if (transform.rotation.eulerAngles.x >= 270.0f &&
            transform.rotation.eulerAngles.x <= 300.0f )
        {
            transition_timer_time += Time.deltaTime;
            if(transition_timer_time > transition_timer_flag)
            {
                if(props != null)
                {
                    List<string> keys = new List<string>(decorations.Keys);
                    for ( int i = 0; i < decorations.Count; i++)
                    {
                        string key = keys[i];
                        decorations.TryGetValue(key, out Deco meshes);

                        GameObject decoration = new GameObject(key);
                        decoration.AddComponent<MeshFilter>();
                        decoration.GetComponent<MeshFilter>().mesh = meshes.mesh;
                        decoration.AddComponent<MeshRenderer>();
                        decoration.GetComponent<MeshRenderer>().material = meshes.material;
                        decoration.transform.SetParent(stand, false);
                        Instantiate(decoration, stand);
                    }
                    Destroy(props);
                }
            }
        }
        else
        {
            transition_timer_time = Mathf.Lerp(0.0f, transition_timer_time, 0.4f);
        }

        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,10.0f, ray_mask))
        {
            if(id_hit == hit.transform.GetInstanceID())
            {
                equip_timer_time += Time.deltaTime;
            }
            else
            {
                id_hit = hit.transform.GetInstanceID();
            }

            if( equip_timer_time>equip_timer_flag && 
                id_rec!=id_hit )
            {
                id_rec = id_hit;
                //hit.collider.; // <-- GameObject
                MeshFilter mesh_filter = hit.collider.GetComponent<MeshFilter>();
                MeshRenderer mesh_renderer = hit.collider.GetComponent<MeshRenderer>();
                if(mesh_filter != null)
                {
                    decorations[mesh_filter.tag] = new Deco(mesh_filter.mesh, mesh_renderer.material);
                }
            }
        }
        else
        {
            equip_timer_time= 0.0f;
            id_hit = 0;
        }
    }
}
