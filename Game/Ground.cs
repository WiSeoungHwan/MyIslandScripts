using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Ground : MonoBehaviour
{
    // MARK: - Properties
    public List<Tile> tileArr = new List<Tile>();
    public List<TileData> tileDatas = new List<TileData>();

    // MARK: - MonoBehaviour CallBacks
    void Awake()
    {

    }
    void Start()
    {

    }

    // MARK: - Private Methods

    private Tile TileInstantiate(Vector3 pos)
    {
        GameObject tileObject = Instantiate(Resources.Load("Tile/tile"), pos, Quaternion.identity) as GameObject;
        var tile = tileObject.GetComponent<Tile>();
        tileObject.transform.parent = gameObject.transform;
        tileObject.SetActive(true);
        return tile;
    }

    // MARK: - Public Methods


    public void InitGround(bool isMine)
    {
        tileDatas = GroundTileDataInit(isMine);
    }

    public List<TileData> GroundTileDataInit(bool isMine)
    {
        var index = 0;
        // 초기에 주어지는 자원의 숫자 
        var materialInitNum = ConstData.materialInitNum;
        List<TileData> tileDatas = new List<TileData>();
        for (int i = 0; i < ConstData.mapSize; i++)
        {
            for (int j = 0; j < ConstData.mapSize; j++)
            {
                TileData tileData = new TileData();
                tileData.isMine = isMine;
                tileData.index = index;
                Vector3 pos = new Vector3(i + transform.position.x, gameObject.transform.position.y, j);
                var tile = TileInstantiate(pos);
                // 플레이어가 고립되지 않을 수 있는 영역
                if (index == 1 || index == 2 || index == 3 || index == 5 || index == 10 || index == 15 || index == 9 || index == 14 || index == 19 || index == 21 || index == 22 || index == 23)
                {
                    // 자원 상태 생성 
                    if (materialInitNum > 0)
                    {
                        if (index > 14)
                        {
                            tileData.tileState = TileState.material;
                        }
                        else
                        {
                            // 초기에 주어지는 자원의 숫자만큼 랜덤 생성
                            tileData.tileState = (TileState)Random.Range(0, 2);
                        }

                    }
                }
                else
                {
                    tileData.tileState = TileState.normal;
                }

                // 랜덤으로 생성된 것 중 자원 타일 이라면 
                if (tileData.tileState == TileState.material)
                {
                    materialInitNum--;
                    // 프로토타입용: 나무데이터를 넣는다 (임시)
                    tileData.materialState = MaterialState.wood;
					tileData.hp = ConstData.materialHp;
                    // 본게임: 자원마다 정해지는 양에서 랜덤 넣기 
					// 본게임: 어떤 자원인지 분기 처리 후 체력도 분기 처리 필요 
                    // 미구현
                }
				
                tile.TileSetup(tileData);
				tileArr.Add(tile);
                tileDatas.Add(tileData);
                index++;
            }
        }
        return tileDatas;
    }
}
