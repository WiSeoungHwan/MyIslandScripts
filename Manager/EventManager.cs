using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE_SINGLE {
    MATERIAL_COLLECT,
    ENEMYMATERAIL_COLLECT,
    TABLE_COUNT_CHANGE,
    TABLE_BROKEN,
    TOWER_WILL_BUILD,
    TABLE_WILL_BUILD,
    BUNKER_WILL_BUILD,
    WILL_BUILD_OFF,
    TOWER_FIRE,
    GM_FIRE,
    TILE_HIT,
    ARROW_HIT,
    GAMEOVER_UNIT_DIE,
    GAMEOVER_TIMEOUT
    
}

public class EventManager: DontDestroy<EventManager> 
{
    public delegate void OnEvent(EVENT_TYPE_SINGLE eventType, Component sender, object param = null);
    // 리스너 오브젝트 딕셔너리 or 배열
    private Dictionary<EVENT_TYPE_SINGLE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE_SINGLE, List<OnEvent>>();
   

    // 리스너 배열에 리스너 추가
    public void on(EVENT_TYPE_SINGLE eventType, OnEvent listener)
    {
        List<OnEvent> ListenList = null;

        if(Listeners.TryGetValue(eventType, out ListenList))
        {
            ListenList.Add(listener);
            return;
        }

        ListenList = new List<OnEvent>();
        ListenList.Add(listener);
        Listeners.Add(eventType, ListenList);

    }
    // 이벤트를 리스너에게 전달
    public void emit(EVENT_TYPE_SINGLE EventType, Component Sender, object Param = null)
    {
        List<OnEvent> ListenList = null;
        if (!Listeners.TryGetValue(EventType, out ListenList))
        {
            return;
        }

        for(int i = 0; i < ListenList.Count; i++)
        {
            if(!ListenList[i].Equals(null))
            {
                ListenList[i](EventType, Sender, Param);
            }
        }
    }
    public void off(EVENT_TYPE_SINGLE eventType, OnEvent target = null)
    {
        if(target == null)
        {        
            // 없으면 이벤트 제거
            Listeners.Remove(eventType);    
        }
        else
        {
            // 타겟 있으면 타겟만 제거
            List<OnEvent> ListenList = null;
            if (!Listeners.TryGetValue(eventType, out ListenList))
            {
                return;
            }
            
            for(int i = 0; i < ListenList.Count; i++)
            {
                if(!ListenList[i].Equals(null))
                {
                    Listeners[eventType].Remove(target);
                }
            }
        }
    }
    public void clear()
    {
        Listeners.Clear();
    }
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE_SINGLE, List<OnEvent>> TmpListeners = new Dictionary<EVENT_TYPE_SINGLE, List<OnEvent>>();

        foreach(KeyValuePair<EVENT_TYPE_SINGLE, List<OnEvent>> Item in Listeners)
        {
            for(int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if(Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            if(Item.Value.Count > 0)
            {
                TmpListeners.Add(Item.Key, Item.Value);
            }
        }
        Listeners = TmpListeners;
    }
    private void OnLevelFinishedLoading()
    {
        RemoveRedundancies();
    }
}