using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Collections;

public class Test : MonoBehaviour
{
    private float fDestroyTime = 2f;
    private float fTickTime;
    private int x;
    private static HubConnection connection;
    private bool isDelay;
    void Start()
    {
        SetPosition(5);
        Debug.Log("Hello World!");
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:27613/MiddleHub")
            .Build();
        connection.Closed += async (error) =>
        {
            await Task.Delay(Random.Range(0, 5) * 1000);
            await connection.StartAsync();
        };

        Connect();
        Debug.Log("Connect");
        Send(Random.Range(0, 5).ToString());
        //Send(Random.Range(0, 5).ToString());
        //Send(Random.Range(0, 5).ToString());
    }

    private async void Connect()
    {
        connection.On<string, string>("ReceiveCreateWorld", (name, message) =>
        {
            Debug.Log($"{name}: {message}");
            SetPosition(int.Parse(message));
        });

        try
        {
            await connection.StartAsync();

            Debug.Log("Connection started");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private async void Send(string msg)
    {
        try
        {
            await connection.InvokeAsync("CreateWorld", connection.GetHashCode().ToString(), msg);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void SetPosition(int x)
    {
        this.x = x;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Send(Random.Range(0, 5).ToString());
            transform.position = new Vector2(x, 0);
        }
    }
}