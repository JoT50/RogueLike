using UnityEngine;

public class Point_script : MonoBehaviour
{
    [Header("Current Player Stats")]
    [Tooltip("Aktualny poziom gracza")]
    public int currentLevel = 1; // Poziom gracza

    [Tooltip("Aktualne do�wiadczenie gracza")]
    public int currentExp = 0;   // Do�wiadczenie gracza

    [Tooltip("Do�wiadczenie wymagane do awansu na kolejny poziom")]
    public int expToNextLevel = 20; // Exp do nast�pnego poziomu

    [Header("Level Up Settings")]
    [Tooltip("O ile zwi�ksza si� wymaganie exp na ka�dy kolejny poziom")]
    public int expIncreasePerLevel = 10; // Wzrost wymaga� exp na poziom

    [Space]
    [Tooltip("Czy pokazywa� debugowanie w konsoli?")]
    public bool debugMode = false;

    [Tooltip("Warto�� do�wiadczenia, jak� daje ta kulka do�wiadczenia")]
    public int expValue = 10; // Ilo�� do�wiadczenia, jak� gracz zbiera po dotkni�ciu kulki

    [Header("Attraction Settings")]
    [Tooltip("Zasi�g przyci�gania punkt�w do�wiadczenia")]
    public float attractionRange = 5f; // Zasi�g przyci�gania
    [Tooltip("Pr�dko�� przyci�gania punkt�w do gracza")]
    public float attractionSpeed = 2f; // Pr�dko�� przyci�gania

    private Transform player; // Transform gracza

    void Start()
    {
        // Szukamy gracza w scenie
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Obliczamy odleg�o�� mi�dzy punktem a graczem
            float distance = Vector2.Distance(transform.position, player.position);

            // Je�li punkt jest w zasi�gu, przyci�gamy go
            if (distance <= attractionRange)
            {
                // Obliczamy kierunek od punktu do gracza
                Vector2 direction = (player.position - transform.position).normalized;

                // Przemieszczamy punkt w stron� gracza z okre�lon� pr�dko�ci�
                transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
            }
        }
    }

    // Funkcja wywo�ywana, gdy gracz wejdzie w zasi�g kulki do�wiadczenia
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Upewnij si�, �e gracz ma tag "Player"
        {
            Debug.Log("Gracz wszed� w kontakt z kulk� do�wiadczenia!");
            PlayerLevel playerLevel = other.GetComponent<PlayerLevel>(); // Pobieramy skrypt gracza
            if (playerLevel != null)
            {
                playerLevel.AddExp(expValue); // Dodajemy do�wiadczenie
            }

            Destroy(gameObject); // Zniszczenie kulki po zebraniu
        }
    }

    // Funkcja dodawania do�wiadczenia (zostaje w Twoim skrypcie dla gracza)
    public void AddExp(int amount)
    {
        currentExp += amount;
        if (debugMode) Debug.Log($"Dodano exp: {amount}. Aktualny exp: {currentExp}/{expToNextLevel}");

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel += expIncreasePerLevel;

        if (debugMode) Debug.Log($"Poziom zwi�kszony! Aktualny poziom: {currentLevel}, nast�pny poziom wymaga: {expToNextLevel} exp.");
    }
}
