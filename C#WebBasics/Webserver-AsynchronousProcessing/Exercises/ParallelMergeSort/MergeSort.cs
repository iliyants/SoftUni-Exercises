using System;
using System.Threading.Tasks;

namespace ParallelMergeSort
{
    public class MergeSort
    {
        private static void Merge(int[] input, int left, int middle, int right)
        {
            int[] leftArray = new int[middle - left + 1];
            int[] rightArray = new int[right - middle];

            Array.Copy(input, left, leftArray, 0, middle - left + 1);
            Array.Copy(input, middle + 1, rightArray, 0, right - middle);

            int i = 0;
            int j = 0;
            for (int k = left; k < right + 1; k++)
            {
                if (i == leftArray.Length)
                {
                    input[k] = rightArray[j];
                    j++;
                }
                else if (j == rightArray.Length)
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else if (leftArray[i] <= rightArray[j])
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else
                {
                    input[k] = rightArray[j];
                    j++;
                }
            }
        }

        static public async Task Sort(int[] input, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                Task leftSort = Sort(input, left, middle);
                Task rightSort = Sort(input, middle + 1, right);

                await leftSort;
                await rightSort;

                Merge(input, left, middle, right);
            }
        }
    }
}
