﻿using System.Reflection;
using System.Text;
using DSharpPlus;
using DSharpPlus.SlashCommands;

const string @namespace = "Watchdog.Bot.Enums";
const string @class = "EventType";
const string path = "Watchdog.Bot/Enums/EventType.cs";

var events = ExtractEvents(typeof(DiscordClient), typeof(SlashCommandsExtension)).ToList();
GenerateEnumsFile(@namespace, @class, events, path);

IEnumerable<EventInfo> ExtractEvents(params Type[] types)
{
    foreach (var type in types)
        foreach (var eventInfo in type.GetEvents())
            yield return eventInfo;
}

void GenerateEnumsFile(string ns, string s, IEnumerable<EventInfo> eventInfos, string path1)
{
    StringBuilder result = new StringBuilder()
        .AppendLine($"// Generated by Watchdog.Tools.EventTypeGenerator {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}")
        .AppendLine().AppendLine((string?)$"namespace {ns};")
        .AppendLine().AppendLine((string?)$"public enum {s}")
        .AppendLine("{");
    foreach (var eventInfo in eventInfos)
        result.AppendLine($"    {eventInfo.Name},");
    result.AppendLine("}");
    File.WriteAllText(path1, result.ToString());
}