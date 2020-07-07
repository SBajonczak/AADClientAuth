# Anmerkungen
Diese Applikation erzeugt ein SSO zu der hinterlegten App in Azure. 

# Erforderliche Komponenten 

## Azure
Diese Applikation erfordert eine fertig konfigurierte Applikation in Azure Active Directory.

## .Net
Des Weiteren wird .net Core in der [Version 3.1](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) vorausgesetzt.
# Konfiguration

Bitte die appsettings.json entsprechend anpassen. 


|Wert|Erläuterung|
|-|-|
|ClientID|Dies ist die AnwendungsID. |
|TenantID|Dies ist die TenantId des Azure AD-Tenants|
|AuthorityUrl|Die Authority Url setzt sich zusammen aus "https://login.microsoftonline.com/`{TenantID}`". Wurde aber als separater Eintrag erstellt, da diese auch abweichend sein kann (Beispielsweise `Common`)|


# Starten der Applikation
Folgende Befehle werden ausgeführt 

1. Wiederherstellen der Pakete
`dotnet restore`

1. Ausführen 
`dotnet run`