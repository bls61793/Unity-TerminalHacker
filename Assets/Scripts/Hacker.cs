using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameScreen
{
    Login, LoadAndConnect, Menu, Password, Win
}

public class Hacker : MonoBehaviour
{
    //Game Configuration
    static string[] EasyPasswords = { "Father", "Grandma", "Grandpa", "Mother", "Children", "Daughter",};
    static string[] MediumPasswords = { "Crustacean", "Cephalopod", "Hammerhead", "Wobbegong" };
    static string[] HardPasswords = {"Czech Republic", "North Korea", "Switzerland", "United Kingdom" };
    static string[] ContactNames = { "Invalid", "Tom", "Dick", "Harry" };
    const string BugCommand = "\\*TROLLENGINE Inject -f -r -a\n";

    const string EasyPassHint = "Family Ties";
    const string MedPassHint = "Sea Creature";
    const string HardPassHint = "Country";

    Terminal GameTerminal = null;
    Keyboard GameKeyboard = null;

    //Game State
    AudioSource Source = null;

    int TempIndex;
    string[] ContactPasswords = new string[ContactNames.Length];
    int ActiveContactIndex;
    string UserName = "";
    bool[] bugInstalled;
    bool isSequencePlaying = false;

    //Propetries
    public AudioClip ModemConnectSound;
    private string PassHint;
    GameScreen screen = GameScreen.Login;
    private bool isFirstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Components
        print("Initializing Components");
        Source = gameObject.GetComponent<AudioSource>();
        bugInstalled = new bool[ContactNames.Length];
        GameTerminal = gameObject.GetComponentInChildren<Terminal>();
        GameKeyboard = gameObject.GetComponentInChildren<Keyboard>();

        print("Components Initialized");

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

