using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE {
    MATERIAL_COLLECT,
    TABLE_COUNT_CHANGE,
    TOWER_WILL_BUILD,
    TABLE_WILL_BUILD,
    BUNKER_WILL_BUILD,
    WILL_BUILD_OFF,
    TOWER_FIRE,
    GM_FIRE
    
}

public class EventManager: MonoBehaviour 
{
    // 인스턴스에 접근
    public static EventManager Instance
    {
        get { return instance; }
    }
    // 싱글턴 디자인패턴 이벤트매니저 인스턴스 내부참조
    private static EventManager instance = null;
    public delegate void OnEvent(EVENT_TYPE eventType, Component sender, object param = null);
    // 리스너 오브젝트 딕셔너리 or 배열
    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();
    void Awake () {
		if(instance == null)
        {
            // 인스턴스가 없으면 현재클래스가 인스턴스로 할당
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {   
            // 인스턴스가 있으면 현재파괴
            DestroyImmediate(gameObject);
        }
    }
    // 리스너 배열에 리스너 추가
    public void on(EVENT_TYPE eventType, OnEvent listener)
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
    public void emit(EVENT_TYPE EventType, Component Sender, object Param = null)
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
    public void off(EVENT_TYPE eventType, OnEvent target = null)
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
        Dictionary<EVENT_TYPE, List<OnEvent>> TmpListeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

        foreach(KeyValuePair<EVENT_TYPE, List<OnEvent>> Item in Listeners)
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