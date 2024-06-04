using UnityEngine;

public class SummoningZone : MonoBehaviour
{
    private void Start()
    {
        // Get the material of the summoning zone
        Material material = GetComponent<Renderer>().material;

        // Set the rendering mode to wireframe
        material.SetInt("_Wireframe", 1);
    }
}
