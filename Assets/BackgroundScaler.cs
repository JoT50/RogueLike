using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.0f; // Skalowanie mapy
    [SerializeField] private GameObject[] walls; // Referencje do �cian (obiekty z Colliderami)

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Ustawienie domy�lnej skali
        transform.localScale = Vector3.one;

        // Rozmiar Sprite'a
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        // Wysoko�� i szeroko�� ekranu w jednostkach �wiata
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Skalowanie t�a
        Vector3 scale = transform.localScale;
        scale.x = (worldScreenWidth / width) * scaleMultiplier;
        scale.y = (worldScreenHeight / height) * scaleMultiplier;
        transform.localScale = scale;

        // Skalowanie i pozycjonowanie �cian
        AdjustWalls(scale.x, scale.y);
    }

    private void AdjustWalls(float scaleX, float scaleY)
    {
        if (walls == null || walls.Length == 0)
        {
            Debug.LogWarning("Nie dodano �cian do BackgroundScaler.");
            return;
        }

        foreach (GameObject wall in walls)
        {
            if (wall.TryGetComponent<BoxCollider2D>(out BoxCollider2D collider))
            {
                // Skaluj i pozycjonuj �cian� w zale�no�ci od skali mapy
                Vector3 wallScale = wall.transform.localScale;
                wallScale.x *= scaleX;
                wallScale.y *= scaleY;
                wall.transform.localScale = wallScale;

                // Mo�esz r�wnie� dostosowa� pozycj�, je�li �ciany s� offsetowane
                Vector3 wallPosition = wall.transform.position;
                if (wall.name == "TopWall") wallPosition.y = scaleY / 2f; // �ciana g�rna
                if (wall.name == "BottomWall") wallPosition.y = -scaleY / 2f; // �ciana dolna
                if (wall.name == "LeftWall") wallPosition.x = -scaleX / 2f; // �ciana lewa
                if (wall.name == "RightWall") wallPosition.x = scaleX / 2f; // �ciana prawa
                wall.transform.position = wallPosition;
            }
        }
    }
}
