using System.Collections.Generic;
using UnityEngine;

public class CameraInteractSight : MonoBehaviour
{
    public LayerMask ray_mask;
    public GameObject head;
    public GameObject mirror;
    public Transform courtainLeft;
    public Transform courtainRight;
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
        public Vector3 scale;
        public Quaternion rotation;
        public Deco(Mesh mesh, Material material, Vector3 scale, Quaternion rotation)
        {
            this.mesh = mesh;
            this.material = material;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
    public Dictionary<string, Deco> decorations = new Dictionary<string, Deco>();

    public float transition_timer_flag;
    public float transition_timer_time= 0.0f;
    public Vector3 mirror_normal = new Vector3 (0,0,-1);
    void Update()
    {
        if (mirror.activeSelf)
        {
            courtainLeft.position = new Vector3(Mathf.Lerp(courtainLeft.position.x, -5.0f, 0.03f), courtainLeft.position.y, courtainLeft.position.z);
            courtainRight.position= new Vector3(Mathf.Lerp(courtainRight.position.x, 5.0f, 0.03f), courtainRight.position.y,courtainRight.position.z);
        }
        else
        {
            courtainLeft.position = new Vector3(Mathf.Lerp(courtainLeft.position.x, -2.5f, 0.03f), courtainLeft.position.y, courtainLeft.position.z);
            courtainRight.position= new Vector3(Mathf.Lerp(courtainRight.position.x, 2.5f, 0.03f), courtainRight.position.y,courtainRight.position.z);
        }

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
                    mirror.SetActive(true);
                    
                    head.transform.SetParent(stand, false);
                    GameObject head_itm = Instantiate(head, stand);
                    head_itm.transform.localScale = Vector3.one*0.48f;
                    
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
                        decoration.GetComponent<Transform>().localScale = meshes.scale;
                        decoration.GetComponent<Transform>().rotation = meshes.rotation;
                        decoration.transform.SetParent(stand, false);
                    }
                    Destroy(props);
                    transition_timer_time = 0.0f;
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
                Transform hit_transform = hit.transform;
                if(mesh_filter != null)
                {
                    decorations[mesh_filter.tag] = new Deco(mesh_filter.mesh, mesh_renderer.material, hit_transform.lossyScale, hit_transform.localRotation);
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
