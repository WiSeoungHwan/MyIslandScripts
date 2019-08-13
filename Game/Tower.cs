using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region Private Field

    private TowerState state = TowerState.none;
    private int tileIndex;
    private bool isMine;
    private Bullet bullet;

    #endregion

    #region Public Methods

    public void TowerInit(TowerState state, int tileIndex, bool isMine){
        this.state = state;
        this.tileIndex = tileIndex;
        this.isMine = isMine;
    }

    private void Fire(){
        Targeting(tileIndex);
        if(bullet){
            bullet.Fire();
        }
    }
    #endregion

    #region Private Methods

    private void Targeting(int index){
        Tile tile = GameManager.Instance.GetGroundData(isMine).tileArr[RandomNum(tileIndex)];
        CreateBullet(tile);
    }

    private void CreateBullet(Tile tile){
        GameObject ball = Instantiate(Resources.Load("CannonBall"),this.transform.position, Quaternion.identity) as GameObject;
        ball.SetActive(true);
        this.bullet = ball.AddComponent<Bullet>();
        this.bullet.BulletInit(tile);
    }

    private int RandomNum(int num){
        List<int> numList = new List<int>();
        if (num == 0 || num % 5 == 0){
            numList.Add(num + 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num + 6);
            numList.Add(num - 4);
        }else if (num == 4 || (num - 4) % 5 == 0){
            numList.Add(num - 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num - 6);
            numList.Add(num + 4);
        }else{
            numList.Add(num - 1);
            numList.Add(num + 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num - 6);
            numList.Add(num + 6);
            numList.Add(num - 4);
            numList.Add(num + 4);
        }
        Debug.Log(numList);
        for(int i  = numList.Count - 1; i >= 0; i--){
            if(numList[i] > 24 || numList[i] < 0){
                numList.RemoveAt(i);
            }
        }
        foreach(int i in numList){
            Debug.Log(i);
        }
        var randomNum = Random.Range(0,numList.Count);
        return numList[randomNum];
    }

    #endregion
}
