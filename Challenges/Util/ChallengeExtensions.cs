﻿using System.Diagnostics;
using System.Reflection;

namespace Challenges.Util;

public static partial class ChallengeExtensions
{
    public static async Task<Result> CompleteChallenge(this IChallenge challenge, string input, string example1, string example2)
    {
        var start = Stopwatch.GetTimestamp();
        var examplePartOneResult = example1 != "" ? await challenge.TaskPartOne(example1) : null;
        var examplePartTwoResult = example2 != "" ? await challenge.TaskPartTwo(example2) : null;
        var partOneResult = input != "" ? await challenge.TaskPartOne(input) : null;
        var partTwoResult = input != "" ? await challenge.TaskPartTwo(input) : null;
        
        var name = challenge.GetName();

        var delta = Stopwatch.GetElapsedTime(start);
        
        var result = new Result(name, $"{delta.TotalSeconds:N0}s : {delta.Milliseconds:N0}ms", examplePartOneResult, examplePartTwoResult, partOneResult, partTwoResult, $"{Year(challenge)}/{Day(challenge):00}");
        return result;
    }

    public static string GetName(this IChallenge challenge)
    {
        return (
            challenge
                .GetType()
                .GetCustomAttribute(typeof(ChallengeName)) as ChallengeName
        )?.Name ?? "Unknown";
    }

    public static string WorkingDir(int year)
    {
        return Path.Combine($"Y{year}");
    }

    public static string WorkingDir(int year, int day)
    {
        return Path.Combine(WorkingDir(year), $"Day{day:00}");
    }

    public static string WorkingDir(this IChallenge challenge)
    {
        return WorkingDir(challenge.Year(), challenge.Day());
    }

    public static int Year(Type t)
    {
        return int.Parse((t.FullName?.Split('.')[2])?[1..]!);
    }

    public static int Year(this IChallenge challenge)
    {
        return Year(challenge.GetType());
    }

    public static int Day(Type t)
    {
        return int.Parse((t.FullName?.Split('.')[3])?[3..]!);
    }

    public static int Day(this IChallenge challenge)
    {
        return Day(challenge.GetType());
    }

    public static bool IsChallenge(Type challenge, DateTime startDateTime, DateTime? endDateTime)
    {
        var date = new DateTime(Year(challenge), 12, Day(challenge));
        return date >= startDateTime && (
            (endDateTime is null)
            || (date <= endDateTime));

    }
}