    IEnumerator BugMachine()
    {

        GameKeyboard.PauseControls = true;

        if (!bugInstalled[ActiveContactIndex])
        {
            while (TempIndex < BugCommand.Length)
            {
                print(TempIndex);
                GameKeyboard.PlayRandomSound();
                GameKeyboard.connectedToTerminal.ReceiveFrameInput(BugCommand.Substring(TempIndex, 1));
                TempIndex++;
                yield return new WaitForSeconds(0.08f);
            }
            print("BugInstalled For Contact Index: " + ActiveContactIndex);
            bugInstalled[ActiveContactIndex] = true;
        }
        Terminal.ClearScreen();
        Terminal.WriteLine("*TROLLENGINE: Injecting Troll Bug*");
        yield return new WaitForSeconds(3f);
        Terminal.WriteLine("***********");
        yield return new WaitForSeconds(1f);
        Terminal.WriteLine(@"

░░░░░▄▄▄▄▀▀▀▀▀▀▀▀▄▄▄▄▄▄░░░░░░░ 
░░░░░█░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░▀▀▄░░░░ 
░░░░█░░░▒▒▒▒▒▒░░░░░░░░▒▒▒░░█░░░ 
░░░█░░░░░░▄██▀▄▄░░░░░▄▄▄░░░░█░░ 
░▄▀▒▄▄▄▒░█▀▀▀▀▄▄█░░░██▄▄█░░░░█░ 
█░▒█▒▄░▀▄▄▄▀░░░░░░░░█░░░▒▒▒▒▒░█
█░▒█░█▀▄▄░░░░░█▀░░░░▀▄░░▄▀▀▀▄▒█ 
░█░▀▄░█▄░█▀▄▄░▀░▀▀░▄▄▀░░░░█░░█░ 
░░█░░░▀▄▀█▄▄░█▀▀▀▄▄▄▄▀▀█▀██░█░░ 
░░░█░░░░██░░▀█▄▄▄█▄▄█▄████░█░░░ 
░░░░█░░░░▀▀▄░█░░░█░█▀██████░█░░ 
░░░░░▀▄░░░░░▀▀▄▄▄█▄█▄█▄█▄▀░░█░░ 
░░░░░░░▀▄▄░▒▒▒▒░░░░░░░░░░▒░░░█░ 
░░░░░░░░░░▀▀▄▄░▒▒▒▒▒▒▒▒▒▒░░░░█░ 
░░░░░░░░░░░░░░▀▄▄▄▄▄░░░░░░░░█░░");
        Terminal.WriteLine("Bug Injection Successful!");
        yield return new WaitForSeconds(3f);
        Terminal.WriteLine("Covering Tracks....");
        yield return new WaitForSeconds(1f);
        Terminal.WriteLine("Escaping..");
        yield return new WaitForSeconds(1f);
        GameKeyboard.PauseControls = false;
        screen = GameScreen.Menu;
        WriteMainMenu();
        isSequencePlaying = false;
        StopAllCoroutines();
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
        if (!(screen == GameScreen.Menu) || Input == null)
        {
            return;
        }
        else { print(Input); }

        if (Input.Equals("menu", StringComparison.OrdinalIgnoreCase))
        {
            WriteMainMenu();
            return;
        }
        else if (Input.Equals("quit", StringComparison.OrdinalIgnoreCase) 
            || Input.Equals("exit", StringComparison.OrdinalIgnoreCase) 
            || Input.Equals("logoff", StringComparison.OrdinalIgnoreCase)
            || Input.Equals("shutdown", StringComparison.OrdinalIgnoreCase))
        {
            Application.Quit();
        }
        else
        {
            SetActiveContact(Input.Trim());
        }
    }

    private void SetActiveContact(string Input)
    {
        int intInput = -1;
        try
        {
            intInput = int.Parse(Input);
        }
        catch
        {
            try
            {
                intInput = int.Parse(Input.Remove(1));
            }
            catch
            {
                try
                {
                    string str = Input.TrimEnd('.');
                    str = str.TrimStart('.');
                    print(str);
                    intInput = int.Parse(str);
                }
                catch
                {
                    intInput = -1;
                }

            }
            //Terminal.ClearScreen();
        }
        //Check if our input is a valid Contact Index
        if (intInput <= ContactNames.Length && intInput > 0)
        {
            ActiveContactIndex = intInput;
            screen = GameScreen.Password;
            PromptForPassword();
        }
        else
        {
            Terminal.WriteLine("Error. Invalid Input Detected. \n Please enter one of the numbers listed above, or \"Menu\" to return to refresh \n the menu.");
        }

        switch (intInput)
        {
            case 1:
                PassHint = EasyPassHint;
                break;
            case 2:
                PassHint = MedPassHint;
                break;
            case 3:
                PassHint =  HardPassHint;
                break;
        }

    }

    //TODO Sanitize User Input
    private void ProcessPasswordGuess(string Input)
    {
        if (Input.Equals(ContactPasswords[ActiveContactIndex]))
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
        DrawWinScreen(Input);
    }

    private void DrawWinScreen(string Input)
    {
        if (!isSequencePlaying)
        {
            TempIndex = 0;
            isSequencePlaying = true;
            StartCoroutine("BugMachine");
        }
        else
        {
        }


        //WriteHostMenu(Input);
        //throw new NotImplementedException();
    }

    private void WriteHostMenu(string Input)
    {
        //WriteHostline(", "C");
    }

    void WriteMainMenu()
    {
        if (isFirstTime)
        {
            LoadContacts();
            isFirstTime = false;
        }
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello " + UserName);

        WriteContactList();
    }

    private void LoadContacts()
    {
        //TODO Implement FOREACH
        ContactPasswords[0] = "-1";
        ContactPasswords[1] = EasyPasswords[UnityEngine.Random.Range(0, EasyPasswords.Length)];
        ContactPasswords[2] = MediumPasswords[UnityEngine.Random.Range(0, MediumPasswords.Length)];
        ContactPasswords[3] = HardPasswords[UnityEngine.Random.Range(0, MediumPasswords.Length)];
        bugInstalled[0] = false;
        bugInstalled[1] = false;
        bugInstalled[2] = false;
        bugInstalled[3] = false;
    }

    private void WriteContactList()
    {
        Terminal.WriteLine("TrollEngine Found "+ContactNames.Length+" Contacts.");
        Terminal.WriteLine("Which contact do you wish to hack?");
        for (int i = 1; i < ContactNames.Length; i++)
        {
            string str = (i + ". " + ContactNames[i] + "'s PC");
            if (bugInstalled[i])
            {
                str = str + "  <PWNED>  ";
            }
            Terminal.WriteLine(str);
        }

    }

    void PromptForPassword()
    {
        Terminal.ClearScreen();
        WriteHostline("Hello " + ContactNames[ActiveContactIndex] + "!");
        WriteHostline("Please enter your password to confirm your identity");
    }

    void WriteHostline(string Message)
    {
        Terminal.WriteLine(ContactNames[ActiveContactIndex] + "'PC>" + Message);
    }

    void WriteHostline(string Message, string Directory)
    {
        Terminal.WriteLine(ContactNames[ActiveContactIndex] + "'PC\\"+Directory+">");
    }

    void WritePWordResponseScreen(bool PassCorrect)
    {
        if (PassCorrect)
        {
            Terminal.WriteLine("*TROLLENGINE: Connected to host. Type \\ESCAPE to return to contact selection*");
            WriteHostline("Hello " + ContactNames[ActiveContactIndex] + ". Welcome Home!");
            screen = GameScreen.Win;
            GameKeyboard.connectedToTerminal.ReceiveFrameInput("\n");
        }
        else
        {
            Terminal.WriteLine("*TROLLENGINE: Connected to host. \n\t Type \\ESCAPE to return to contact selection*");
            WriteHostline("Password Entered Was Incorrect. Please Check Your Input and Try Again.");
            WriteHostline("HINT: " + PassHint);
        }
        
    }
}
