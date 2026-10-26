using TMPro;
using UnityEngine;

public class CoinCherecter : WalletManager
{
    [SerializeField] public static int coinValue = 1;

    public float rotationSpeed = 100f;



    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddScore();
            Destroy(gameObject);
        }
    }
    private void AddScore()
    {
        Debug.Log("popal");
        CurrentNumber += coinValue;
        UpdateText();

    }
    private void UpdateText()
    {
        _balls.text = CurrentNumber.ToString();
    }

}
