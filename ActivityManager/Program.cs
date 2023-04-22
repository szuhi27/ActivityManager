using ActivityManager;

const string TypeFilePath = @"SaveFile.json";
string openActivityPath = "";

ActivityType[] allActivityTypes = Array.Empty<ActivityType>();
ActivityType currentActivityType = new(0,"",Array.Empty<Activity>());
Activity openActivity = new();
bool inMenu = true;
DataManager dataManager = new DataManager();

Console.WriteLine("Hello"); //print all saves

if (File.Exists(TypeFilePath))
{
    allActivityTypes = dataManager.LoadJson<ActivityType[]>(TypeFilePath);
    Console.WriteLine("Type Id of save you want to load. To see all commands type: 'help'."); 
}

WriteAllItemsFromArray(currentActivityType.Activities, "Create new save by typing: 'new save'");


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
                WriteAllItemsFromArray(allActivityTypes, "Create new save by typing: 'new save'");
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


void WriteAllItemsFromArray<T>(T[] collection, string emptyMsg)
{
    if(collection.Length != 0)
    {
        foreach (var item in collection)
        {
            Console.WriteLine($"{item}");
        }
    }
    else
    {
        Console.WriteLine(emptyMsg);
    }
}
/*
void WriteAllActivityTypes()
{
    foreach (var item in allActivityTypes)
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
}*/

void HandleNewTypeSave()
{
    Console.WriteLine("Enter name: ");
    var input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        allActivityTypes = dataManager.CreateNewSave(allActivityTypes, input, TypeFilePath);
        currentActivityType = allActivityTypes[allActivityTypes.Length-1];
        Console.WriteLine($"Current save: {currentActivityType.Id}-{currentActivityType.Name}");
        WriteAllItemsFromArray(currentActivityType.Activities, " - No activites yet - ");
    }
    else
    {
        Console.WriteLine("Name can't be null");
    }
}

void HandleTypeChoiceById(int id)
{
    if (allActivityTypes.Length == 0) //check if user wants to load when no saves
    {
        Console.WriteLine("You don't have any saves, to create one type: 'new save'.");
    }
    else
    {
        try
        {
            currentActivityType = dataManager.ReturnChosenActivityType(id, allActivityTypes);
            Console.Clear();
            WriteAllItemsFromArray(currentActivityType.Activities, " - No activites yet - ");
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
        openActivity = new(currentActivityType.Activities.Length + 1);
        dataManager.SaveJson(openActivity, openActivityPath);
        currentActivityType = dataManager.ReturnModifiedActivityArray(currentActivityType, openActivity);
        allActivityTypes = dataManager.ReturnModifiedActivityTypeArray(allActivityTypes, currentActivityType);
        dataManager.SaveJson(allActivityTypes, TypeFilePath);
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