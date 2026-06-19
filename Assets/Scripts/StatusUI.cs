using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowObject;
    [SerializeField] private GameObject speedDownArrowObject;
    [SerializeField] private GameObject speedLeftArrowObject;
    [SerializeField] private GameObject speedRightArrowObject;
    [SerializeField] private UnityEngine.UI.Image fuelImage;


    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {
        speedUpArrowObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        speedDownArrowObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        speedRightArrowObject.SetActive(Lander.Instance.GetSpeedX() >= 0);
        speedLeftArrowObject.SetActive(Lander.Instance.GetSpeedX() < 0);


        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();

        statsTextMesh.text =
                            GameManager.Instance.GetLevelNumber() + "\n" +
                            GameManager.Instance.GetScore() + "\n" +
                             Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
                             Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
                             Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f));
    }
}
