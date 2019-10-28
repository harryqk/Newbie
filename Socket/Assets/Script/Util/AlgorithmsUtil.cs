using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlgorithmsUtil
{

    public static void quickSort(ref Vector2[] a, int low, int high)
    {
        if (low >= high)
        {
            return;
        }
        int first = low, last = high;
        //此时a[low]被保存到key，所以元素a[low]可以当作是一个空位，用于保存数据，之后每赋值一次，也会有一个位置空出来，直到last==first，此时a[last]==a[first]=key
        Vector2 key = Vector2.zero;
        key.x = a[low].x;
        key.y = a[low].y;
        while (first < last)
        {
            while (first < last && bigOrEqual(a[last], key))
            {
                last--;
            }
            a[first].x = a[last].x;
            a[first].y = a[last].y;
            while (first < last && lessOrEqual(a[first], key))
            {
                first++;
            }
            a[last].x = a[first].x;
            a[last].y = a[first].y;
        }
        a[first].x = key.x;
        a[first].y = key.y;
        //递归排序数组左边的元素
        quickSort(ref a, low, first - 1);
        //递归排序右边的元素
        quickSort(ref a, first + 1, high);
    }

    static bool bigOrEqual(Vector2 a, Vector2 b)
    {
        if (a.x > b.x)
        {
            return true;
        }
        else if (a.x < b.x)
        {
            return false;
        }
        else
        {
            if (a.y > b.y)
            {
                return true;
            }
            else if (a.y < b.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    static bool lessOrEqual(Vector2 a, Vector2 b)
    {
        if (a.x > b.x)
        {
            return false;
        }
        else if (a.x < b.x)
        {
            return true;
        }
        else
        {
            if (a.y > b.y)
            {
                return false;
            }
            else if (a.y < b.y)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
    }
    public static void test()
    {
        Vector2[] a = { new Vector2(3, 3), new Vector2(2, 100), new Vector2(2, 3), new Vector2(2, 5), new Vector2(4, 20), new Vector2(11, 12), new Vector2(2, 3), new Vector2(1, 55), new Vector2(6, 7) };

        quickSort(ref a, 0, a.Length - 1);/*这里原文第三个参数要减1否则内存越界*/

        for (int i = 0; i < a.Length; i++)
        {
            if (i == a.Length - 1)
            {
                Debug.Log(a[i]);
                break;
            }
            Debug.Log(a[i]);
        }
    }


    public static void test1()
    {
        int[] a = { 57, 68, 59, 52, 72, 28, 96, 33, 24 };

        Quicksort(a, 0, a.Length - 1);/*这里原文第三个参数要减1否则内存越界*/

        for (int i = 0; i < a.Length; i++)
        {
            if (i == a.Length - 1)
            {
                Debug.Log(a[i]);
                break;
            }
            Debug.Log(a[i]);
        }
    }
    public static void Quicksort(int[] a, int low, int high)
    {
        if (low >= high)
        {
            return;
        }
        int first = low, last = high;
        //此时a[low]被保存到key，所以元素a[low]可以当作是一个空位，用于保存数据，之后每赋值一次，也会有一个位置空出来，直到last==first，此时a[last]==a[first]=key
        int key = a[low];
        while (first < last)
        {
            while (first < last && a[last] >= key)
            {
                last--;
            }
            a[first] = a[last];
            while (first < last && a[first] <= key)
            {
                first++;
            }
            a[last] = a[first];
        }
        a[first] = key;
        //递归排序数组左边的元素
        Quicksort(a, low, first - 1);
        //递归排序右边的元素
        Quicksort(a, first + 1, high);
    }
}
