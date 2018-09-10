// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTableParser : MonoBehaviour
{
	private void Awake()
	{
		LocalizationTableParser.Parse();
	}

	public static void Parse()
	{
		string[] array = LocalizationTableParser.input.Split(new char[]
		{
			'\n'
		});
		UnityEngine.Debug.Log("lines count " + array.Length);
		List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
		for (int i = 0; i < array[0].Length - 1; i++)
		{
			Dictionary<string, string> item = new Dictionary<string, string>();
			list.Add(item);
		}
		for (int j = 0; j < array.Length; j++)
		{
			string[] array2 = array[j].Split(new char[]
			{
				','
			});
			string key = array2[0];
			for (int k = 1; k < array2.Length; k++)
			{
				list[k - 1].Add(key, array2[k]);
			}
		}
		string text = string.Empty;
		for (int l = 0; l < list.Count; l++)
		{
			foreach (KeyValuePair<string, string> keyValuePair in list[l])
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					keyValuePair.Key,
					",",
					keyValuePair.Value,
					"\n"
				});
			}
			text += "\n\n";
		}
		UnityEngine.Debug.Log(text);
	}

	private static string input = "Lang,English,Chinese Simplified,Chinese Traditional,French,German,Italian,Japanese,Korean,Portuguese (Brazil),Spanish,Swedish,Thai,Turkish,Vietnamese\ndesign_upd_line_1,Check,查看,快來瞧,Allez,Schau,Dai un'occhiata ,新し,새 ,Confira ,¡Observa ,Kolla ,พบกั,Yeni,Khám phá \ndesign_upd_line_2,out our new,我们的新,瞧我們的全新,découvrir notre,dir unser neues ,al nostro nuovo ,いデザイン,디자인을 ,nosso design ,nuestro nuevo,in vår nya ,บดีไซน์ใหม่,tasarımımıza,thiết kế mới của\ndesign_upd_line_3,design!,设计！,遊戲設計！,nouveau design !,Design an!,design!,が登場！,확인해보세요!,novo!,diseño!,design!,ของเรา!,göz at!,chúng tôi!\ndesign_upd_body,You can switch to old design in settings,您可在设置中切换至旧设计,您也可在設定中回復原本的遊戲設計模式,Vous pouvez repasser à l'ancien design dans les réglages,Du kannst in den Einstellungen wieder zum alten Design wechseln,Puoi passare al vecchio design nelle impostazioni,設定で古いデザインに切り替えることができます,설정에서 기존 디자인으로 전환할 수 있습니다,Você pode mudar para o design antigo nas configurações,Puedes cambiar al diseño antiguo en los ajustes,Du kan byta till den gamla designen i inställningarna,คุณสามารถสลับไปเป็นดีไซน์เก่าได้ในการตั้งค่า,Eski tasarıma Ayarlar'dan geçiş yapabilirsin,Bạn có thể chuyển sang thiết kế cũ trong các thiết lập\nswitch_new_design,Switch to the new design,切换至新设计,切換成新設計模式,Passer au nouveau design,Zum neuen Design wechseln,Passa al nuovo design,新しいデザインに切り替える,새 디자인으로 전환,Mudar para o design novo,Cambiar al diseño nuevo,Byt till den nya designen,สลับไปเป็นดีไซน์ใหม่,Yeni tasarıma geçiş yap,Chuyển sang thiết kế mới\nswitch_legacy_design,Switch to the old design,切换至旧设计,切換成舊設計模式,Passer à l'ancien design,Zum alten Design wechseln,Passa al vecchio design,古いデザインに切り替える,기존 디자인으로 전환,Mudar para o design antigo,Cambiar al diseño antiguo,Byt till den gamla designen,สลับไปเป็นดีไซน์เ่ก่า,Eski tasarıma geçiş yap,Chuyển sang thiết kế cũ";
}
