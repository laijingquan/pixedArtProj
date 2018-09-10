// dnSpy decompiler from Assembly-CSharp.dll
using System;

public interface IConsentManager
{
	string PolicyUrl { get; }

	string VendorUrl { get; }

	event Action<bool> OnResult;

	void Check();

	void Clean();

	void GrantConsent();
}
