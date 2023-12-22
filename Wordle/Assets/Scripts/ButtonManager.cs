using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Play()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); //Asettaa savedscene tietokantaan nykyisen scenen arvon
        Time.timeScale = 1.0f; //Asettaa pelin ajaksi yksi jolloin peli toimii
        SceneManager.LoadScene(1); //Vaihtaa skenen peli skeneen
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); //Asettaa savedscene tietokantaan nykyisen scenen arvon
        Time.timeScale = 1.0f; //Asettaa pelin ajaksi yksi jolloin peli toimii
        SceneManager.LoadScene("MainMenu"); //Vaihtaa skenen mainmenu skeneen
    }

    public void Quit()
    {
        Time.timeScale = 1.0f; //Asettaa pelin ajaksi yksi jolloin peli toimii
        Debug.Log("QUIT"); //Tulostaa debug konsoliin sanan quit
        Application.Quit(); //Sulkee pelin
    }

    public void Settings()
    {
        PlayerPrefs.SetInt("SavedScene", SceneManager.GetActiveScene().buildIndex); //Asettaa savedscene tietokantaan nykyisen scenen arvon
        Time.timeScale = 1.0f; //Asettaa pelin ajaksi yksi jolloin peli toimii
        SceneManager.LoadScene("Guide"); //Vaihtaa skenen asetus skeneen
    }

    public void Back()
    {
        Time.timeScale = 1.0f; //Asettaa pelin ajaksi yksi jolloin peli toimii
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene")); //Vaihtaa skenen viimeiseksi olleeseen skeneen
    }
}