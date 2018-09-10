// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AdjustTokenParser
{
	public static void ParseToDict()
	{
		UnityEngine.Debug.Log("android");
		AdjustTokenParser.ParseStrToDict(AdjustTokenParser.android);
		UnityEngine.Debug.Log("ios");
		AdjustTokenParser.ParseStrToDict(AdjustTokenParser.ios);
	}

	public static void ParseStrToDict(string data)
	{
		string[] array = data.Split(new char[]
		{
			'\n'
		});
		UnityEngine.Debug.Log("lines " + array.Length);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				','
			});
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"{\"",
				array2[0],
				"\",\"",
				array2[1],
				"\"},\n"
			});
		}
		UnityEngine.Debug.Log(text);
	}

	private static string android = "ad_assert,pyu08a\nad_banner_click,v8uyzf\nad_banner_impression,r50nsh\nad_interstitial_click,wfewu9\nad_interstitial_impression,x794nx\nad_interstitial_impression_callback,duckix\nad_interstitial_loaded1st,iwp0hd\nad_interstitial_potential_gameScreen,rrka40\nad_removed,uu346s\nad_rewarded_canceled,g2bmq8\nad_rewarded_click,13bgpe\nad_rewarded_finished,1rtfvv\nad_rewarded_impression,xh8ty1\nad_rewarded_loaded,utke4z\nad_rewarded_request,40tt8k\nAD_TIPS_BONUS_SHOWN,9yelnv\nAPPLICATION_OPEN,na1a48\ncoloring_animation_skip,fhn5y3\ncoloring_continue,7wdojd\ncoloring_finished,htk8ea\ncoloring_finished_daily,8g075v\ncoloring_restart,cl6x72\ncoloring_shared,idvg67\ncoloring_start,fhszdx\ncoloring_start_daily,ex25qv\ndailyList_featured_daily,yk4ty1\ndailyList_tap,ytx2vu\ndb_creation_error,nvpowx\ndeepLink_bonusClaimed,vtkso3\ndeepLink_bonusError,fvipdt\ndeepLink_queueError,5gyig7\nerror_firebase_messaging,2rxnf0\ngameScreen_hint,cfpfaj\ngameScreen_magicHint,we735i\nINAPP_PURCHASED,lxdpj0\nm_mins_a_day_10,5ptmip\nm_mins_a_day_20,l3sm95\nm_mins_a_day_40,myfgx6\nm_mins_a_day_60,oyu5kx\nm_open_inrow_14d,j76sj6\nm_open_inrow_28d,8ba7my\nm_open_inrow_3d,ik4bza\nm_open_inrow_5d,e7jpao\nm_open_inrow_7d,niq788\nm_pics_finished_100,2yhn8k\nm_pics_finished_15,df14bj\nm_pics_finished_30,pfvn9d\nm_pics_finished_5,7t5b8k\nm_pics_finished_50,2j3aq6\nm_watch_end_10,1mpdv0\nmainList_bonusEmpty_findMore,sfu15q\nmainList_featured_daily,o9ziz0\nmainList_featured_editors,c95xko\nmainList_featured_link,yrj0ez\nmainList_filter_categoryBonus,t241gi\nmainList_filter_categroy,1433n3\nmainList_filter_findMore,rrk6hx\nmainList_filter_hide_off,63afuz\nmainList_filter_hide_on,3u2x3o\nmainList_tap,k9wzyp\nmainList_tapBonus,os6c3z\nmainList_tapSearch,fdoxp2\nrateit_fromMenu,5t1rd8\nrateit_popup,v2h0nx\nrateit_popup_cancel,njlz3v\nrateit_popup_confirm,jr9vbl";

	private static string ios = "ad_assert,fqggav\nad_banner_cached,pd86qd\nad_banner_click,augo3j\nad_banner_failed,s9o11f\nad_banner_impression,x0hh1c\nad_interstitial_cached,vpdt5q\nad_interstitial_click,s46qgj\nad_interstitial_failed,g5p6r6\nad_interstitial_show,h5ueh9\nad_interstitial_skipByInterDelay,401bvj\nad_interstitial_skipByRewardedDelay,q5fqgm\nad_removed,d4tk1l\nad_rewarded_cached,n4g47w\nad_rewarded_click,cy20bn\nad_rewarded_didexpire,gabu5x\nad_rewarded_failed,vjhmme\nad_rewarded_failtoplay,cs96a3\nad_rewarded_shouldreward,6rjvf8\nad_rewarded_show,u90otv\nAD_TIPS_BONUS_SHOWN,uinpv8\nAPPLICATION_OPEN,ajinoq\ncoloring_animation_skip,td8a41\ncoloring_start_daily,sy7eos\ndailyList_featured_daily,qc102d\ndb_creation_error,oc69xz\ndeepLink_bonusClaimed,noha5s\ndeepLink_bonusError,cutd5k\ndeepLink_queueError,1lufdh\nerror_firebase_messaging,bogztw\ngameScreen_hint,fc9gc6\ngameScreen_magicHint,kxp2rh\nINAPP_PURCHASED,kidmqd\nm_mins_a_day_10,47hmkv\nm_mins_a_day_20,c2gev6\nm_mins_a_day_40,7pcidy\nm_mins_a_day_60,f1fm15\nm_open_inrow_14d,diblzc\nm_open_inrow_28d,852v6h\nm_open_inrow_3d,s16s35\nm_open_inrow_5d,29xr7k\nm_open_inrow_7d,wknsha\nm_pics_finished_100,ftsjiu\nm_pics_finished_15,9hrz62\nm_pics_finished_30,v6derr\nm_pics_finished_5,vc2dmm\nm_pics_finished_50,p1j1rl\nm_watch_end_10,jimhzq\nmainList_bonusEmpty_findMore,9ldlbf\nmainList_featured_daily,2cycbh\nmainList_featured_editors,pr0rol\nmainList_featured_link,2blm08\nmainList_filter_categoryBonus,62rttq\nmainList_filter_categroy,u6szjk\nmainList_filter_findMore,51fl04\nmainList_filter_hide_off,d4bpgk\nmainList_filter_hide_on,5z7jsy\nmainList_tapBonus,ma9vbm\nmainList_tapSearch,4kurbh\nrateit_fromMenu,h6p3ck\nrateit_popup,6jiexr\nrateit_popup_cancel,3eou6e\nrateit_popup_confirm,kafuu6";
}
