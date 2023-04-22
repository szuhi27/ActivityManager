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
            string files = File.ReadAllText(@"SaveFile.json");
            return JsonSerializer.Deserialize<ActivityType[]>(files)!;
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
            File.WriteAllText(@"SaveFile.json", json);
            return activities.ToArray();
        }

        public ActivityType StartNewActivity(ActivityType currentType)
        {
            Activity activity = new(currentType.Activities.Length + 1);
            string json = JsonSerializer.Serialize(activity);
            File.WriteAllText(@$"OngoingActivity{currentType.Id}", json);

            List<Activity> newActivitiesList = currentType.Activities.ToList();
            newActivitiesList.Add(activity);

            return currentType with { Activities = newActivitiesList.ToArray() };
        }

    }
}
