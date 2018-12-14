namespace GameJam
{
    using UnityEditor;
    using UnityEngine;

    public static class Tests
    {
        [MenuItem("Tests/Load Dialogue")]
        public static void LoadDialogue()
        {
            Dialogue dialogue = Dialogue.FromAsset("Bob");
            foreach (var line in dialogue.Lines)
            {
                Debug.Log(line);
            }
            Debug.Log(dialogue.FinalWords);
        }
        [MenuItem("Tests/Load CloseUp")]
        public static void LoadCloseUp()
        {
            string closeUpName = "Bob";
            CloseUp closeUp = CloseUp.FromAsset(closeUpName);
            Debug.Log(closeUp.Head.name);
            Debug.Log(closeUp.Hairs.name);
            Debug.Log(closeUp.Ears.name);
            Debug.Log(closeUp.Eyes.name);
            Debug.Log(closeUp.Mouth.name);
            Debug.Log(closeUp.Nose.name);
        }
    }
}