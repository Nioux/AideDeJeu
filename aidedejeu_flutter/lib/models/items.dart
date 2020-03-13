import 'package:aidedejeu_flutter/models/filters.dart';

class Item {
  String id;
  String rootId;
  String parentLink;
  String name;
  String normalizedName;
  String parentName;
  int nameLevel;
  String alias;
  String aliasText;
  String normalizedAlias;
  String source;
  String markdown;
  String fullText;
  String itemType;
  List<Item> children;

  Item(Map<String, dynamic> map) {
    this.id = map["Id"];
    this.rootId = map["RootId"];
    this.name = map["Name"];
    this.alias = map["AltName"];
    this.aliasText = map["AltNameText"];
    this.markdown = map["Markdown"];
    this.itemType = map["ItemType"];
  }
}

class MonsterItem extends Item {
  String family;
  String type;
  String size;
  String alignment;
  String terrain;
  String legendary;
  String armorClass;
  String hitPoints;
  String speed;
  String strength;
  String dexterity;
  String constitution;
  String intelligence;
  String wisdom;
  String charisma;
  String savingThrows;
  String skills;
  String damageVulnerabilities;
  String damageImmunities;
  String conditionImmunities;
  String damageResistances;
  String senses;
  String languages;
  String challenge;
  int xp;

  MonsterItem(Map<String, dynamic> map) : super(map) {
    this.family = map["Family"];
    this.type = map["Type"];
    this.size = map["Size"];
    this.alignment = map["Alignment"];
    this.terrain = map["Terrain"];
    this.legendary = map["Legendary"];
    this.armorClass = map["ArmorClass"];
    this.hitPoints = map["HitPoints"];
    this.speed = map["Speed"];
    this.strength = map["Strength"];
    this.dexterity = map["Dexterity"];
    this.constitution = map["Constitution"];
    this.intelligence = map["Intelligence"];
    this.wisdom = map["Wisdom"];
    this.charisma = map["Charisma"];
    this.savingThrows = map["SavingThrows"];
    this.skills = map["Skills"];
    this.damageVulnerabilities = map["DamageVulnerabilities"];
    this.damageImmunities = map["DamageImmunities"];
    this.conditionImmunities = map["ConditionImmunities"];
    this.damageResistances = map["DamageResistances"];
    this.senses = map["Senses"];
    this.languages = map["Languages"];
    this.challenge = map["Challenge"];
    this.xp = map["XP"];
  }
}

class SpellItem extends Item {
  String family;
  String level;
  String type;
  String ritual;
  String castingTime;
  String range;
  String components;
  String verbalComponent;
  String somaticComponent;
  String materialComponent;
  String concentration;
  String duration;
  String classes;

  SpellItem(Map<String, dynamic> map) : super(map) {
    this.family = map["Family"];
    this.level = map["Level"];
    this.type = map["Type"];
    this.ritual = map["Ritual"];
    this.castingTime = map["CastingTime"];
    this.range = map["Range"];
    this.components = map["Components"];
    this.verbalComponent = map["VerbalComponent"];
    this.somaticComponent = map["SomaticComponent"];
    this.materialComponent = map["MaterialComponent"];
    this.concentration = map["Concentration"];
    this.duration = map["Duration"];
    this.classes = map["Classes"];
  }
}

class Items extends Item {
  Items(Map<String, dynamic> map) : super(map);
}

abstract class FilteredItems extends Items {
  String family;

  FilteredItems(Map<String, dynamic> map) : super(map) {
    this.family = map["Family"];
  }

  List<Filter> toFilterList();
}

class MonsterItems extends FilteredItems {
  Filter types;
  Filter challenges;
  Filter sizes;
  Filter sources;
  Filter terrains;

  MonsterItems(Map<String, dynamic> map) : super(map) {
    this.types = Filter(
        name: "Types",
        displayName: "monstersTypes",
        type: FilterType.Choices,
        values: map["Types"].toString().split("|"));
    this.challenges = Filter(
        name: "Challenges",
        displayName: "monstersChallenges",
        type: FilterType.Range,
        values: map["Challenges"].toString().split("|"));
    this.sizes = Filter(
        name: "Sizes",
        displayName: "monstersSizes",
        type: FilterType.Range,
        values: map["Sizes"].toString().split("|"));
    this.sources = Filter(
        name: "Sources",
        displayName: "monstersSources",
        type: FilterType.Choices,
        values: map["Sources"].toString().split("|"));
    this.terrains = Filter(
        name: "Terrains",
        displayName: "monstersTerrains",
        type: FilterType.Choices,
        values: map["Terrains"].toString().split("|"));
  }

  List<Filter> toFilterList() => {
        types,
        challenges,
        sizes,
        sources,
        terrains,
      }.toList();
}

class SpellItems extends FilteredItems {
  Filter classes;
  Filter levels;
  Filter schools;
  Filter rituals;
  Filter castingTimes;
  Filter ranges;
  Filter verbalComponents;
  Filter somaticComponents;
  Filter materialComponents;
  Filter concentrations;
  Filter durations;
  Filter sources;

