<h2> # BotoxCore </h2>

Spécialement crée pour Dofus , mais il peut être utilisé sur beaucoup d'autre jeu ( il vous suffira d'implémenter le protocol spécifique à ce jeu ) ( si vous n'avez pas encore trouver une implémentation du protocol , vous pouvez vous en servir pour afficher les données qui transite )

Petite explication du code ( les 3 grand points ) :
  - Utilisation du SocketHook (https://cadernis.fr/index.php?threads/sockethook-injector-alternative-%C3%A0-no-ankama-dll.2221/page-2#post-24796) pour rediriger le client , que j'ai un peu modifié. ( Retrait de la condition de redirection d'IP, ducoup ça redirige toute les IP , puis ajout de la fonction IpRedirected() , qui sera appellé à chaque redirection de connection )
  - Le protocol Dofus à était pris depuis ce lien : https://cldine.gitlab.io/-/protocol-autoparser/-/jobs/691246963/artifacts/protocol.json
  - Le proxy :
La class CustomProxy est un serveur qui va gérez toute les connections d'un client Local ( dans notre cas c'est Dofus mais c'est pas spécifique à Dofus ), et la class ProxyElement gère une connection réceptionné par CustomProxy. ( La transition des packets se fait dans ProxyElement à partir des events ) 
A chaque connection sur le CustomProxy , un faux Client est connecté vers le serveur original ( récupérer via la fonction IpRedirected() )

<h2> Dependances </h2>
  -NLog (https://nlog-project.org/)</br>  
  -NewtonsoftJson (https://www.newtonsoft.com/json)</br>  
  -EasyHook (https://easyhook.github.io/)</br>

<h2> Utilisation </h2>

```csharp
// T est votre class dériveant de ProtocolTreatment ( la class qui gère le traitement du protocol ) dans le cas de Dofus c'est MessageInformation
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

<h2> Handler </h2>

Le handler se fait dans une class qui dérive de MessageHandler (https://github.com/Mrpotatosse/BotoxCore/blob/master/BotoxCore/Handlers/MessageHandler.cs)

Un exemple de class (https://github.com/Mrpotatosse/BotoxCore/blob/master/BotoxCore/Handlers/Customs/Connection/ProtocolRequiredHandler.cs) :

```csharp
// l'attribut est nécessaire au fonctionnement du handler ( si vous ne le mettez pas , la class ne sera pas reconnu comme étant un handler )
[Handler(ProtocolId = 1)]
public class ProtocolRequiredHandler : MessageHandler
{
    // pour le log
    protected override Logger logger => LogManager.GetCurrentClassLogger();

    // le constructeur doit présenter la même forme 
    // il est déconseillé de modifier le constructeur (ajouter d'autres arguments) étant donné que ça produira une erreur 
    public ProtocolRequiredHandler(CustomClient local, CustomClient remote, ProtocolJsonContent content) 
        : base(local, remote, content)
    {
    
    }

    // la fonction Handle est obligatoire
    public override void Handle()
    {
        logger.Info("handle protocol required");
    }
    // la fonction EndHandle est optionel
    public override void EndHandle()
    {
        
    }
    // la fonction Error est optionel
    public override void Error(Exception e)
    {

    }
}
```

( j'éditerai le readme un peu plus tard )
