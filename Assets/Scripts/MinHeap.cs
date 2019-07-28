using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinHeap<T> where T : IHeapItem<T>
{
    T[] items;
    int count;
    
    public MinHeap(int size) {
        items = new T[size];
    }

    public void Add(T item) {
        item.HeapIndex = count;
        items[count] = item;
        SortUp(item);
        count++;
    }

    public T Poll() {
        T head = items[0];
        count--;
        items[0] = items[count];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return head;
    }

    public bool Contains(T item) {
        return Equals(item, items[item.HeapIndex]);
    }

    public int Count {
        get {
            return count;
        }
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    void SortDown(T item) {
        while (true) {
            int left = item.HeapIndex * 2 + 1;
            int right = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (left < count) {
                swapIndex = left;
                if (right < count && items[left].CompareTo(items[right]) < 0) swapIndex = right;

                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true) {
            T parent = items[parentIndex];
            if (item.CompareTo(parent) > 0) {
                Swap(item, parent);
            } else {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap (T item1, T item2) {
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;
        int temp = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = temp;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}