using ActivityManager;

const string TypeFilePath = @"SaveFile.json";
string openActivityPath = "";

ActivityType[] activityTypes = Array.Empty<ActivityType>();
ActivityType currentActivityType = new ActivityType(0,"",Array.Empty<Activity>());
Activity openActivity = new();
bool inMenu = true;
DataManager dataManager = new DataManager();

Console.WriteLine("Helo"); //print all saves

if (File.Exists(TypeFilePath))
{
    activityTypes = dataManager.LoadAndReturnAllActivityTypes();
    WriteAllActivityTypes();
    Console.WriteLine("Type Id of save you want to load. To see all commands type: 'help'.");
}
else
{
    Console.WriteLine("Create new save by typing: 'new save'");
}


while (true)
{
    var input = Console.ReadLine();

    if(int.TryParse(input, out int intInput))
    {
        HandleTypeChoiceById(intInput);
    }
    else if (!string.IsNullOrEmpty(input))
    {
        switch (input)
        {
            case "exit":
                Environment.Exit(1);
                break;
            case "help":
                PrintHelp(inMenu);
                break;
            case "clear":
                Console.Clear();
                break;
            case "menu":
                Console.Clear();
                WriteAllActivityTypes();
                inMenu = true;
                break;
            case "new save":
                HandleNewTypeSave();
                break;
            case "start":
                HandleStartNewActivity();
                break;
        }
    }
    else
    {
        Console.WriteLine("Unknown input!");
    }
}



void WriteAllActivityTypes()
{
    foreach (var item in activityTypes)
    {
        Console.WriteLine($"{item}");
    }
}

void WriteAllActivities()
{
    if (currentActivityType.Activities.Length != 0)
    {
        foreach (var item in currentActivityType.Activities)
        {
            Console.WriteLine($"{item}");
        }
    }
    else
    {
        Console.WriteLine(" - No activites yet - ");
    }
}

void HandleNewTypeSave()
{
    Console.WriteLine("Enter name: ");
    var input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        activityTypes = dataManager.CreateNewSave(activityTypes, input);
        currentActivityType = activityTypes[activityTypes.Length-1];
        Console.WriteLine($"Current save: {currentActivityType.Id}-{currentActivityType.Name}");
        WriteAllActivities();
    }
    else
    {
        Console.WriteLine("Name can't be null");
    }
}

void HandleTypeChoiceById(int id)
{
    if (activityTypes.Length == 0) //check if user wants to load when no saves
    {
        Console.WriteLine("You don't have any saves, to create one type: 'new save'.");
    }
    else
    {
        try
        {
            currentActivityType = dataManager.ReturnChosenActivityType(id, activityTypes);
            Console.Clear();
            WriteAllActivities();
            inMenu = false;
            openActivityPath = @$"OngoingActivity{currentActivityType.Id}";
        }
        catch (WrongInputException e)
        {
            Console.WriteLine(e.Message);
        }
        //todo see if there is ongoing record
    }
}

void HandleStartNewActivity()
{
    if(!File.Exists(openActivityPath))
    {
        currentActivityType = dataManager.StartNewActivity(currentActivityType);
    }
    else
    {
        Console.WriteLine("There is already an open activity! Close that first!");
    }
    
}

void PrintHelp(bool menuMode)
{
    if (menuMode)
    {
        Console.WriteLine(
        "exit -> exit\n" +
        "create new activity type (group) -> new save\n" +
        "load an activity type -> [enter it's id]\n"
        );
    }
    else
    {
        Console.WriteLine(
            "start new activity -> start\n" +
            "stop activity -> stop\n" +
            "add note -> note\n" +
            "save activity -> save\n" +
            "go to menu -> menu"
        );
    }
    
}