using System.Transactions;

namespace TerroristDataAnalyzer
{
    class Terrorist
    {
        public string name;
        public string weapons;
        public int age;
        public (double, double) location;
        public string affiliation;

        public Terrorist(string name, string weapons, int age, (double, double) location, string affiliation)
        {
            this.name = name;
            this.weapons = weapons;
            this.age = age;
            this.affiliation = affiliation;
            this.location = location;
        }
    }

    class Program
    {
        static List<Terrorist> terroristList = new List<Terrorist>();
        static Dictionary<string, int> numberOfWeapons = new Dictionary<string, int>();
        static Dictionary<string, int> organizationByNumber = new Dictionary<string, int>();

        static void seedData()
        {
            var sampleTerrorists = new List<Terrorist>
    {
        new Terrorist("Ahmed", "AK-47", 30, (32.08, 34.78), "Group A"),
        new Terrorist("Khaled", "RPG", 25, (31.76, 35.21), "Group B"),
        new Terrorist("Omar", "AK-47", 28, (32.79, 34.98), "Group A"),
        new Terrorist("Youssef", "Knife", 22, (31.04, 34.85), "Group C"),
        new Terrorist("Ali", "RPG", 35, (32.10, 34.85), "Group B"),
        new Terrorist("Mustafa", "AK-47", 27, (33.22, 43.67), "Group A"),
        new Terrorist("Hassan", "Knife", 23, (30.52, 36.24), "Group C")
    };

            foreach (var terrorist in sampleTerrorists)
            {
                terroristList.Add(terrorist);

                if (numberOfWeapons.ContainsKey(terrorist.weapons))
                    numberOfWeapons[terrorist.weapons]++;
                else
                    numberOfWeapons[terrorist.weapons] = 1;

                if (organizationByNumber.ContainsKey(terrorist.affiliation))
                    organizationByNumber[terrorist.affiliation]++;
                else
                    organizationByNumber[terrorist.affiliation] = 1;
            }

            Console.WriteLine("Sample data loaded.");
        }

        static void initListTerrorist()
        {
            Console.WriteLine("enter num of terrorist");
            int numOfTerrorist = int.Parse(Console.ReadLine());
            for (int i = 0; i < numOfTerrorist; i++)
            {
                Console.WriteLine("enter name");
                string name = Console.ReadLine();
                Console.WriteLine("enter age");
                int age = int.Parse(Console.ReadLine());
                Console.WriteLine("enter weapons");
                string weapons = Console.ReadLine();
                Console.WriteLine("enter affiliation");
                string affiliation = Console.ReadLine();
                Console.WriteLine("enter location");
                Console.WriteLine("enter lat");
                double lat = double.Parse(Console.ReadLine());
                Console.WriteLine("enter lont");
                double lont = double.Parse(Console.ReadLine());
                (double, double) location = (lat, lont);

                Terrorist terrorist = new Terrorist(name, weapons, age, location, affiliation);
                terroristList.Add(terrorist);
                if (numberOfWeapons.ContainsKey(weapons))
                    numberOfWeapons[weapons]++;
                else
                    numberOfWeapons[weapons] = 1;

                if (organizationByNumber.ContainsKey(affiliation))
                    organizationByNumber[affiliation]++;
                else
                    organizationByNumber[affiliation] = 1;

            }
        }

        static string FindTheLeastCommonWeapon()
        {
            int minWeapon = numberOfWeapons.Values.First();
            string leastWeapon = numberOfWeapons.Keys.First();

            foreach (KeyValuePair<string, int> kvp in numberOfWeapons)
            {
                if (kvp.Value < minWeapon)
                {
                    minWeapon = kvp.Value;
                    leastWeapon = kvp.Key;
                }
            }
            Console.WriteLine($"Least common weapon: {leastWeapon} ({minWeapon} times)");
            return leastWeapon;
        }

        static string FindTheMostCommonWeapon()
        {
            int maxWeapon = numberOfWeapons.Values.First();
            string mostWeapon = numberOfWeapons.Keys.First();

            foreach (KeyValuePair<string, int> kvp in numberOfWeapons)
            {
                if (kvp.Value > maxWeapon)
                {
                    maxWeapon = kvp.Value;
                    mostWeapon = kvp.Key;
                }
            }
            Console.WriteLine($"Most common weapon: {mostWeapon} ({maxWeapon} times)");
            return mostWeapon;
        }

