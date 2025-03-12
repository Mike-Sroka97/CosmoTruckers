using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static int GetRandomWeightedIndex(List<int> weights)
    {
        int weightSum = 0;
        //Get total sum of all weights
        for (int i = 0; i < weights.Count; i++)
            weightSum += weights[i];

        int index = 0;

        int lastIndex = weights.Count - 1;

        //Loop over 'all' weights
        while (index < lastIndex)
        {
            //If random number is less than the current index we return that index
            if (UnityEngine.Random.Range(0, weightSum) < weights[index])
                return index;

            //Remove weight from total sum
            weightSum -= weights[index++];
        }

        return index;
    }

    /// <summary>
    /// Essentially flips a coin
    /// </summary>
    /// <returns></returns>
    public static bool RandomBool()
    {
        int random = Random.Range(0, 2);
        return random == 0;
    }

    private static System.Random rng = new System.Random();
    /// <summary>
    /// Randomize list
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Randomize Array
    /// </summary>
    public static void Shuffle<T>(this T[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Find nearest parent of type with depth
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="searchingTransform"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public static T FindNearestParentOfType<T>(Transform searchingTransform, int depth = 3)
    {
        //Search parents equal to depth to find object of type
        for(int i = 0; i < depth; i++)
        {
            //checking if current parent contains desired type
            if (searchingTransform.GetComponent<T>() != null)
                return searchingTransform.GetComponent<T>();

            //parent at next depth
            searchingTransform = searchingTransform.parent;

            //parent at current depth does not exist
            if(!searchingTransform)
            {
                Debug.LogError("Depth exceeded number of parents *retard*");
                return default(T);
            }
        }

        //no objects within specified depth contain desired type
        Debug.LogError("No object of type was found within specified depth");
        return default(T);
    }
}
