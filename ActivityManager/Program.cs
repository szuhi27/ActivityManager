using ActivityManager;

const string TYPE_FILE_PATH = @"SaveFile.json";
const string NO_TYPE_MSG = "Create new type by typing: 'new type'";
string openActivityPath = "";

ActivityType[] allActivityTypes = Array.Empty<ActivityType>();
ActivityType? currentActivityType = null;
Activity? openActivity = new();
bool inMenu = true;
DataManager dataManager = new();

Console.WriteLine("Hello"); //print all saves

if (File.Exists(TYPE_FILE_PATH))
{
    allActivityTypes = dataManager.LoadJson<ActivityType[]>(TYPE_FILE_PATH);
    Console.WriteLine("Type Id of save you want to load. To see all commands type: 'help'."); 
}

WriteAllItemsFromArray(allActivityTypes, NO_TYPE_MSG);


while (true)
{
    var input = Console.ReadLine();

    if(int.TryParse(input, out int intInput))
    {
        HandleTypeChoiceById(intInput);
    }
    else if (!string.IsNullOrEmpty(input))
    {
        switch (input.ToLower())
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
                currentActivityType = null;
                openActivity = null;
                openActivityPath = "";
                WriteAllItemsFromArray(allActivityTypes, NO_TYPE_MSG);
                inMenu = true;
                break;
            case "new type":
                HandleNewTypeSave();
                break;
            case "start":
                HandleStartNewActivity();
                break;
            case "stop":
                HandleStopActivity();
                break;
            case "note":
                HandleAddNoteToActivity();
                break;
            case "save":
                HandleSaveActivity();
                break;
            default:
                Console.WriteLine("Unknown input!");
                break;
        }
    }
    else
    {
        Console.WriteLine("Input cant be null!");
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

void HandleNewTypeSave()
{
    Console.WriteLine("Enter name: ");
    var input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        allActivityTypes = dataManager.CreateNewSave(allActivityTypes, input, TYPE_FILE_PATH);
        currentActivityType = allActivityTypes[allActivityTypes.Length-1];
        openActivityPath = @$"OngoingActivity{currentActivityType.Id}.json";
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
        Console.WriteLine($"You don't have any types! {NO_TYPE_MSG}");
    }
    else
    {
        try
        {
            currentActivityType = dataManager.ReturnChosenActivityType(id, allActivityTypes);
            Console.Clear();
            Console.WriteLine($"Current save: {currentActivityType.Id}-{currentActivityType.Name}");
            WriteAllItemsFromArray(currentActivityType.Activities, " - No activites yet - ");
            inMenu = false;
            openActivityPath = @$"OngoingActivity{currentActivityType.Id}.json";
            if (File.Exists(openActivityPath))
            {
                openActivity = dataManager.LoadJson<Activity>(openActivityPath);
                Console.WriteLine($"Open activity {openActivity}");
            }
        }
        catch (WrongInputException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

void HandleStartNewActivity()
{
    if(!File.Exists(openActivityPath))
    {
        openActivity = new(currentActivityType.Activities.Length + 1);
        ActivityStartedOrModified(DataManager.ActivityModifierCall.add);
    }
    else
    {
        Console.WriteLine("There is already an open activity! Close that first!");
    }
}

void HandleStopActivity()
{
    if (File.Exists(openActivityPath))
    {
        openActivity.StopActivity();
        ActivityStartedOrModified(DataManager.ActivityModifierCall.edit);
    }
    else
    {
        Console.WriteLine("There is no open activity to stop! Setart one with: start");
    }
}

void HandleAddNoteToActivity()
{
    if (File.Exists(openActivityPath))
    {
        Console.WriteLine("enter note");
        var input = Console.ReadLine();
        if(!string.IsNullOrEmpty(input))
        {
            openActivity.AddNote(input);
            ActivityStartedOrModified(DataManager.ActivityModifierCall.edit);
        }
        else
        {
            Console.WriteLine("Note is empty, exiting -add note- mode.");
            Console.WriteLine($"{openActivity}");
        }
    }
    else
    {
        Console.WriteLine("There is no open activity to add note to! Setart one with: start");
    }
}

void HandleSaveActivity()
{
    if (!File.Exists(openActivityPath))
    {
        Console.WriteLine("There is no open activity to save! Setart one with: start");
    }
    else if (openActivity.EndTime == "")
    {
        Console.WriteLine("Stop the activity first!");
    }
    else
    {
        ActivitySaved(DataManager.ActivityModifierCall.edit);
        Console.Clear();
        WriteAllItemsFromArray(currentActivityType.Activities, " - No activites yet - ");
    }
}

void ActivityStartedOrModified(DataManager.ActivityModifierCall call)
{
    dataManager.SaveJson(openActivity, openActivityPath);
    ActivityTypeModified(call);
    Console.WriteLine($"{openActivity}");
}

void ActivitySaved(DataManager.ActivityModifierCall call)
{
    ActivityTypeModified(call);
    openActivity = null;
    dataManager.DeleteFile(openActivityPath);
}

void ActivityTypeModified(DataManager.ActivityModifierCall call)
{
    currentActivityType = dataManager.ReturnModifiedActivityArray(currentActivityType, openActivity, call);
    allActivityTypes = dataManager.ReturnModifiedActivityTypeArray(allActivityTypes, currentActivityType);
    dataManager.SaveJson(allActivityTypes, TYPE_FILE_PATH);
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