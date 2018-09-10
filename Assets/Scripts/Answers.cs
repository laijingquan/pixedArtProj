// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using Fabric.Answers.Internal;
using UnityEngine;

namespace Fabric.Answers
{
	public class Answers : MonoBehaviour
	{
		private static IAnswers Implementation
		{
			get
			{
				if (Answers.implementation == null)
				{
					Answers.implementation = new AnswersAndroidImplementation();
				}
				return Answers.implementation;
			}
		}

		public static void LogSignUp(string method = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogSignUp(method, success, customAttributes);
		}

		public static void LogLogin(string method = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLogin(method, success, customAttributes);
		}

		public static void LogShare(string method = null, string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogShare(method, contentName, contentType, contentId, customAttributes);
		}

		public static void LogInvite(string method = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogInvite(method, customAttributes);
		}

		public static void LogLevelStart(string level = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLevelStart(level, customAttributes);
		}

		public static void LogLevelEnd(string level = null, double? score = null, bool? success = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogLevelEnd(level, score, success, customAttributes);
		}

		public static void LogAddToCart(decimal? itemPrice = null, string currency = null, string itemName = null, string itemType = null, string itemId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogAddToCart(itemPrice, currency, itemName, itemType, itemId, customAttributes);
		}

		public static void LogPurchase(decimal? price = null, string currency = null, bool? success = null, string itemName = null, string itemType = null, string itemId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogPurchase(price, currency, success, itemName, itemType, itemId, customAttributes);
		}

		public static void LogStartCheckout(decimal? totalPrice = null, string currency = null, int? itemCount = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogStartCheckout(totalPrice, currency, itemCount, customAttributes);
		}

		public static void LogRating(int? rating = null, string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogRating(rating, contentName, contentType, contentId, customAttributes);
		}

		public static void LogContentView(string contentName = null, string contentType = null, string contentId = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogContentView(contentName, contentType, contentId, customAttributes);
		}

		public static void LogSearch(string query = null, Dictionary<string, object> customAttributes = null)
		{
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogSearch(query, customAttributes);
		}

		public static void LogCustom(string eventName, Dictionary<string, object> customAttributes = null)
		{
			if (eventName == null)
			{
				UnityEngine.Debug.Log("Answers' Custom Events require event names. Skipping this event because its name is null.");
				return;
			}
			if (customAttributes == null)
			{
				customAttributes = new Dictionary<string, object>();
			}
			Answers.Implementation.LogCustom(eventName, customAttributes);
		}

		private static IAnswers implementation;
	}
}
