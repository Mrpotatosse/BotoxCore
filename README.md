<h2> # BotoxCore </h2>

Spécialement crée pour Dofus , mais il peut être utilisé sur beaucoup d'autre jeu ( il vous suffira d'implémenter le protocol spécifique à ce jeu )

Petite explication du code ( les 3 grand points ) :
  - Utilisation du SocketHook (https://cadernis.fr/index.php?threads/sockethook-injector-alternative-%C3%A0-no-ankama-dll.2221/page-2#post-24796) pour rediriger le client , que j'ai un peu modifié. ( Retrait de la condition de redirection d'IP, ducoup ça redirige toute les IP , puis ajout de la fonction IpRedirected() , qui sera appellé à chaque redirection de connection )
  - Le protocol Dofus à était pris depuis ce lien : https://cldine.gitlab.io/-/protocol-autoparser/-/jobs/691246963/artifacts/protocol.json
  - Le proxy :
La class CustomProxy est un serveur qui va gérez toute les connections du client Dofus, et la class ProxyElement gère une connection réceptionné par CustomProxy. ( La transition des packets se fait dans ProxyElement à partir des events )

( j'éditerai le readme un peu plus tard )

