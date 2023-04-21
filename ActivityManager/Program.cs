using ActivityManager;

Console.WriteLine("Helo");

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
    }
    catch (WrongInputException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
