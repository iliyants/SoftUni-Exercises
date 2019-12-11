using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ConsoleApp89
{

    class Program
    {
        static void Main(string[] args)
        {

            string keyWord = string.Empty;
            BigInteger bagCapacity = BigInteger.Parse(Console.ReadLine());

            BigInteger currentAmount = 0;

            BigInteger totalCashAmount = 0;
            BigInteger totalGemAmount = 0;
            BigInteger totalGoldAmount = 0;

            var bag = new Dictionary<string, Dictionary<string, BigInteger>>();


            var items = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < items.Count - 1; i += 2)
            {
                string material = items[i];
                BigInteger quantity = BigInteger.Parse(items[i + 1]);
                if (currentAmount + quantity > bagCapacity)
                {
                    continue;
                }


                string gemCheck = material.Substring(material.Length - 3, 3).ToLower();
                string goldCheck = material.ToLower();

                if (material.Length == 3 && totalGemAmount >= (totalCashAmount + quantity))
                {
                    if (!bag.ContainsKey("Cash"))
                    {
                        bag["Cash"] = new Dictionary<string, BigInteger>();
                    }
                    keyWord = "Cash";
                    bool checkDuplicae = DuplicateCheck(material, bag, quantity, keyWord);
                    if (!checkDuplicae)
                    {
                        bag["Cash"][material] = quantity;
                    }
                    totalCashAmount += quantity;
                }
                else if (gemCheck == "gem" && (totalGoldAmount >= totalGemAmount + quantity) && material.Length >= 4)
                {
                    if (!bag.ContainsKey("Gem"))
                    {
                        bag["Gem"] = new Dictionary<string, BigInteger>();
                    }
                    keyWord = "Gem";
                    bool checkDuplicae = DuplicateCheck(material, bag, quantity, keyWord);
                    if (!checkDuplicae)
                    {
                        bag["Gem"][material] = quantity;
                    }
                    totalGemAmount += quantity;
                }
                else if (goldCheck == "gold")
                {
                    if (!bag.ContainsKey("Gold"))
                    {
                        bag["Gold"] = new Dictionary<string, BigInteger>();
                    }
                    keyWord = "Gold";
                    bool checkDuplicae = DuplicateCheck(material, bag, quantity, keyWord);
                    if (!checkDuplicae)
                    {
                        bag["Gold"][material] = quantity;
                    }
                    totalGoldAmount += quantity;
                }
                else
                {
                    continue;
                }

                currentAmount += quantity;
            }


            foreach (var item in bag)
            {
                if (item.Key == "Gold")
                {
                    Console.WriteLine($"<{item.Key}> ${totalGoldAmount}");
                }
                else if (item.Key == "Gem")
                {
                    Console.WriteLine($"<{item.Key}> ${totalGemAmount}");
                }
                else
                {
                    Console.WriteLine($"<{item.Key}> ${totalCashAmount}");
                }
                foreach (var kvp in item.Value.OrderByDescending(x => x.Key).ThenByDescending(x => x.Value))
                {
                    Console.WriteLine($"##{kvp.Key} - {kvp.Value}");
                }
            }




        }

        private static bool DuplicateCheck(string material, Dictionary<string, Dictionary<string, BigInteger>> bag, BigInteger quantity, string keyWord)
        {

            string currentMaterial = string.Empty;
            string currentMaterialToLower = string.Empty;
            string safeMaterial = material.ToLower();

            foreach (var item in bag.Where(x => x.Key == $"{keyWord}"))
            {
                foreach (var kvp in item.Value)
                {
                    currentMaterial = kvp.Key;
                    currentMaterialToLower = kvp.Key.ToLower();
                    if (currentMaterialToLower == safeMaterial)
                    {
                        bag[$"{keyWord}"][currentMaterial] += quantity;
                        return true;

                    }
                }
            }
            return false;
        }


    }
}