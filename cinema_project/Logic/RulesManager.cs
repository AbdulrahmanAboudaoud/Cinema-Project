public static class RulesManager
{
    private const string RulesCSVFile = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\cinemarules.csv";
    public static List<string> ReadRulesFromCSV(string RulesCSVFile)
    {
        List<string> rules = new List<string>();

        try
        {
            using (StreamReader reader = new StreamReader(RulesCSVFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    rules.Add(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }
        return rules;
    }

    public static void WriteRulesToCSV(List<string> rules, string rulesCSVFile)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(rulesCSVFile))
            {
                foreach (string rule in rules)
                {
                    writer.WriteLine(rule);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to CSV file: {ex.Message}");
        }
    }

    public static void ViewAllRules()
    {
        List<string> Rules = new List<string>();
        Rules = ReadRulesFromCSV(RulesCSVFile);
        int RuleNumber = 1;
        foreach (string Rule in Rules)
        {
            if (RuleNumber == 1)
            {
                Console.WriteLine("\n");
            }
            Console.WriteLine($"Rule {RuleNumber}: {Rule}");
            RuleNumber++;
            if (RuleNumber > Rules.Count)
            {
                Console.WriteLine("\n");
            }
        }
    }

    public static void EditRules(int RuleNumber)
    {
        List<string> Rules = new List<string>();
        Rules = ReadRulesFromCSV(RulesCSVFile);
        string NewRule = Console.ReadLine();
        if (NewRule != null)
        {
            Rules[RuleNumber - 1] = NewRule;
            Console.WriteLine("Rule successfully altered.");
        }
        else
        {
            Console.WriteLine("Rule is empty.");
        }
        WriteRulesToCSV(Rules, RulesCSVFile);
    }

    public static void AddRule(string NewRule)
    {
        List<string> Rules = ReadRulesFromCSV(RulesCSVFile);
        Rules.Add(NewRule);
        WriteRulesToCSV(Rules, RulesCSVFile);
        Console.WriteLine("Rule successfully added.");
    }

    public static void RemoveRule(int RuleNumber) 
    {
        List<string> Rules = ReadRulesFromCSV(RulesCSVFile);
        string RuleToRemove = Rules[RuleNumber - 1];
        Rules.Remove(RuleToRemove);
        WriteRulesToCSV(Rules, RulesCSVFile);
        Console.WriteLine("Rule successfully removed.");
    }
}