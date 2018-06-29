using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GenerateNames : MonoBehaviour
{

    [System.Serializable]
    internal class NameParts
    {
        public string[] consonants;

        public string[] startConsonants;

        public string[] endConsonants;

        public string[] vowels;

        public string[] suffixes;
    }

    internal enum ConsonantOptions
    {
        None,
        Start,
        End
    }

    private const string NAME_PARTS_PATH = "Assets/Config/name_parts.json";

    private const int MIN_SYLLABLES = 1;
    private const int MAX_SYLLABLES = 2;

    private const int MIN_LENGTH = 4;

    private const float START_CONSONANT_CHANCE = 1.0f;
    private const float END_CONSONANT_CHANCE = 0.7f;
    private const float SUFFIX_CHANCE = 0.3f;
    private const float SILENT_E_CHANCE = 0.4f;

    private NameParts nameParts;

    private void Awake()
    {
        LoadData();
        for (int i = 0; i < 10; i++)
        {
            print(GenerateName());
        }
    }

    public void LoadData()
    {
        using (StreamReader sr = new StreamReader(NAME_PARTS_PATH))
        {
            string json = sr.ReadToEnd();
            nameParts = JsonUtility.FromJson<NameParts>(json);
        }
    }

    public string GenerateName()
    {
        int numSyllables = Random.Range(MIN_SYLLABLES, MAX_SYLLABLES + 1);
        StringBuilder name = new StringBuilder();

        for (int i = 0; i < numSyllables; i++)
        {
            bool last = numSyllables > 1 && i == numSyllables - 1;
            name.Append(GenerateSyllable(last));
        }

        //prevent single-letter names
        if (name.Length == 1)
        {
            name.Append(GenerateSyllable(true));
        }

        if (name.Length < MIN_LENGTH || Random.value < SUFFIX_CHANCE)
        {
            name.Append(GetSuffix());
        }

        //capitalize first letter
        string result = name.ToString();
        result = result[0].ToString().ToUpper() + result.Substring(1);

        return result;
    }

    private string GenerateSyllable(bool last)
    {
        if (last && Random.value < SILENT_E_CHANCE)
        {
            return GenerateLastSyllable();
        }
        StringBuilder syllable = new StringBuilder();
        if (Random.value < START_CONSONANT_CHANCE)
        {
            syllable.Append(GetConsonant(ConsonantOptions.Start));
        }
        syllable.Append(GetVowel());
        if (Random.value < END_CONSONANT_CHANCE)
        {
            syllable.Append(GetConsonant(ConsonantOptions.End));
        }

        return syllable.ToString();
    }

    private string GenerateLastSyllable()
    {
        return GetConsonant(ConsonantOptions.None) + "e";
    }

    private string GetConsonant(ConsonantOptions option)
    {
        string[] otherConsonants;
        if (option == ConsonantOptions.Start)
        {
            otherConsonants = nameParts.startConsonants;
        }
        else if(option == ConsonantOptions.End)
        {
            otherConsonants = nameParts.endConsonants;
        }
        else
        {
            otherConsonants = new string[0];
        }
        int cLen = nameParts.consonants.Length;
        int r = Random.Range(0, cLen + otherConsonants.Length);
        return r < cLen ? nameParts.consonants[r] : otherConsonants[r - cLen];
    }

    private string GetVowel()
    {
        int r = Random.Range(0, nameParts.vowels.Length);
        return nameParts.vowels[r];
    }

    private string GetSuffix()
    {
        int r = Random.Range(0, nameParts.suffixes.Length);
        return nameParts.suffixes[r];
    }
}
