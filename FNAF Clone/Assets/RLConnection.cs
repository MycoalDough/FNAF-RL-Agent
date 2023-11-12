using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using TMPro;

public class RLConnection : MonoBehaviour
{
    [Header("Socket")]
    private const string host = "127.0.0.1"; // localhost
    private const int port = 12345;
    TcpClient client;
    NetworkStream stream;
    private Thread receiveThread;
    private bool isRunning = true;

    public int test;

    [Header("Import")]
    public PowerManager powerManager;
    public DoorManager doorLeft;
    public DoorManager doorRight;
    public BonnieAI bonnieAI;
    public ChicaAI chicaAI;
    public FreddyAI freddyAI;
    public FoxyAI foxyAI;
    public TextMeshProUGUI text;

    [Header("System Imports")]
    public LightManager leftLight;
    public LightManager rightLight;
    public CameraManager cm;
    public TabletController tc;
    public Alarm alarm;

    [Header("Variables Needed")]
    public int bonnieLastSeen;
    public int chicaLastSeen;
    public int freddyLastSeen;
    public int foxyLastSeen;
    public int lastCheckedTime;

    [Header("Config")]
    public float score;
    public bool reconnect;
    private static RLConnection instance;
    public int reward;
    public int iterations;
    /*/[power, doorLeft, doorRight, bonniePos, bonnieTimer, freddyPos, freddyTimer, foxyPos, foxyTimer, chicaPos, chicaTimer, freddySFX]

    the doors determine whether they're close or open
    the pos's are their current position 
    timer's are how long has it been since you last checked on them
    freddySFX is the sound he makes when moving /*/

    public void changeTimeScale()
    {
        Time.timeScale = 3;
    }

