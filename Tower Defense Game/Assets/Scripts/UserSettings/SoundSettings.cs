using UnityEngine;
using UnityEngine.UI;

//handles the sound music of the game settings
public class SoundSettings : MonoBehaviour
{
    [Header("Master, Sound, Music")]
    [SerializeField] private Text[] soundsPercentText = null;                   //the text that displays the percentages of sounds etc
    [SerializeField] private Slider[] soundsSliders = null;                     //the sliders that control the settings of sounds
    [SerializeField] private float[] sounds = new float[3];                     //the values sounds settings

    //loads default sound data
    private void Start()
    {
        LoadSoundData();
    }

    //Loads the sound data from the save file
    private void LoadSoundData()
    {
        float[] sounds = new float[3];
        sounds[0] = SaveDataManager.instance.Master;
        sounds[1] = SaveDataManager.instance.Sound;
        sounds[2] = SaveDataManager.instance.Music;
        for (int i = 0; i < soundsSliders.Length; i++)
        {
            soundsSliders[i].value = sounds[i];
            SetSound(i);
        }
    }

    //Sets the sound based on the selected slider, iteration referes to the id of the arrays such as sliders and sounds float
    public void SetSound(int iteration)
    {
        sounds[iteration] = soundsSliders[iteration].value;
        float calcValue = 0;
        calcValue = sounds[iteration] * 100;
        soundsPercentText[iteration].text = calcValue.ToString("0") + "%";
    }

    //Confirms sounds changes and saves them to our files
    public void ConfirmSoundSettings()
    {
        SaveDataManager.instance.SetMaster(sounds[0]);
        SaveDataManager.instance.SetSound(sounds[1]);
        SaveDataManager.instance.SetMusic(sounds[2]);
        SaveDataManager.instance.Save();
    }
}
