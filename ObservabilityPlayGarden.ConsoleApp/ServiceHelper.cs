﻿using System.Diagnostics;

namespace ObservabilityPlayGarden.ConsoleApp
{
    internal static class ServiceHelper
    {
        public static void Work1()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity(name: "Work1 Activity", kind: ActivityKind.Producer);
            Console.WriteLine("Work1 Completed");
        }

        public static async Task Work2()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();
            try
            {
                // add activity event
                var eventTags = new ActivityTagsCollection();
                eventTags.Add("userId", 3);
                activity.AddEvent(new("google istek başladı", tags: eventTags));

                // add activity tag
                activity.AddTag("request.schema", "https");
                activity.AddTag("request.method", "get");

                Work1();

                var client = new HttpClient();
                var response = await client.GetStringAsync("http://google.com");
                //var response = await client.GetStringAsync("htttp://google.com");

                activity.AddTag("request.responselenght", response.Length);


                await Work3("Hello World");
                Console.WriteLine("Work2 Completed");
            }
            catch (Exception ex)
            {
                activity.SetStatus(ActivityStatusCode.Error, ex.Message);
            }

        }

        public static async Task<int> Work3(string txt)
        {
            using var activity = ActivitySourceProvider.Source.StartActivity("Work3 Activity Write to File", kind: ActivityKind.Client);

            await File.WriteAllTextAsync("myFile.txt", txt);
            
            Console.WriteLine("Work3 Completed");
            return (await File.ReadAllTextAsync("myFile.txt")).Length;
        }
    }
}
