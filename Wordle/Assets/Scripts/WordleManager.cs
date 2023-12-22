using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordleManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI WLText;
    [SerializeField] public TextMeshProUGUI wordWasText;
    [SerializeField] public TextMeshProUGUI cantBeEmptyText;

    public TextMeshProUGUI[] tekstit = { };

    public Image[] buttons = { };
    public Image[] wrong = { };
    public Image[] right = { };
    public Image[] almost = { };

    public GameObject[] wrongButtons = { };
    public GameObject[] almostButtons = { };
    public GameObject[] rightButtons = { };
    public GameObject[] uusiButton = { };

    private string[] sanoja =
    {
           "suomi", "pilvi", "kirja", "pieru", "koulu", "kieli", "ruoka", "sauna", "paino", "varas", "vaimo", "lintu", "nielu", "mieli", "sielu", "riepu", "lippu", "laiva", "löyhä", "tikku", "pidin",
           "vessa", "jetti", "sokea", "lääke", "kuori", "hiiri", "tieto", "riita", "vuori", "muori", "tuoli", "pöytä", "urhea", "ammus", "sähkö", "vaate", "pallo", "marsu", "lintu", "hauki", "ahven",
           "järvi", "muuri", "torni", "ratsu", "hirvi", "karhu", "jousi", "jarru", "kuusi", "viisi", "neljä", "kolme", "kaksi", "silmä", "kauhu", "kirje", "velli", "selli", "saari", "varis", "lokki",
           "tuuri", "monni", "koodi", "arkku", "palli", "kynsi", "sormi", "korva", "huono", "paras", "jänis", "kettu", "pukki", "joulu", "syksy", "kevät", "vuosi", "kylmä", "kuuma", "apina", "huone",
           "paita", "märkä", "matto", "kenkä", "kukka", "pyörä", "syödä", "lyödä", "myydä"
    };

    private char[,] arvatutKirjaimet =  {
        { ' ', ' ', ' ', ' ', ' '},
        { ' ', ' ', ' ', ' ', ' '},
        { ' ', ' ', ' ', ' ', ' '},
        { ' ', ' ', ' ', ' ', ' '},
        { ' ', ' ', ' ', ' ', ' '},
        { ' ', ' ', ' ', ' ', ' '}
    };

    private char[] tarkasteleArvattavaaSanaa = new char[5];


    string stringiarvo;
    string arvattavaSana = "";

    char currentLetter;

    bool notMatchedWords = false;
    bool cantRemove = false;
    bool canContinue = true;

    int count = 5;
    int count2 = 5;
    int count3 = 5;
    int inttiarvo;
    int zero = 0;
    int fakeZero = 0;
    int lives = 6;
    int row = 0;
    int restartGame = 0;

    public void Start()
    {
        arvattavaSana = sanoja[Random.Range(0, sanoja.Length)];
        Debug.Log("Arvattava sana on: " + arvattavaSana);
        uusiButton[0].SetActive(false);
        WLText.text = "";
        cantBeEmptyText.text = "";
        wordWasText.text = "";
        restartGame = 0;
        cantRemove = false;
        var lhines = arvattavaSana.Split(new char[] { '\n' }); //Splittaa arvattavan sanan kirjaimiksi, jotta niitä voidaan vertailla
        foreach (string lhine in lhines) //Lisää kirjaimet listaan
        {
            int num = 0;
            foreach (char khirjain in lhine)
            {
                tarkasteleArvattavaaSanaa[num] = khirjain;
                num++;
            }
        }
        
    }
    private void Update()
    {
       if (lives == 0)
        {
            uusiButton[0].SetActive(true);
            restartGame = 1;
            WLText.text = "Hävisit!";
            wordWasText.text = "Sana oli: " + arvattavaSana.ToUpper();
        }
    }

    public void updateRow()
    {
        row = whatRow(lives); //Määrittää rivin elämien perusteella 6  elämää = rivi 0
        for (int i = 0; i < 5; i++)
        {
            if (arvatutKirjaimet[row, i] == ' ')
            {
                arvatutKirjaimet[row, i] = currentLetter;
                tekstit[i + fakeZero].text = arvatutKirjaimet[row, i].ToString().ToUpper();
                break;
            }
        }
    }

    public void removeLetter()
    {
        count3 = 5;
        row = whatRow(lives); //Määrittää rivin elämien perusteella 6  elämää = rivi 0

        if(!cantRemove)
        {
            if (arvatutKirjaimet[0, 0] == ' ')
            {
            }

            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (arvatutKirjaimet[row, i] == ' ')
                    {
                        arvatutKirjaimet[row, i - 1] = currentLetter;
                        tekstit[i - 1 + fakeZero].text = arvatutKirjaimet[row, i - 1].ToString().ToUpper();
                        count3--;
                    }

                }
            }
        }
        
        

        if (count3 == 5 && !cantRemove)
        {
            arvatutKirjaimet[row, 4] = currentLetter;
            tekstit[4 + fakeZero].text = arvatutKirjaimet[row, 4].ToString().ToUpper();
        }

    }

    public void check()
    {
        if (lives != 0)
        {
            count = 5;
            count2 = 5;
            for (int i = 0; i < 5; i++) // tarkistaa onko sana samat
            {
                row = whatRow(lives);
                if (tarkasteleArvattavaaSanaa[i] != arvatutKirjaimet[row, i]) //Kirjain ei ole sanassa
                {
                    notMatchedWords = true;
                    count--;
                }

                else if(count == 5)
                {
                    notMatchedWords = false;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (arvatutKirjaimet[row, i] == ' ')
                {
                    cantBeEmptyText.text = "Ruutu ei voi olla tyhjä!";
                    count2--;
                    canContinue = false;
                }

                else if (count2 == 5)
                {
                    cantBeEmptyText.text = "";
                    canContinue = true;
                }
            }

            if (canContinue)
            {
                if (!notMatchedWords) //Sana oli oikea (voitto)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        tekstit[j + zero].text = arvatutKirjaimet[row, j].ToString().ToUpper();
                        right[j + zero].gameObject.SetActive(true);

                        //Muuttaa buttonien värit
                        stringiarvo = buttonColor(arvatutKirjaimet[row, j].ToString());
                        inttiarvo = int.Parse(stringiarvo);
                        rightButtons[inttiarvo].SetActive(true);
                    }
                    WLText.text = "Voitit!";
                    uusiButton[0].SetActive(true);
                    restartGame = 1;
                    cantRemove = true;
                }

                else //if (notMatchedWords) //Sana ei ollut oikea (väärin)
                {
                    row = whatRow(lives); //Määrittää rivin elämien perusteella 6  elämää = rivi 0
                    lives--; // miinustaa elämän

                    for (int j = 0; j < 5; j++) //
                    {
                        bool isCorrectLetter = tarkasteleArvattavaaSanaa[j] == arvatutKirjaimet[row, j]; //On true jos arvatutkirjaimet kohdan kirjain on sama kuin arvatunsanan 

                        if (isCorrectLetter) //Jos arvatut kirjaimet kohdan kirjain on sama kuin arvatunsanan kirjain
                        {
                            tekstit[j + zero].text = arvatutKirjaimet[row, j].ToString().ToUpper();
                            right[j + zero].gameObject.SetActive(true);                      
                        }

                        else if (arvattavaSana.Contains(arvatutKirjaimet[row, j]))
                        {
                            tekstit[j + zero].text = arvatutKirjaimet[row, j].ToString().ToUpper();
                            almost[j + zero].gameObject.SetActive(true);
                        }

                        else
                        {
                            if (row == 5)
                            {
                                tekstit[j + zero].text = arvatutKirjaimet[row, j].ToString().ToUpper();
                                wrong[j + zero].gameObject.SetActive(true);
                            }
                            else
                            {
                                tekstit[j + zero].text = arvatutKirjaimet[row, j].ToString().ToUpper();
                                wrong[j + zero].gameObject.SetActive(true);
                            }

                        }

                    }

                    //Muuttaa buttonien värit
                    for (int i = 0; i < 5; i++)
                    {
                        bool isCorrectLetter = tarkasteleArvattavaaSanaa[i] == arvatutKirjaimet[row, i];

                        if (arvattavaSana.Contains(arvatutKirjaimet[row, i]))
                        {
                            //Oranssi
                            stringiarvo = buttonColor(arvatutKirjaimet[row, i].ToString());
                            inttiarvo = int.Parse(stringiarvo);
                            almostButtons[inttiarvo].SetActive(true);
                        }

                        if(isCorrectLetter){
                            //Vihriä
                            stringiarvo = buttonColor(arvatutKirjaimet[row, i].ToString());
                            inttiarvo = int.Parse(stringiarvo);
                            rightButtons[inttiarvo].SetActive(true);
                        }

                        if (!isCorrectLetter && !arvattavaSana.Contains(arvatutKirjaimet[row, i]))
                        {
                            //Harmee
                            stringiarvo = buttonColor(arvatutKirjaimet[row, i].ToString());
                            inttiarvo = int.Parse(stringiarvo);
                            wrongButtons[inttiarvo].SetActive(true);
                        }


                    }
                    zero = zero + 5;
                    fakeZero = fakeZero + 5;
                }
            }
            
        }

        else
        {
            uusiButton[0].SetActive(true);
            restartGame = 1;
            cantRemove = true;
            WLText.text = "Hävisit!";
            wordWasText.text = "Sana oli: " + arvattavaSana.ToUpper();
        }
    }

    public string buttonColor(string key)
    {
        if (key == "q")
        {
            return "0";
        }
        if (key == "w")
        {
            return "1";
        }
        if (key == "e")
        {
            return "2";
        }
        if (key == "r")
        {
            return "3";
        }
        if (key == "t")
        {
            return "4";
        }
        if (key == "y")
        {
            return "5";
        }
        if (key == "u")
        {
            return "6";
        }
        if (key == "i")
        {
            return "7";
        }
        if (key == "o")
        {
            return "8";
        }
        if (key == "p")
        {
            return "9";
        }
        if (key == "a")
        {
            return "10";
        }
        if (key == "s")
        {
            return "11";
        }
        if (key == "d")
        {
            return "12";
        }
        if (key == "f")
        {
            return "13";
        }
        if (key == "g")
        {
            return "14";
        }
        if (key == "h")
        {
            return "15";
        }
        if (key == "j")
        {
            return "16";
        }
        if (key == "k")
        {
            return "17";
        }
        if (key == "l")
        {
            return "18";
        }
        if (key == "ö")
        {
            return "19";
        }
        if (key == "ä")
        {
            return "20";
        }
        if (key == "z")
        {
            return "21";
        }
        if (key == "x")
        {
            return "22";
        }
        if (key == "c")
        {
            return "23";
        }
        if (key == "v")
        {
            return "24";
        }
        if (key == "b")
        {
            return "25";
        }
        if (key == "n")
        {
            return "26";
        }

        if (key == "m")
        {
            return "27";
        }

        else
        {
            return "";
        }
    }


    public void arvausButton()
    {
        if(restartGame == 0)
        {
            WLText.text = "";
            check();
        }

        else if(restartGame == 1)
        {
            SceneManager.LoadScene(1);
        }
        
    }
    
    public int whatRow(int lives)//Funktio yrityksien muuttamisesta, mihin kohtaan listaa kirjaimet tallennetaan.
    {
        if (lives == 6)
        {
            return 0;
        }

        if (lives == 5)
        {
            return 1;
        }

        if (lives == 4)
        {
            return 2;
        }

        if (lives == 3)
        {
            return 3;
        }

        if (lives == 2)
        {
            return 4;
        }

        else
        {
            return 5;
        }
    }


    public void QKey()
    {
        currentLetter = 'q';
        updateRow();
    }
    public void WKey()
    {
        currentLetter = 'w';
        updateRow();
    }
    public void EKey()
    {
        currentLetter = 'e';
        updateRow();
    }
    public void RKey()
    {
        currentLetter = 'r';
        updateRow();
    }
    public void TKey()
    {
        currentLetter = 't';
        updateRow();
    }
    public void YKey()
    {
        currentLetter = 'y';
        updateRow();
    }
    public void UKey()
    {
        currentLetter = 'u';
        updateRow();
    }
    public void IKey()
    {
    
        currentLetter = 'i';
        updateRow();
    }
    public void OKey()
    {
        currentLetter = 'o';
        updateRow();
    }
    public void PKey()
    {
        currentLetter = 'p';
        updateRow();
    }
    public void AKey()
    {
        currentLetter = 'a';
        updateRow();
    }
    public void SKey()
    {
        currentLetter = 's';
        updateRow();
    }
    public void DKey()
    {
        currentLetter = 'd';
        updateRow();
    }
    public void FKey()
    {
        currentLetter = 'f';
        updateRow();
    }
    public void GKey()
    {
        currentLetter = 'g';
        updateRow();
    }
    public void HKey()
    {
        currentLetter = 'h';
        updateRow();
    }
    public void JKey()
    {
        currentLetter = 'j';
        updateRow();
    }
    public void KKey()
    {
        currentLetter = 'k';
        updateRow();
    }
    public void LKey()
    {
        currentLetter = 'l';
        updateRow();
    }
    public void ÖKey()
    {
        currentLetter = 'ö';
        updateRow();
    }
    public void ÄKey()
    {
        currentLetter = 'ä';
        updateRow();
    }
    public void ZKey()
    {
        currentLetter = 'z';
        updateRow();
    }
    public void XKey()
    {
        currentLetter = 'x';
        updateRow();
    }
    public void CKey()
    {
        currentLetter = 'c';
        updateRow();
    }
    public void VKey()
    {
        currentLetter = 'v';
        updateRow();
    }
    public void BKey()
    {
        currentLetter = 'b';
        updateRow();
    }
    public void NKey()
    {
        currentLetter = 'n';
        updateRow();
    }
    public void MKey()
    {
        currentLetter = 'm';
        updateRow();
    }

    public void PoistaKey()
    {
        Debug.Log("Poista");
        currentLetter = ' ';
        removeLetter();
    }

}
