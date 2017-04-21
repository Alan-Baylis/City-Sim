using System;

namespace SquareTime {
    class Heap<T> where T : IHeapItem<T> {
        T[] items;
        int itemCount;
        public Heap (int maxSize){
             items = new T[maxSize];
        }
        public int Count
        {
            get
            {
                return itemCount;
            }
        }
        public void Add (T itemToAdd) {
            items[itemCount] = itemToAdd; // append the new item to the end
            itemToAdd.HeapIndex = itemCount;// set it's heap index
            sortUp(itemToAdd);//check it's parent and see if it needs to be swapped... loop until in the right place
            itemCount++;//incrememnt item count
        }
        public T RemoveTop() {
            T topItem = items[0]; // item with the lowest f cost
            itemCount--;
            items[0] = items[itemCount];//take the last item with the highest f cost and suffle to the top
            items[0].HeapIndex = 0;
            sortDown(items[0]); //check with it's children to sort it down until in the right place.
            return topItem;
        }
        void sortUp(T item) {
            //takes an item and checks with its parent to see if it is in right place (lowest f cost on top) 
            //used if not at the start
            while (true) {//infinite loop through array;
                int parentIndex = (item.HeapIndex - 1) / 2; // always works due to int division
                if( item.CompareTo(items[parentIndex]) > 0) {// if more than zero then the child should be above the parent
                    Swap(item, items[parentIndex]);//swap places, then loop to check if it is in the right place now.
                }
                else {
                    return; // if it is in the right place, no further checks required.
                }
            }
        }
        void sortDown(T item) {//takes an item and checks with it's children if it is in the right place, used if you are editing a value at the start.
            while (true) {
                int childA = item.HeapIndex * 2 + 1;
                int childB = item.HeapIndex * 2 + 2;
                int swapIndex = 0;//child that will be swapped;
                if (childA < itemCount) {//potential could be a child
                    swapIndex = childA;
                    if(childB< itemCount) {//both childs can be potential
                        if(items[childA].CompareTo(items[childB])  < 0) {// if A would come after B in a orderded list than B should be the item we check to swap 
                            swapIndex = childB;
                        }
                    }
                    //whole point is to find the child that's value is most similar to the parents
                    if(item.CompareTo(items[swapIndex]) < 0) {//should the item come after the child if so swap
                        Swap(item, items[swapIndex]);
                    }
                    else {
                        return; // in the right place.
                    }
                
                }
                else {
                    return; //cannot possibly have a child as would be out of range of array so in the right place
                }

            }
        }
        public bool Contains(T item) {
            return Equals(item, items[item.HeapIndex]);
        }
        public void UpdateItem(T item) {
            //check it contains
            if (Contains(item)) {
                sortUp(items[item.HeapIndex]);
            }
        }
        void Swap(T a, T b) {
            items[a.HeapIndex] = b;
            items[b.HeapIndex] = a;
            int tempAHeap = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = tempAHeap;
        }
    }
    public interface IHeapItem<T> : IComparable<T> {
        int HeapIndex
        {
            get; set;
        }
    }
}
