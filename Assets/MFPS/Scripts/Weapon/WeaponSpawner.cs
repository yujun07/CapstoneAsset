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
    [SerializeField] int numberOfGunsToSpawn;
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
        if (bl_PhotonNetwork.IsMasterClient)//bl_GameManager.Instance.GameMatchState == MatchState.Starting
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                for (int i = 0; i < numberOfGunsToSpawn; i++)
                {
                    RandomGunSpawn(spawnPoint);
                }
            }
        }
    }
    private void LoadGunPrefabs()
    {
        gunPrefabs = Resources.LoadAll<GameObject>(PickUpGunsPath);
    }

    private void RandomGunSpawn(Transform spawnPoint)
    {
        int gunIndex = Random.Range(0, gunPrefabs.Length);
        GameObject selectedGunPrefab = gunPrefabs[gunIndex];

        GameObject prefabObject = PhotonNetwork.InstantiateRoomObject(PickUpGunsPath + selectedGunPrefab.name, spawnPoint.position, Quaternion.identity);
        prefabObject.transform.SetParent(parentObject);
    }
}
