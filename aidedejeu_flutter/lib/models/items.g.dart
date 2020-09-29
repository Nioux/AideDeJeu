// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'items.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Item _$ItemFromJson(Map<String, dynamic> json) {
  return Item()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$ItemToJson(Item instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

GenericItem _$GenericItemFromJson(Map<String, dynamic> json) {
  return GenericItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$GenericItemToJson(GenericItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

MonsterItem _$MonsterItemFromJson(Map<String, dynamic> json) {
  return MonsterItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String
    ..type = json['Type'] as String
    ..size = json['Size'] as String
    ..alignment = json['Alignment'] as String
    ..terrain = json['Terrain'] as String
    ..legendary = json['Legendary'] as String
    ..armorClass = json['ArmorClass'] as String
    ..hitPoints = json['HitPoints'] as String
    ..speed = json['Speed'] as String
    ..strength = json['Strength'] as String
    ..dexterity = json['Dexterity'] as String
    ..constitution = json['Constitution'] as String
    ..intelligence = json['Intelligence'] as String
    ..wisdom = json['Wisdom'] as String
    ..charisma = json['Charisma'] as String
    ..savingThrows = json['SavingThrows'] as String
    ..skills = json['Skills'] as String
    ..damageVulnerabilities = json['DamageVulnerabilities'] as String
    ..damageImmunities = json['DamageImmunities'] as String
    ..conditionImmunities = json['ConditionImmunities'] as String
    ..damageResistances = json['DamageResistances'] as String
    ..senses = json['Senses'] as String
    ..languages = json['Languages'] as String
    ..challenge = json['Challenge'] as String
    ..xp = json['Xp'] as int;
}

Map<String, dynamic> _$MonsterItemToJson(MonsterItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
      'Type': instance.type,
      'Size': instance.size,
      'Alignment': instance.alignment,
      'Terrain': instance.terrain,
      'Legendary': instance.legendary,
      'ArmorClass': instance.armorClass,
      'HitPoints': instance.hitPoints,
      'Speed': instance.speed,
      'Strength': instance.strength,
      'Dexterity': instance.dexterity,
      'Constitution': instance.constitution,
      'Intelligence': instance.intelligence,
      'Wisdom': instance.wisdom,
      'Charisma': instance.charisma,
      'SavingThrows': instance.savingThrows,
      'Skills': instance.skills,
      'DamageVulnerabilities': instance.damageVulnerabilities,
      'DamageImmunities': instance.damageImmunities,
      'ConditionImmunities': instance.conditionImmunities,
      'DamageResistances': instance.damageResistances,
      'Senses': instance.senses,
      'Languages': instance.languages,
      'Challenge': instance.challenge,
      'Xp': instance.xp,
    };

SpellItem _$SpellItemFromJson(Map<String, dynamic> json) {
  return SpellItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String
    ..level = json['Level'] as String
    ..type = json['Type'] as String
    ..ritual = json['Ritual'] as String
    ..castingTime = json['CastingTime'] as String
    ..range = json['Range'] as String
    ..components = json['Components'] as String
    ..verbalComponent = json['VerbalComponent'] as String
    ..somaticComponent = json['SomaticComponent'] as String
    ..materialComponent = json['MaterialComponent'] as String
    ..concentration = json['Concentration'] as String
    ..duration = json['Duration'] as String
    ..classes = json['Classes'] as String;
}

Map<String, dynamic> _$SpellItemToJson(SpellItem instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
      'Level': instance.level,
      'Type': instance.type,
      'Ritual': instance.ritual,
      'CastingTime': instance.castingTime,
      'Range': instance.range,
      'Components': instance.components,
      'VerbalComponent': instance.verbalComponent,
      'SomaticComponent': instance.somaticComponent,
      'MaterialComponent': instance.materialComponent,
      'Concentration': instance.concentration,
      'Duration': instance.duration,
      'Classes': instance.classes,
    };

Items _$ItemsFromJson(Map<String, dynamic> json) {
  return Items()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList();
}

Map<String, dynamic> _$ItemsToJson(Items instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
    };

FilteredItems _$FilteredItemsFromJson(Map<String, dynamic> json) {
  return FilteredItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String;
}

Map<String, dynamic> _$FilteredItemsToJson(FilteredItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
    };

MonsterItems _$MonsterItemsFromJson(Map<String, dynamic> json) {
  return MonsterItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String
    ..types = json['Types'] as String
    ..challenges = json['Challenges'] as String
    ..sizes = json['Sizes'] as String
    ..sources = json['Sources'] as String
    ..terrains = json['Terrains'] as String;
}

Map<String, dynamic> _$MonsterItemsToJson(MonsterItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
      'Types': instance.types,
      'Challenges': instance.challenges,
      'Sizes': instance.sizes,
      'Sources': instance.sources,
      'Terrains': instance.terrains,
    };

SpellItems _$SpellItemsFromJson(Map<String, dynamic> json) {
  return SpellItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String
    ..classes = json['Classes'] as String
    ..levels = json['Levels'] as String
    ..schools = json['Schools'] as String
    ..rituals = json['Rituals'] as String
    ..castingTimes = json['CastingTimes'] as String
    ..ranges = json['Ranges'] as String
    ..verbalComponents = json['VerbalComponents'] as String
    ..somaticComponents = json['SomaticComponents'] as String
    ..materialComponents = json['MaterialComponents'] as String
    ..concentrations = json['Concentrations'] as String
    ..durations = json['Durations'] as String
    ..sources = json['Sources'] as String;
}

Map<String, dynamic> _$SpellItemsToJson(SpellItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
      'Classes': instance.classes,
      'Levels': instance.levels,
      'Schools': instance.schools,
      'Rituals': instance.rituals,
      'CastingTimes': instance.castingTimes,
      'Ranges': instance.ranges,
      'VerbalComponents': instance.verbalComponents,
      'SomaticComponents': instance.somaticComponents,
      'MaterialComponents': instance.materialComponents,
      'Concentrations': instance.concentrations,
      'Durations': instance.durations,
      'Sources': instance.sources,
    };

RaceItem _$RaceItemFromJson(Map<String, dynamic> json) {
  return RaceItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..fullName = json['FullName'] as String
    ..hasSubRaces = json['HasSubRaces'] as bool
    ..strengthBonus = json['StrengthBonus'] as String
    ..dexterityBonus = json['DexterityBonus'] as String
    ..constitutionBonus = json['ConstitutionBonus'] as String
    ..intelligenceBonus = json['IntelligenceBonus'] as String
    ..wisdomBonus = json['WisdomBonus'] as String
    ..charismaBonus = json['CharismaBonus'] as String
    ..dispatchedBonus = json['DispatchedBonus'] as String
    ..maxDispatchedStrengthBonus = json['MaxDispatchedStrengthBonus'] as String
    ..maxDispatchedDexterityBonus =
        json['MaxDispatchedDexterityBonus'] as String
    ..maxDispatchedConstitutionBonus =
        json['MaxDispatchedConstitutionBonus'] as String
    ..maxDispatchedIntelligenceBonus =
        json['MaxDispatchedIntelligenceBonus'] as String
    ..maxDispatchedWisdomBonus = json['MaxDispatchedWisdomBonus'] as String
    ..maxDispatchedCharismaBonus = json['MaxDispatchedCharismaBonus'] as String
    ..abilityScoreIncrease = json['AbilityScoreIncrease'] as String
    ..age = json['Age'] as String
    ..alignment = json['Alignment'] as String
    ..size = json['Size'] as String
    ..speed = json['Speed'] as String
    ..darkvision = json['Darkvision'] as String
    ..languages = json['Languages'] as String;
}

Map<String, dynamic> _$RaceItemToJson(RaceItem instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'FullName': instance.fullName,
      'HasSubRaces': instance.hasSubRaces,
      'StrengthBonus': instance.strengthBonus,
      'DexterityBonus': instance.dexterityBonus,
      'ConstitutionBonus': instance.constitutionBonus,
      'IntelligenceBonus': instance.intelligenceBonus,
      'WisdomBonus': instance.wisdomBonus,
      'CharismaBonus': instance.charismaBonus,
      'DispatchedBonus': instance.dispatchedBonus,
      'MaxDispatchedStrengthBonus': instance.maxDispatchedStrengthBonus,
      'MaxDispatchedDexterityBonus': instance.maxDispatchedDexterityBonus,
      'MaxDispatchedConstitutionBonus': instance.maxDispatchedConstitutionBonus,
      'MaxDispatchedIntelligenceBonus': instance.maxDispatchedIntelligenceBonus,
      'MaxDispatchedWisdomBonus': instance.maxDispatchedWisdomBonus,
      'MaxDispatchedCharismaBonus': instance.maxDispatchedCharismaBonus,
      'AbilityScoreIncrease': instance.abilityScoreIncrease,
      'Age': instance.age,
      'Alignment': instance.alignment,
      'Size': instance.size,
      'Speed': instance.speed,
      'Darkvision': instance.darkvision,
      'Languages': instance.languages,
    };

SubRaceItem _$SubRaceItemFromJson(Map<String, dynamic> json) {
  return SubRaceItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..fullName = json['FullName'] as String
    ..hasSubRaces = json['HasSubRaces'] as bool
    ..strengthBonus = json['StrengthBonus'] as String
    ..dexterityBonus = json['DexterityBonus'] as String
    ..constitutionBonus = json['ConstitutionBonus'] as String
    ..intelligenceBonus = json['IntelligenceBonus'] as String
    ..wisdomBonus = json['WisdomBonus'] as String
    ..charismaBonus = json['CharismaBonus'] as String
    ..dispatchedBonus = json['DispatchedBonus'] as String
    ..maxDispatchedStrengthBonus = json['MaxDispatchedStrengthBonus'] as String
    ..maxDispatchedDexterityBonus =
        json['MaxDispatchedDexterityBonus'] as String
    ..maxDispatchedConstitutionBonus =
        json['MaxDispatchedConstitutionBonus'] as String
    ..maxDispatchedIntelligenceBonus =
        json['MaxDispatchedIntelligenceBonus'] as String
    ..maxDispatchedWisdomBonus = json['MaxDispatchedWisdomBonus'] as String
    ..maxDispatchedCharismaBonus = json['MaxDispatchedCharismaBonus'] as String
    ..abilityScoreIncrease = json['AbilityScoreIncrease'] as String
    ..age = json['Age'] as String
    ..alignment = json['Alignment'] as String
    ..size = json['Size'] as String
    ..speed = json['Speed'] as String
    ..darkvision = json['Darkvision'] as String
    ..languages = json['Languages'] as String
    ..parentRaceId = json['ParentRaceId'] as String;
}

Map<String, dynamic> _$SubRaceItemToJson(SubRaceItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'FullName': instance.fullName,
      'HasSubRaces': instance.hasSubRaces,
      'StrengthBonus': instance.strengthBonus,
      'DexterityBonus': instance.dexterityBonus,
      'ConstitutionBonus': instance.constitutionBonus,
      'IntelligenceBonus': instance.intelligenceBonus,
      'WisdomBonus': instance.wisdomBonus,
      'CharismaBonus': instance.charismaBonus,
      'DispatchedBonus': instance.dispatchedBonus,
      'MaxDispatchedStrengthBonus': instance.maxDispatchedStrengthBonus,
      'MaxDispatchedDexterityBonus': instance.maxDispatchedDexterityBonus,
      'MaxDispatchedConstitutionBonus': instance.maxDispatchedConstitutionBonus,
      'MaxDispatchedIntelligenceBonus': instance.maxDispatchedIntelligenceBonus,
      'MaxDispatchedWisdomBonus': instance.maxDispatchedWisdomBonus,
      'MaxDispatchedCharismaBonus': instance.maxDispatchedCharismaBonus,
      'AbilityScoreIncrease': instance.abilityScoreIncrease,
      'Age': instance.age,
      'Alignment': instance.alignment,
      'Size': instance.size,
      'Speed': instance.speed,
      'Darkvision': instance.darkvision,
      'Languages': instance.languages,
      'ParentRaceId': instance.parentRaceId,
    };

RaceItems _$RaceItemsFromJson(Map<String, dynamic> json) {
  return RaceItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String;
}

Map<String, dynamic> _$RaceItemsToJson(RaceItems instance) => <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
    };

OriginItem _$OriginItemFromJson(Map<String, dynamic> json) {
  return OriginItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..regionsOfOrigin = json['RegionsOfOrigin'] as String
    ..mainLanguages = json['MainLanguages'] as String
    ..aspirations = json['Aspirations'] as String
    ..availableSkills = json['AvailableSkills'] as String;
}

Map<String, dynamic> _$OriginItemToJson(OriginItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'RegionsOfOrigin': instance.regionsOfOrigin,
      'MainLanguages': instance.mainLanguages,
      'Aspirations': instance.aspirations,
      'AvailableSkills': instance.availableSkills,
    };

OriginItems _$OriginItemsFromJson(Map<String, dynamic> json) {
  return OriginItems()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..family = json['Family'] as String;
}

Map<String, dynamic> _$OriginItemsToJson(OriginItems instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'Family': instance.family,
    };

BackgroundItem _$BackgroundItemFromJson(Map<String, dynamic> json) {
  return BackgroundItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..skillProficiencies = json['SkillProficiencies'] as String
    ..masteredTools = json['MasteredTools'] as String
    ..masteredLanguages = json['MasteredLanguages'] as String
    ..equipment = json['Equipment'] as String;
}

Map<String, dynamic> _$BackgroundItemToJson(BackgroundItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'SkillProficiencies': instance.skillProficiencies,
      'MasteredTools': instance.masteredTools,
      'MasteredLanguages': instance.masteredLanguages,
      'Equipment': instance.equipment,
    };

SubBackgroundItem _$SubBackgroundItemFromJson(Map<String, dynamic> json) {
  return SubBackgroundItem()
    ..id = json['Id'] as String
    ..rootId = json['RootId'] as String
    ..parentLink = json['ParentLink'] as String
    ..name = json['Name'] as String
    ..normalizedName = json['NormalizedName'] as String
    ..parentName = json['ParentName'] as String
    ..nameLevel = json['NameLevel'] as int
    ..altName = json['AltName'] as String
    ..altNameText = json['AltNameText'] as String
    ..normalizedAlias = json['NormalizedAlias'] as String
    ..source = json['Source'] as String
    ..markdown = json['Markdown'] as String
    ..fullText = json['FullText'] as String
    ..itemType = json['ItemType'] as String
    ..children = (json['Children'] as List)
        ?.map(
            (e) => e == null ? null : Item.fromJson(e as Map<String, dynamic>))
        ?.toList()
    ..skillProficiencies = json['SkillProficiencies'] as String
    ..masteredTools = json['MasteredTools'] as String
    ..masteredLanguages = json['MasteredLanguages'] as String
    ..equipment = json['Equipment'] as String;
}

Map<String, dynamic> _$SubBackgroundItemToJson(SubBackgroundItem instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'RootId': instance.rootId,
      'ParentLink': instance.parentLink,
      'Name': instance.name,
      'NormalizedName': instance.normalizedName,
      'ParentName': instance.parentName,
      'NameLevel': instance.nameLevel,
      'AltName': instance.altName,
      'AltNameText': instance.altNameText,
      'NormalizedAlias': instance.normalizedAlias,
      'Source': instance.source,
      'Markdown': instance.markdown,
      'FullText': instance.fullText,
      'ItemType': instance.itemType,
      'Children': instance.children?.map((e) => e?.toJson())?.toList(),
      'SkillProficiencies': instance.skillProficiencies,
      'MasteredTools': instance.masteredTools,
      'MasteredLanguages': instance.masteredLanguages,
      'Equipment': instance.equipment,
    };
