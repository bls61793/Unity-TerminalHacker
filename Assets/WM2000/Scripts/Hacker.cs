using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    AudioSource Source = null;
    public AudioClip ModemConnectSound;

    enum GameScreen { Login, LoadAndConnect, Menu, Password, Win }
    GameScreen screen = GameScreen.Login;
    int ContactDifficultySelection = -1;


    string UserName = "";

    // Start is called before the first frame update
    void Start()
    {
        Source = gameObject.GetComponent<AudioSource>();
        StartCoroutine("TrollEngineLogin");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnUserInput(string Input)
    {
        if (screen == GameScreen.Login)
        {
            UserName = Input;
            Terminal.WriteLine("Username Changed To: " + UserName);
        }
        if (screen == GameScreen.LoadAndConnect)
        {
            if (Input == "connect")
            {
                StopCoroutine("LoadApplication");
                Source.Stop();
                screen = GameScreen.Menu;
                WriteMainMenu();
            }
        }
        if (screen == GameScreen.Menu)
        {
            ProcessMenuInput(Input);
        }


    }

    private void ProcessMenuInput(string Input)
    {
        if (Input == "1")
        {
            ContactDifficultySelection = 1;
            Terminal.WriteLine("Tom's PC Selected");
            screen = GameScreen.Password;
        }
        else if (Input == "2")
        {
            ContactDifficultySelection = 2;
            Terminal.WriteLine("Dick's PC Selected");
            screen = GameScreen.Password;
        }
        else if (Input == "3")
        {
            Terminal.WriteLine("Harry's PC Selected");
            screen = GameScreen.Password;
        }
        else if (Input == "Menu" || Input == "menu")
        {
            ContactDifficultySelection = 0;
            WriteMainMenu();
        }
        else
        {
            Terminal.WriteLine("Error. Invalid Input Detected. \n Please enter one of the numbers listed above, or \"Menu\" to return to refresh \n the menu.");
        }
    }

    IEnumerator TrollEngineLogin()
    {
        Terminal.WriteLine("Enter Your Username...");
        while (UserName.Equals(""))
        {
            yield return new WaitForSeconds(1f);
        }
        screen = GameScreen.LoadAndConnect;
        StartCoroutine("LoadApplication");
        yield return new WaitForEndOfFrame();
    }

    IEnumerator LoadApplication()
    {
        StopCoroutine("TrollEngineLogin");
        Terminal.WriteLine("Starting TrollEngine...");
        yield return new WaitForSeconds(5.0f);
        Terminal.WriteLine("Loading Contact Names \n and IP Addresses...");
        yield return new WaitForSeconds(1.0f);
        Terminal.WriteLine("Establishing Connection...");
        Source.PlayOneShot(ModemConnectSound, 0.2f);
        yield return new WaitForSeconds(30.0f);
        Terminal.WriteLine("Connection Established");
        yield return new WaitForSeconds(5.0f);
        screen = GameScreen.Menu;
        WriteMainMenu();

    }


    void WriteMainMenu()
    {
        ContactDifficultySelection = 0;
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello " + UserName);
        Terminal.WriteLine("TrollEngine Found 3 Contacts.");
        Terminal.WriteLine("Which contact do you wish to hack?\n 1. Tom's PC \n 2. Dick's PC \n 3. Harry's PC");

    }

}
