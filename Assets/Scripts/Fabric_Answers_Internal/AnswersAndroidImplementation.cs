// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

namespace Fabric.Answers.Internal
{
	internal class AnswersAndroidImplementation : IAnswers
	{
		public AnswersAndroidImplementation()
		{
			this.answersSharedInstance = new AnswersSharedInstanceJavaObject();
		}

		public void LogSignUp(string method, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("SignUpEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutSuccess(success);
			this.answersSharedInstance.Log("logSignUp", answersEventInstanceJavaObject);
		}

		public void LogLogin(string method, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LoginEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutSuccess(success);
			this.answersSharedInstance.Log("logLogin", answersEventInstanceJavaObject);
		}

		public void LogShare(string method, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("ShareEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.PutMethod(method);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			this.answersSharedInstance.Log("logShare", answersEventInstanceJavaObject);
		}

		public void LogInvite(string method, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("InviteEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.PutMethod(method);
			this.answersSharedInstance.Log("logInvite", answersEventInstanceJavaObject);
		}

		public void LogLevelStart(string level, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LevelStartEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putLevelName", level);
			this.answersSharedInstance.Log("logLevelStart", answersEventInstanceJavaObject);
		}

		public void LogLevelEnd(string level, double? score, bool? success, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("LevelEndEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putLevelName", level);
			answersEventInstanceJavaObject.InvokeSafelyAsDouble("putScore", score);
			answersEventInstanceJavaObject.PutSuccess(success);
			this.answersSharedInstance.Log("logLevelEnd", answersEventInstanceJavaObject);
		}

		public void LogAddToCart(decimal? itemPrice, string currency, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("AddToCartEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putItemPrice", itemPrice);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemName", itemName);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemId", itemId);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemType", itemType);
			this.answersSharedInstance.Log("logAddToCart", answersEventInstanceJavaObject);
		}

		public void LogPurchase(decimal? price, string currency, bool? success, string itemName, string itemType, string itemId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("PurchaseEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putItemPrice", price);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.PutSuccess(success);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemName", itemName);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemId", itemId);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putItemType", itemType);
			this.answersSharedInstance.Log("logPurchase", answersEventInstanceJavaObject);
		}

		public void LogStartCheckout(decimal? totalPrice, string currency, int? itemCount, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("StartCheckoutEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsDecimal("putTotalPrice", totalPrice);
			answersEventInstanceJavaObject.PutCurrency(currency);
			answersEventInstanceJavaObject.InvokeSafelyAsInt("putItemCount", itemCount);
			this.answersSharedInstance.Log("logStartCheckout", answersEventInstanceJavaObject);
		}

		public void LogRating(int? rating, string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("RatingEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsInt("putRating", rating);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			this.answersSharedInstance.Log("logRating", answersEventInstanceJavaObject);
		}

		public void LogContentView(string contentName, string contentType, string contentId, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("ContentViewEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.PutContentName(contentName);
			answersEventInstanceJavaObject.PutContentType(contentType);
			answersEventInstanceJavaObject.PutContentId(contentId);
			this.answersSharedInstance.Log("logContentView", answersEventInstanceJavaObject);
		}

		public void LogSearch(string query, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject answersEventInstanceJavaObject = new AnswersEventInstanceJavaObject("SearchEvent", customAttributes, new string[0]);
			answersEventInstanceJavaObject.InvokeSafelyAsString("putQuery", query);
			this.answersSharedInstance.Log("logSearch", answersEventInstanceJavaObject);
		}

		public void LogCustom(string eventName, Dictionary<string, object> customAttributes)
		{
			AnswersEventInstanceJavaObject eventInstance = new AnswersEventInstanceJavaObject("CustomEvent", customAttributes, new string[]
			{
				eventName
			});
			this.answersSharedInstance.Log("logCustom", eventInstance);
		}

		private AnswersSharedInstanceJavaObject answersSharedInstance;
	}
}
