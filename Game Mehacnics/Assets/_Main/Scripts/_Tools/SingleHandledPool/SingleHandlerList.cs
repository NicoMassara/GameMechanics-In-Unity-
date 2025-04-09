using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Tools.SingleHandledPool
{
    public class SingleHandlerList<T> where T : IPoolable<T>
    {
        private readonly List<T> _list;
        private int _maxAvailableIndex;
        
        public bool IsEmpty => _maxAvailableIndex == -1;
        public bool IsFull => GetLastRealIndex() == _maxAvailableIndex;


        public SingleHandlerList()
        {
            _list = new List<T>();
            RestartIndex();
        }

        public SingleHandlerList(List<T> list)
        {
            _list = list;
            RestartIndex();
        }

        private int GetLastRealIndex()
        {
            return _list.Count - 1;
        }

        public void RestartIndex()
        {
            _maxAvailableIndex = _list.Count - 1;
        }
        public void IncreaseMaxIndex()
        {
            _maxAvailableIndex++;
            _maxAvailableIndex = Mathf.Clamp(_maxAvailableIndex, 0, _list.Count - 1);
        }

        public void DecreaseMaxIndex()
        {
            if(IsEmpty) return;
            
            _maxAvailableIndex--;
            _maxAvailableIndex = Mathf.Clamp(_maxAvailableIndex, -1, _list.Count - 1);
        }

        public void AddItem(T item)
        {
            if(_list.Contains(item)) return;
            
            _list.Add(item);
            IncreaseMaxIndex();

            int lastRealIndex = GetLastRealIndex();
            T tempItem = _list[_maxAvailableIndex];
            T newItem = _list[lastRealIndex];
            _list[_maxAvailableIndex] = newItem;
            _list[lastRealIndex] = tempItem;
            
            item.OnDisable += OnDisableHandler;
        }

        public void RemoveItem(T item)
        {
            if(!_list.Contains(item)) return;
            
            int itemIndex = _list.IndexOf(item);

            if (itemIndex > _maxAvailableIndex)
            {
                item.OnDisable -= OnDisableHandler;
                _list.RemoveAt(itemIndex);
            }
            else
            {
                T lastAvailableItem = _list[_maxAvailableIndex];
                T itemToRemove = _list[itemIndex];
                _list[_maxAvailableIndex] = itemToRemove;
                _list[itemIndex] = lastAvailableItem;
                DecreaseMaxIndex();
                _list.Remove(item);
                item.OnDisable -= OnDisableHandler;
            }
        }

        public T GetLastItem()
        {
            if(IsEmpty) return default;
            
            return GetItemByIndex(_maxAvailableIndex);
        }

        public T GetFirstItem()
        {
            if(IsEmpty) return default;
            
            return GetItemByIndex(0);
        }

        public T GetRandomItem()
        {
            if(IsEmpty) return default;
            
            int randomIndex = Random.Range(0, _maxAvailableIndex+1);
            
            return GetItemByIndex(randomIndex);
        }

        private T GetItemByIndex(int index)
        {
            T selectedItem = _list[index];
            T tempItem = _list[_maxAvailableIndex];
            _list[_maxAvailableIndex] = selectedItem;
            _list[index] = tempItem;
            
            selectedItem.Enable();
            
            DecreaseMaxIndex();
            
            return selectedItem;
        }

        private void OnDisableHandler(T item)
        {
            item.Disable();
            IncreaseMaxIndex();
        }
    }
}