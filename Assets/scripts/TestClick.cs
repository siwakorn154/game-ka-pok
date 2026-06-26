using UnityEngine;
using UnityEngine.UI;

public class TestClick : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Button ทำงาน!");
        });
    }
}