using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static void CallPlayerDeath()
    {
        Debug.Log("Player is dead! Combat is over...");
    }

    public static void CallAllEnemiesDeath()
    {
        Debug.Log("All Enemies are gone! Combat is over...");
    }

    public static void CallSpawnEnd()
    {
        Debug.Log("Spawning is done!");
    }
}