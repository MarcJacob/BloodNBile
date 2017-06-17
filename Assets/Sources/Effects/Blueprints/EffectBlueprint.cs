using System;
using System.Collections.Generic;

/**
 * <summary> Un Effect Blueprint est un "instantiateur" d'effets qui est attaché à un sort. Il contient des informations
 * sur le ou les effets qu'il instancie en premier auxquelles on peut accéder dès la création du sort au lancement du jeu.
 * Sa méthode Instantiate() prend en paramètre diverses informations que l'on connait après que l'on ai prit connaissance du
 * lanceur et d'autres informations sur son "monde".</summary>
 */
public abstract class EffectBlueprint
{
    public abstract void Instantiate(Unit caster, BnBMatch world);

}
