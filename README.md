Application de simulation moléculaire

Objectif de l'application

L'objectif de cette application est de simuler des interactions moléculaires en utilisant Unity. L'utilisateur peut contrôler une caméra pour explorer l'environnement et observer les molécules en mouvement. Des molécules peuvent également être générées à partir d'atomes. 

Contrôles principaux de la caméra

Utilisez les touches ZQSD pour vous déplacer en avant, à gauche, en arrière et à droite.
Maintenez le clic droit de la souris enfoncé et déplacez la souris pour effectuer une rotation de la caméra.
Utilisez les touches Left Ctrl et Left Shift pour descendre et monter respectivement.
Appuyez sur la touche T pour réinitialiser la position et la rotation de la caméra.
Appuyez sur la touche Escape accéder au menu de l'application.

Fonctionnalités principales

L'application propose un navigateur de caméra permettant à l'utilisateur de se déplacer dans l'environnement de simulation moléculaire. La caméra peut être déplacée dans différentes directions à l'aide des touches de contrôle mentionnées précédemment.

L'application permet également la génération de molécules. Les molécules sont créées lorsque des atomes se rencontrent, où elles interagissent selon des règles physiques. Cela permet à l'utilisateur d'observer les comportements et les interactions des molécules en temps réel.

Fonctionnalités supplémentaires

En plus des fonctionnalités principales, l'application propose les éléments suivants :

Interface utilisateur : Une interface utilisateur conviviale facilite l'accès aux différentes fonctionnalités de l'application, telles que la génération de molécules, les paramètres de simulation, etc.

Cliquer sur un atome ou sur une molécule permet par ailleurs d'afficher les spécificités de l'objet en question, spécificités renseignées dans les classes objets Atom et Melecule.

Système d'événements : Un système d'événements est intégré pour permettre la détection et la gestion d'événements importants, tels qu'un journal d'évenement mais aussi en interne pour lier l'UI est la logique.

Pistes d'amélioration : 

J'aurai souhaité ajouter d'autres fonctionnalités pour rendre l'application plus intéressante :
- des presets d'atomes différents générants différentes molécules.
- un bouton rewind pour revenir sur un évenement comme une fusion d'atomes.
- une UI perméttant de modifier les inputs de la caméra.
- la possibilité d'attraper et de bouger les objets physiques dans l'environnement. 
- pouvoir focaliser la caméra sur un objet et l'inspécter à 360° en laissant la simulation tourner.
- pouvoir ajouter des atomes dans la simulation en temps réel.

J'ai souhaité, par cette application mettre en avant des nombreux preceptes de Unity utiles à la mise d'application gamifiées ou de jeu vidéo :
- POO (héritage)
- Architecture event base 
- Paterns (ici Singleton)
- UI
- Technique intégration Unity : utilisation des focntionnalités physiques du moteur