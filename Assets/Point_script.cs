using UnityEngine;

public class Point_script : MonoBehaviour
{
    [Header("Current Player Stats")]
    [Tooltip("Aktualny poziom gracza")]
    public int currentLevel = 1; // Poziom gracza

    [Tooltip("Aktualne doœwiadczenie gracza")]
    public int currentExp = 0;   // Doœwiadczenie gracza

    [Tooltip("Doœwiadczenie wymagane do awansu na kolejny poziom")]
    public int expToNextLevel = 20; // Exp do nastêpnego poziomu

    [Header("Level Up Settings")]
    [Tooltip("O ile zwiêksza siê wymaganie exp na ka¿dy kolejny poziom")]
    public int expIncreasePerLevel = 10; // Wzrost wymagañ exp na poziom

    [Space]
    [Tooltip("Czy pokazywaæ debugowanie w konsoli?")]
    public bool debugMode = false;

    [Tooltip("Wartoœæ doœwiadczenia, jak¹ daje ta kulka doœwiadczenia")]
    public int expValue = 10; // Iloœæ doœwiadczenia, jak¹ gracz zbiera po dotkniêciu kulki

    [Header("Attraction Settings")]
    [Tooltip("Zasiêg przyci¹gania punktów doœwiadczenia")]
    public float attractionRange = 5f; // Zasiêg przyci¹gania
    [Tooltip("Prêdkoœæ przyci¹gania punktów do gracza")]
    public float attractionSpeed = 2f; // Prêdkoœæ przyci¹gania

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
            // Obliczamy odleg³oœæ miêdzy punktem a graczem
            float distance = Vector2.Distance(transform.position, player.position);

            // Jeœli punkt jest w zasiêgu, przyci¹gamy go
            if (distance <= attractionRange)
            {
                // Obliczamy kierunek od punktu do gracza
                Vector2 direction = (player.position - transform.position).normalized;

                // Przemieszczamy punkt w stronê gracza z okreœlon¹ prêdkoœci¹
                transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
            }
        }
    }

    // Funkcja wywo³ywana, gdy gracz wejdzie w zasiêg kulki doœwiadczenia
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Upewnij siê, ¿e gracz ma tag "Player"
        {
            Debug.Log("Gracz wszed³ w kontakt z kulk¹ doœwiadczenia!");
            PlayerLevel playerLevel = other.GetComponent<PlayerLevel>(); // Pobieramy skrypt gracza
            if (playerLevel != null)
            {
                playerLevel.AddExp(expValue); // Dodajemy doœwiadczenie
            }

            Destroy(gameObject); // Zniszczenie kulki po zebraniu
        }
    }

    // Funkcja dodawania doœwiadczenia (zostaje w Twoim skrypcie dla gracza)
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

        if (debugMode) Debug.Log($"Poziom zwiêkszony! Aktualny poziom: {currentLevel}, nastêpny poziom wymaga: {expToNextLevel} exp.");
    }
}
