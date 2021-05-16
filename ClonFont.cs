using BepInEx;
using BiendeoCHLib;
using BiendeoCHLib.Patches;
using BiendeoCHLib.Patches.Attributes;
using BiendeoCHLib.Wrappers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GUI;

namespace ClonFont
{
	[BepInPlugin("com.yoshiog.clonfont", "ClonFont", "1.0.0.0")]
	[BepInDependency("com.biendeo.biendeochlib")]
	public class ClonFont : BaseUnityPlugin
	{
		public static ClonFont Instance { get; private set; }
		private string sceneName;

		private TextMeshProUGUI currPhrase;
		private TextMeshProUGUI nextPhrase;
		private TextMeshProUGUI fadePhrase;
		private TextMeshProUGUI soloObj;

		public bool sceneChanged;

		private Font customFont;
		private string fontPath;
		private FileInfo fontFile;

		private string lyricFontName;
		private string hudSoloFontName;
		private TMP_FontAsset lyricsFontAss;
		private TMP_FontAsset hudSoloFontAss;
		
		
		public ClonFont()
        {
			Instance = this;
		}

		public TMP_FontAsset LoadFont(string ourFont)
        {
			fontPath = Path.Combine(Paths.BepInExRootPath, "plugins", "ClonFont", "Fonts", ourFont + ".ttf");
			fontFile = new FileInfo(fontPath);
			TMP_FontAsset customFontAsset;
			if (fontFile.Exists && ourFont.ToLower() != "default")
			{
				customFont = new Font(fontPath);
				customFontAsset = TMP_FontAsset.CreateFontAsset(customFont);
				return customFontAsset;
			}
			else
            {
				return null;
            }
		}

		public void ReloadFontConfig()
        {
			string fontConfigPath = Path.Combine(Paths.BepInExRootPath, "plugins", "ClonFont", "fonts.ini");
			if (!File.Exists(fontConfigPath))
            {
				IniBruv.IniWriteValue("Fonts", "LyricsFont", "default");
				IniBruv.IniWriteValue("Fonts", "HUDSoloFont", "NOT CURRENTLY WORKING");
            }
			lyricFontName = IniBruv.IniReadValue("Fonts", "LyricsFont");
			hudSoloFontName = IniBruv.IniReadValue("Fonts", "HUDSoloFont");
			
			lyricsFontAss = LoadFont(lyricFontName);
			hudSoloFontAss = LoadFont(hudSoloFontName);
			// TODO: implement menu font and others?
		}

		#region Unity Methods
		public void Start()
		{
			
			SceneManager.activeSceneChanged += delegate (Scene _, Scene __)
			{
				sceneChanged = true;
			};
			ReloadFontConfig();
		}

		public void LateUpdate()
		{
			sceneName = SceneManager.GetActiveScene().name;
			if (sceneName == "Gameplay")
			{
				if (lyricsFontAss != null)
				{
					currPhrase = GameObject.Find("CurrentPhrase").GetComponent<TextMeshProUGUI>();
					nextPhrase = GameObject.Find("NextPhrase").GetComponent<TextMeshProUGUI>();
					fadePhrase = GameObject.Find("fadePhrase").GetComponent<TextMeshProUGUI>();
					currPhrase.font = lyricsFontAss;
					nextPhrase.font = lyricsFontAss;
					fadePhrase.font = lyricsFontAss;
				}
				if (hudSoloFontAss != null)
                {
					// Doesn't seem to work atm, I'll have to figure out the solo HUD's object name
					soloObj = GameObject.Find("Solo Counter").GetComponent<TextMeshProUGUI>();
					soloObj.font = hudSoloFontAss;
                }
			}
			if (this.sceneChanged)
            {
				ReloadFontConfig();
				this.sceneChanged = false;
			}
		}
		#endregion
	}
}