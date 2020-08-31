<h2> # BotoxCore </h2>

Spécialement crée pour Dofus , mais il peut être utilisé sur beaucoup d'autre jeu ( il vous suffira d'implémenter le protocol spécifique à ce jeu )

Petite explication du code ( les 3 grand points ) :
  - Utilisation du SocketHook (https://cadernis.fr/index.php?threads/sockethook-injector-alternative-%C3%A0-no-ankama-dll.2221/page-2#post-24796) pour rediriger le client , que j'ai un peu modifié. ( Retrait de la condition de redirection d'IP, ducoup ça redirige toute les IP , puis ajout de la fonction IpRedirected() , qui sera appellé à chaque redirection de connection )
  - Le protocol Dofus à était pris depuis ce lien : https://cldine.gitlab.io/-/protocol-autoparser/-/jobs/691246963/artifacts/protocol.json
  - Le proxy :
La class CustomProxy est un serveur qui va gérez toute les connections d'un client Local ( dans notre cas c'est Dofus mais c'est pas spécifique à Dofus ), et la class ProxyElement gère une connection réceptionné par CustomProxy. ( La transition des packets se fait dans ProxyElement à partir des events )

<h2> Dependances </h2>
  -NLog (https://nlog-project.org/)
  -NewtonsoftJson (https://www.newtonsoft.com/json)
  -EasyHook (https://easyhook.github.io/)

<h2> Utilisation </h2>

```csharp
// T is your ProtocolTreatment class ( for Dofus it's BotoxMessageInformation )
// ( https://github.com/Mrpotatosse/BotoxCore/blob/master/BotoxDofusProtocol/Protocol/MessageInformation.cs ) 
Hooker<T> hooker = HookManager<T>.Instance.CreateHooker();
hooker.Inject();
```

<h2> Configuration </h2>
La configuration '/.startup.json' ( dans le dossier bin/éxécutable ) est créer automatiquement
```json
{
  "dofus_location": "D:/DofusApp/Dofus.exe",
  "dll_location": "./SocketHook.dll",
  "default_proxy_port": 666,
  "show_log": true,
  "save_log": true,
  "show_message": true,
  "show_message_content": true,
  "show_data": false
}
```
  
( j'éditerai le readme un peu plus tard )
