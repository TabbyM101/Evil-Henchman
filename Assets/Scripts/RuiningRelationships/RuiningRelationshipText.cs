using UnityEngine;

[CreateAssetMenu(fileName = "RuiningRelationshipText", menuName = "ScriptableObjects/RuiningRelationshipText")]
public class RuiningRelationshipText : ScriptableObject
{
    public string FromWho;

    public Sprite FromProfile;

    public string From1;

    public string To1ShortGood;
    public string To1ShortBad;
    public string To1Good;
    public string To1Bad;

    public string From2Good;
    public string From2Bad;
    public string To2ShortGood;
    public string To2ShortBad;
    public string To2Good;
    public string To2Bad;

    public string From3Good;
    public string From3Bad;
    public string To3ShortGood;
    public string To3ShortBad;
    public string To3Good;
    public string To3Bad;

    public string FromOverallSuccess;
    public string FromOverallFailure;
    
}
