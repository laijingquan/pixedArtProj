// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fabric.Answers.Internal
{
	internal class AnswersEventInstanceJavaObject
	{
		public AnswersEventInstanceJavaObject(string eventType, Dictionary<string, object> customAttributes, params string[] args)
		{
			this.javaObject = new AndroidJavaObject(string.Format("com.crashlytics.android.answers.{0}", eventType), args);
			foreach (KeyValuePair<string, object> keyValuePair in customAttributes)
			{
				string key = keyValuePair.Key;
				object value = keyValuePair.Value;
				if (value == null)
				{
					UnityEngine.Debug.Log(string.Format("[Answers] Expected custom attribute value to be non-null. Received: {0}", value));
				}
				else if (AnswersEventInstanceJavaObject.IsNumericType(value))
				{
					this.javaObject.Call<AndroidJavaObject>("putCustomAttribute", new object[]
					{
						key,
						AnswersEventInstanceJavaObject.AsDouble(value)
					});
				}
				else if (value is string)
				{
					this.javaObject.Call<AndroidJavaObject>("putCustomAttribute", new object[]
					{
						key,
						value
					});
				}
				else
				{
					UnityEngine.Debug.Log(string.Format("[Answers] Expected custom attribute value to be a string or numeric. Received: {0}", value));
				}
			}
		}

		public void PutMethod(string method)
		{
			this.InvokeSafelyAsString("putMethod", method);
		}

		public void PutSuccess(bool? success)
		{
			this.InvokeSafelyAsBoolean("putSuccess", success);
		}

		public void PutContentName(string contentName)
		{
			this.InvokeSafelyAsString("putContentName", contentName);
		}

		public void PutContentType(string contentType)
		{
			this.InvokeSafelyAsString("putContentType", contentType);
		}

		public void PutContentId(string contentId)
		{
			this.InvokeSafelyAsString("putContentId", contentId);
		}

		public void PutCurrency(string currency)
		{
			this.InvokeSafelyAsCurrency("putCurrency", currency);
		}

		public void InvokeSafelyAsCurrency(string methodName, string currency)
		{
			if (currency != null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Currency");
				AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[]
				{
					currency
				});
				this.javaObject.Call<AndroidJavaObject>("putCurrency", new object[]
				{
					androidJavaObject
				});
			}
		}

		public void InvokeSafelyAsBoolean(string methodName, bool? arg)
		{
			if (arg != null)
			{
				this.javaObject.Call<AndroidJavaObject>(methodName, new object[]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsInt(string methodName, int? arg)
		{
			if (arg != null)
			{
				this.javaObject.Call<AndroidJavaObject>(methodName, new object[]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsString(string methodName, string arg)
		{
			if (arg != null)
			{
				this.javaObject.Call<AndroidJavaObject>(methodName, new object[]
				{
					arg
				});
			}
		}

		public void InvokeSafelyAsDecimal(string methodName, object arg)
		{
			if (arg != null)
			{
				this.javaObject.Call<AndroidJavaObject>(methodName, new object[]
				{
					new AndroidJavaObject("java.math.BigDecimal", new object[]
					{
						arg.ToString()
					})
				});
			}
		}

		public void InvokeSafelyAsDouble(string methodName, object arg)
		{
			if (arg != null)
			{
				this.javaObject.Call<AndroidJavaObject>(methodName, new object[]
				{
					AnswersEventInstanceJavaObject.AsDouble(arg)
				});
			}
		}

		private static bool IsNumericType(object o)
		{
			switch (Type.GetTypeCode(o.GetType()))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			default:
				return false;
			}
		}

		private static AndroidJavaObject AsDouble(object param)
		{
			return new AndroidJavaObject("java.lang.Double", new object[]
			{
				param.ToString()
			});
		}

		public AndroidJavaObject javaObject;
	}
}
