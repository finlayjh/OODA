using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quiz", menuName = "Quiz")]
public class Quiz : ScriptableObject 
{
    public string[] options = new string[] {"OPTION 1", "OPTION 2", "OPTION 3" };
    public int CorrectAnswer;
    public int SelectedAnswer = -1;
}
