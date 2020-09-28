import 'package:aidedejeu_flutter/models/filters.dart';
import 'package:json_annotation/json_annotation.dart';

part 'items.g.dart';

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class Item {
  @JsonKey(name: "Id")
  String id;
  @JsonKey(name: "RootId")
  String rootId;
  @JsonKey(name: "ParentLink")
  String parentLink;
  @JsonKey(name: "Name")
  String name;
  @JsonKey(name: "NormalizedName")
  String normalizedName;
  @JsonKey(name: "ParentName")
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


Item();
/*
  Item(Map<String, dynamic> map) {
    this.id = map["Id"];
    this.rootId = map["RootId"];
    this.name = map["Name"];
    this.alias = map["AltName"];
    this.aliasText = map["AltNameText"];
    this.markdown = map["Markdown"];
    this.itemType = map["ItemType"];
  }
*/
  factory Item.fromJson(Map<String, dynamic> map) => _$ItemFromJson(map);
  Map<String, dynamic> toJson() => _$ItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class GenericItem extends Item {
  GenericItem();
  factory GenericItem.fromJson(Map<String, dynamic> map) => _$GenericItemFromJson(map);
  Map<String, dynamic> toJson() => _$GenericItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
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

  MonsterItem();

/*
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
  */
  factory MonsterItem.fromJson(Map<String, dynamic> map) => _$MonsterItemFromJson(map);
  Map<String, dynamic> toJson() => _$MonsterItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
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

  SpellItem();
/*  SpellItem(Map<String, dynamic> map) : super(map) {
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
  }*/
  factory SpellItem.fromJson(Map<String, dynamic> map) => _$SpellItemFromJson(map);
  Map<String, dynamic> toJson() => _$SpellItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
class Items extends Item {
  Items();
//  Items(Map<String, dynamic> map) : super(map);
  factory Items.fromJson(Map<String, dynamic> map) => _$ItemsFromJson(map);
  Map<String, dynamic> toJson() => _$ItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class FilteredItems extends Items {
  String family;

  FilteredItems();

/*  FilteredItems(Map<String, dynamic> map) : super(map) {
    this.family = map["Family"];
  }*/

  List<Filter> toFilterList() => [].toList();

  factory FilteredItems.fromJson(Map<String, dynamic> map) => _$FilteredItemsFromJson(map);
  Map<String, dynamic> toJson() => _$FilteredItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class MonsterItems extends FilteredItems {
  String typesString;
  String challengesString;
  String sizesString;
  String sourcesString;
  String terrainsString;

  @JsonKey(ignore: true)
  Filter types;
  @JsonKey(ignore: true)
  Filter challenges;
  @JsonKey(ignore: true)
  Filter sizes;
  @JsonKey(ignore: true)
  Filter sources;
  @JsonKey(ignore: true)
  Filter terrains;

  MonsterItems();

/*  MonsterItems(Map<String, dynamic> map) : super(map) {
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
*/
  List<Filter> toFilterList() => {
        types,
        challenges,
        sizes,
        sources,
        terrains,
      }.toList();

  factory MonsterItems.fromJson(Map<String, dynamic> map) => _$MonsterItemsFromJson(map);
  Map<String, dynamic> toJson() => _$MonsterItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class SpellItems extends FilteredItems {
  @JsonKey(ignore: true)
  Filter classes;
  @JsonKey(ignore: true)
  Filter levels;
  @JsonKey(ignore: true)
  Filter schools;
  @JsonKey(ignore: true)
  Filter rituals;
  @JsonKey(ignore: true)
  Filter castingTimes;
  @JsonKey(ignore: true)
  Filter ranges;
  @JsonKey(ignore: true)
  Filter verbalComponents;
  @JsonKey(ignore: true)
  Filter somaticComponents;
  @JsonKey(ignore: true)
  Filter materialComponents;
  @JsonKey(ignore: true)
  Filter concentrations;
  @JsonKey(ignore: true)
  Filter durations;
  @JsonKey(ignore: true)
  Filter sources;

  SpellItems();

/*  SpellItems(Map<String, dynamic> map) : super(map) {
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
*/
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

  factory SpellItems.fromJson(Map<String, dynamic> map) => _$SpellItemsFromJson(map);
  Map<String, dynamic> toJson() => _$SpellItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
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

  RaceItem();

/*  RaceItem(Map<String, dynamic> map) : super(map) {
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
  }*/
  factory RaceItem.fromJson(Map<String, dynamic> map) => _$RaceItemFromJson(map);
  Map<String, dynamic> toJson() => _$RaceItemToJson(this);

}

@JsonSerializable(explicitToJson: true)
class SubRaceItem extends RaceItem {
  String parentRaceId;

  SubRaceItem();

/*  SubRaceItem(Map<String, dynamic> map) : super(map) {
    this.parentRaceId = map["ParentRaceId"];
  }*/
  factory SubRaceItem.fromJson(Map<String, dynamic> map) => _$SubRaceItemFromJson(map);
  Map<String, dynamic> toJson() => _$SubRaceItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
class RaceItems extends FilteredItems {
//  RaceItems(Map<String, dynamic> map) : super(map);

  RaceItems();

  @override
  List<Filter> toFilterList() {
    return [].toList();
  }
  factory RaceItems.fromJson(Map<String, dynamic> map) => _$RaceItemsFromJson(map);
  Map<String, dynamic> toJson() => _$RaceItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class OriginItem extends Item {
  String regionsOfOrigin;
  String mainLanguages;
  String aspirations;
  String availableSkills;

  OriginItem();

/*  OriginItem(Map<String, dynamic> map) : super(map) {
    this.regionsOfOrigin = map["RegionsOfOrigin"];
    this.mainLanguages = map["MainLanguages"];
    this.aspirations = map["Aspirations"];
    this.availableSkills = map["AvailableSkills"];
  }*/
  factory OriginItem.fromJson(Map<String, dynamic> map) => _$OriginItemFromJson(map);
  Map<String, dynamic> toJson() => _$OriginItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
class OriginItems extends FilteredItems {
//  OriginItems(Map<String, dynamic> map) : super(map);

  OriginItems();

  @override
  List<Filter> toFilterList() {
    return [].toList();
  }
  factory OriginItems.fromJson(Map<String, dynamic> map) => _$OriginItemsFromJson(map);
  Map<String, dynamic> toJson() => _$OriginItemsToJson(this);
}

@JsonSerializable(explicitToJson: true)
class BackgroundItem extends Item {
  String skillProficiencies;
  String masteredTools;
  String masteredLanguages;
  String equipment;

  BackgroundItem();
/*
  BackgroundItem(Map<String, dynamic> map) : super(map) {
    this.skillProficiencies = map["SkillProficiencies"];
    this.masteredTools = map["MasteredTools"];
    this.masteredLanguages = map["MasteredLanguages"];
    this.equipment = map["Equipment"];
  }*/
  factory BackgroundItem.fromJson(Map<String, dynamic> map) => _$BackgroundItemFromJson(map);
  Map<String, dynamic> toJson() => _$BackgroundItemToJson(this);
}

@JsonSerializable(explicitToJson: true)
class SubBackgroundItem extends BackgroundItem {
  SubBackgroundItem();
//  SubBackgroundItem(Map<String, dynamic> map) : super(map);
  factory SubBackgroundItem.fromJson(Map<String, dynamic> map) => _$SubBackgroundItemFromJson(map);
  Map<String, dynamic> toJson() => _$SubBackgroundItemToJson(this);
}

Item itemFromMap(Map<String, dynamic> map) {
  switch (map["ItemType"]) {
    case "GenericItem":
      return GenericItem.fromJson(map);
    case "RaceItem":
      return RaceItem.fromJson(map);
    case "SubRaceItem":
      return SubRaceItem.fromJson(map);
    case "RaceItems":
      return RaceItems.fromJson(map);
    case "OriginItem":
      return OriginItem.fromJson(map);
    case "OriginItems":
      return OriginItems.fromJson(map);
    case "BackgroundItem":
      return BackgroundItem.fromJson(map);
    case "SubBackgroundItem":
      return SubBackgroundItem.fromJson(map);
    case "MonsterItem":
      return MonsterItem.fromJson(map);
    case "MonsterItems":
      return MonsterItems.fromJson(map);
    case "SpellItem":
      return SpellItem.fromJson(map);
    case "SpellItems":
      return SpellItems.fromJson(map);
  }
  return Item.fromJson(map);
}

List<T> itemsFromMapList<T extends Item>(List<Map<String, dynamic>> mapList) {
  return List.generate(mapList.length, (i) {
    return itemFromMap(mapList[i]);
  });
}
