# TP6 Exercice 1 MeshLab - Modélisation Géométrique 

## Buddha

### Image de départ

![BuddhaBase](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\BuddhaBase.PNG)

A noter : à chaque test je repars d'une base saine : en ré-important le fichier de base.



### Premier test

![Buddha_1](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_1.PNG)

![Buddha_2](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_2.PNG)

![Buddha_3](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_3.PNG)

![Buddha_4](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_4.PNG)



Sur ces 4 images, seul le nombre de face recherché est modifié, les autres paramètres restant exactement les mêmes. De manière assez logique, plus le nombre de face diminue, plus l'image est simplifié et perd du détail. Sur la dernière image, on remarque clairement que les détails du visage ont disparus.



### Second test

![Buddha_1](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_1.PNG)

![Buddha_5](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_5.PNG)

![Buddha_6](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_6.PNG)

![Buddha_7](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_7.PNG)

![Buddha_8](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_8.PNG)

![Buddha_9](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_9.PNG)

Sur ce test, le nombre de faces est fixé à 20,000. Le seule paramètre qui est modifié est le seuil de qualité. Ce dernier paramètre indique à l'algorithme à quel point il doit garder la forme originale.

La seule différence notable que l'on peut noter est entre la première et dernière image. Le facteur de qualité étant situé à 0 pour la dernière, ce maillage contient plus de sommets et de faces que les autres. Une faible partie des détails est alors gardé.



### Troisième test

![Buddha_1](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_1.PNG)

![Buddha_10](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_10.PNG)

![Buddha_11](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Buddha_11.PNG)

Dans ce test, je me suis intéressé au paramètre de pourcentage de réduction. Plus ce dernier est faible, plus le nombre de sommets et de faces est faible aussi. En conséquence, une partie des détails du maillages sont perdus.





## Voiture

J'ai réalisé les mêmes tests avec une maillage de voiture dont voici une capture d'écran de base :

![CarBase](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\CarBase.PNG)



### Premier test

![Car_1](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Car_1.PNG)

Sur ce test, j'ai réduit le nombre de faces à atteindre à 8149 (la moitié du nombre de départ) Les autres paramètres sont fixés à 0.5 pour le pourcentage de réduction et le seuil de qualité. Sur les phares, le toit et les roues on remarque très bien la disparition de faces et donc de détails.



### Second test

![Car_4](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Car_4.PNG)

Sur ce test, j'ai uniquement changé le pourcentage de réduction et le facteur de qualité à 1 chacun. On remarque alors que le maillage est le même que celui de départ. Aucun sommet ni face n'a été perdu.



### Troisième test

![Car_5](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Car_5.PNG)

Pour ce dernier test, j'ai réduit le pourcentage de réduction à 0.2. Ce faisant, le maillage obtenu est très dégradé : on a perdu les rayons des roues, les poignets de portes et du détail de manière général.



## Clustering Decimation vs Edge Collapse

Clustering Decimation :

![1](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Compare\1.PNG)



Edge Collapse :

![2](D:\Cours\Modelisation_Geometrique\CH6_simplification\TP\Screenshots\Compare\2.PNG)



Ces deux algorithmes ont été appliqués avec les paramètres de base proposé par MeshLab.
On remarque très vite que le Clustering Decimation perd beaucoup de détails (rayons des roues, grille d'aération) de manière non uniforme (On voit que l'on perd certains rayons de la roue mais pas tous, la barre le long du capot a aussi disparu).

Il peut en conclure que l'algorithme Edge Collapse garde mieux la forme de départ que l'algorithme Clustering Decimation.