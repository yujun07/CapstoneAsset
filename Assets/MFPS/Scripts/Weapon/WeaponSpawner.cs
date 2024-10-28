using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.TextCore.Text;

public class WeaponSpawner : bl_MonoBehaviour
{
    private const string PickUpGunsPath = "PickUpGuns/";
    public Transform[] spawnPoints;
    private GameObject[] gunPrefabs;
    private Transform parentObject;
    protected override void Awake()
    {
        base.Awake();
        if(parentObject == null)
        {
           GameObject parent = new GameObject("WeaponParent");
           parentObject = parent.transform;
        }
        
        LoadGunPrefabs();
    }
    protected override void OnEnable()
    {
        if (!bl_PhotonNetwork.IsConnected) return;
        base.OnEnable();
        foreach (Transform spawnPoint in spawnPoints)
        {
            RandomGunSpawn();
        }
    }
    private void LoadGunPrefabs()
    {
        gunPrefabs = Resources.LoadAll<GameObject>(PickUpGunsPath);
    }

    private void RandomGunSpawn()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        int gunIndex = Random.Range(0, gunPrefabs.Length);
        GameObject selectedGunPrefab = gunPrefabs[gunIndex];

        GameObject prefabObject = PhotonNetwork.Instantiate(PickUpGunsPath + selectedGunPrefab.name, spawnPoint.position, Quaternion.identity);
        prefabObject.transform.SetParent(parentObject);
    }
}
