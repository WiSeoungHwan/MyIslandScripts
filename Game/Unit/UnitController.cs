using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyIsland
{
    // Unit 관련 관리 객체 
    public class UnitController : MonoBehaviour
    {
        #region Serialize Field
        // Ground
        [SerializeField]
        private Ground ground;
        [SerializeField]
        private UnitControl unitControl;
        #endregion

        #region Private Field
        private bool isPlayer;
        private UnitData unitData;
        #endregion

        #region Public Method
        public void UnitControllerInit(bool isPlayer, GameObject selectedTheme, MaterialInitData materialInitData, UnitInitData unitInitData)
        {
            //플레이어 진영인지
            this.isPlayer = isPlayer;
            // 그라운드 타일 및 자원 생성 
            ground.GroundInit(isPlayer, selectedTheme, materialInitData);
            // 유닛 데이터 생성 
            unitData = new UnitData()
            {
                unitLevel = unitInitData.unitLevel,
                unitIndex = unitInitData.unitIndex,
                unitHp = unitInitData.unitHp,
                unitDemage = unitInitData.unitDemage,
                unitMaterial = new UnitMaterialData()
            };
            // 유닛 위치 초기화 
            if (isPlayer)
            {
                unitControl.transform.position = ReturnMovePos(unitData.unitIndex);
            }
            // 델리게이트 설정
            unitControl.SetDelegate(Move, Collect);


        }
        #endregion

        #region Private Methods
        private void Move(int clickTileIndex)  // 플레이어가 노멀타일을 클릭하면 들어오는 함수 클릭한 타일의 인덱스가 들어옴 
        {
            if(!isNearTile(clickTileIndex)){return;}
            unitControl.transform.position = ReturnMovePos(clickTileIndex);
            unitData.unitIndex = clickTileIndex;

        }
        private void Collect(MaterialState materialState, int clickTileIndex){ // 플레이어가 자원 타일을 클릭하면 들어오는 함수 
            if(!isNearTile(clickTileIndex)){return;}
            if(ground.GetTile(clickTileIndex).TileHurt(unitData.unitDemage) == false){return;}
            switch(materialState){
                case MaterialState.WOOD:
                    unitData.unitMaterial.wood += unitData.unitDemage;
                    break;
                case MaterialState.STONE:
                    unitData.unitMaterial.stone += unitData.unitDemage;
                    break;
                case MaterialState.IRON:
                    unitData.unitMaterial.iron += unitData.unitDemage;
                    break;
                case MaterialState.ADAM:
                    unitData.unitMaterial.adam += unitData.unitDemage;
                    break;
            }
            
            Debug.Log(unitData.unitMaterial.wood);

        }
        private bool isNearTile(int clickTileIndex){
            bool isNearTile = false;
            if((unitData.unitIndex + 5) % 5 == 0){ // 왼쪽 사이드 쪽에 있었을때 예외 처리
                if(clickTileIndex == unitData.unitIndex + 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5){
                    isNearTile = true;
                }
            }else if((unitData.unitIndex + 1) % 5 == 0){ // 오른쪽 사이드에 있었을때 예외 처리
                if(clickTileIndex == unitData.unitIndex - 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5){
                    isNearTile = true;
                }
            }else{
                if(clickTileIndex == unitData.unitIndex + 1 || clickTileIndex == unitData.unitIndex - 1 || clickTileIndex == unitData.unitIndex + 5 || clickTileIndex == unitData.unitIndex - 5){
                    isNearTile = true;
                }
            }
            return isNearTile;
        }

        private Vector3 ReturnMovePos(int index){
            Vector3 tilePos = ground.GetTile(index).transform.position;
            return new Vector3(tilePos.x, 1f, tilePos.z);
        }
        #endregion
    }

}
