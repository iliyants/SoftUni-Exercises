using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {

        var departments = new Dictionary<string, List<string>>();
        var doctors = new Dictionary<string, List<string>>();


        while (true)
        {
            string input = Console.ReadLine();
            if (input == "Output")
            {
                Print(departments, doctors);
                break;
            }
            string[] tokens = input.Split();

            string department = tokens[0];
            string doctor = tokens[1] + " " + tokens[2];
            string patient = tokens[3];

            if (!departments.ContainsKey(department))
            {
                departments[department] = new List<string>();
                departments[department].Add(patient);
            }
            else
            {
                if (departments[department].Count <= 60)
                {
                    departments[department].Add(patient);
                }
                else
                {
                    continue;
                }
            }
            if (!doctors.ContainsKey(doctor))
            {
                doctors[doctor] = new List<string>();
                doctors[doctor].Add(patient);
            }
            else
            {
                doctors[doctor].Add(patient);
            }

        }

    }

    private static void Print(Dictionary<string, List<string>> departments, Dictionary<string, List<string>> doctors)
    {
        var set = new SortedSet<string>();
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "End")
            {
                return;
            }
            string[] tokens = input.Split();

            if (tokens.Length == 1)
            {
                foreach (var patient in departments.Where(x => x.Key == tokens[0]))
                {
                    Console.WriteLine(string.Join("\n", patient.Value));
                }
            }
            else
            {
                string checkIfDoctor = tokens[0] + " " + tokens[1];
                if (doctors.ContainsKey(checkIfDoctor))
                {

                    foreach (var patient in doctors.Where(x => x.Key == checkIfDoctor))
                    {
                        foreach (var kvp in patient.Value)
                        {
                            set.Add(kvp);
                        }
                    }
                    Console.WriteLine(string.Join("\n", set));
                    set.Clear();
                }
                else
                {
                    string department = tokens[0];
                    int room = int.Parse(tokens[1]);
                    if (room < 0 || room > 20)
                    {
                        continue;
                    }

                    var patients = departments[department].ToList();
                    var result = patients.Skip(3 * (room - 1)).ToList();

                    while (result.Count > 3)
                    {
                        result.RemoveAt(result.Count - 1);
                    }

                    foreach (var item in result)
                    {
                        set.Add(item);
                    }
                    Console.WriteLine(string.Join("\n", set));
                    set.Clear();



                }
            }
        }
    }
}
