// dnSpy decompiler from Assembly-CSharp.dll
using System;

public interface IConsentProvider
{
	event Action ConsentInitialized;

	string PolicyURL { get; }

	string VendorsURL { get; }

	FAdsNetwork.ConsentStatus CurrentConsentStatus { get; }

	void InitForConsent(string adUnit);

	void GrantConsent();

	bool ShouldShowConsentDialog { get; }

	bool? IsGDPRApplicable { get; }
}
