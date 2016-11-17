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
        public const int FOUND_ENTERING_VARIABLE = 1;



        /* insert: To insert a new double to a existing array and extend its length by 1.
         * Input: srcArray - The source array that will be inserted in.  newData - the double number that will be inserted.
         * Return: the new array that contains the new double as its last element.
         */
        public static double[] insert(double[] srcArray, double newData)
        {
            if (srcArray == null)
            {
                return null;
            }
            double[] newArray = new double[srcArray.Length + 1];
            for (int i = 0; i < srcArray.Length; i++)
            {
                newArray[i] = srcArray[i];
            }
            newArray[newArray.Length - 1] = newData;
            return newArray;
        }

        /* insert: To insert a new double to a existing array and extend its length by 1.
         * Input: srcArray - The source array that will be inserted in.  newData - the double number that will be inserted.
         * Return: the new array that contains the new double as its last element.
         */
        public static Equation[] insert(Equation[] srcArray, Equation newData)
        {
            if (srcArray == null)
            {
                return null;
            }
            Equation[] newArray = new Equation[srcArray.Length + 1];
            for (int i = 0; i < srcArray.Length; i++)
            {
                newArray[i] = srcArray[i];
            }
            newArray[newArray.Length - 1] = newData;
            return newArray;
        }

        /* insert: To insert a new string to a existing string array and extend its length by 1.
         * Input: srcArray - The source array that will be inserted in.  newData - the string that will be inserted.
         * Return: the new array that contains the new string as its last element.
         */
        public static String[] insert(String[] srcArray, String newData)
        {
            if (srcArray == null)
            {
                return null;
            }
            String[] newArray = new String[srcArray.Length + 1];
            for (int i = 0; i < srcArray.Length; i++)
            {
                newArray[i] = srcArray[i];
            }
            newArray[newArray.Length - 1] = newData;
            return newArray;
        }

        /* removeFrom: remove an indexed data from the source array and shorten the array length by 1
         * Input: srcArray - the source array index - the index that the data on that position will be removed.
         * Return: The new array without certain data
         */
        public static double[] removeFrom(double[] srcArray, int index)
        {
            if (srcArray == null)
            {
                return null;
            }
            if (srcArray.Length <= index)
            {
                return null;
            }
            double[] newArray = new double[srcArray.Length - 1];
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

        /* removeFrom: remove an indexed data from the source array and shorten the array length by 1
         * Input: srcArray - the source array index - the index that the data on that position will be removed.
         * Return: The new array without certain data
         */
        public static String[] removeFrom(String[] srcArray, int index)
        {
            if (srcArray == null)
            {
                return null;
            }
            if (srcArray.Length <= index)
            {
                return null;
            }
            String[] newArray = new String[srcArray.Length - 1];
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

        /* removeFrom: remove an indexed data from the source array and shorten the array length by 1
         * Input: srcArray - the source array index - the index that the data on that position will be removed.
         * Return: The new array without certain data
         */
        public static Equation[] removeFrom(Equation[] srcArray, int index)
        {
            if (srcArray == null)
            {
                return null;
            }
            if (srcArray.Length <= index)
            {
                return null;
            }
            Equation[] newArray = new Equation[srcArray.Length - 1];
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
        
    }
}
