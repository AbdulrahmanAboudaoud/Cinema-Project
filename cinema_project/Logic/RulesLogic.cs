public class RulesLogic : IAdd
{
    public static void ViewAllRules()
    {
        List<string> Rules = new List<string>();
        Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
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
        Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
        Console.WriteLine("Insert the new version of rule:");
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
        RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
    }

    public void AddItem<T>(T item)
    {
        if (item is string NewRule)
        {
            List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
            Rules.Add(NewRule);
            RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
            Console.WriteLine("Rule successfully added.");
        }
        else
        {
            Console.WriteLine("Invalid type for AddItem in RulesLogic. Expected type is string.");
        }
    }

    public static void RemoveRule(int RuleNumber)
    {
        List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
        string RuleToRemove = Rules[RuleNumber - 1];
        Rules.Remove(RuleToRemove);
        RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
        Console.WriteLine("Rule successfully removed.");
    }
}
