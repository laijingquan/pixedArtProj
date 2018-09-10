// dnSpy decompiler from Assembly-CSharp.dll
using System;

public static class SceneName
{
	public static string menu
	{
		get
		{
			if (GeneralSettings.IsOldDesign)
			{
				return "select";
			}
			return "select_new";
		}
	}

	public static string game
	{
		get
		{
			if (GeneralSettings.IsOldDesign)
			{
				return "game";
			}
			return "game_new";
		}
	}

	public const string start = "start";

	public const string menu_legacy = "select";

	public const string game_legacy = "game";

	public const string menu_neo = "select_new";

	public const string game_neo = "game_new";
}
