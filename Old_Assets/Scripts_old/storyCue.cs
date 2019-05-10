using UnityEngine;
using System.Collections;

public class storyCue : MonoBehaviour {

    public GameObject phaseToSpawn;
    public bool cinematic = false;
    public bool loadNextPhaseAfterCine = false;
    private storyController story;
    public float cueDistance = 0f;
    private GameObject cueSphere;

    void CueSphere()
    {
        if (!cueSphere)
            cueSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        cueSphere.GetComponent<SphereCollider>().enabled = false;
        cueSphere.GetComponent<MeshRenderer>().enabled = false;
        cueSphere.transform.parent = transform;
        cueSphere.transform.localPosition = Vector3.zero;
        cueSphere.transform.localScale = cueDistance * Vector3.one;
    }
    

    void Start()
    {
        story = GameObject.FindGameObjectWithTag("Narrator").GetComponent<storyController>();
    }

    void Update()
    {
        float rangeToPlayer = Vector3.Distance(ScoreKeeper.playerAlive.transform.position, transform.position);
        if (rangeToPlayer < cueDistance)
            Cue();
        
    }

    void Cue()
    {
        int newphase = 0;

        if (phaseToSpawn)
            newphase = story.phaseid(phaseToSpawn);

        if (newphase != 0)
        {
            story.nextPhase = newphase;
            story.startCinematicIntroToPhase = cinematic;
            story.callNextPhaseAfterCinematic = story;
        }
        else
            GetComponentInChildren<ScreenPlay>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Cue();
        }
    }
}
