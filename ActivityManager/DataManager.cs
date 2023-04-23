using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ActivityManager
{
    internal class DataManager
    {
        /*
         * all file manipulation is here (load, save, delete)
         * also longer repetitive codes that modify certain variables are here
         */

        public enum ActivityModifierCall
        {
            add,
            edit, 
            remove
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

        public ActivityType[] CreateNewSave(ActivityType[] activityTypes, string name, string path)
        {
            List<ActivityType> activities = activityTypes.ToList();
            activities.Add(new ActivityType(activityTypes.Length+1,name, Array.Empty<Activity>()));
            SaveJson(activities.ToArray(), path);
            return activities.ToArray();
        }

        public ActivityType ReturnModifiedActivityArray(ActivityType atype, Activity activity, ActivityModifierCall call)
        {
            List<Activity> newActivitiesList = atype.Activities.ToList();
            switch (call)
            {
                case ActivityModifierCall.add:
                    newActivitiesList.Add(activity);
                    break;
                case ActivityModifierCall.edit:
                    for (int i = 0; i < newActivitiesList.Count; i++)
                    {
                        if (newActivitiesList[i].Id == activity.Id)
                        {
                            newActivitiesList[i] = activity;
                        }
                    }
                    break;
                case ActivityModifierCall.remove:
                    newActivitiesList.Remove(activity);
                    break;
            }

            return atype with { Activities = newActivitiesList.ToArray() };
        }

        public ActivityType[] ReturnModifiedActivityTypeArray(ActivityType[] types, ActivityType currentType)
        {
            for(int i = 0; i < types.Length; i++)
            {
                if (types[i].Id == currentType.Id)
                {
                    types[i] = currentType;
                    return types;
                }
            }
            return types;
        }

        public T LoadJson<T>(string path){
            string json = File.ReadAllText(path);  
            return JsonSerializer.Deserialize<T>(json)!;
        }

        public void SaveJson<T>(T data, string path){
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        public void DeleteFile(string path){
            if(File.Exists(path)){
                File.Delete(path);
            }
        }
    }
}
