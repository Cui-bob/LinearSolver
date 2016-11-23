using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace LinearSolver
{
    /*
     * This class is to provide some utility method to help process arrays, while also provide several 
     * constant that indicate different meanings.
     */ 
    public class util
    {

        //The constants integer that indicates the type of the function
        public const int LESSTHAN_EQUALTO = 1;
        public const int LESSTHAN = 2;
        public const int EQUALTO = 3;
        public const int LARGERTHAN = 4;
        public const int LARGERTHAN_EQUALTO = 5;

        //These constants integer are indicates the state of current processing.
        public const int FORMAT_EQUATION = 0;
        public const int FOUND_ENTERING_VARIABLE = 1;
        public const int FOUND_PIVOT_ELEMENT = 2;
        public const int CALCULATING = 3;


        /* removeFrom: To remove a element in a specified location from a given array.
         * Use geneic to make to method supports different type of inout Object.
         * Input: 
         *      T[] srcArray: The given array that an element will be removed from.
         *      T index: The specified index that the element in this location will be removed.
         * Return: A T[] array without certain element and length is shortened by 1
         */
        public static T[] removeFrom<T>(T[] srcArray, int index)
        {
            if (srcArray == null)
            {
                return null;
            }
            if (srcArray.Length <= index)
            {
                return null;
            }
            T[] newArray = new T[srcArray.Length - 1];
            for (int i = 0; i < index; i++)
            {
                newArray[i] = srcArray[i];
            }
            for (int i = index; i < newArray.Length; i++)
            {
                newArray[i] = srcArray[i + 1];
            }
            return newArray;
        }

        /* removeFrom: remove the given Object from the array.
         * Input:
         *      T[] srcArray: Source array.
         *      T srcObject: The given Object.
         * Return: a new array without the given Object, or the original array
         * if there is no such a object in the array.
         */ 
        public static T[] removeFrom<T>(T[] srcArray, T srcObject)
        {
            if (srcArray == null)
            {
                return null;
            }
            for (int i = 0; i < srcArray.Length; i++)
            {
                if (srcArray[i].Equals(srcObject))
                {
                    return removeFrom<T>(srcArray,i);
                }
            }
            return srcArray;
        }

        /* insert<T>: To insert a <T> Object to the end of a <T[]> array. 
         * Use Generic to make the methods support different type of Object.
         * Input: 
         *      T[] srcArray: The source array that the new object will be inseted to.
         *      T newData: The new objcet that will be inserted.
         * Return: A <T[]> array that contains the new data as its last element.
         */ 
        public static T[] insert<T>(T[] srcArray, T newData)
        {
            if (srcArray == null)
            {
                return default(T[]);
            }
            T[] newArray = new T[srcArray.Length + 1];
            for (int i = 0; i < srcArray.Length; i++)
            {
                newArray[i] = srcArray[i];
            }
            newArray[newArray.Length - 1] = newData;
            return newArray;
        }

    }
}
