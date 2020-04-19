using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFloor : MonoBehaviour
{
    public float randomizeFloorDelay = 5;
    public List<Material> tileMaterials = new List<Material>();
    public MeshRenderer floorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomizeFloor());
    }

    IEnumerator RandomizeFloor()
    {
        while(true)
        {
            List<Material> materialPool = new List<Material>(tileMaterials);
            Material[] newMaterials = new Material[4];

            for(int i=0; i<newMaterials.Length; i++)
            {
                int randomIndex = Random.Range(0, materialPool.Count);

                newMaterials[i] = materialPool[randomIndex];

                materialPool.RemoveAt(randomIndex);
            }

            floorRenderer.materials = newMaterials;

            yield return new WaitForSeconds(randomizeFloorDelay);
        }
    }
}
