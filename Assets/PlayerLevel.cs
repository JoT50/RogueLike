using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 20;
    public int expIncreasePerLevel = 10;

    public float attractionRange = 5f;
    public float attractionSpeed = 2f;
    public float collectDistance = 0.5f;
    public float yOffset = -0.5f;

    private void Update()
    {
        // Find all points in the scene
        Point_script[] points = FindObjectsByType<Point_script>(FindObjectsSortMode.None);

        foreach (Point_script point in points)
        {
            Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y + yOffset);
            float distance = Vector2.Distance(point.transform.position, targetPosition);

            if (distance <= attractionRange)
            {
                // Attract the point
                point.transform.position = Vector2.MoveTowards(
                    point.transform.position,
                    targetPosition,
                    attractionSpeed * Time.deltaTime
                );
            }

            if (distance <= collectDistance)
            {
                // Collect the point
                CollectPoint(point);
            }
        }
    }

    private void CollectPoint(Point_script point)
    {
        Debug.Log("Collected experience point!");
        AddExp(point.expValue);
        Destroy(point.gameObject);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Added experience: {amount}. Current experience: {currentExp}/{expToNextLevel}");

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

        Debug.Log($"Level Up! New level: {currentLevel}. Experience: {currentExp}/{expToNextLevel}");

        // Notify GameManager to trigger event
        GameManager.Instance.TriggerEvent();
    }
}
