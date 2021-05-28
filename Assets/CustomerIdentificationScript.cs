using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class CustomerIdentificationScript: MonoBehaviour
{
	public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
	public AudioSource SecondMusic;
	
	public KMSelectable[] TypableText;
	public KMSelectable[] ShiftButtons;
	public KMSelectable[] UselessButtons;
	public KMSelectable Backspace;
	public KMSelectable Enter;
	public KMSelectable SpaceBar;
	public KMSelectable Border;
	
	public MeshRenderer[] BorderAndTile;
	public Material[] Chapters;
	public SpriteRenderer SeedPacket;
	public Sprite[] SeedPacketIdentifier;
	public Sprite DefaultSprite;
	public Sprite DeathSprite;
	public Material[] ImageLighting;
	
	public MeshRenderer[] LightBulbs;
	public Material[] TheLights;
	
	public TextMesh[] Text;
	public TextMesh TextBox;
	public GameObject TheBox;
	public SpriteRenderer AnotherShower;
	public SpriteRenderer AnotherAnotherShower;
	public Sprite ThumbsUp;
	
	public GameObject[] IShow;
	
	bool Shifted = false;
	
	public AudioClip[] NotBuffer;
	public AudioClip[] Buffer;
	
	string[][] ChangedText = new string[2][]{
		new string[47] {"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/"},
		new string[47] {"~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|", "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"}
	};
	
	int[] Unique = {0, 0, 0};
	
	bool Playable = false;
	bool Enterable = false;
	bool Toggleable = true;
	int Stages = 0;
	
	int ChapterNumber;
	
	//Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;

	void Awake()
	{
		moduleId = moduleIdCounter++;
		for (int b = 0; b < TypableText.Count(); b++)
        {
            int KeyPress = b;
            TypableText[KeyPress].OnInteract += delegate
            {
                TypableKey(KeyPress);
				return false;
            };
        }
		
		for (int a = 0; a < ShiftButtons.Count(); a++)
        {
            int Shifting = a;
            ShiftButtons[Shifting].OnInteract += delegate
            {
                PressShift(Shifting);
				return false;
            };
        }
		
		for (int c = 0; c < UselessButtons.Count(); c++)
        {
            int Useless = c;
            UselessButtons[Useless].OnInteract += delegate
            {
                UselessButtons[Useless].AddInteractionPunch(.2f);
				Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
				return false;
            };
        }
		
		Backspace.OnInteract += delegate () { PressBackspace(); return false; };
		Enter.OnInteract += delegate () { PressEnter(); return false; };
		SpaceBar.OnInteract += delegate () { PressSpaceBar(); return false; };
		Border.OnInteract += delegate () { PressBorder(); return false; };
	}
	
	
	void Start()
	{
		this.GetComponent<KMSelectable>().UpdateChildren();
		UniquePlay();
		Module.OnActivate += Introduction;
	}
	
	void Introduction()
	{
		StartCoroutine(Reintroduction());
	}
	
	void UniquePlay()
	{
		for (int c = 0; c < Unique.Count(); c++)
        {
            Unique[c] = UnityEngine.Random.Range(0, SeedPacketIdentifier.Count());
        }
		
		if (Unique[0] == Unique[1] || Unique[0] == Unique[2] || Unique[1] == Unique[2])
		{
			UniquePlay();
		}
	}
	
	IEnumerator Reintroduction()
	{
		Intro = true;
		Debug.LogFormat("[Customer Identification #{0}] Play the sting!", moduleId);
		SecondMusic.clip = NotBuffer[0];
		SecondMusic.Play();
        while (SecondMusic.isPlaying)
		{
			yield return new WaitForSecondsRealtime(0.01f);
		}
		Playable = true;
		Intro = false;
	}
	
	void TypableKey(int KeyPress)
	{
		TypableText[KeyPress].AddInteractionPunch(.2f);
		Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
		if (Playable && Enterable)
		{
			float width = 0;
			foreach (char symbol in TextBox.text)
			{
				CharacterInfo info;
				if (TextBox.font.GetCharacterInfo(symbol, out info, TextBox.fontSize, TextBox.fontStyle))
				{
					width += info.advance;
				}
			}
			width =  width * TextBox.characterSize * 0.1f;
			
			if (width < 0.28f)
			{
				TextBox.text += Text[KeyPress].text;
				if (width > 0.28)
				{
					string Copper = TextBox.text;
					Copper = Copper.Remove(Copper.Length - 1);
					TextBox.text = Copper;
				}
			}
		}
	}
	
	void PressBackspace()
	{
		Backspace.AddInteractionPunch(.2f);
		Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
		if (Playable)
		{
			if (TextBox.text.Length != 0)
			{
				string Copper = TextBox.text;
				Copper = Copper.Remove(Copper.Length - 1);
				TextBox.text = Copper;
			}
		}
	}
	
	void PressSpaceBar()
	{
		SpaceBar.AddInteractionPunch(.2f);
		Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
		if (Playable && Enterable)
		{
			float width = 0;
			foreach (char symbol in TextBox.text)
			{
				CharacterInfo info;
				if (TextBox.font.GetCharacterInfo(symbol, out info, TextBox.fontSize, TextBox.fontStyle))
				{
					width += info.advance;
				}
			}
			width =  width * TextBox.characterSize * 0.1f;
			
			if (width < 0.28f)
			{
				TextBox.text += " ";
				if (width > 0.28)
				{
					string Copper = TextBox.text;
					Copper = Copper.Remove(Copper.Length - 1);
					TextBox.text = Copper;
				}
			}
		}
	}
	
	void PressBorder()
	{
		Border.AddInteractionPunch(.2f);
		if (Playable && Toggleable)
		{
			StartCoroutine(PlayTheQueue());
		}
	}
	
	void PressEnter()
	{
		Enter.AddInteractionPunch(.2f);
		Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
		if (Playable && Enterable)
		{
			StartCoroutine(TheCorrect());
		}
	}
	
	void PressShift(int Shifting)
	{
		ShiftButtons[Shifting].AddInteractionPunch(.2f);
		Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
		if (Shifted == true)
		{
			Shifted = false;
			StartingNumber = 0;
		}
		
		else
		{
			Shifted = true;
			StartingNumber = 1;
		}
		
		if (Shifted == true)
		{
			for (int b = 0; b < Text.Count(); b++)
			{
				Text[b].text = ChangedText[1][b];
			}
		}
		
		else
		{
			for (int a = 0; a < Text.Count(); a++)
			{
				Text[a].text = ChangedText[0][a];
			}
		}
	}
	
	IEnumerator PlayTheQueue()
	{
		Toggleable = false;
		ActiveBorder = true;
		Playable = false;
		Debug.LogFormat("[Customer Identification #{0}] The name of the customer shown: {1}", moduleId, SeedPacketIdentifier[Unique[Stages]].name);
		SeedPacket.sprite = SeedPacketIdentifier[Unique[Stages]];
		SeedPacket.material = ImageLighting[1];
		yield return new WaitForSecondsRealtime(7.5f);
		SeedPacket.sprite = DefaultSprite;
		SeedPacket.material = ImageLighting[0];
		Playable = true;
		ActiveBorder = false;
		Enterable = true;
	}
	
	IEnumerator TheCorrect()
	{
		string Analysis = TextBox.text;
		TextBox.text = "";
		Debug.LogFormat("[Customer Identification #{0}] Text that was submitted: {1}", moduleId, Analysis);
			if (Analysis  == SeedPacketIdentifier[Unique[Stages]].name)
			{
				Stages++;
				Playable = false;
				Enterable = false;
				if (Stages == 3)
				{
					Animating1 = true;
					Debug.LogFormat("[Customer Identification #{0}] You solved the module three times in a row. GG!", moduleId);
					SecondMusic.clip = NotBuffer[8];
					SecondMusic.Play();
					StartCoroutine(RoulleteToWin());
					while (SecondMusic.isPlaying)
					{
						LightBulbs[0].material = TheLights[0];
						LightBulbs[1].material = TheLights[0];
						LightBulbs[2].material = TheLights[1];
						yield return new WaitForSecondsRealtime(0.02f);
						LightBulbs[0].material = TheLights[0];
						LightBulbs[1].material = TheLights[1];
						LightBulbs[2].material = TheLights[0];
						yield return new WaitForSecondsRealtime(0.02f);
						LightBulbs[0].material = TheLights[1];
						LightBulbs[1].material = TheLights[0];
						LightBulbs[2].material = TheLights[0];
						yield return new WaitForSecondsRealtime(0.02f);
					}
					LightBulbs[0].material = TheLights[1];
					LightBulbs[1].material = TheLights[1];
					LightBulbs[2].material = TheLights[1];
					Debug.LogFormat("[Customer Identification #{0}] The module is done.", moduleId);
					Module.HandlePass();
					Animating1 = false;
				}
			
				else
				{
					Debug.LogFormat("[Customer Identification #{0}] The text matches the name of the customer. Good job!", moduleId);
					Animating1 = true;
					AnotherShower.sprite = SeedPacketIdentifier[Unique[Stages-1]];
					int Decider = UnityEngine.Random.Range(0,2); if (Decider == 1) SecondMusic.clip = NotBuffer[2];  else SecondMusic.clip = NotBuffer[4];
					SecondMusic.Play();
					while (SecondMusic.isPlaying)
					{
						yield return new WaitForSecondsRealtime(0.075f);
					}
					LightBulbs[Stages-1].material = TheLights[1];
					SeedPacket.sprite = DefaultSprite;
					Playable = true;
					Toggleable = true;
					Animating1 = false;
				}
			}
			
			else
			{
				Debug.LogFormat("[Customer Identification #{0}] The text does not match the name of the customer. Oh no!", moduleId);
				Animating1 = true;
				SecondMusic.clip = NotBuffer[5 + UnityEngine.Random.Range(0, 3)];
				SecondMusic.Play();
				Enterable = false;
				LightBulbs[0].material = TheLights[2];
				LightBulbs[1].material = TheLights[2];
				LightBulbs[2].material = TheLights[2];
				Debug.LogFormat("[Customer Identification #{0}] Strike!", moduleId);
				while (SecondMusic.isPlaying)
				{
					yield return new WaitForSecondsRealtime(0.075f);
				}
				SeedPacket.sprite = DefaultSprite;
				LightBulbs[0].material = TheLights[0];
				LightBulbs[1].material = TheLights[0];
				LightBulbs[2].material = TheLights[0];
				Playable = true;
				Toggleable = true;
				Animating1 = false;
				Stages = 0;
				Module.HandleStrike();
				Debug.LogFormat("[Customer Identification #{0}] The module resetted and striked as a cost for giving an incorrect answer.", moduleId);
				UniquePlay();
			}
	}
	
	IEnumerator RoulleteToWin()
	{
		while (SecondMusic.isPlaying)
		{
			for (int x = 0; x < 3; x++)
			{
				AnotherShower.sprite = SeedPacketIdentifier[Unique[x]];
				yield return new WaitForSecondsRealtime(0.2f);
			}
		}
	}
	
	//twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"To press the border in the module, use the command !{0} play, or !{0} playfocus | To type a text in the text box, use the command !{0} type <text> | To submit the text in the text box, use the command !{0} submit | To clear the text in the text box, use the command !{0} clear, or !{0} fastclear";
    #pragma warning restore 414
	
	int StartingNumber = 0;
	bool Intro = false;
	bool ActiveBorder = false;
	bool Animating1 = false;
	string Current = "";
	
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] parameters = command.Split(' ');
		if (Regex.IsMatch(parameters[0], @"^\s*type\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The keys are not yet pressable. Command was ignored";
				yield break;
			}
			
			for (int x = 0; x < parameters.Length - 1; x++)
			{
				foreach (char c in parameters[x+1])
				{
					if (!c.ToString().EqualsAny(ChangedText[0]) && !c.ToString().EqualsAny(ChangedText[1]))
					{
						yield return "sendtochaterror The command being submitted contains a character that is not typable in the given keyboard";
						yield break;
					}
				}
			}
			
			for (int y = 0; y < parameters.Length - 1; y++)
			{
				yield return "trycancel The command to type the text given was halted due to a cancel request";
				foreach (char c in parameters[y+1])
				{
					yield return "trycancel The command to type the text given was halted due to a cancel request";
					Current = TextBox.text;
					if (!c.ToString().EqualsAny(ChangedText[StartingNumber]))
					{
						ShiftButtons[0].OnInteract();
						yield return new WaitForSeconds(0.05f);
					}
					
					for (int z = 0; z < ChangedText[StartingNumber].Count(); z++)
					{
						if (c.ToString() == ChangedText[StartingNumber][z])
						{
							TypableText[z].OnInteract();
							yield return new WaitForSeconds(0.05f);
							break;
						}
					}
					
					if (Current == TextBox.text)
					{
						yield return "sendtochaterror The command was stopped due to the text box not able to recieve more characters";
						yield break;
					}
				}

				if (y != parameters.Length - 2)
				{
					SpaceBar.OnInteract();
					yield return new WaitForSeconds(0.05f);
				}
				
				if (Current == TextBox.text)
				{
					yield return "sendtochaterror The command was stopped due to the text box not able to recieve more characters";
					yield break;
				}
			}
		}
		
		else if (Regex.IsMatch(command, @"^\s*clear\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			
			while (TextBox.text.Length != 0)
			{
				yield return "trycancel The command to clear text in the text box was halted due to a cancel request";
				Backspace.OnInteract();
				yield return new WaitForSeconds(0.05f);
			}
		}
		
		else if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			yield return "solve";
			yield return "strike";
				Enter.OnInteract();
		}
		
		else if (Regex.IsMatch(command, @"^\s*play\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored.";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == true)
			{
				yield return "sendtochaterror You are not able to press the border again. Command was ignored";
				yield break;
			}
			
			Border.OnInteract();
		}
		
		else if (Regex.IsMatch(command, @"^\s*playfocus\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == true)
			{
				yield return "sendtochaterror You are not able to press the border again. Command was ignored";
				yield break;
			}
			
			Border.OnInteract();
			while (Playable == false)
			{
				yield return new WaitForSeconds(0.02f);
			}
		}
		
		else if (Regex.IsMatch(command, @"^\s*fastclear\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Intro == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored.";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still showing the seed packet. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			
			while (TextBox.text.Length != 0)
			{
				Backspace.OnInteract();
			}
		}
	}
}
	
