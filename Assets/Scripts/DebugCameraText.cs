using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject cam_object;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        Transform transform = cam_object.GetComponent<Transform>();
        CameraInteractSight interaction = cam_object.GetComponent<CameraInteractSight>();
        text.text = string.Format(
            "position = {0}\n" +
            "rotation = {1}\n" +
            "deco size = {2}\n" +
            "deco time = {3}\n" +
            "deco list = {4}\n" +
            "trans time = {5}\n" +
            "",
            transform.position,
            transform.rotation.eulerAngles,
            interaction.decorations.Count,
            interaction.equip_timer_time,
            interaction.decorations,
            interaction.transition_timer_time);

    }
}
