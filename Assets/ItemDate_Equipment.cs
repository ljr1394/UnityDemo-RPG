using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Date", menuName = "Date/Equipment", order = 1)]
public class ItemDate_Equipment : ItemDate
{
    public EquipmentType equipmentType;

    [Header("Mojor stats")]
    public int strenght;
    public int agility;
    public int intelligencs;
    public int vitality;

    [Header("Offensive stats")]
    public int damage;
    public int critPower;
    public int critChange;

    [Header("Defensive stats")]
    public int armor;
    public int health;
    public int evasion;
    public int magicResistance;

    [Header("Magic ints")]
    public int iceDamage;
    public int fireDamage;
    public int lightningDamage;


    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strenght.AddModifier(strenght);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligencs.AddModifier(intelligencs);
        playerStats.vitality.AddModifier(vitality);
        playerStats.damage.AddModifier(damage);
        playerStats.critChange.AddModifier(critChange);
        playerStats.critPower.AddModifier(critPower);
        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strenght.RemoveModifier(strenght);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligencs.RemoveModifier(intelligencs);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChange.RemoveModifier(critChange);
        playerStats.critPower.RemoveModifier(critPower);
        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

    }

}
