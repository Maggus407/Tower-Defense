using UnityEngine;

// Description: MoneyTower is a tower that gives money to the player when it is placed.
public class MoneyTower : MonoBehaviour
{
    private float countDown = 1;
    private void Update()
    {
        if (gameObject.GetComponent<Collider>().enabled)
        {
            if (countDown <= 0)
            {
                PlayerStats.Money += 1;
                countDown = 1;
            }
            else
            {
                countDown -= Time.deltaTime;
            }
        }
    }
}
