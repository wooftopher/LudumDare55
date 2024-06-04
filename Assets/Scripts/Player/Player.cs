using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject summonsPrefabL1;
    [SerializeField] private GameObject summonsPrefabL2;
    [SerializeField] private GameObject summonsPrefabL3;

    [SerializeField] private float summonCooldown = 0f;
    [SerializeField] private float fusingCooldown = 5f; 

    [SerializeField] private float manaCostPerSummon = 10f; // Mana cost per summon

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float lastSummonTime;
    private float lastFusingTime;
    private PlayerMana playerMana;
    private bool isSummoning = false;
    private bool isFusing = false;
    private bool isSpellZone = false;

    public GameObject spellZonePrefab;
    public int requiredSummonsCount = 3;
    GameObject spellZone;

   private Animator anim;
   private SpriteRenderer sprRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprRenderer =GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        lastSummonTime = -summonCooldown; // To allow initial summon without cooldown
        lastFusingTime = -fusingCooldown;

        playerMana = GetComponent<PlayerMana>(); // Get the PlayerMana component attached to the player
    }

    private void Update()
    {
        if (isSummoning || isFusing)
        {
            rb.velocity = Vector2.zero; // Stop player movement
            return; // Exit the Update method to prevent further movement updates
        }
        if (spellZone != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            spellZone.transform.position = worldPosition;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isSpellZone && Time.time - lastFusingTime > fusingCooldown) {
            // Instantiate spell zone at cursor position
            isSpellZone = true;
            InstantiateSpellZone();
            Debug.Log("space pressed");
        }

        

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;

        if (Input.GetMouseButtonDown(0) && !isSpellZone)
        {
            if (Time.time - lastSummonTime > summonCooldown)
                SummonObject();
            else
                Debug.Log("Summon on cooldown!");
        }
        else if (Input.GetMouseButtonDown(0) && isSpellZone){
            CheckForSummonInSpellZone();
        }
        anim.SetBool("run", rb.velocity.magnitude != 0);
        if (rb.velocity.x > 0) {
            // Set the sprite to face right
            sprRenderer.flipX = false; // Assuming facing right by default
        }
        else if (rb.velocity.x < 0) {
        // Set the sprite to face left
        sprRenderer.flipX = true; // Flip the sprite horizontally
        }
    }

    private void SummonObject()
{
    // Check if the player has enough mana to summon
    if (playerMana.CurrentMana >= manaCostPerSummon)
    {
        if (summonsPrefabL1 != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // Perform a 2D raycast from the mouse position, checking collisions with all layers
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);

            // Iterate over all raycast hits
            foreach (RaycastHit2D hit in hits)
            {
                // Check if the raycast hits the summoning zone collider
                if (hit.collider != null && hit.collider.CompareTag("SummoningZone"))
                {
                    // Set the flag to indicate that the player is summoning
                    isSummoning = true;

                    // Instantiate the summons prefab with a slight delay
                    StartCoroutine(SummonWithDelay(worldPosition));
                    return; // Exit the function after summoning
                }
            }

            // If no summoning zone collider was hit
            Debug.Log("Cannot summon outside the designated zone!");
        }
    }
    else
    {
        Debug.Log("Not enough mana to summon!");
    }
}


    private IEnumerator SummonWithDelay(Vector3 spawnPosition)
    {
        // Wait for 1 second before summoninganim.SetBool("isCasting", true);
        anim.SetBool("isCasting", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isCasting", false);


        // Instantiate the summons prefab at the specified position
        Instantiate(summonsPrefabL1, spawnPosition, Quaternion.identity);
        lastSummonTime = Time.time; // Update last summon time

        // Reduce player's mana
        playerMana.DecreaseMana(manaCostPerSummon);

        // Reset the summoning flag to allow further movement
        isSummoning = false;
    }

    private void InstantiateSpellZone()
    {
        // Get mouse position
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Instantiate spell zone prefab at cursor position
        spellZone = Instantiate(spellZonePrefab, worldPosition, Quaternion.identity); 
    }

    private void CheckForSummonInSpellZone(){

        // Get mouse position
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Check summons inside spell zone
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPosition, spellZone.transform.localScale.x / 2f);

        // Create a list to store the first three summons found
        List<GameObject> firstThreeSummons = new List<GameObject>();
        int summonsCount = 0;

        foreach (Collider2D hit in hits)
        {
            // Check if the collider's GameObject has the name "SummonL2"
            Summon summonComponent = hit.gameObject.GetComponent<Summon>();
            if (summonComponent != null && summonComponent.level == 2)
            {
                // Add the summon to the list
                firstThreeSummons.Add(hit.gameObject);
                summonsCount++;
            }
            // If we've found three summons, break out of the loop
            if (summonsCount >= 3)
            {
                CastSpellOnSummons(firstThreeSummons.ToArray(), 3);
                break;
            }
            else {
                Debug.Log("Not enough summonL2 in spell zone");
            }
    
        }
        

        // If we haven't found enough "SummonL2" summons, check for "SummonL1"
        if (summonsCount < 3)
        {
            summonsCount = 0;
            firstThreeSummons.Clear();
            foreach (Collider2D hit in hits)
            {
                // Check if the collider's GameObject has the name "SummonL1"
                Summon summonComponent = hit.gameObject.GetComponent<Summon>();
                if (summonComponent != null && summonComponent.level == 1)
                {
                    // Add the summon to the list
                    firstThreeSummons.Add(hit.gameObject);
                    summonsCount++;
                }
                // If we've found three summons, break out of the loop
                if (summonsCount >= 3)
                {
                    CastSpellOnSummons(firstThreeSummons.ToArray(), 2);
                    break;
                }
                else {
                    Debug.Log("Not enough summon in spell zone");
                }

            }
        }
        isSpellZone = false;

        // Destroy spell zone after casting the spell
        Destroy(spellZone);
    }


    private IEnumerator Channeling(float delay, GameObject[] summons, int summonLevel)
    {
       
        // Check if the summons array is not null and contains at least three summons
        if (summons != null && summons.Length >= 3)
        {
            // Calculate the center position of the three summons
            Vector2 centerPosition = Vector2.zero;
            foreach (GameObject summon in summons)
            {
                centerPosition += (Vector2)summon.transform.position;
            }
            centerPosition /= 3f;

            if (summonLevel == 2)
                Instantiate(summonsPrefabL2, centerPosition, Quaternion.identity);
            else if (summonLevel == 3)
                Instantiate(summonsPrefabL3, centerPosition, Quaternion.identity);

            // Destroy the original three summons
            foreach (GameObject summon in summons)
            {
                Destroy(summon);
            }

            // Log a message indicating that the spell was cast on summons
            Debug.Log("Spell cast on summons!");
        }
        else
        {
            // Log a message indicating that not enough summons were found
            Debug.Log("Not enough summons found to cast spell!");
        }
        isFusing = true;
        anim.SetBool("isCasting", true);
        yield return new WaitForSeconds(delay);
        anim.SetBool("isCasting", false);
        lastFusingTime = Time.time;
        isFusing = false;
    }

    private void CastSpellOnSummons(GameObject[] summons, int summonLevel)
    {
        // Start the channeling coroutine with a delay of 1 second
        StartCoroutine(Channeling(1f, summons, summonLevel));
    }


}