    private void Awake()
    {
        powerManager = GameObject.FindObjectOfType<PowerManager>().GetComponent<PowerManager>();
        bonnieAI = GameObject.FindObjectOfType<BonnieAI>().GetComponent<BonnieAI>();
        chicaAI = GameObject.FindObjectOfType<ChicaAI>().GetComponent<ChicaAI>();
        foxyAI = GameObject.FindObjectOfType<FoxyAI>().GetComponent<FoxyAI>();
        freddyAI = GameObject.FindObjectOfType<FreddyAI>().GetComponent<FreddyAI>();
        cm = GameObject.FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
        tc = GameObject.FindObjectOfType<TabletController>().GetComponent<TabletController>();
        alarm = GameObject.FindObjectOfType<Alarm>().GetComponent<Alarm>();
        doorLeft = GameObject.Find("DoorButtonL").gameObject.GetComponent<DoorManager>();
        doorRight = GameObject.Find("DoorButtonR").gameObject.GetComponent<DoorManager>();
        leftLight = GameObject.Find("LightButtonL").gameObject.GetComponent<LightManager>();
        rightLight = GameObject.Find("LightButtonR").gameObject.GetComponent<LightManager>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (reconnect)
            {
                ConnectToServer();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient(host, port);
            stream = client.GetStream();

            // Start the receive thread
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e.Message}");
        }
    }

    void ReceiveData()
    {
        byte[] data = new byte[1024];
        while (isRunning)
        {
            try
            {
                int bytesRead = stream.Read(data, 0, data.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Debug.Log($"Received message from server: {message}");
                    string toSend;
                    if (message == "get_state")
                    {
                        toSend = sendInput();
                        byte[] dataToSend = Encoding.UTF8.GetBytes(toSend);
                        stream.Write(dataToSend, 0, dataToSend.Length);  // Fix the size parameter here
                    }
                    else if (message.Contains("play_step"))
                    {
                        string[] step = message.Split(':');
                        toSend = playStep(int.Parse(step[1]));
                        Debug.Log(toSend);
                        byte[] dataToSend = Encoding.UTF8.GetBytes(toSend);
                        stream.Write(dataToSend, 0, dataToSend.Length);  // Fix the size parameter here
                    }
                    if (message == "reset")
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(ReloadScene());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
            }
        }
    }

    void OnDestroy()
    {
        isRunning = false;

        // Close the stream and client when the GameObject is destroyed
        if (stream != null)
            stream.Close();

        if (client != null)
            client.Close();

        if (receiveThread != null && receiveThread.IsAlive)
            receiveThread.Join(); // Ensure the receive thread is properly terminated
    }


    private void Update()
    {
        score += Time.deltaTime;
    }

    public void runInGame(int action)
    {
        if (tc.isUsing) { UnityMainThreadDispatcher.Instance().Enqueue(change(true)); }

        switch (action)
        {
            case 0:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseZero());
                break;
            case 1:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseOne());
                break;
            case 2:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseTwo());
                break;
            case 3:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseThree());
                break;
            case 14:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseFourteen());
                break;
            case 15:
                UnityMainThreadDispatcher.Instance().Enqueue(CaseFifteen());
                break;
            default:
                break;
        }
    }

    // Coroutine for case 0
    IEnumerator CaseZero()
    {
        doorLeft.Open();
        yield return null;
    }

    // Coroutine for case 1
    IEnumerator CaseOne()
    {
        doorRight.Open();
        yield return null;
    }

    // Coroutine for case 2
    IEnumerator CaseTwo()
    {
        doorLeft.Close();
        yield return null;
    }

    // Coroutine for case 3
    IEnumerator CaseThree()
    {
        doorRight.Close();
        yield return null;
    }

    // Coroutine for case 14
    IEnumerator CaseFourteen()
    {
        leftLight.setActiveFor(1f);
        yield return null;
    }

    // Coroutine for case 15
    IEnumerator CaseFifteen()
    {
        rightLight.setActiveFor(1f);
        yield return null;
    }

    public void camera1(int action)
    {
        if (!tc.isUsing) { UnityMainThreadDispatcher.Instance().Enqueue(change(false)); }
        UnityMainThreadDispatcher.Instance().Enqueue(camChange(action));
    }

    IEnumerator change(bool b)
    {
        if (!tc.gameObject.activeInHierarchy) { yield return null; }
        else
        {
            tc.ct(b);
        }
        yield return null;
    }

    IEnumerator camChange(int b)
    {
        cm.changeCamera(b - 3);
        yield return null;
    }
    public string playStep(int action) 
    /* 0) open left
     * 1) open right
     * 2) close left
     * 3) close right
     * 4-13) cam 1-10
     * 14) light left
     * 15) light right
     * 16) do nothing
        */
    {


        reward = 0;
        bool done = false;
        bool over = false;
        if (powerManager.power != 0)
        {
            if(action > 3 && action <= 13)
            {
                camera1(action);
                over = true;

                //bonnie
                if (bonnieAI.bonnieSeen < 0.5f)
                {
                    if (bonnieAI.spot != bonnieLastSeen)
                    {
                        reward += 10;
                    }
                    else
                    {
                        reward -= 10;
                    }
                    bonnieLastSeen = bonnieAI.spot;
                }
                //chica
                if (chicaAI.bonnieSeen < 0.5f)
                {
                    if (chicaAI.spot != chicaLastSeen)
                    {
                        reward += 10;
                    }
                    else
                    {
                        reward -= 10;
                    }
                    chicaLastSeen = chicaAI.spot;
                }
                //freddy
                if (freddyAI.bonnieSeen < 0.5f)
                {
                    if (freddyAI.spot != freddyLastSeen)
                    {
                        reward += 10;
                    }
                    else
                    {
                        reward -= 10;
                    }
                    freddyLastSeen = freddyAI.spot;
                }

                if (foxyAI.foxySeen < 0.5f)
                {
                    if (foxyAI.stages != foxyLastSeen)
                    {
                        reward += 10;
                    }
                    else
                    {
                        reward -= 10;
                    }
                    foxyLastSeen = foxyAI.stages;
                }

                if(foxyAI.foxySeen > 0.5f && freddyAI.bonnieSeen > 0.5f && bonnieAI.bonnieSeen > 0.5f && chicaAI.bonnieSeen > 0.5f)
                {
                    reward -= 10;
                }
            }

            if (!over)
            {
                runInGame(action);
            }
        }

        //reward time!
        //check if animatronics were just viewed
        if(alarm.timeAlarm == 6)
        {
            reward = 80;
            done = true;
        }
        if (alarm.gameOver)
        {
            reward = -80;
            done = true;
        }
        if(powerManager.power == 0)
        {
            reward = -80;
            done = true;
        }
        else
        {
            

            //timer check
            if (alarm.timeAlarm > lastCheckedTime)
            {
                reward += 10;
            }
            lastCheckedTime = alarm.timeAlarm;

            //left door

            if (doorLeft.isClosed)
            {
                if ((doorLeft.isClosed && bonnieAI.isAtFinalDoor) || (doorLeft.isClosed && foxyAI.stages == 2))
                {
                    reward += 30;
                }
                else
                {
                    reward -= 20;
                }
            }

            //right door
            if (doorRight.isClosed)
            {
                if ((doorRight.isClosed && chicaAI.isAtFinalDoor) || (doorRight.isClosed && freddyAI.isAtFinalDoor))
                {
                    reward += 30;
                }
                else
                {
                    reward -= 20;
                }
            }

            UnityMainThreadDispatcher.Instance().Enqueue(lightRewards());
        }


        return reward + ":" + done + ":" + score;

    }

    IEnumerator lightRewards()
    {
        if (leftLight.lightGameObject.activeInHierarchy)
        {
            if ((leftLight.lightGameObject.activeInHierarchy && bonnieAI.isAtFinalDoor) || (leftLight.lightGameObject.activeInHierarchy && foxyAI.stages == 2))
            {
                reward += 10;
            }
            else
            {
                reward -= 10;
            }
        }

        //right light
        if (rightLight.lightGameObject.activeInHierarchy)
        {
            if ((rightLight.lightGameObject.activeInHierarchy && chicaAI.isAtFinalDoor))
            {
                reward += 10;
            }
            else
            {
                reward -= 10;
            }
        }
        yield return null;
    }

    public string sendInput()
    {
        string input = "";
        List<string> toAdd = new List<string>();
        toAdd.Add(alarm.timeAlarm.ToString());
        if(powerManager.power > 0)
        {
            toAdd.Add(powerManager.power.ToString());
        }
        else
        {
            toAdd.Add("0.0000");
        }
        toAdd.Add(doorLeft.isClosed.ToString());
        toAdd.Add(doorRight.isClosed.ToString());

        toAdd.Add(bonnieAI.bonnieSeen.ToString());
        toAdd.Add(bonnieAI.lastSeenSpot.ToString());
        toAdd.Add(bonnieAI.goingBack.ToString());
        toAdd.Add(bonnieAI.finalDoorCheck.ToString());

        toAdd.Add(chicaAI.bonnieSeen.ToString());
        toAdd.Add(chicaAI.lastSeenSpot.ToString());
        toAdd.Add(chicaAI.goingBack.ToString());
        toAdd.Add(chicaAI.finalDoorCheck.ToString());

        toAdd.Add(freddyAI.bonnieSeen.ToString());
        toAdd.Add(freddyAI.lastSeenSpot.ToString());
        toAdd.Add(freddyAI.goingBack.ToString());
        toAdd.Add(freddyAI.finalDoorCheck.ToString());
        toAdd.Add(freddyAI.sfx.ToString());

        toAdd.Add(foxyAI.foxySeen.ToString());
        toAdd.Add(foxyAI.stages.ToString());

        for(int i = 0; i < 18; i++)
        {
            input += toAdd[i] + ",";
        }
        input += toAdd[18];

        return input;
    }


    

    public IEnumerator ReloadScene()
    {
        // Reload the scene
        tc.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += onSceneLoaded;
        tc.gameObject.SetActive(true);
        //resetEverything();
        score = 0;
        iterations++;
        yield return null;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        powerManager = GameObject.FindObjectOfType<PowerManager>().GetComponent<PowerManager>();
        bonnieAI = GameObject.FindObjectOfType<BonnieAI>().GetComponent<BonnieAI>();
        chicaAI = GameObject.FindObjectOfType<ChicaAI>().GetComponent<ChicaAI>();
        foxyAI = GameObject.FindObjectOfType<FoxyAI>().GetComponent<FoxyAI>();
        freddyAI = GameObject.FindObjectOfType<FreddyAI>().GetComponent<FreddyAI>();
        cm = GameObject.FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
        tc = GameObject.FindObjectOfType<TabletController>().GetComponent<TabletController>();
        alarm = GameObject.FindObjectOfType<Alarm>().GetComponent<Alarm>();
        doorLeft = GameObject.Find("DoorButtonL").gameObject.GetComponent<DoorManager>();
        doorRight = GameObject.Find("DoorButtonR").gameObject.GetComponent<DoorManager>();
        leftLight = GameObject.Find("LightButtonL").gameObject.GetComponent<LightManager>();
        rightLight = GameObject.Find("LightButtonR").gameObject.GetComponent<LightManager>();
        text = GameObject.Find("NumberOfGames").gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "Iterations: " + iterations;
    }
}