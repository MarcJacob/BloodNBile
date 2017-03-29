# BloodNBile
Projet tutoré 2017 - 2018 IUT informatique Lyon 1 PIPERNO PICON-BRAVO TOULIER DALERY JACOB

Notes pour les développeurs :

- Hiérarchie de dossiers :
    - "Bulk" contient le plus gros des Assets : modèles, sons, textures...
    L'ensemble des données qui n'ont pas besoin d'être chargées durant le jeu.
    
    - "Resources" contient les assets crées à partir des éléments de Bulk qui auront besoin de
    pouvoir être chargés durant le jeu.
    
    - "Sources" contient le code source du jeu.
    
    Notez que chaque développeur doit notifier les autres avant de créer plus de dossiers dans un dossier utilisé par d'autres développeurs au même moment, exactement comme quand il modifie des fichiers. Cela permettre d'éviter les conflits.
    
- Fichiers :

    - En fonction des tâches assignées à chaque développeur, il devra créer des fichiers et en modifier quelques uns.
    
    - Si un développeur doit modifier un fichier qui sera peut être modifié par un autre, il faut trouver un accord sur qui crée quelles fonctions. S'il y a besoin d'une même fonction par encore écrite des deux côtés, il peut y avoir changement / redéfinition des tâches
    assignées. Si ce n'est pas possible alors il faudra se préparer à régler le conflit à la main après le merge.
    
    - Dans la mesure du possible, il ne faudra pas que les développeurs aient accès au travail des autres avant le prochain commit master.
    
    - Si quelqu'un a terminé un assez grand nombre de tâche et a atteint un "milestone", alors il doit attendre que les autres aient fait de même avant de continuer de travailler. Il est donc conseillé, pour les plus motivés, de faire un petit projet à côté pour s'améliorer sur Unity ou autres logiciels en attendant.
    
- Tâches :

    - Les tâches sont assignées par consensus du groupe, généralement une personne par catégorie ou sous catégorie pour éviter les conflits.
    - Il ne faut pas créer de tâches sans prévenir le reste du groupe, car celle ci pourait "marcher" sur une tâche d'un autre membre du groupe.
    - La description de la tâche doit être courte, mais pas vague. S'il y a besoin de diviser la tâche en sous tâches pour plus de précision, le développeur peut le faire sans demander au reste du groupe.
    
- Comment travailler ?

    - Chaque développeur dispose d'une branche "Sandbox" et d'une branche "Trunk". Sandbox sert à tester, réfléchir.. et peut être manipulée librement, même supprimée et re-créée. Trunk en revanche doit contenir des commits réfléchis, avec le moins de bugs possible, donc attention à ne pas l'utiliser comme outil de sauvegarde du travail.
    
    - Avant de coder des structures de code, bien réfléchir à quel est le but de cette structure, combien de classes faut-il créer de façon à ce que une classe = une grande tâche, sans pour autant créer trop de fichiers. Utiliser un graphe UML pour les problèmes particulièrement compliqués.