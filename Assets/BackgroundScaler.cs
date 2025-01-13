using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.0f; // Skalowanie mapy
    [SerializeField] private GameObject[] walls; // Referencje do œcian (obiekty z Colliderami)

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Ustawienie domyœlnej skali
        transform.localScale = Vector3.one;

        // Rozmiar Sprite'a
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        // Wysokoœæ i szerokoœæ ekranu w jednostkach œwiata
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Skalowanie t³a
        Vector3 scale = transform.localScale;
        scale.x = (worldScreenWidth / width) * scaleMultiplier;
        scale.y = (worldScreenHeight / height) * scaleMultiplier;
        transform.localScale = scale;

        // Skalowanie i pozycjonowanie œcian
        AdjustWalls(scale.x, scale.y);
    }

    private void AdjustWalls(float scaleX, float scaleY)
    {
        if (walls == null || walls.Length == 0)
        {
            Debug.LogWarning("Nie dodano œcian do BackgroundScaler.");
            return;
        }

        foreach (GameObject wall in walls)
        {
            if (wall.TryGetComponent<BoxCollider2D>(out BoxCollider2D collider))
            {
                // Skaluj i pozycjonuj œcianê w zale¿noœci od skali mapy
                Vector3 wallScale = wall.transform.localScale;
                wallScale.x *= scaleX;
                wallScale.y *= scaleY;
                wall.transform.localScale = wallScale;

                // Mo¿esz równie¿ dostosowaæ pozycjê, jeœli œciany s¹ offsetowane
                Vector3 wallPosition = wall.transform.position;
                if (wall.name == "TopWall") wallPosition.y = scaleY / 2f; // Œciana górna
                if (wall.name == "BottomWall") wallPosition.y = -scaleY / 2f; // Œciana dolna
                if (wall.name == "LeftWall") wallPosition.x = -scaleX / 2f; // Œciana lewa
                if (wall.name == "RightWall") wallPosition.x = scaleX / 2f; // Œciana prawa
                wall.transform.position = wallPosition;
            }
        }
    }
}
