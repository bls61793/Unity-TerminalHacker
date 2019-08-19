using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    AudioSource Source = null;
    public AudioClip ModemConnectSound;
    string ContactName = null;
    string ContactPassword = null;

    enum GameScreen { Login, LoadAndConnect, Menu, Password, Win }
    GameScreen screen = GameScreen.Login;

    string UserName = "";

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Components
        Source = gameObject.GetComponent<AudioSource>();

        screen = GameScreen.Login; //Set same screen in case the dfault isn't set properly.
        StartCoroutine("TrollEngineLogin");
    }

    IEnumerator TrollEngineLogin()
    {
        Terminal.WriteLine("Enter Your Username...");
        while (string.IsNullOrWhiteSpace(UserName))
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine("LoadApplication");
        yield return new WaitForEndOfFrame();
    }

    IEnumerator LoadApplication()
    {
        StopCoroutine("TrollEngineLogin");
        screen = GameScreen.LoadAndConnect;
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

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: Trim Whitespace and Reject Full White-space names.
    void OnUserInput(string Input)
    {
        if (screen == GameScreen.Login)
        {
            ProcessLoginScreenInput(Input);
        }
        else if (screen == GameScreen.LoadAndConnect)
        {
            Input = ProcessLoadandConnecShortcut(Input);
        }
        else if (screen == GameScreen.Menu)
        {
            ProcessMenuInput(Input);
        }
        else if (screen == GameScreen.Password)
        {
            ProcessPasswordGuess(Input);
        }
        else if (screen == GameScreen.Win)
        {
            ProcessWinInput(Input);
        }
        else
        {
            //INVALID GAME SCREEN. 
            //TODO: Throw Error
        }
    }

    //TODO: DEBUG Method - Remove at build
    //Determine if the "connect" shortcut command was used to skip connection Effect
    private string ProcessLoadandConnecShortcut(string Input)
    {
        if (Input.Equals("Connect", StringComparison.OrdinalIgnoreCase))
        {
            StopCoroutine("LoadApplication");
            Source.Stop();
            screen = GameScreen.Menu;
            Input = null;
            WriteMainMenu();
        }

        return Input;
    }

    private void ProcessLoginScreenInput(string Input)
    {
        UserName = Input;
        if (string.IsNullOrWhiteSpace(UserName))
        {
            Terminal.WriteLine("Please Enter a Username");
        }
        else
        {
            Terminal.WriteLine("Username Changed To: " + UserName);
        }
    }

    private void ProcessMenuInput(string Input)
    {
        if (Input == null)
        {
            //Do NOTHING
            return;
        }
        else
        {
            //print(Input);
        }
        if (!(screen == GameScreen.Menu) || Input == null)
        {
            return;
        }
        else { print(Input); }

        switch (Input.Trim())
        {
            case "Menu":
            case "menu":
                WriteMainMenu();
                break;
            case "1":
            case "1.":
                Terminal.WriteLine("Tom's PC Selected");
                ContactName = "Tom";
                screen = GameScreen.Password;
                ContactPassword = "Father";
                PromptForPassword();
                break;
            case "2":
            case "2.":
                Terminal.WriteLine("Dick's PC Selected");
                ContactName = "Dick";
                screen = GameScreen.Password;
                ContactPassword = "Scoundrel";
                PromptForPassword();
                break;
            case "3":
            case "3.":
                Terminal.WriteLine("Harry's PC Selected");
                ContactName = "Harry";
                screen = GameScreen.Password;
                ContactPassword = "Twelve Plus One";
                PromptForPassword();
                break;
            default:
                Terminal.WriteLine("Error. Invalid Input Detected. \n Please enter one of the numbers listed above, or \"Menu\" to return to refresh \n the menu.");
                break;
        }
    }

    //TODO Sanitize User Input
    private void ProcessPasswordGuess(string Input)
    {
        if (Input == ContactPassword)
        {
            Terminal.ClearScreen();
            WritePWordResponseScreen(true);
        }
        else if (Input.Equals("\\ESCAPE", StringComparison.OrdinalIgnoreCase))
        {
            screen = GameScreen.Menu;
            Input = null;
            WriteMainMenu();
        }
        else
        {
            WritePWordResponseScreen(false);
        }
    }

    private void ProcessWinInput(string Input)
    {
        if ((Input.Trim().Equals("\\ESCAPE", StringComparison.OrdinalIgnoreCase)))
        {
            screen = GameScreen.Menu;
            WriteMainMenu();
        }
    }

    void WriteMainMenu()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello " + UserName);
        Terminal.WriteLine("TrollEngine Found 3 Contacts.");
        Terminal.WriteLine("Which contact do you wish to hack?\n 1. Tom's PC \n 2. Dick's PC \n 3. Harry's PC");

    }

    void PromptForPassword()
    {
        Terminal.ClearScreen();
        WriteHostline("Hello " + ContactName + "!");
        WriteHostline("Please enter your password to confirm your identity");
    }

    void WriteHostline(string Message)
    {
        Terminal.WriteLine(ContactName + "'PC>" + Message);
    }

    void WritePWordResponseScreen(bool PassCorrect)
    {
        if (PassCorrect)
        {
            Terminal.WriteLine(ContactName + "'s PC>Hello " + ContactName + ". Welcome Home!");
            screen = GameScreen.Win;
            Terminal.WriteLine("*TROLLENGINE: Connected to host. Type \\ESCAPE to return to contact selection*");
        }
        else
        {
            Terminal.WriteLine(ContactName + "s PC>Password Entered Was Incorrect. \n*TROLLENGINE: Connected to host. Type \\ESCAPE to return to contact selection*");
        }
        
    }
}
