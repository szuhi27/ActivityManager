using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ActivityManager
{
    internal class DataManager
    {
        public ActivityType[] LoadAndReturnAllActivityTypes() 
        {
            string path = @"GameSaves.json";
            if (File.Exists(path))
            {
                string files = File.ReadAllText(path);
                return JsonSerializer.Deserialize<ActivityType[]>(files)!;
            }
            else
            {
                throw new Exception("Create new save by typing: 'new save'");
            }
        }

        public ActivityType ReturnChosenActivityType(int id, ActivityType[] activityTypes)
        {
            for (int j = 0; j < activityTypes.Length; j++)
            {
                if (activityTypes[j].Id == id)
                {
                    return activityTypes[j];
                }
            }
            throw new WrongInputException($"Save with {id} id does not exist!");
        }

        public ActivityType[] CreateNewSave(ActivityType[] activityTypes, string name)
        {
            List<ActivityType> activities = activityTypes.ToList();
            activities.Add(new ActivityType(activityTypes.Length+1,name, Array.Empty<Activity>()));
            string json = JsonSerializer.Serialize(activities.ToArray());
            File.WriteAllText(@"GameSaves.json", json);
            return activities.ToArray();
        }

    }
}
