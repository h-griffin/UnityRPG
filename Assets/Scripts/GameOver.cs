using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// access to scenes
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string mainMenuScene;
    public string loadGameScene;


    // Start is called before the first frame update
    void Start()
    {
        // music
        AudioManager.instance.PlayBGM(4);

        // player movement
        //PlayerController.instance.gameObject.SetActive(false);
        //GameMenu.instance.gameObject.SetActive(false);
        //BattleManager.instance.gameObject.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitToMain()
    {
        //remove prefabs - essentials (restart at menu)
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        //Destroy(BattleManager.instance.gameObject);


        SceneManager.LoadScene(mainMenuScene);
    }
    public void LoadLastSave()
    {
        //remove prefabs - essentials (restart at menu)
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        //Destroy(BattleManager.instance.gameObject);
        // keep audio manager

        SceneManager.LoadScene(loadGameScene);

    }
}
