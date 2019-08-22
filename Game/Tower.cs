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
    private Arrow arrow;
    private Scope scope;

    #endregion

    #region Public Methods

    public void TowerInit(TowerState state, int tileIndex, bool isMine)
    {
        this.state = state;
        this.tileIndex = tileIndex;
        this.isMine = isMine;
    }

    private void Fire()
    {
        Targeting(tileIndex);
        if (bullet)
        {
            bullet.Fire();
        }
        if (arrow)
        {
            arrow.Fire();
        }

    }
    #endregion

    #region Private Methods

    private void Targeting(int index)
    {
        Tile tile;
        if (state == TowerState.parabola)
        {
            tile = GameManager.Instance.GetGroundData(isMine).tileArr[RandomNum(tileIndex)];
        }
        else if (state == TowerState.straight)
        {
            tile = isMine ? GameManager.Instance.GetGroundData(isMine).tileArr[tileIndex % 5] : GameManager.Instance.GetGroundData(isMine).tileArr[(tileIndex % 5) + 20];
        }
        else
        {
            tile = GameManager.Instance.GetGroundData(isMine).tileArr[RandomNum(tileIndex)];
        }
        tile.TileTargeting(true);
        CreateBullet(tile);
    }

    private void CreateBullet(Tile tile)
    {
        switch (state)
        {
            case TowerState.parabola:
                GameObject ball = Instantiate(Resources.Load("CannonBall"), this.transform.position, Quaternion.identity) as GameObject;
                ball.SetActive(true);
                this.bullet = ball.AddComponent<Bullet>();
                this.bullet.BulletInit(tile);
                break;
            case TowerState.straight:
                GameObject arrow = Instantiate(Resources.Load("Arrow"), new Vector3(this.transform.position.x,0.5f,this.transform.position.z), Quaternion.identity) as GameObject;
                arrow.SetActive(true);
                this.arrow = arrow.GetComponent<Arrow>();
                if (isMine)
                {
                    this.arrow.transform.Rotate(0, 90, 0);
                }
                else
                {
                    this.arrow.transform.Rotate(0, -90f, 0);
                }
                List<Tile> routes = new List<Tile>();
                for (int i = 0; i < 5; ++i)
                {
                    Tile route;
                    if(isMine){
                        route = GameManager.Instance.GetGroundData(isMine).tileArr[tile.tileData.index + 5 * i];
                    }else{
                        route = GameManager.Instance.GetGroundData(isMine).tileArr[(tileIndex % 5) + 5 * i];
                    }
                    route.TileTargeting(true);
                    routes.Add(route);
                }
                this.arrow.TargetSetup(tile, routes);
                break;
            case TowerState.scope:
                GameObject scope = Instantiate(Resources.Load("Scope"), new Vector3(this.transform.position.x,0.5f,this.transform.position.z), Quaternion.identity) as GameObject;
                scope.SetActive(true);
                this.scope = scope.AddComponent<Scope>();
                if (isMine)
                {
                    this.scope.transform.Rotate(0, 90, 0);
                }
                else
                {
                    this.scope.transform.Rotate(0, -90f, 0);
                }
                List<Tile> scopes = new List<Tile>();
                //TODO: - Scope 범위 타일 지정 
                this.scope.ScopeInit(tile, scopes);

                break;
        }

    }

    private int RandomNum(int num)
    {
        List<int> numList = new List<int>();
        if (num == 0 || num % 5 == 0)
        {
            numList.Add(num + 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num + 6);
            numList.Add(num - 4);
        }
        else if (num == 4 || (num - 4) % 5 == 0)
        {
            numList.Add(num - 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num - 6);
            numList.Add(num + 4);
        }
        else
        {
            numList.Add(num - 1);
            numList.Add(num + 1);
            numList.Add(num - 5);
            numList.Add(num + 5);
            numList.Add(num - 6);
            numList.Add(num + 6);
            numList.Add(num - 4);
            numList.Add(num + 4);
        }
        numList.Add(num);
        for (int i = numList.Count - 1; i >= 0; i--)
        {
            if (numList[i] > 24 || numList[i] < 0)
            {
                numList.RemoveAt(i);
            }
        }
        var randomNum = Random.Range(0, numList.Count);
        return numList[randomNum];
    }

    #endregion
}