  SpellItems(Map<String, dynamic> map) : super(map) {
    this.classes = Filter(
        name: "Classes",
        displayName: "spellsClasses",
        type: FilterType.Choices,
        values: map["Classes"].toString().split("|"));
    this.levels = Filter(
        name: "Levels",
        displayName: "spellsLevels",
        type: FilterType.Choices,
        values: map["Levels"].toString().split("|"));
    this.schools = Filter(
        name: "Schools",
        displayName: "spellsSchools",
        type: FilterType.Choices,
        values: map["Schools"].toString().split("|"));
    this.rituals = Filter(
        name: "Rituals",
        displayName: "spellsRituals",
        type: FilterType.Choices,
        values: map["Rituals"].toString().split("|"));
    this.castingTimes = Filter(
        name: "CastingTimes",
        displayName: "spellsCastingTimes",
        type: FilterType.Choices,
        values: map["CastingTimes"].toString().split("|"));
    this.ranges = Filter(
        name: "Ranges",
        displayName: "spellsRanges",
        type: FilterType.Choices,
        values: map["Ranges"].toString().split("|"));
    this.verbalComponents = Filter(
        name: "VerbalComponents",
        displayName: "spellsVerbalComponents",
        type: FilterType.Choices,
        values: map["VerbalComponents"].toString().split("|"));
    this.somaticComponents = Filter(
        name: "SomaticComponents",
        displayName: "spellsSomaticComponents",
        type: FilterType.Choices,
        values: map["SomaticComponents"].toString().split("|"));
    this.materialComponents = Filter(
        name: "MaterialComponents",
        displayName: "spellsMaterialComponents",
        type: FilterType.Choices,
        values: map["MaterialComponents"].toString().split("|"));
    this.concentrations = Filter(
        name: "Concentrations",
        displayName: "spellsConcentrations",
        type: FilterType.Choices,
        values: map["Concentrations"].toString().split("|"));
    this.durations = Filter(
        name: "Durations",
        displayName: "spellsDurations",
        type: FilterType.Choices,
        values: map["Durations"].toString().split("|"));
    this.sources = Filter(
        name: "Sources",
        displayName: "spellsSources",
        type: FilterType.Choices,
        values: map["Sources"].toString().split("|"));
  }

  List<Filter> toFilterList() => {
        classes,
        levels,
        schools,
        rituals,
        castingTimes,
        ranges,
        verbalComponents,
        somaticComponents,
        materialComponents,
        concentrations,
        durations,
        sources,
      }.toList();
}

class RaceItem extends Item {
  String fullName;
  bool hasSubRaces;
  String strengthBonus;
  String dexterityBonus;
  String constitutionBonus;
  String intelligenceBonus;
  String wisdomBonus;
  String charismaBonus;
  String dispatchedBonus;
  String maxDispatchedStrengthBonus;
  String maxDispatchedDexterityBonus;
  String maxDispatchedConstitutionBonus;
  String maxDispatchedIntelligenceBonus;
  String maxDispatchedWisdomBonus;
  String maxDispatchedCharismaBonus;
  String abilityScoreIncrease;
  String age;
  String alignment;
  String size;
  String speed;
  String darkvision;
  String languages;

  RaceItem(Map<String, dynamic> map) : super(map) {
    this.fullName = map["FullName"];
    this.hasSubRaces = map["HasSubRaces"] == "true";
    this.strengthBonus = map["StrengthBonus"];
    this.dexterityBonus = map["DexterityBonus"];
    this.constitutionBonus = map["ConstitutionBonus"];
    this.intelligenceBonus = map["IntelligenceBonus"];
    this.wisdomBonus = map["WisdomBonus"];
    this.charismaBonus = map["CharismaBonus"];
    this.dispatchedBonus = map["DispatchedBonus"];
    this.maxDispatchedStrengthBonus = map["MaxDispatchedStrengthBonus"];
    this.maxDispatchedDexterityBonus = map["MaxDispatchedDexterityBonus"];
    this.maxDispatchedConstitutionBonus = map["MaxDispatchedConstitutionBonus"];
    this.maxDispatchedIntelligenceBonus = map["MaxDispatchedIntelligenceBonus"];
    this.maxDispatchedWisdomBonus = map["MaxDispatchedWisdomBonus"];
    this.maxDispatchedCharismaBonus = map["MaxDispatchedCharismaBonus"];
    this.abilityScoreIncrease = map["AbilityScoreIncrease"];
    this.age = map["Age"];
    this.alignment = map["Alignment"];
    this.size = map["Size"];
    this.speed = map["Speed"];
    this.darkvision = map["Darkvision"];
    this.languages = map["Languages"];
  }
}

class SubRaceItem extends RaceItem {
  String parentRaceId;

  SubRaceItem(Map<String, dynamic> map) : super(map) {
    this.parentRaceId = map["ParentRaceId"];
  }
}

class RaceItems extends FilteredItems {
  RaceItems(Map<String, dynamic> map) : super(map);

  @override
  List<Filter> toFilterList() {
    return [].toList();
  }
}

class BackgroundItem extends Item {
  String skillProficiencies;
  String masteredTools;
  String masteredLanguages;
  String equipment;

  BackgroundItem(Map<String, dynamic> map) : super(map) {
    this.skillProficiencies = map["SkillProficiencies"];
    this.masteredTools = map["MasteredTools"];
    this.masteredLanguages = map["MasteredLanguages"];
    this.equipment = map["Equipment"];
  }
}

class SubBackgroundItem extends BackgroundItem {
  SubBackgroundItem(Map<String, dynamic> map) : super(map);
}

Item itemFromMap(Map<String, dynamic> map) {
  switch (map["ItemType"]) {
    case "RaceItem":
      return RaceItem(map);
    case "SubRaceItem":
      return SubRaceItem(map);
    case "RaceItems":
      return RaceItems(map);
    case "BackgroundItem":
      return BackgroundItem(map);
    case "SubBackgroundItem":
      return SubBackgroundItem(map);
    case "MonsterItem":
      return MonsterItem(map);
    case "MonsterItems":
      return MonsterItems(map);
    case "SpellItem":
      return SpellItem(map);
    case "SpellItems":
      return SpellItems(map);
  }
  return Item(map);
}

List<T> itemsFromMapList<T extends Item>(List<Map<String, dynamic>> mapList) {
  return List.generate(mapList.length, (i) {
    return itemFromMap(mapList[i]);
  });
}
