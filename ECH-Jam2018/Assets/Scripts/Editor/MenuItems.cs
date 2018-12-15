﻿namespace GameJam
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class MenuItems
    {
        [MenuItem("Utilities/Save sample dialogue")]
        public static void SaveDialogue()
        {
            var sampleDialogue = new Dialogue()
            {
                Name = "DefaultDialogueName",
                DefaultSpeaker = "Bob",
                Lines = new List<string> {
                    "I used to work as a sailor",
                    "Adam: Cool, where did you go?",
                    "Pretty much everywhere... Europe, Asia, Africa",
                    "Then... well... nothing...",
                },
                FinalWords = "I got nothing more to say..."
            };
            var path = Application.persistentDataPath + "/EmptyDialogue.json";
            var json = JsonUtility.ToJson(sampleDialogue, true);
            using (var writer = File.CreateText(path))
            {
                writer.Write(json);
            }
        }
        [MenuItem("Utilities/Open persistent path")]
        public static void OpenPersistentPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        [MenuItem("Utilities/Convert dialogue")]
        public static void ConvertDialogue()
        {
            var rawDialogues = Resources.LoadAll<TextAsset>("RawDialogue");
            foreach (var rawDialogue in rawDialogues)
            {
                var dialoguePath = Application.dataPath + "/Resources/Dialogue/" + rawDialogue.name + ".json";
                var dialogue = new Dialogue()
                {
                    Name = rawDialogue.name,
                    Lines = new List<string>(),
                };
                var text = rawDialogue.text;
                var lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                dialogue.Lines.AddRange(lines);
                var json = JsonUtility.ToJson(dialogue, true);
                using (var writer = File.CreateText(dialoguePath))
                {
                    writer.Write(json);
                }
            }
        }
    }
}