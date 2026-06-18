using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Lander lander;
    private int score;


    private void Start()
    {
        lander.OnCoinPickup += Lander_OnCoinPickup;
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }
    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }
}
