using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject CompleteCanvas;

    public int CurrentEnemyCount;
    private int MaxEnemyCount;
    public bool GameEnd;




    public Text text;

    
    void Awake()
    {
        if(!instance)
            instance = this;

        MaxEnemyCount = enemy.transform.childCount;
        CurrentEnemyCount = 0;
        UpdateText();
    }

    void Start()
    {
        
    }

    void UpdateText()
    {
        text.text = CurrentEnemyCount.ToString() + " / " + MaxEnemyCount.ToString();
    }
    
    public void EnemyDie()
    {
        CurrentEnemyCount++;
        UpdateText();

        if(CurrentEnemyCount == MaxEnemyCount)
        {
            GameEnd = true;
            CompletePanel();
        }
    }

    void CompletePanel()
    {
        CompleteCanvas.SetActive(true);
        StartCoroutine(FadeinCoroutine());
    }

    IEnumerator FadeinCoroutine()
    {
        float curfade = 0;
        float panelfade = 0.66f;
        float buttonfade = 1.0f;
        float comtextfade = 1.0f;
        GameObject Panel = CompleteCanvas.GetComponent<Transform>().Find("Panel").gameObject;
        GameObject Button = CompleteCanvas.GetComponent<Transform>().Find("Button").gameObject;
        GameObject ComText = CompleteCanvas.GetComponent<Transform>().Find("CompleteText").gameObject;

        while (curfade < 1.0f)
        {

            curfade += 0.01f;
            if(curfade < panelfade)
                Panel.GetComponent<Image>().color = new Color(0, 0, 0, curfade);

            if(curfade < buttonfade)
                Button.GetComponent<Image>().color = new Color(0, 0, 0, curfade);

            if (curfade < comtextfade)
                ComText.GetComponent<Text>().color = new Color(0, 0, 0, curfade);

            yield return new WaitForSeconds(0.01f);
        }
    }
    
    public void Restart()
    {
        StartCoroutine(RestartCorutine());
    }

    IEnumerator RestartCorutine()
    {
        GameObject Panel = CompleteCanvas.GetComponent<Transform>().Find("Panel").gameObject;
        float curfade = Panel.GetComponent<Image>().color.a;
        GameObject Button = CompleteCanvas.GetComponent<Transform>().Find("Button").gameObject;

        Button.SetActive(false);



        while (curfade < 1.0f)
        {
            curfade += 0.01f;
            if (curfade < 1.0f)
                Panel.GetComponent<Image>().color = new Color(0, 0, 0, curfade);

            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadScene(0);
    }
}
