using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 5; // Num of genes.
    DNA dna;
    [SerializeField] GameObject eyes;

    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottom = false;
    bool seeTop = false;
    Vector3 startPosition;

    float timeAlive = 0; // Survived time.
    float distanceTravelled = 0;
    int crash = 0;
    bool alive = true;

    Rigidbody2D rb;
    PopulationManager pm;
    [SerializeField] float horizontalSpeed = 0.1f;

    public DNA GetDna()
    {
        return dna;
    }

    public float GetTimeAlive()
    {
        return timeAlive;
    }

    public float GetDistanceTraveled()
    {
        return distanceTravelled;
    }

    public int GetCrash()
    {
        return crash;
    }


    public void Init()
    {
        //initialise DNA
        //0 forward
        //1 upwall
        //2 downwall
        //3 normal upward

        dna = new DNA(DNALength, 200);
        transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        startPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        pm = FindObjectOfType<PopulationManager>();
    }

    // bots who crash back arrow dies..
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("dead"))
        {
            alive = false;
        }
    }

    // Bots crash.
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("top") ||
            col.gameObject.CompareTag("bottom") ||
            col.gameObject.CompareTag("upwall") ||
            col.gameObject.CompareTag("downwall"))
        {
            crash++;
        }
    }

    void Update()
    {
        if (!alive) return;

        seeUpWall = false;
        seeDownWall = false;
        seeTop = false;
        seeBottom = false;

        // Cast ray to the right (forward in 2D context)
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.right, 1.0f);

        // Visual ray for display only.
        Debug.DrawRay(eyes.transform.position, eyes.transform.right * 1.0f, Color.red); // Right ray

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "upwall")
            {
                seeUpWall = true;
            }
            else if (hit.collider.gameObject.tag == "downwall")
            {
                seeDownWall = true;
            }
        }

        // Cast ray upwards
        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f);

        // Visual ray for display only.
        Debug.DrawRay(eyes.transform.position, eyes.transform.up * 1.0f, Color.red); // Up ray

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "top")
            {
                seeTop = true;
            }
        }

        // Cast ray downwards
        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f);

        // Visual ray for display only.
        Debug.DrawRay(eyes.transform.position, -eyes.transform.up * 1.0f, Color.red); // Down ray

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "bottom")
            {
                seeBottom = true;
            }
        }

        timeAlive = pm.GetElapsed();
    }


    void FixedUpdate()
    {
        if (!alive) return;

        // read DNA
        float h = 0; // Up down force.
        float v = 1.0f; //dna.GetGene(0);

        if (seeUpWall)
        {
            h = dna.GetGene(0);
        }
        else if (seeDownWall)
        {
            h = dna.GetGene(1);
        }
        else if (seeTop)
        {
            h = dna.GetGene(2);
        }
        else if (seeBottom)
        {
            h = dna.GetGene(3);
        }
        else
        {
            h = dna.GetGene(4);
        }

        rb.AddForce(this.transform.right * v);
        rb.AddForce(this.transform.up * h * horizontalSpeed);
        distanceTravelled = Vector3.Distance(startPosition, this.transform.position);
    }
}

