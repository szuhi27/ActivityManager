using ActivityManager;

ActivityType[] activityTypes = Array.Empty<ActivityType>();
ActivityType openActivityType;
List<Activity> activitiesList;
Activity openActivity;

DataManager dataManager = new DataManager();

Console.WriteLine("Helo"); //print all saves

try
{
    activityTypes = dataManager.LoadAndReturnAllActivityTypes();
    WriteAllActivityTypes();
    Console.WriteLine("Type Id of save you want to load. To see all commands type: 'help'.");
}
catch (Exception ex)
{   
    Console.WriteLine(ex.Message);
}

while (true)
{
    try
    {
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            throw new WrongInputException("Input can't be empty");
        }
        else if (input == "exit")
        {
            break;
        }
        else if (input == "help")
        {
            //todo
        }
        else if (input == "new save")
        {
            HandleNewSave();
        }
        else if(int.TryParse(input, out int intInput))
        {
            HandleSaveChoiceById(intInput);
        }
        else
        {
            throw new WrongInputException("Unknown input!");
        }
    }
    catch (WrongInputException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void WriteAllActivityTypes()
{
    foreach (var item in activityTypes)
    {
        Console.WriteLine($"{item}");
    }
}

void WriteAllActies()
{
    if(activitiesList.Count == 0)
    {
        Console.WriteLine(" - No activites yet - ");
    }
    else
    {
        foreach (var item in activitiesList)
        {
            Console.WriteLine($"{item}");
        }
    }
}

void HandleNewSave()
{
    Console.WriteLine("Enter name: ");
    var input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        activityTypes = dataManager.CreateNewSave(activityTypes, input);
        openActivityType = activityTypes[activityTypes.Length-1];
        Console.WriteLine($"Current save: {openActivityType.Id}-{openActivityType.Name}");
        activitiesList = openActivityType.Activities.ToList();
        WriteAllActies();
    }
    else
    {
        throw new WrongInputException("Name can't be null");
    }
}

void HandleSaveChoiceById(int id)
{
    if (activityTypes.Length == 0)
    {
        Console.WriteLine("You don't have any saves, to create one type: 'new save'.");
    }
    else
    {
        try
        {
            openActivityType = dataManager.ReturnChosenActivityType(id, activityTypes);
            activitiesList = openActivityType.Activities.ToList();
            //todo clear panel
            WriteAllActies();
        }
        catch (WrongInputException e)
        {
            Console.WriteLine(e.Message);
        }
        //todo see if there is ongoing record
    }
}