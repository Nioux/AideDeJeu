import 'package:aidedejeu_flutter/models/filters.dart';
import 'package:json_annotation/json_annotation.dart';

part 'items.g.dart';

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class Item {
  String id;
  String rootId;
  String parentLink;
  String name;
  String normalizedName;
  String parentName;
  int nameLevel;
  String altName;
  String altNameText;
  String normalizedAlias;
  String source;
  String markdown;
  String fullText;
  String itemType;
  List<Item> children;

  Item();  

  factory Item.fromJson(Map<String, dynamic> map) => _$ItemFromJson(map);
  Map<String, dynamic> toJson() => _$ItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class GenericItem extends Item {
  GenericItem();

  factory GenericItem.fromJson(Map<String, dynamic> map) => _$GenericItemFromJson(map);
  Map<String, dynamic> toJson() => _$GenericItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
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

  factory MonsterItem.fromJson(Map<String, dynamic> map) => _$MonsterItemFromJson(map);
  Map<String, dynamic> toJson() => _$MonsterItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
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

  factory SpellItem.fromJson(Map<String, dynamic> map) => _$SpellItemFromJson(map);
  Map<String, dynamic> toJson() => _$SpellItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class Items extends Item {
  Items();

  factory Items.fromJson(Map<String, dynamic> map) => _$ItemsFromJson(map);
  Map<String, dynamic> toJson() => _$ItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class FilteredItems extends Items {
  String family;

  FilteredItems();

  @JsonKey(ignore: true)
  List<Filter> toFilterList() => [].toList();

  factory FilteredItems.fromJson(Map<String, dynamic> map) => _$FilteredItemsFromJson(map);
  Map<String, dynamic> toJson() => _$FilteredItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class MonsterItems extends FilteredItems {
  String types;
  String challenges;
  String sizes;
  String sources;
  String terrains;

  @JsonKey(ignore: true)
  Filter get typesFilter => Filter(
        name: "Types",
        displayName: "monstersTypes",
        type: FilterType.Choices,
        values: types?.split("|"));
  @JsonKey(ignore: true)
  Filter get challengesFilter => Filter(
        name: "Challenges",
        displayName: "monstersChallenges",
        type: FilterType.Range,
        values: challenges?.split("|"));
  @JsonKey(ignore: true)
  Filter get sizesFilter => Filter(
        name: "Sizes",
        displayName: "monstersSizes",
        type: FilterType.Range,
        values: sizes?.split("|"));
  @JsonKey(ignore: true)
  Filter get sourcesFilter => Filter(
        name: "Sources",
        displayName: "monstersSources",
        type: FilterType.Choices,
        values: sources?.split("|"));
  @JsonKey(ignore: true)
  Filter get terrainsFilter => Filter(
        name: "Terrains",
        displayName: "monstersTerrains",
        type: FilterType.Choices,
        values: terrains?.split("|"));

  MonsterItems();

  @JsonKey(ignore: true)
  List<Filter> toFilterList() => {
        typesFilter,
        challengesFilter,
        sizesFilter,
        sourcesFilter,
        terrainsFilter,
      }.toList();

  factory MonsterItems.fromJson(Map<String, dynamic> map) => _$MonsterItemsFromJson(map);
  Map<String, dynamic> toJson() => _$MonsterItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class SpellItems extends FilteredItems {
  String classes;
  String levels;
  String schools;
  String rituals;
  String castingTimes;
  String ranges;
  String verbalComponents;
  String somaticComponents;
  String materialComponents;
  String concentrations;
  String durations;
  String sources;

  @JsonKey(ignore: true)
  Filter get classesFilter => Filter(
        name: "Classes",
        displayName: "spellsClasses",
        type: FilterType.Choices,
        values: classes?.split("|"));
  @JsonKey(ignore: true)
  Filter get levelsFilter => Filter(
        name: "Levels",
        displayName: "spellsLevels",
        type: FilterType.Choices,
        values: levels?.split("|"));
  @JsonKey(ignore: true)
  Filter get schoolsFilter => Filter(
        name: "Schools",
        displayName: "spellsSchools",
        type: FilterType.Choices,
        values: schools?.split("|"));
  @JsonKey(ignore: true)
  Filter get ritualsFilter => Filter(
        name: "Rituals",
        displayName: "spellsRituals",
        type: FilterType.Choices,
        values: rituals?.split("|"));
  @JsonKey(ignore: true)
  Filter get castingTimesFilter => Filter(
        name: "CastingTimes",
        displayName: "spellsCastingTimes",
        type: FilterType.Choices,
        values: castingTimes?.split("|"));
  @JsonKey(ignore: true)
  Filter get rangesFilter => Filter(
        name: "Ranges",
        displayName: "spellsRanges",
        type: FilterType.Choices,
        values: ranges?.split("|"));
  @JsonKey(ignore: true)
  Filter get verbalComponentsFilter => Filter(
        name: "VerbalComponents",
        displayName: "spellsVerbalComponents",
        type: FilterType.Choices,
        values: verbalComponents?.split("|"));
  @JsonKey(ignore: true)
  Filter get somaticComponentsFilter => Filter(
        name: "SomaticComponents",
        displayName: "spellsSomaticComponents",
        type: FilterType.Choices,
        values: somaticComponents?.split("|"));
  @JsonKey(ignore: true)
  Filter get materialComponentsFilter => Filter(
        name: "MaterialComponents",
        displayName: "spellsMaterialComponents",
        type: FilterType.Choices,
        values: materialComponents?.split("|"));
  @JsonKey(ignore: true)
  Filter get concentrationsFilter => Filter(
        name: "Concentrations",
        displayName: "spellsConcentrations",
        type: FilterType.Choices,
        values: concentrations?.split("|"));
  @JsonKey(ignore: true)
  Filter get durationsFilter => Filter(
        name: "Durations",
        displayName: "spellsDurations",
        type: FilterType.Choices,
        values: durations?.split("|"));
  @JsonKey(ignore: true)
  Filter get sourcesFilter => Filter(
        name: "Sources",
        displayName: "spellsSources",
        type: FilterType.Choices,
        values: sources?.split("|"));

  SpellItems();

  @JsonKey(ignore: true)
  List<Filter> toFilterList() => {
        classesFilter,
        levelsFilter,
        schoolsFilter,
        ritualsFilter,
        castingTimesFilter,
        rangesFilter,
        verbalComponentsFilter,
        somaticComponentsFilter,
        materialComponentsFilter,
        concentrationsFilter,
        durationsFilter,
        sourcesFilter,
      }.toList();

  factory SpellItems.fromJson(Map<String, dynamic> map) => _$SpellItemsFromJson(map);
  Map<String, dynamic> toJson() => _$SpellItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
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

  factory RaceItem.fromJson(Map<String, dynamic> map) => _$RaceItemFromJson(map);
  Map<String, dynamic> toJson() => _$RaceItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class SubRaceItem extends RaceItem {
  String parentRaceId;

  SubRaceItem();

  factory SubRaceItem.fromJson(Map<String, dynamic> map) => _$SubRaceItemFromJson(map);
  Map<String, dynamic> toJson() => _$SubRaceItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class RaceItems extends FilteredItems {

  RaceItems();

  @JsonKey(ignore: true)
  @override
  List<Filter> toFilterList() {
    return [].toList();
  }

  factory RaceItems.fromJson(Map<String, dynamic> map) => _$RaceItemsFromJson(map);
  Map<String, dynamic> toJson() => _$RaceItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class OriginItem extends Item {
  String regionsOfOrigin;
  String mainLanguages;
  String aspirations;
  String availableSkills;

  OriginItem();

  factory OriginItem.fromJson(Map<String, dynamic> map) => _$OriginItemFromJson(map);
  Map<String, dynamic> toJson() => _$OriginItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class OriginItems extends FilteredItems {

  OriginItems();

  @JsonKey(ignore: true)
  @override
  List<Filter> toFilterList() {
    return [].toList();
  }
  factory OriginItems.fromJson(Map<String, dynamic> map) => _$OriginItemsFromJson(map);
  Map<String, dynamic> toJson() => _$OriginItemsToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class BackgroundItem extends Item {
  String skillProficiencies;
  String masteredTools;
  String masteredLanguages;
  String equipment;

  BackgroundItem();

  factory BackgroundItem.fromJson(Map<String, dynamic> map) => _$BackgroundItemFromJson(map);
  Map<String, dynamic> toJson() => _$BackgroundItemToJson(this);
}

@JsonSerializable(explicitToJson: true, fieldRename: FieldRename.pascal)
class SubBackgroundItem extends BackgroundItem {
  SubBackgroundItem();

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
