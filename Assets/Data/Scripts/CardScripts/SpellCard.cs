using System;

public class SpellCard : Card
{
    protected BaseSpellEffect spellEffect;

    public override void PlayCard(TerrainTile tile)
    {
        if (ResourceManager.Instance.TrySpendResource(resourceRequired, Convert.ToInt32(cardCost)))
        {
            spellEffect.CastSpell(tile);

            isSelected = false;
            Destroy(gameObject);
            OnCardPlayed();
        }
        else
        {
            string str = "Not enough resources";
            TextManager.Instance.ShowWarningText(str);
            isSelected = false;
        }
    }

    protected override void CopySOData(BaseCardDataSO cardSO)
    {
        base.CopySOData(cardSO);

        spellEffect = ((SpellCardDataSO)cardSO).spellEffect;
    }

}
