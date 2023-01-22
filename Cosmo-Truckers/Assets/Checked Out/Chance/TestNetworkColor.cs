using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestNetworkColor : NetworkBehaviour
{
    [SerializeField] Color[] Colors;
    [SyncVar] Color newColor;

    [SerializeField] float speed = 5;

    public override void OnStartClient()
    {
        if (!hasAuthority) return;

        NetworkTestManager.Instance.AddPlayers(this.gameObject);

        CmdColor();
        Position();
        gameObject.name = "PlayerOBJ";
    }

    private void Update()
    {
        if (!hasAuthority) return;

        float hor = (Input.GetAxis("Horizontal") * speed) * Time.deltaTime;
        float ver = (Input.GetAxis("Vertical") * speed) * Time.deltaTime;

        transform.Translate(hor, ver, 0);
    }

    void Position()
    {
        transform.position = new Vector3(0, -12, 0);

        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            transform.position += new Vector3(3, 0, 0);
        }
    }

    public void ColorChange() => CmdColor();

    [Command(requiresAuthority = false)]
    void CmdColor()
    {
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
        newColor = newColor == new Color(0,0,0,0) ? Colors[Random.Range(0, Colors.Length)] : newColor;

        RpcColor(newColor);
    }

    [ClientRpc]
    void RpcColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