        static string organizationWithTheLeastMembers()
        {
            int minOrganization = organizationByNumber.Values.First();
            string leastOrganization = organizationByNumber.Keys.First();

            foreach (KeyValuePair<string, int> kvp in organizationByNumber)
            {
                if (kvp.Value < minOrganization)
                {
                    minOrganization = kvp.Value;
                    leastOrganization = kvp.Key;
                }
            }
            Console.WriteLine($"Organization with least members: {leastOrganization} ({minOrganization} members)");
            return leastOrganization;
        }

        static string organizationWithTheMostMembers()
        {
            int maxOrganization = organizationByNumber.Values.First();
            string mostOrganization = organizationByNumber.Keys.First();

            foreach (KeyValuePair<string, int> kvp in organizationByNumber)
            {
                if (kvp.Value > maxOrganization)
                {
                    maxOrganization = kvp.Value;
                    mostOrganization = kvp.Key;
                }
            }
            Console.WriteLine($"Organization with most members: {mostOrganization} ({maxOrganization} members)");
            return mostOrganization;
        }

        static (Terrorist, Terrorist) Find2TerroristClosest((double lat, double lont) myLocation)
        {
            (Terrorist, Terrorist) closestTerrorist = (terroristList[0], terroristList[1]);

            double firstdx = myLocation.lat - terroristList[0].location.Item1;
            double firstdy = myLocation.lont - terroristList[0].location.Item2;
            double firstminDistance = Math.Sqrt(firstdx * firstdx + firstdy * firstdy);

            double seconddx = myLocation.lat - terroristList[1].location.Item1;
            double seconddy = myLocation.lont - terroristList[1].location.Item2;
            double secondminDistance = Math.Sqrt(seconddx * seconddx + seconddy * seconddy);

            foreach (Terrorist terrorist in terroristList)
            {
                Terrorist currentTerrorist = terrorist;
                double currentdx = myLocation.lat - terrorist.location.Item1;
                double currentdy = myLocation.lont - terrorist.location.Item2;
                double currentDistance = Math.Sqrt(currentdx * currentdx + currentdy * currentdy);

                if (currentDistance < firstminDistance && firstminDistance > secondminDistance)
                {
                    firstminDistance = currentDistance;
                    closestTerrorist.Item1 = currentTerrorist;
                }
                else if (currentDistance < secondminDistance)
                {
                    secondminDistance = currentDistance;
                    closestTerrorist.Item2 = currentTerrorist;
                }
            }

            Console.WriteLine($"Closest Terrorist: {closestTerrorist.Item1.name} at distance {firstminDistance}");
            Console.WriteLine($"Second Closest Terrorist: {closestTerrorist.Item2.name} at distance {secondminDistance}");

            return closestTerrorist;
        }

        static void menu()
        {
            seedData();
            bool keepingRun = true;

            while (keepingRun)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1 - Most common weapon");
                Console.WriteLine("2 - Least common weapon");
                Console.WriteLine("3 - Organization with most members");
                Console.WriteLine("4 - Organization with least members");
                Console.WriteLine("5 - Find 2 closest terrorists to a location");
                Console.WriteLine("6 - exit");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        FindTheMostCommonWeapon();
                        break;
                    case 2:
                        FindTheLeastCommonWeapon();
                        break;
                    case 3:
                        organizationWithTheMostMembers();
                        break;
                    case 4:
                        organizationWithTheLeastMembers();
                        break;
                    case 5:
                        Console.WriteLine("enter lat");
                        double lat = double.Parse(Console.ReadLine());
                        Console.WriteLine("enter lont");
                        double lont = double.Parse(Console.ReadLine());
                        (Terrorist, Terrorist) twoClosest = Find2TerroristClosest((lat, lont));
                        break;
                    case 6:
                        Console.WriteLine("bye bye");
                        keepingRun = false;
                        break;
                    default:
                        Console.WriteLine("your choice not exist");
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            menu();
        }
    }
}